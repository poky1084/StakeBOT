using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StakeBotUI
{
    public class BetResult
    {
        public string  Game             { get; set; }
        public decimal Amount           { get; set; }
        public decimal PayoutMultiplier { get; set; }
        public decimal Payout           { get; set; }
        public decimal Profit           { get; set; }
        public bool    Win              { get; set; }
        public string  BetId            { get; set; }
        public string  Extra            { get; set; }
        public bool    HasError         { get; set; }
        public string  ErrorMessage     { get; set; }
        public string  ErrorType        { get; set; }
        public decimal RollResult       { get; set; }  // numeric result for dice/limbo/etc.
        public string  RollDisplay      { get; set; }  // display string for Roll column
        /// <summary>Actual result colors for diamonds stop-on-pattern check (comma-separated).</summary>
        public List<string> ResultColors { get; set; }
        /// <summary>True when this bet was the one that triggered a stop condition (highlighted orange in history).</summary>
        public bool    TriggeredStop    { get; set; }
        
    }
    

    public class BotEngine
    {
        public decimal nextbet, chance, basebet, basechance;
        public bool    bethigh;
        public long    bets, wins, losses;
        public decimal profit, balance, wagered;
        public int     currentstreak;

        private bool   _win, _shouldStop;
        public  bool   StoppedDueToError { get; private set; }
        private CancellationToken _ct;
        private BotSettings _s;
        private int[]  _hiloPattern;
        private int    _hiloPos;
        private readonly object _statLock = new object();

        /// <summary>Last time an [ERROR] or [API ERROR] was logged — used to throttle to one log per 2 s.</summary>
        private DateTime _lastErrorLogTime = DateTime.MinValue;
        private readonly object _errorLogLock = new object();

        private Func<string, object, CancellationToken, Task<JObject>> _post;
        private Action<string>     _log;
        private Action<BotEngine>  _updateUI;
        private Action<BetResult>  _addHistory;
        /// <summary>Fires on every card revealed in a Hilo round: (rank, suit, payoutMultiplier).
        /// payoutMultiplier &lt;= 1.0 signals the start card.</summary>
        private Action<string, string, double> _onHiloCard;

        public void Init(
            BotSettings s,
            Func<string, object, CancellationToken, Task<JObject>> post,
            Action<string> log,
            Action<BotEngine> updateUI,
            Action<BetResult> addHistory,
            Action<string, string, double> onHiloCard = null)
        {
            _s = s; _post = post; _log = log; _updateUI = updateUI; _addHistory = addHistory;
            _onHiloCard = onHiloCard;
        }

        public async Task RunAsync(CancellationToken ct, decimal startBalance = 0m)
        {
            _ct = ct;
            nextbet = _s.BaseBet;       basebet    = nextbet;
            chance  = _s.DiceChance;    basechance = chance;
            bethigh = _s.DiceBetHigh;
            bets = wins = losses = 0;
            profit = wagered = 0m;
            balance = startBalance;     // real wallet amount so balance-based conditions work correctly
            currentstreak = 0;
            _win = false; _shouldStop = false; StoppedDueToError = false;
            _hiloPattern = ParseIntList(_s.HiloPattern);
            _hiloPos = 0;

            // No SynchronizationContext needed — engine runs on a background
            // thread; all UI callbacks already have InvokeRequired guards.

            while (!ct.IsCancellationRequested && !_shouldStop)
            {
                foreach (var block in _s.GetConditions(_s.SelectedGame))
                    if (IsMatching(block)) Execute(block);

                if (_shouldStop || ct.IsCancellationRequested) break;
                if (nextbet < 0m) nextbet = 0m;

                // HiLo must always run in normal mode — fast mode is incompatible
                // with its multi-step card sequence and is forced off here regardless
                // of the FastMode setting.
                bool effectiveFastMode = _s.FastMode &&
                    !string.Equals(_s.SelectedGame, "hilo", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(_s.SelectedGame, "videopoker", StringComparison.OrdinalIgnoreCase);

                if (effectiveFastMode)
                {
                    // ── FAST MODE ─────────────────────────────────────────
                    // Fire the request and immediately attach a continuation
                    // that processes the result on the UI thread whenever the
                    // response arrives. The loop does NOT await the response —
                    // it only waits the delay before firing the next request.
                    var betTask = MakeBet(ct);

                    _ = betTask.ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            string emsg = t.Exception?.InnerException?.Message ?? t.Exception?.Message ?? "";
                            LogError($"[ERROR] {emsg}");
                            // Any exception (HTTP, network, rate-limit) → loop keeps firing naturally
                            return;
                        }
                        if (t.IsCanceled) return;
                        var r = t.Result;
                        if (r == null) return;
                        if (r.HasError)
                        {
                            if (IsParallelBetError(r)) return; // transient collision — skip silently
                            if (IsSeedRotateError(r))
                            {
                                LogError($"[SEED ROTATE] {r.ErrorMessage} — retrying");
                                return; // loop keeps firing naturally
                            }
                            bool isRateLimit = IsRateLimitError(r.ErrorMessage);
                            LogError($"[API ERROR] {r.ErrorMessage}");
                            // Any API error → loop keeps firing naturally (never stop on HTTP errors)
                            return;
                        }
                        ApplyResult(r);
                    }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);

                    // Wait the delay then fire the next request
                    if (_s.BetDelayMs > 0)
                        try { await Task.Delay(_s.BetDelayMs, ct); } catch { break; }
                    else
                        await Task.Yield(); // yield once so the UI stays responsive
                }
                else
                {
                    // ── NORMAL MODE ───────────────────────────────────────
                    // Wait for the response before doing anything else.
                    BetResult result;
                    try { result = await MakeBet(ct); }
                    catch (OperationCanceledException) { break; }
                    catch (Exception ex)
                    {
                        string emsg = ex.Message ?? "";
                        LogError($"[ERROR] {emsg}");
                        // Any exception (HTTP, network, rate-limit) → wait 2 s and retry
                        try { await Task.Delay(2000, ct); } catch { break; }
                        continue;
                    }

                    if (result.HasError)
                    {
                        if (IsParallelBetError(result))
                        {
                            // Transient collision — just retry without stopping or logging noise
                            continue;
                        }
                        if (IsSeedRotateError(result))
                        {
                            // Server rotated the provably-fair seed — transient, just retry
                            LogError($"[SEED ROTATE] {result.ErrorMessage} — retrying");
                            try { await Task.Delay(500, ct); } catch { break; }
                            continue;
                        }
                        bool isRateLimit = IsRateLimitError(result.ErrorMessage);
                        LogError($"[API ERROR] {result.ErrorMessage}");
                        // Any API error → wait 2 s and retry (never stop on HTTP errors)
                        try { await Task.Delay(isRateLimit ? 1000 : 2000, ct); } catch { break; }
                        continue;
                    }

                    ApplyResult(result);

                    if (_s.BetDelayMs > 0)
                        try { await Task.Delay(_s.BetDelayMs, ct); } catch { break; }
                }
            }
        }

        // ── Shared result processor ──────────────────────────────────────
        // Called from thread-pool continuations (fast mode) or the loop
        // thread (normal mode) — _statLock guards the shared counters.
        private void ApplyResult(BetResult result)
        {
            lock (_statLock)
            {
                _win = result.Win;
                wagered += result.Amount;
                profit += result.Profit;
                balance += result.Profit;
                bets++;
                if (_win) { wins++; currentstreak = currentstreak > 0 ? currentstreak + 1 : 1; }
                else { losses++; currentstreak = currentstreak < 0 ? currentstreak - 1 : -1; }
            }

            _updateUI?.Invoke(this);

            // ── Evaluate stop conditions BEFORE adding to history so the
            //    triggering row can be highlighted orange in the UI ──────
            bool wasAlreadyStopped = _shouldStop;

            if (_s.StopOnMultiplier && result.PayoutMultiplier >= _s.StopMultiplierValue)
            { _shouldStop = true; _log?.Invoke($"[STOP] Payout {result.PayoutMultiplier:F4}x ≥ {_s.StopMultiplierValue}x"); }

            if (!_shouldStop && _s.SelectedGame == "dice" && _s.StopOnDiceResult && result.RollResult > 0
                && Math.Round(result.RollResult, 2, MidpointRounding.AwayFromZero) == Math.Round(_s.StopDiceResultValue, 2, MidpointRounding.AwayFromZero))
            { _shouldStop = true; _log?.Invoke($"[STOP] Dice {result.RollResult:F2} = target"); }

            if (!_shouldStop && _s.SelectedGame == "diamonds" && _s.StopOnDiamondsWin)
            {
                bool matched = DiamondPatternMatches(_s.DiamondsColors, result.ResultColors);
                if (matched || ((result.ResultColors == null || result.ResultColors.Count == 0) && result.Win))
                {
                    _shouldStop = true;
                    string desc = matched ? $"ordered pattern matched ({string.Join(",", result.ResultColors)})" : "win (colors unavailable)";
                    _log?.Invoke($"[STOP] Diamonds pattern [{_s.DiamondsColors}] — {desc}");
                }
            }

            // Mark the result that caused the stop, then add it to history.
            // Skip the orange flag if the user manually clicked Stop (token cancelled).
            result.TriggeredStop = _shouldStop && !wasAlreadyStopped && !_ct.IsCancellationRequested;
            _addHistory?.Invoke(result);
        }


        // ═══════════════════════════════════════════════════════════
        //  CONDITION ENGINE
        // ═══════════════════════════════════════════════════════════
        private bool IsMatching(ConditionBlockData b)
        {
            decimal v;
            if (b.Type == "bets")
            {
                switch (b.BetType)
                {
                    case "bet":  v = bets; break;
                    case "win":  if (!_win) return false; v = b.OnType == "every" ? wins : Math.Max(0, currentstreak); break;
                    case "lose": if (_win) return false;  v = b.OnType == "every" ? losses : Math.Max(0, -currentstreak); break;
                    default: return false;
                }
                switch (b.OnType)
                {
                    case "every": case "everyStreakOf": return b.OnValue > 0 && v > 0 && (v % b.OnValue) == 0;
                    case "firstStreakOf":    return v == b.OnValue;
                    case "streakGreaterThan": return v > b.OnValue;
                    case "streakLowerThan":  return v < b.OnValue;
                    default: return false;
                }
            }
            else
            {
                switch (b.ProfitType)
                {
                    case "balance": v = balance; break;
                    case "loss":    v = -profit; break;
                    case "profit":  v = profit;  break;
                    default: return false;
                }
                switch (b.OnType)
                {
                    case "greaterThan":          return v > b.OnValue;
                    case "greaterThanOrEqualTo": return v >= b.OnValue;
                    case "lowerThan":            return v < b.OnValue;
                    case "lowerThanOrEqualTo":   return v <= b.OnValue;
                    default: return false;
                }
            }
        }

        private void Execute(ConditionBlockData b)
        {
            switch (b.DoType)
            {
                case "increaseByPercentage":  nextbet  *= 1m + b.DoValue / 100m; break;
                case "decreaseByPercentage":  nextbet  *= 1m - b.DoValue / 100m; break;
                case "increaseWinChanceBy":   chance   *= 1m + b.DoValue / 100m; break;
                case "decreaseWinChanceBy":   chance   *= 1m - b.DoValue / 100m; break;
                case "addToAmount":           nextbet  += b.DoValue; break;
                case "subtractFromAmount":    nextbet  -= b.DoValue; break;
                case "addToWinChance":        chance   += b.DoValue; break;
                case "subtractFromWinChance": chance   -= b.DoValue; break;
                case "setAmount":             nextbet   = b.DoValue; break;
                case "setWinChance":          chance    = b.DoValue; break;
                case "switchOverUnder":       bethigh   = !bethigh;  break;
                case "resetAmount":           nextbet   = basebet;   break;
                case "resetWinChance":        chance    = basechance; break;
                case "stop":                  _shouldStop = true;    break;
            }
        }

        // ═══════════════════════════════════════════════════════════
        //  RESPONSE HELPERS
        // ═══════════════════════════════════════════════════════════
        private static (JToken bet, string error, string errorType) ExtractBet(JObject json)
        {
            if (json == null) return (null, "null response", null);
            if (json["errors"] is JArray errs && errs.Count > 0)
            {
                string errType = errs[0]["errorType"]?.Value<string>();
                string errMsg  = errs[0]["message"]?.Value<string>() ?? errType ?? "API error";
                return (null, errMsg, errType);
            }
            if (json["data"] is JObject data)
            { var f = data.Properties().FirstOrDefault(); return (f?.Value, null, null); }
            var fp = json.Properties().FirstOrDefault(p => p.Name != "errors" && p.Name != "_raw");
            return (fp?.Value, null, null);
        }

        private BetResult ErrorResult(string game, string msg, string errType = null) =>
            new BetResult { Game = game, HasError = true, ErrorMessage = msg, ErrorType = errType, Amount = nextbet };

        private BetResult MakeResult(JToken bet, string game, string extra = "", string rollDisplay = null)
        {
            if (bet == null)
                return new BetResult { Game = game, Win = false, Amount = nextbet, Profit = -nextbet, Extra = extra, RollDisplay = rollDisplay ?? "" };
            bool    active = bet["active"]?.Value<bool>()    ?? false;
            decimal pm     = bet["payoutMultiplier"]?.Value<decimal>() ?? 0m;
            decimal amt    = bet["amount"]?.Value<decimal>() ?? nextbet;
            decimal py     = bet["payout"]?.Value<decimal>() ?? 0m;
            return new BetResult
            {
                Game = game, Amount = amt, PayoutMultiplier = pm, Payout = py,
                Profit = py - amt, Win = !active && pm >= 1m,
                BetId  = bet["id"]?.Value<string>(), Extra = extra,
                RollDisplay = rollDisplay ?? ""
            };
        }

        // ═══════════════════════════════════════════════════════════
        //  BET DISPATCH
        // ═══════════════════════════════════════════════════════════
        private Task<BetResult> MakeBet(CancellationToken ct)
        {
            switch (_s.SelectedGame)
            {
                case "dice":        return BetDice(ct);
                case "limbo":       return BetLimbo(ct);
                case "hilo":        return BetHilo(ct);
                case "mines":       return BetMines(ct);
                case "keno":        return BetKeno(ct);
                case "plinko":      return BetPlinko(ct);
                case "wheel":       return BetWheel(ct);
                case "baccarat":    return BetBaccarat(ct);
                case "roulette":    return BetRoulette(ct);
                case "pump":        return BetPump(ct);
                case "dragontower": return BetDragonTower(ct);
                case "bars":        return BetBars(ct);
                case "tomeoflife":  return BetTomeOfLife(ct);
                case "scarabspin":  return BetScarabSpin(ct);
                case "diamonds":    return BetDiamonds(ct);
                case "cases":       return BetCases(ct);
                case "rps":         return BetRps(ct);
                case "flip":        return BetFlip(ct);
                case "snakes":      return BetSnakes(ct);
                case "darts":       return BetDarts(ct);
                case "packs":       return BetPacks(ct);
                case "moles":       return BetMoles(ct);
                case "chicken":     return BetChicken(ct);
                case "tarot":       return BetTarot(ct);
                case "drill":       return BetDrill(ct);
                case "primedice":   return BetPrimedice(ct);
                case "videopoker":  return BetVideoPoker(ct);
                default: throw new NotSupportedException($"Game '{_s.SelectedGame}' not implemented.");
            }
        }

        // ── DICE ─────────────────────────────────────────────────────────
        private async Task<BetResult> BetDice(CancellationToken ct)
        {
            decimal ch = Math.Max(0.01m, Math.Min(98.99m, chance));
            double target = bethigh ? (double)(100m - ch) : (double)ch;
            string cond = bethigh ? "above" : "below";
            var body = new { target, condition = cond, identifier = RandId(), amount = (double)nextbet, currency = _s.SelectedCurrency };
            var json = await Post("_api/casino/dice/roll", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("dice", err, errType);
            decimal roll = bet?["state"]?["result"]?.Value<decimal>() ?? 0m;
            var r = MakeResult(bet, "dice", $"{target:F2} {cond}", roll.ToString("F2"));
            r.RollResult = roll;
            return r;
        }

        // ── LIMBO ────────────────────────────────────────────────────────
        private async Task<BetResult> BetLimbo(CancellationToken ct)
        {
            var body = new { multiplierTarget = (double)_s.LimboTarget, identifier = RandId(), amount = (double)nextbet, currency = _s.SelectedCurrency };
            var json = await Post("_api/casino/limbo/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("limbo", err, errType);
            decimal roll = bet?["state"]?["result"]?.Value<decimal>() ?? 0m;
            var r = MakeResult(bet, "limbo", $">{_s.LimboTarget:F4}x", roll > 0 ? roll.ToString("F2") + "x" : "");
            r.RollResult = roll;
            return r;
        }

        // ── HILO ─────────────────────────────────────────────────────────
        private async Task<BetResult> BetHilo(CancellationToken ct)
        {
            if (_hiloPattern == null || _hiloPattern.Length == 0)
                throw new InvalidOperationException("Hilo pattern is empty.");

            string scRank = (_s.HiloStartCardRank ?? "").Trim().ToUpper();
            string scSuit = (_s.HiloStartCardSuit ?? "").Trim().ToUpper();
            bool hasStartCard = !string.IsNullOrEmpty(scRank) && !string.IsNullOrEmpty(scSuit);

            // Build request body as JObject so startCard is always serialized correctly.
            // The API requires startCard as { rank: "A", suit: "C" } — never a plain string.
            var startBody = new Newtonsoft.Json.Linq.JObject();
            startBody["identifier"] = RandId();
            startBody["currency"]   = _s.SelectedCurrency;
            startBody["amount"]     = (double)nextbet;
            if (hasStartCard)
            {
                startBody["startCard"] = new Newtonsoft.Json.Linq.JObject
                {
                    ["rank"] = scRank,
                    ["suit"] = scSuit
                };
            }

            var startJson = await Post("_api/casino/hilo/bet", startBody, ct);
            var (startBet, startErr, startErrType) = ExtractBet(startJson);

            // existingGame: an active round is already open — stop the bot cleanly
            // so the user can finish it manually via the HiLo card-strip buttons.
            // We do NOT cashout here; the user decides how to proceed.
            if (startErr != null && startErrType == "existingGame")
            {
                _log("[HILO] An active game is already running. Finish it manually via the card strip, then restart the bot.");
                _shouldStop = true;
                //Form1 myApp = new Form1();

                // Change the variable
                Form1.isManual = true;
                
                // Return a zero-profit non-error result so the main loop does
                // NOT set StoppedDueToError and does NOT play the error beep.
                return new BetResult { Game = "hilo", HasError = false, Win = false, Amount = nextbet, Profit = 0m };
            }

            if (startErr != null) return ErrorResult("hilo", startErr, startErrType);
            if (startBet == null) return MakeResult(null, "hilo");

            bool active = startBet["active"]?.Value<bool>() ?? false;
            if (!active) return MakeResult(startBet, "hilo", "start-loss");

            string lastRank = startBet["state"]?["startCard"]?["rank"]?.Value<string>() ?? "?";
            string lastSuit = startBet["state"]?["startCard"]?["suit"]?.Value<string>() ?? "?";
            string patternDesc = _s.HiloPattern;

            // Fire UI callback for the start card (multi <= 1.0 is the signal)
            _onHiloCard?.Invoke(lastRank, lastSuit, 1.0);

            // Accumulate every card rank revealed — start with the deal card.
            var cardHistory = new System.Collections.Generic.List<string> { lastRank };

            JToken lastBet = startBet;

            for (int step = 0; step < _hiloPattern.Length && active; step++)
            {
                int pNum = _hiloPattern[step];
                string guess = HiloGuess(pNum, lastRank);

                // Pattern 7 = Skip → send "skip" as the guess to the /next endpoint.

                var nextJson = await Post("_api/casino/hilo/next", new { guess }, ct);
                var (nextBet, nextErr, nextErrType) = ExtractBet(nextJson);
                if (nextErr != null)
                {
                    try { await Post("_api/casino/hilo/cashout", new { identifier = RandId() }, ct); } catch { }
                    return ErrorResult("hilo", nextErr, nextErrType);
                }
                if (nextBet == null) { active = false; break; }

                lastBet = nextBet;
                active = nextBet["active"]?.Value<bool>() ?? false;
                var rounds = nextBet["state"]?["rounds"] as JArray;
                if (rounds != null && rounds.Count > 0)
                {
                    // Always record the new card — even if the same rank appears twice
                    lastRank = rounds[rounds.Count - 1]?["card"]?["rank"]?.Value<string>() ?? lastRank;
                    lastSuit = rounds[rounds.Count - 1]?["card"]?["suit"]?.Value<string>() ?? lastSuit;
                    double revealedMulti = rounds[rounds.Count - 1]?["payoutMultiplier"]?.Value<double>() ?? 0;
                    cardHistory.Add(lastRank);

                    // Notify the UI card strip — multi > 1.0 distinguishes from start card
                    _onHiloCard?.Invoke(lastRank, lastSuit, revealedMulti > 1.0 ? revealedMulti : 1.01);
                }
                // No early return here — cashout only fires after the full pattern completes.
            }

            // Build card sequence string for Roll column
            string cardSeq = string.Join(",", cardHistory);

            if (active)
            {
                // All pattern steps finished and game is still running → cashout now.
                var cashJson = await Post("_api/casino/hilo/cashout", new { identifier = RandId() }, ct);
                var (cashBet, cashErr, cashErrType) = ExtractBet(cashJson);
                if (cashErr != null) return ErrorResult("hilo", cashErr, cashErrType);
                return MakeResult(cashBet, "hilo", $"pattern={patternDesc}|cashout", cardSeq);
            }
            // Game ended (bust) before pattern completed — return the last received bet result.
            return MakeResult(lastBet, "hilo", $"pattern={patternDesc}", cardSeq);
        }

        private string HiloGuess(int p, string rank)
        {
            // Valid API guesses: higher, lower, higherEqual, lowerEqual, equal
            // 0=Low  1=High  2=Equal  3=Random  4=LowestOdds(best payout)  5=HighestOdds(safest)  7=Skip(cashout)
            string rankU = rank?.ToUpper() ?? "";

            // Pattern 7 always sends "skip", regardless of the current card.
            if (p == 7) return "skip";

            // Pattern 1 (High): only Ace uses strict "higher" (can't go higherEqual from the bottom);
            // every other card — including King — uses "higherEqual".
            if (p == 1) return rankU == "A" ? "higher" : "higherEqual";

            // Pattern 0 (Low): only King uses strict "lower" (can't go lowerEqual from the top);
            // every other card — including Ace — uses "lowerEqual".
            if (p == 0) return rankU == "K" ? "lower" : "lowerEqual";

            // Pattern 2 always sends "equal" — applies to any card including A and K.
            if (p == 2) return "equal";

            // Pattern 4 with A or K → "equal" (only valid directional guess on edge cards)
            if (p == 4 && (rankU == "A" || rankU == "K")) return "equal";

            // Hard edge-cases for all remaining patterns: Ace is the lowest card (can only go
            // higher), King is the highest card (can only go lower).
            if (rankU == "A") return "higher";
            if (rankU == "K") return "lower";

            var highRanks = new HashSet<string> { "Q","J","10","9","8" };
            bool isHigh = highRanks.Contains(rankU);
            switch (p)
            {
                case 3: return new Random().Next(2) == 0 ? "higherEqual" : "lowerEqual"; // random for non-A/K cards
                case 4: return isHigh ? "higherEqual" : "lowerEqual";  // highest odds = safest side
                case 5: return isHigh ? "lowerEqual"  : "higherEqual"; // lowest odds  = best payout side
                default: return "higher";
            }
        }

        // ── MINES ────────────────────────────────────────────────────────
        private async Task<BetResult> BetMines(CancellationToken ct)
        {
            int[] fields = ParseIntList(_s.MinesFields).Select(f => f - 1).Where(f => f >= 0 && f < 25).ToArray();
            var body = new { amount = (double)nextbet, currency = _s.SelectedCurrency, identifier = RandId(), minesCount = _s.MinesMines, fields };
            var json = await Post("_api/casino/mines/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("mines", err, errType);
            if (bet == null) return MakeResult(null, "mines");
            bool active = bet["active"]?.Value<bool>() ?? false;
            string extra = $"{_s.MinesMines} mines / {fields.Length} tiles";
            if (!active) return MakeResult(bet, "mines", extra, "hit mine");
            var cashJson = await Post("_api/casino/mines/cashout", new { identifier = RandId() }, ct);
            var (cashBet, cashErr, cashErrType) = ExtractBet(cashJson);
            if (cashErr != null) return ErrorResult("mines", cashErr, cashErrType);
            decimal pm = cashBet?["payoutMultiplier"]?.Value<decimal>() ?? 0m;
            return MakeResult(cashBet ?? bet, "mines", extra, pm > 0 ? pm.ToString("F4") + "x" : "");
        }

        // ── KENO ─────────────────────────────────────────────────────────
        private async Task<BetResult> BetKeno(CancellationToken ct)
        {
            int[] numbers = ParseIntList(_s.KenoNumbers);
            var body = new { amount = (double)nextbet, currency = _s.SelectedCurrency, identifier = RandId(), risk = _s.KenoRisk ?? "low", numbers };
            var json = await Post("_api/casino/keno/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("keno", err, errType);
            // Compute hit count for roll display
            string rollDisp = "";
            try
            {
                var selected = (bet?["state"]?["selectedNumbers"] as JArray)?.Select(t => t.Value<int>()).ToHashSet() ?? new HashSet<int>();
                var drawn    = (bet?["state"]?["drawnNumbers"]    as JArray)?.Select(t => t.Value<int>()).ToList()  ?? new List<int>();
                int hits = drawn.Count(n => selected.Contains(n));
                rollDisp = $"{hits}/{selected.Count} hits";
            }
            catch { }
            return MakeResult(bet, "keno", $"{_s.KenoRisk}|{numbers.Length}picks", rollDisp);
        }

        // ── PLINKO ───────────────────────────────────────────────────────
        private async Task<BetResult> BetPlinko(CancellationToken ct)
        {
            var body = new
            {
                operationName = "PlinkoBet",
                query = "mutation PlinkoBet($amount: Float!, $currency: CurrencyEnum!, $risk: CasinoGamePlinkoRiskEnum!, $rows: Int!, $identifier: String!) {\n  plinkoBet(\n    amount: $amount\n    currency: $currency\n    risk: $risk\n    rows: $rows\n    identifier: $identifier\n  ) {\n    ...CasinoBet\n    state {\n      ...CasinoGamePlinko\n    }\n  }\n}\n\nfragment CasinoGamePlinko on CasinoGamePlinko {\n  risk\n  rows\n  point\n  path\n}\n\nfragment CasinoBet on CasinoBet {\n  id\n  active\n  payoutMultiplier\n  amountMultiplier\n  amount\n  payout\n  updatedAt\n  currency\n  game\n  user {\n    id\n    name\n  }\n}",
                variables = new { amount = (double)nextbet, currency = _s.SelectedCurrency, risk = _s.PlinkoRisk, rows = _s.PlinkoRows, identifier = RandId() }
            };
            var json = await Post("_api/graphql", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("plinko", err, errType);
            string point = bet?["state"]?["point"]?.Value<string>() ?? "";
            decimal multiplier = bet?["payoutMultiplier"]?.Value<decimal>() ?? 0m;
            var r = MakeResult(
                                bet,
                                "plinko",
                                $"{_s.PlinkoRisk}|{_s.PlinkoRows}rows",
                                string.IsNullOrEmpty(point) ? "" : $"{(multiplier != 0 ? multiplier.ToString() : "")}"
                            );
            r.RollResult = multiplier;
            return r;
        }

        // ── WHEEL ────────────────────────────────────────────────────────
        private async Task<BetResult> BetWheel(CancellationToken ct)
        {
            var body = new { amount = (double)nextbet, currency = _s.SelectedCurrency, identifier = RandId(), risk = _s.WheelRisk, segments = _s.WheelSegments };
            var json = await Post("_api/casino/wheel/spin", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("wheel", err, errType);
            decimal roll = bet?["state"]?["result"]?.Value<decimal>() ?? 0m;
            var r = MakeResult(bet, "wheel", $"{_s.WheelRisk}|{_s.WheelSegments}seg", roll > 0 ? roll.ToString("F0") : "");
            r.RollResult = roll; return r;
        }

        // ── BACCARAT ─────────────────────────────────────────────────────
        private async Task<BetResult> BetBaccarat(CancellationToken ct)
        {
            var body = new { currency = _s.SelectedCurrency, identifier = RandId(), tie = (double)_s.BaccaratTie, player = (double)_s.BaccaratPlayer, banker = (double)_s.BaccaratBanker };
            var json = await Post("_api/casino/baccarat/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("baccarat", err, errType);
            string res = bet?["state"]?["result"]?.Value<string>() ?? "";
            return MakeResult(bet, "baccarat", "", string.IsNullOrEmpty(res) ? "" : res);
        }

        // ── ROULETTE ─────────────────────────────────────────────────────
        private async Task<BetResult> BetRoulette(CancellationToken ct)
        {
            var chips = JsonConvert.DeserializeObject<JArray>(_s.RouletteChips ?? "[]");
            var colors = new JArray(); var numbers = new JArray(); var rows = new JArray(); var pars = new JArray(); var ranges = new JArray();
            if (chips != null) foreach (var chip in chips)
            {
                string v = chip["value"]?.Value<string>() ?? "";
                if (v.Contains("color")) colors.Add(chip);
                else if (v.Contains("number")) numbers.Add(chip);
                else if (v.Contains("row")) rows.Add(chip);
                else if (v.Contains("parity")) pars.Add(chip);
                else if (v.Contains("range")) ranges.Add(chip);
            }
            var body = new { currency = _s.SelectedCurrency, identifier = RandId(), rows, parities = pars, ranges, colors, numbers };
            var json = await Post("_api/casino/roulette/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("roulette", err, errType);
            decimal roll = bet?["state"]?["result"]?.Value<decimal>() ?? -1m;
            var r = MakeResult(bet, "roulette", "", roll >= 0 ? roll.ToString("F0") : "");
            r.RollResult = roll; return r;
        }

        // ── PUMP ─────────────────────────────────────────────────────────
        private async Task<BetResult> BetPump(CancellationToken ct)
        {
            var body = new { amount = (double)nextbet, currency = _s.SelectedCurrency, identifier = RandId(), round = _s.PumpPumps, difficulty = _s.PumpDifficulty };
            var json = await Post("_api/casino/pump/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("pump", err, errType);
            decimal pop = bet?["state"]?["_popPoint"]?.Value<decimal>() ?? 0m;
            var r = MakeResult(bet, "pump", $"{_s.PumpDifficulty}|{_s.PumpPumps}pumps", pop > 0 ? "pop@" + pop.ToString("F0") : "");
            r.RollResult = pop; return r;
        }

        // ── DRAGON TOWER ─────────────────────────────────────────────────
        private async Task<BetResult> BetDragonTower(CancellationToken ct)
        {
            int[] eggs = ParseIntList(_s.DragonEggs);
            var body = new { amount = (double)nextbet, currency = _s.SelectedCurrency, identifier = RandId(), difficulty = _s.DragonDifficulty, eggs };
            var json = await Post("_api/casino/dragon-tower/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("dragontower", err, errType);
            decimal round = bet?["state"]?["currentRound"]?.Value<decimal>() ?? 0m;
            var r = MakeResult(bet, "dragontower", $"{_s.DragonDifficulty}|{eggs.Length}eggs", round > 0 ? "rd" + round.ToString("F0") : "");
            r.RollResult = round; return r;
        }

        // ── BARS ─────────────────────────────────────────────────────────
        private async Task<BetResult> BetBars(CancellationToken ct)
        {
            int[] tiles = ParseIntList(_s.BarsTiles);
            var body = new { amount = (double)nextbet, currency = _s.SelectedCurrency, identifier = RandId(), difficulty = _s.BarsDifficulty, tiles };
            var json = await Post("_api/casino/bars/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("bars", err, errType);
            decimal pm = bet?["payoutMultiplier"]?.Value<decimal>() ?? 0m;
            return MakeResult(bet, "bars", $"{_s.BarsDifficulty}", pm > 0 ? pm.ToString("F4") + "x" : "");
        }

        // ── TOME OF LIFE ──────────────────────────────────────────────────
        private async Task<BetResult> BetTomeOfLife(CancellationToken ct)
        {
            var body = new { currency = _s.SelectedCurrency, amount = (double)nextbet, lines = _s.SlotLines, identifier = RandId() };
            var json = await Post("_api/casino/slots-tome-of-life/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("tomeoflife", err, errType);
            decimal pm = bet?["payoutMultiplier"]?.Value<decimal>() ?? 0m;
            return MakeResult(bet, "tomeoflife", $"{_s.SlotLines}lines", pm > 0 ? pm.ToString("F4") + "x" : "");
        }

        // ── SCARAB SPIN ───────────────────────────────────────────────────
        private async Task<BetResult> BetScarabSpin(CancellationToken ct)
        {
            var body = new { currency = _s.SelectedCurrency, amount = (double)nextbet, lines = _s.SlotLines, identifier = RandId() };
            var json = await Post("_api/casino/slots/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("scarabspin", err, errType);
            decimal pm = bet?["payoutMultiplier"]?.Value<decimal>() ?? 0m;
            return MakeResult(bet, "scarabspin", $"{_s.SlotLines}lines", pm > 0 ? pm.ToString("F4") + "x" : "");
        }

        // ── DIAMONDS ──────────────────────────────────────────────────────
        private async Task<BetResult> BetDiamonds(CancellationToken ct)
        {
            var body = new { currency = _s.SelectedCurrency, amount = (double)nextbet, identifier = RandId() };
            var json = await Post("_api/casino/diamonds/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("diamonds", err, errType);
            decimal pm = bet?["payoutMultiplier"]?.Value<decimal>() ?? 0m;

            // Extract result hand colors for stop-on-pattern comparison
            string resultColorsStr = "";
            List<string> resultColorsList = new List<string>();

            try
            {
                var state = bet?["state"];
                JArray hand = state?["hand"] as JArray
                           ?? state?["result"] as JArray
                           ?? state?["diamonds"] as JArray;

                if (hand != null && hand.Count > 0)
                {
                    foreach (var item in hand)
                    {
                        string color = (item.Type == JTokenType.String
                            ? item.Value<string>()
                            : item["color"]?.Value<string>() ?? item["value"]?.Value<string>() ?? item.ToString())
                            ?.ToLower().Trim();

                        if (!string.IsNullOrEmpty(color))
                        {
                            resultColorsList.Add(color);
                        }
                    }
                    resultColorsStr = string.Join(",", resultColorsList);
                }
            }
            catch { }

            var r = MakeResult(bet, "diamonds", _s.DiamondsColors, pm > 0 ? pm.ToString("F4") + "x" : "");
            r.ResultColors = resultColorsList;  // Store the list of colors
            r.RollDisplay = ""; // Clear text display, we'll use images
            return r;
        }

        // ── CASES ────────────────────────────────────────────────────────
        private async Task<BetResult> BetCases(CancellationToken ct)
        {
            var body = new { currency = _s.SelectedCurrency, amount = (double)nextbet, difficulty = _s.CasesDifficulty, identifier = RandId() };
            var json = await Post("_api/casino/cases/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("cases", err, errType);
            string res = bet?["state"]?["result"]?.ToString() ?? "";
            return MakeResult(bet, "cases", _s.CasesDifficulty, string.IsNullOrEmpty(res) ? "" : res);
        }

        // ── ROCK PAPER SCISSORS ───────────────────────────────────────────
        private async Task<BetResult> BetRps(CancellationToken ct)
        {
            string[] guesses = SafeArray(_s.RpsGuesses) ?? new[] { "rock" };
            var body = new { currency = _s.SelectedCurrency, amount = (double)nextbet, identifier = RandId(), guesses };
            var json = await Post("_api/casino/rock-paper-scissors/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("rps", err, errType);
            decimal pm = bet?["payoutMultiplier"]?.Value<decimal>() ?? 0m;
            return MakeResult(bet, "rps", string.Join(",", guesses), pm > 0 ? pm.ToString("F4") + "x" : "");
        }

        // ── COIN FLIP ────────────────────────────────────────────────────
        private async Task<BetResult> BetFlip(CancellationToken ct)
        {
            string[] guesses = SafeArray(_s.FlipGuesses) ?? new[] { "heads" };
            var body = new { operationName = "FlipBet",
                query = "mutation FlipBet($amount: Float!, $currency: CurrencyEnum!, $identifier: String, $guesses: [FlipConditionEnum!]!) {\n  flipBet(\n    amount: $amount\n    currency: $currency\n    identifier: $identifier\n    guesses: $guesses\n  ) {\n    ...CasinoBet\n    state {\n      ...CasinoGameFlip\n    }\n  }\n}\n\nfragment CasinoGameFlip on CasinoGameFlip {\n  currentRound\n  payoutMultiplier\n  playedRounds\n  flips\n}\n\nfragment CasinoBet on CasinoBet {\n  id\n  active\n  payoutMultiplier\n  amountMultiplier\n  amount\n  payout\n  updatedAt\n  currency\n  game\n  user {\n    id\n    name\n  }\n}",
                variables = new { amount = (double)nextbet, currency = _s.SelectedCurrency, identifier = RandId(), guesses } };
            var json = await Post("_api/graphql", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("flip", err, errType);
            decimal rnd = bet?["state"]?["currentRound"]?.Value<decimal>() ?? 0m;
            var r = MakeResult(bet, "flip", string.Join(",", guesses), rnd > 0 ? "rd" + rnd.ToString("F0") : "");
            r.RollResult = rnd; return r;
        }

        // ── SNAKES ───────────────────────────────────────────────────────
        private async Task<BetResult> BetSnakes(CancellationToken ct)
        {
            var body = new { currency = _s.SelectedCurrency, amount = (double)nextbet, identifier = RandId(), difficulty = _s.SnakesDifficulty, rollCount = _s.SnakesRolls };
            var json = await Post("_api/casino/snakes/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("snakes", err, errType);
            // Count winning rounds
            int wins2 = 0;
            try { var rounds = bet?["state"]?["rounds"] as JObject; if (rounds != null) wins2 = rounds.Properties().Count(p => (p.Value["payoutMultiplier"]?.Value<decimal>() ?? 0m) >= 1m); } catch { }
            return MakeResult(bet, "snakes", $"{_s.SnakesDifficulty}|{_s.SnakesRolls}rolls", wins2 > 0 ? $"{wins2}wins" : "");
        }

        // ── DARTS ────────────────────────────────────────────────────────
        private async Task<BetResult> BetDarts(CancellationToken ct)
        {
            var body = new { currency = _s.SelectedCurrency, amount = (double)nextbet, identifier = RandId(), difficulty = _s.DartsDifficulty };
            var json = await Post("_api/casino/darts/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("darts", err, errType);
            decimal pm = bet?["payoutMultiplier"]?.Value<decimal>() ?? 0m;
            return MakeResult(bet, "darts", _s.DartsDifficulty, pm > 0 ? pm.ToString("F4") + "x" : "");
        }

        // ── PACKS ────────────────────────────────────────────────────────
        private async Task<BetResult> BetPacks(CancellationToken ct)
        {
            var body = new { amount = (double)nextbet, currency = _s.SelectedCurrency, identifier = RandId() };
            var json = await Post("_api/casino/packs/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("packs", err, errType);
            decimal pm = bet?["payoutMultiplier"]?.Value<decimal>() ?? 0m;
            return MakeResult(bet, "packs", "", pm > 0 ? pm.ToString("F4") + "x" : "");
        }

        // ── MOLES ────────────────────────────────────────────────────────
        private async Task<BetResult> BetMoles(CancellationToken ct)
        {
            int[] picks = ParseIntList(_s.MolesPicks);
            var body = new { amount = (double)nextbet, currency = _s.SelectedCurrency, identifier = RandId(), molesCount = _s.MolesMoles, picks };
            var json = await Post("_api/casino/moles/autobet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("moles", err, errType);
            decimal rnd = bet?["state"]?["currentRound"]?.Value<decimal>() ?? 0m;
            var r = MakeResult(bet, "moles", $"{_s.MolesMoles}moles|{picks.Length}picks", rnd > 0 ? "rd" + rnd.ToString("F0") : "");
            r.RollResult = rnd; return r;
        }

        // ── CHICKEN ──────────────────────────────────────────────────────
        private async Task<BetResult> BetChicken(CancellationToken ct)
        {
            var body = new { amount = (double)nextbet, currency = _s.SelectedCurrency, identifier = RandId(), difficulty = _s.ChickenDifficulty, round = _s.ChickenSteps };
            var json = await Post("_api/casino/chicken/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("chicken", err, errType);
            decimal death = bet?["state"]?["_deathPoint"]?.Value<decimal>() ?? 0m;
            var r = MakeResult(bet, "chicken", $"{_s.ChickenDifficulty}|{_s.ChickenSteps}steps", death > 0 ? "died@" + death.ToString("F0") : "");
            r.RollResult = death; return r;
        }

        // ── TAROT ────────────────────────────────────────────────────────
        private async Task<BetResult> BetTarot(CancellationToken ct)
        {
            var body = new { amount = (double)nextbet, currency = _s.SelectedCurrency, identifier = RandId(), difficulty = _s.TarotDifficulty };
            var json = await Post("_api/casino/tarot/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("tarot", err, errType);
            decimal pm = bet?["payoutMultiplier"]?.Value<decimal>() ?? 0m;
            return MakeResult(bet, "tarot", _s.TarotDifficulty, pm > 0 ? pm.ToString("F4") + "x" : "");
        }

        // ── DRILL ────────────────────────────────────────────────────────
        private async Task<BetResult> BetDrill(CancellationToken ct)
        {
            var body = new { amount = (double)nextbet, currency = _s.SelectedCurrency, identifier = RandId(), target = (double)_s.DrillTarget, pick = _s.DrillPick };
            var json = await Post("_api/casino/drill/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("drill", err, errType);
            decimal multi = 0m;
            try
            {
                var results = bet?["state"]?["drillResults"] as JArray;
                int idx = Math.Max(0, _s.DrillPick - 1);
                if (results != null && idx < results.Count)
                    multi = results[idx]?["multiplier"]?.Value<decimal>() ?? 0m;
            }
            catch { }
            return MakeResult(bet, "drill", $"t={_s.DrillTarget}|pick={_s.DrillPick}", multi > 0 ? multi.ToString("F4") + "x" : "");
        }

        // ── PRIMEDICE ────────────────────────────────────────────────────
        private async Task<BetResult> BetPrimedice(CancellationToken ct)
        {
            bool useTwo = _s.PDCondition == "rollBetween" || _s.PDCondition == "rollOutside";
            object t3 = useTwo ? (object)null : (double)_s.PDTarget3;
            object t4 = useTwo ? (object)null : (double)_s.PDTarget4;
            var body = new { target1 = (double)_s.PDTarget1, target2 = (double)_s.PDTarget2, target3 = t3, target4 = t4,
                             condition = _s.PDCondition, identifier = RandId(), amount = (double)nextbet, currency = _s.SelectedCurrency };
            var json = await Post("_api/casino/primedice-x/bet", body, ct);
            var (bet, err, errType) = ExtractBet(json);
            if (err != null) return ErrorResult("primedice", err, errType);
            decimal roll = bet?["state"]?["result"]?.Value<decimal>() ?? 0m;
            var r = MakeResult(bet, "primedice", _s.PDCondition, roll > 0 ? roll.ToString("F2") : "");
            r.RollResult = roll; return r;
        }

        // ── VIDEO POKER ──────────────────────────────────────────────────────
        //  Two-step flow: deal (initialHand) → compute optimal hold → draw.
        //  Strategy options (BotSettings.VideoPokerStrategy):
        //    "optimal"   — full 9/6 Jacks-or-Better expected-value table
        //    "royalHunt" — always chase Royal Flush; overrides EV for
        //                  pairs/trips/two-pair in favour of RF draws
        // ─────────────────────────────────────────────────────────────────────
        private struct VPCard { public string rank; public string suit; }

        private static readonly string[] VP_RANKS =
            { "2","3","4","5","6","7","8","9","10","J","Q","K","A" };
        private static readonly HashSet<string> VP_HIGH  = new HashSet<string> { "J","Q","K","A" };
        private static readonly HashSet<string> VP_ROYAL = new HashSet<string> { "10","J","Q","K","A" };
        private static int VR(string r) => Array.IndexOf(VP_RANKS, r);

        private async Task<BetResult> BetVideoPoker(CancellationToken ct)
        {
            // ── 1. Deal: receive 5 initial cards ─────────────────────────────
            var dealJson = await Post("_api/casino/video-poker/bet",
                new { amount = (double)nextbet, currency = _s.SelectedCurrency }, ct);
            var (deal, dealErr, dealErrType) = ExtractBet(dealJson);

            // ── Active-game recovery ──────────────────────────────────────────────
            // The API rejects a new /bet when a round is still open.
            // Finish the dangling hand by calling /next with zero held cards
            // (full redraw), then return that result so the loop continues normally
            // without stopping or triggering the error beep.
            if (dealErr != null && IsActiveVideoPokerError(dealErr))
            {
                _log?.Invoke("[POKER] Active game detected — completing existing hand via /next (hold nothing)");
                var recoveryJson = await Post("_api/casino/video-poker/next",
                    new { identifier = RandId(), held = new object[0] }, ct);
                var (recDraw, recErr, recErrType) = ExtractBet(recoveryJson);
                if (recErr != null) return ErrorResult("videopoker", recErr, recErrType);

                string recResult = recDraw?["state"]?["handResult"]?.Value<string>() ?? "";
                string recCards  = "";
                try
                {
                    var arr = recDraw?["state"]?["playerHand"] as JArray;
                    if (arr != null)
                        recCards = string.Join(" ", arr.Select(c =>
                            (c["rank"]?.Value<string>() ?? "") +
                            VPSuitSymbol(c["suit"]?.Value<string>() ?? "")));
                }
                catch { }
                if (!string.IsNullOrEmpty(recResult))
                    _log?.Invoke($"[POKER] Recovery result: {recResult}  |  {recCards}");

                return MakeResult(recDraw, "videopoker",
                    string.IsNullOrEmpty(recCards) ? "recovery-draw" : recCards, recResult);
            }

            if (dealErr != null) return ErrorResult("videopoker", dealErr, dealErrType);
            if (deal == null)    return MakeResult(null, "videopoker");

            // API returns either state.initialHand or state.playerHand for the deal
            var initialToken = deal["state"]?["initialHand"] as JArray
                            ?? deal["state"]?["playerHand"]  as JArray;
            if (initialToken == null || initialToken.Count < 5)
                return MakeResult(deal, "videopoker", "bad-deal");

            var hand = initialToken.Select(t => new VPCard
            {
                rank = t["rank"]?.Value<string>() ?? "",
                suit = t["suit"]?.Value<string>() ?? ""
            }).ToList();

            // ── 2. Compute optimal hold ───────────────────────────────────────
            string strategy = _s.VideoPokerStrategy ?? "optimal";
            List<VPCard> held = string.Equals(strategy, "royalHunt", StringComparison.OrdinalIgnoreCase)
                ? VPRoyalHunt(hand)
                : VPOptimalHold(hand);

            string holdDesc = $"hold {held.Count}/5";
            _log?.Invoke($"[POKER] Dealt: {VPHandStr(hand)}  →  Holding {held.Count}: {VPHandStr(held)}");

            // ── 3. Draw: server fills un-held positions ──────────────────────
            var drawJson = await Post("_api/casino/video-poker/next",
                new
                {
                    identifier = RandId(),
                    held       = held.Select(c => new { c.rank, c.suit }).ToArray()
                }, ct);
            var (draw, drawErr, drawErrType) = ExtractBet(drawJson);
            if (drawErr != null) return ErrorResult("videopoker", drawErr, drawErrType);

            // ── 4. Decode final hand ─────────────────────────────────────────
            string handResult = draw?["state"]?["handResult"]?.Value<string>() ?? "";
            string finalCards = "";
            try
            {
                var finalArr = draw?["state"]?["playerHand"] as JArray;
                if (finalArr != null)
                    finalCards = string.Join(" ", finalArr.Select(c =>
                        (c["rank"]?.Value<string>() ?? "") +
                        VPSuitSymbol(c["suit"]?.Value<string>() ?? "")));
            }
            catch { }

            if (!string.IsNullOrEmpty(handResult))
                _log?.Invoke($"[POKER] Result: {handResult}  |  {finalCards}");

            // Roll column = hand-result name  (e.g. "threeOfAKind")
            // Info column = final 5 cards     (e.g. "A♥ K♠ Q♦ J♣ 10♠")
            string infoStr = string.IsNullOrEmpty(finalCards) ? holdDesc : finalCards;
            return MakeResult(draw, "videopoker", infoStr, handResult);
        }

        // ── Suit letter → Unicode symbol ─────────────────────────────────────
        private static string VPSuitSymbol(string suit)
        {
            if (suit == null) return "";
            switch (suit.ToUpper())
            {
                case "H": return " ";
                case "D": return " ";
                case "C": return " ";
                case "S": return " ";
                default:  return suit;
            }
        }

        // ── Diagnostic: card array → "A♥ K♠ Q♦ J♣ 10♠" style string ──────────
        private static string VPHandStr(IEnumerable<VPCard> h) =>
            string.Join(" ", h.Select(c => c.rank + VPSuitSymbol(c.suit)));

        // ══════════════════════════════════════════════════════════════════════
        //  OPTIMAL HOLD  —  9/6 Jacks-or-Better expected-value priority table
        //  Priority mirrors the authoritative JoB strategy:
        //  RF > SF > Quads > 4RF > FH > Flush > Trips > Straight > 4SF >
        //  Two Pair > High Pair > 3RF > 4Flush > Low Pair > 4OutStraight >
        //  3SF-pure > 2RF > High Cards > Draw-5
        // ══════════════════════════════════════════════════════════════════════
        private static List<VPCard> VPOptimalHold(List<VPCard> h)
        {
            var rg     = VPByRank(h);
            var sg     = VPBySuit(h);
            var rd     = rg.OrderByDescending(kv => kv.Value.Count)
                           .ThenByDescending(kv => VR(kv.Key)).ToList();
            var counts = rd.Select(kv => kv.Value.Count).ToList();
            bool flush    = h.All(c => c.suit == h[0].suit);
            bool straight = VPIsStraight(h);
            bool aceLow   = VPIsAceLow(h);
            int  maxVal   = h.Max(c => VR(c.rank));

            // ── TIER 1: Pat hands (hold all 5) ────────────────────────────────
            if (flush && straight && maxVal == 12 && !aceLow)
                return h;                                                     // Royal Flush
            if (flush && straight)
                return h;                                                     // Straight Flush
            if (counts[0] == 4)
                return rd[0].Value;                                           // Four of a Kind

            // ── TIER 2: 4-to-Royal (higher EV than Full House) ───────────────
            var r4 = VPRoyalDraw(h, 4);
            if (r4 != null) return r4;

            // ── TIER 2 cont: remaining pat hands ─────────────────────────────
            if (counts.Count > 1 && counts[0] == 3 && counts[1] == 2)
                return h;                                                     // Full House
            if (flush)  return h;                                             // Flush
            if (counts[0] == 3)
                return rd[0].Value.Take(3).ToList();                          // Three of a Kind
            if (straight) return h;                                           // Straight

            // ── TIER 3: 4-to-Straight-Flush ──────────────────────────────────
            var sf4 = VPSFDraw(h, 4, 1);
            if (sf4 != null) return sf4;

            // Two Pair
            if (counts.Count > 1 && counts[0] == 2 && counts[1] == 2)
                return rd[0].Value.Concat(rd[1].Value).ToList();

            // High Pair (Jacks or better)
            var hiPair = rd.FirstOrDefault(kv => kv.Value.Count == 2 && VP_HIGH.Contains(kv.Key));
            if (hiPair.Value != null) return hiPair.Value;

            // ── TIER 4: 3-to-Royal ────────────────────────────────────────────
            var r3 = VPRoyalDraw(h, 3);
            if (r3 != null) return r3;

            // 4-to-Flush
            var fl4 = sg.Values.FirstOrDefault(g => g.Count == 4);
            if (fl4 != null) return fl4;

            // Low Pair (2s–10s)
            var loPair = rd.FirstOrDefault(kv => kv.Value.Count == 2);
            if (loPair.Value != null) return loPair.Value;

            // ── TIER 5: 4-to-Outside-Straight ────────────────────────────────
            var (strOut, strIn) = VPStraight4(h);
            if (strOut != null) return strOut;

            // 3-to-Straight-Flush (pure consecutive, no gap)
            var sf3 = VPSFDraw(h, 3, 0);
            if (sf3 != null) return sf3;

            // 2-to-Royal
            var r2 = VPRoyalDraw(h, 2);
            if (r2 != null) return r2;

            // 4-to-Inside-Straight with ≥3 high cards
            if (strIn != null && strIn.Count(c => VP_HIGH.Contains(c.rank)) >= 3)
                return strIn;

            // High cards (J / Q / K / A), up to 4
            var highCards = h.Where(c => VP_HIGH.Contains(c.rank))
                             .OrderByDescending(c => VR(c.rank)).Take(4).ToList();
            if (highCards.Count > 0) return highCards;

            // Draw all 5
            return new List<VPCard>();
        }

        // ── Royal-Flush Hunt: chase RF above all but made premiums ────────────
        private static List<VPCard> VPRoyalHunt(List<VPCard> h)
        {
            bool flush = h.All(c => c.suit == h[0].suit);
            bool str   = VPIsStraight(h);
            if (flush && str) return h;                 // SF / RF — already made

            var rg = VPByRank(h);
            var rd = rg.OrderByDescending(kv => kv.Value.Count).ToList();
            if (rd[0].Value.Count == 4) return rd[0].Value; // Quads — keep

            var r4 = VPRoyalDraw(h, 4); if (r4 != null) return r4; // 4-to-RF

            // Full House / Flush / Straight — keep
            var counts = rd.Select(kv => kv.Value.Count).ToList();
            if (counts.Count > 1 && counts[0] == 3 && counts[1] == 2) return h;
            if (flush) return h;
            if (str)   return h;

            // 3-to-RF (breaks trips/two-pair — deliberate for RF hunt)
            var r3 = VPRoyalDraw(h, 3); if (r3 != null) return r3;

            // 2-to-RF
            var r2 = VPRoyalDraw(h, 2); if (r2 != null) return r2;

            // Hold any royal-rank cards
            var royalCards = h.Where(c => VP_ROYAL.Contains(c.rank))
                              .OrderByDescending(c => VR(c.rank)).ToList();
            return royalCards;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  VP HELPERS
        // ══════════════════════════════════════════════════════════════════════
        private static Dictionary<string, List<VPCard>> VPByRank(IEnumerable<VPCard> h)
        {
            var d = new Dictionary<string, List<VPCard>>();
            foreach (var c in h)
            {
                if (!d.ContainsKey(c.rank)) d[c.rank] = new List<VPCard>();
                d[c.rank].Add(c);
            }
            return d;
        }

        private static Dictionary<string, List<VPCard>> VPBySuit(IEnumerable<VPCard> h)
        {
            var d = new Dictionary<string, List<VPCard>>();
            foreach (var c in h)
            {
                if (!d.ContainsKey(c.suit)) d[c.suit] = new List<VPCard>();
                d[c.suit].Add(c);
            }
            return d;
        }

        private static bool VPIsStraight(IList<VPCard> h)
        {
            var v = h.Select(c => VR(c.rank)).OrderBy(x => x).ToArray();
            if (v.Distinct().Count() != 5) return false;
            if (v[4] - v[0] == 4) return true;
            // Ace-low: A-2-3-4-5
            return v[4] == 12 && v[0] == 0 && v[1] == 1 && v[2] == 2 && v[3] == 3;
        }

        private static bool VPIsAceLow(IList<VPCard> h)
        {
            var v = h.Select(c => VR(c.rank)).OrderBy(x => x).ToArray();
            return v[0] == 0 && v[1] == 1 && v[2] == 2 && v[3] == 3 && v[4] == 12;
        }

        /// <summary>Returns the best n-card same-suit Royal subset, or null.</summary>
        private static List<VPCard> VPRoyalDraw(IList<VPCard> h, int n)
        {
            var sg = VPBySuit(h);
            foreach (var g in sg.Values)
            {
                var royal = g.Where(c => VP_ROYAL.Contains(c.rank)).ToList();
                if (royal.Count >= n)
                    return royal.OrderByDescending(c => VR(c.rank)).Take(n).ToList();
            }
            return null;
        }

        /// <summary>Returns the longest same-suit near-consecutive run of ≥ minN cards, allowing ≤ maxGap gaps.</summary>
        private static List<VPCard> VPSFDraw(IList<VPCard> h, int minN, int maxGap)
        {
            var sg = VPBySuit(h);
            List<VPCard> best = null;
            foreach (var g in sg.Values)
            {
                if (g.Count < minN) continue;
                var sorted = g.OrderBy(c => VR(c.rank)).ToList();
                foreach (var combo in VPCombos(sorted, minN))
                {
                    var v    = combo.Select(c => VR(c.rank)).ToList();
                    int span = v.Max() - v.Min();
                    int gaps = span - (combo.Count - 1);
                    if (gaps <= maxGap && span <= 4 &&
                        (best == null || combo.Count > best.Count))
                        best = combo;
                }
            }
            return best;
        }

        /// <summary>Finds 4-to-outside-straight and 4-to-inside-straight (gutshot) draws.</summary>
        private static (List<VPCard> outside, List<VPCard> inside) VPStraight4(IList<VPCard> h)
        {
            var rg     = VPByRank(h);
            var uniq   = rg.Values.Select(g => g[0]).ToList();
            var sorted = uniq.OrderBy(c => VR(c.rank)).ToList();
            List<VPCard> outside = null, inside = null;
            foreach (var combo in VPCombos(sorted, 4))
            {
                var v    = combo.Select(c => VR(c.rank)).ToList();
                int span = v.Max() - v.Min();
                if (span == 3 && v.Distinct().Count() == 4)
                {
                    bool canLow  = v.Min() > 0;
                    bool canHigh = v.Max() < 12; // Ace is 12
                    if (canLow && canHigh) outside = outside ?? combo;
                    else                   inside  = inside  ?? combo;
                }
                else if (span == 4 && v.Distinct().Count() == 4)
                    inside = inside ?? combo;   // gutshot
            }
            return (outside, inside);
        }

        /// <summary>Generic k-combination generator.</summary>
        private static List<List<VPCard>> VPCombos(List<VPCard> arr, int k)
        {
            var res = new List<List<VPCard>>();
            void Rec(int start, List<VPCard> cur)
            {
                if (cur.Count == k) { res.Add(new List<VPCard>(cur)); return; }
                for (int i = start; i < arr.Count; i++)
                {
                    cur.Add(arr[i]);
                    Rec(i + 1, cur);
                    cur.RemoveAt(cur.Count - 1);
                }
            }
            Rec(0, new List<VPCard>());
            return res;
        }

        // ═══════════════════════════════════════════════════════════
        //  INTERNAL HELPERS
        // ═══════════════════════════════════════════════════════════
        /// <summary>
        /// Logs an error message (prefix already included, e.g. "[ERROR] …").
        /// Enforces a minimum 2-second gap between consecutive error log entries
        /// so the log panel is not flooded during rapid failures.
        /// Thread-safe: safe to call from both the main loop and fast-mode continuations.
        /// </summary>
        private void LogError(string message)
        {
            lock (_errorLogLock)
            {
                var now = DateTime.UtcNow;
                if ((now - _lastErrorLogTime).TotalMilliseconds < 2000)
                    return;                 // too soon — swallow this duplicate

                _lastErrorLogTime = now;
            }
            _log?.Invoke(message);
        }

        private async Task<JObject> Post(string path, object body, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            return await _post(path, body, ct);
        }

        /// <summary>True when the error is a parallelCasinoBet collision (not an internalError) — safe to skip and retry.</summary>
        private static bool IsParallelBetError(BetResult r) =>
            r.ErrorType != null
            && r.ErrorType.IndexOf("parallelCasinoBet", StringComparison.OrdinalIgnoreCase) >= 0
            && r.ErrorType.IndexOf("internalError",     StringComparison.OrdinalIgnoreCase) < 0;

        /// <summary>True when the exception is a transport/network failure rather than an HTTP API error response.</summary>
        private static bool IsNetworkError(string msg) =>
            msg != null && !msg.StartsWith("HTTP ", StringComparison.OrdinalIgnoreCase);

        private static bool IsRateLimitError(string msg) =>
            msg != null
            && (msg.IndexOf("slow down", StringComparison.OrdinalIgnoreCase) >= 0 || msg.Contains("429"));

        /// <summary>
        /// True when the API error is a seed-rotation notice.
        /// Stake emits this transiently when the server rotates the provably-fair
        /// seed; the bet itself did not go through, so we can safely retry.
        /// </summary>
        private static bool IsSeedRotateError(BetResult r) =>
            r != null && (IsSeedRotateError(r.ErrorMessage) || IsSeedRotateError(r.ErrorType));

        private static bool IsSeedRotateError(string msg) =>
            msg != null
            && (   msg.IndexOf("new seed pair",     StringComparison.OrdinalIgnoreCase) >= 0
                || msg.IndexOf("choose a new seed", StringComparison.OrdinalIgnoreCase) >= 0
                || msg.IndexOf("seedRotate",        StringComparison.OrdinalIgnoreCase) >= 0
                || (msg.IndexOf("seed",  StringComparison.OrdinalIgnoreCase) >= 0
                 && msg.IndexOf("rotat", StringComparison.OrdinalIgnoreCase) >= 0));

        /// <summary>
        /// True when the API rejected a VideoPoker /bet because an existing
        /// game is still open and must be finished before a new one can start.
        /// </summary>
        private static bool IsActiveVideoPokerError(string msg) =>
            msg != null
            && msg.IndexOf("active", StringComparison.OrdinalIgnoreCase) >= 0
            && (   msg.IndexOf("VideoPoker",  StringComparison.OrdinalIgnoreCase) >= 0
                || msg.IndexOf("video-poker", StringComparison.OrdinalIgnoreCase) >= 0
                || msg.IndexOf("video poker", StringComparison.OrdinalIgnoreCase) >= 0);

        private static string RandId()
        {
            const string c = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var rng = new Random();
            return new string(Enumerable.Range(0, 21).Select(_ => c[rng.Next(c.Length)]).ToArray());
        }

        private static int[] ParseIntList(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return new int[0];
            return s.Split(',').Select(x => x.Trim()).Where(x => int.TryParse(x, out _)).Select(int.Parse).ToArray();
        }

        /// <summary>
        /// Parses a comma-separated color string into a HashSet for order-independent comparison.
        /// </summary>
        private static HashSet<string> ParseColorSet(string colors)
        {
            if (string.IsNullOrWhiteSpace(colors)) return new HashSet<string>();
            return new HashSet<string>(
                colors.ToLower().Split(',')
                      .Select(c => c.Trim())
                      .Where(c => !string.IsNullOrEmpty(c)));
        }

        /// <summary>
        /// Ordered prefix match: every selected color must appear at the same position
        /// in the result hand, in the exact order they were selected.
        /// e.g. selected="red,blue,green" (3 colors) → result hand positions 0,1,2 must be red,blue,green.
        /// If 5 colors are selected all 5 result positions must match in order.
        /// </summary>
        private static bool DiamondPatternMatches(string selected, List<string> resultColors)
        {
            if (string.IsNullOrWhiteSpace(selected) || resultColors == null || resultColors.Count == 0)
                return false;

            string[] sel = selected.ToLower().Split(',')
                                   .Select(s => s.Trim()).Where(s => s.Length > 0).ToArray();

            if (sel.Length == 0 || resultColors.Count < sel.Length) return false;

            for (int i = 0; i < sel.Length; i++)
                if (sel[i] != resultColors[i]) return false;

            return true;
        }

        private static string[] SafeArray(string json)
        {
            try { return JsonConvert.DeserializeObject<string[]>(json); } catch { return null; }
        }
    }
}
