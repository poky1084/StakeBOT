using System;
using System.Collections.Generic;

namespace StakeBotUI
{
    [Serializable]
    public class BotSettings
    {
        public string ApiKey           { get; set; } = "";
        public string AuthMethod       { get; set; } = "Cookie";
        public string SelectedCurrency { get; set; } = "";
        public string Mirror           { get; set; } = "stake.com";
        public string ClearanceCookie  { get; set; } = "";
        public string UserAgent        { get; set; } = "";

        // ─── Core ────────────────────────────────────────────────────────
        public decimal BaseBet         { get; set; } = 0.00000001m;
        public string  SelectedGame    { get; set; } = "dice";

        // ─── Speed / timing ──────────────────────────────────────────────
        /// <summary>When true the engine fires two consecutive bets per loop iteration.</summary>
        public bool FastMode           { get; set; } = false;
        /// <summary>Milliseconds to wait at the end of each loop iteration (ignored in fast-mode OFF — set to 0).</summary>
        public int  BetDelayMs         { get; set; } = 100;

        // ─── Global stop conditions ───────────────────────────────────────
        /// <summary>Stop when any bet's payoutMultiplier >= this value.</summary>
        public bool    StopOnMultiplier      { get; set; } = false;
        public decimal StopMultiplierValue   { get; set; } = 500m;

        /// <summary>Dice only: stop when the rolled number exactly matches this value.</summary>
        public bool    StopOnDiceResult      { get; set; } = false;
        public decimal StopDiceResultValue   { get; set; } = 0m;

        /// <summary>Diamonds only: stop when the 5-color pattern you set results in a win.</summary>
        public bool    StopOnDiamondsWin     { get; set; } = false;

        // ─── Per-game condition blocks ──────────────────────────────────────────
        public Dictionary<string, List<ConditionBlockData>> GameConditions { get; set; }
            = new Dictionary<string, List<ConditionBlockData>>();

        // Helper – always returns a list (never null) for a given game key
        public List<ConditionBlockData> GetConditions(string game)
        {
            if (!GameConditions.ContainsKey(game))
                GameConditions[game] = new List<ConditionBlockData>();
            return GameConditions[game];
        }
        public Dictionary<string, Dictionary<string, List<ConditionBlockData>>> NamedStrategies { get; set; }
            = new Dictionary<string, Dictionary<string, List<ConditionBlockData>>>();

        /// <summary>Tracks which strategy name was last active per game.</summary>
        public Dictionary<string, string> ActiveStrategyName { get; set; }
            = new Dictionary<string, string>();

        // Helper – always returns a dict (never null) for a given game key
        public Dictionary<string, List<ConditionBlockData>> GetNamedStrategies(string game)
        {
            if (!NamedStrategies.ContainsKey(game))
                NamedStrategies[game] = new Dictionary<string, List<ConditionBlockData>>();
            return NamedStrategies[game];
        }

        // ─── DICE ─────────────────────────────────────────────────────────
        public bool    DiceBetHigh     { get; set; } = false;
        public decimal DiceChance      { get; set; } = 49.5m;

        // ─── LIMBO ────────────────────────────────────────────────────────
        public decimal LimboTarget     { get; set; } = 2m;

        // ─── HILO ─────────────────────────────────────────────────────────
        public string  HiloPattern       { get; set; } = "5,5,5";
        public string  HiloStartCardRank { get; set; } = "A";
        /// <summary>Card suit: C H D S (empty = server picks)</summary>
        public string  HiloStartCardSuit { get; set; } = "C";
        /// <summary>None | AllSameColor | AllSameSuit — cashout early if cards deviate.</summary>
        public string  HiloSuitMode      { get; set; } = "None";

        // ─── MINES ────────────────────────────────────────────────────────
        public int     MinesMines      { get; set; } = 1;
        public string  MinesFields     { get; set; } = "1,2,3";

        // ─── KENO ─────────────────────────────────────────────────────────
        public string  KenoNumbers     { get; set; } = "1,2,3,4,5";
        public string  KenoRisk        { get; set; } = "low";

        // ─── PLINKO ───────────────────────────────────────────────────────
        public int     PlinkoRows      { get; set; } = 8;
        public string  PlinkoRisk      { get; set; } = "low";

        // ─── WHEEL ────────────────────────────────────────────────────────
        public int     WheelSegments   { get; set; } = 10;
        public string  WheelRisk       { get; set; } = "low";

        // ─── ROULETTE ─────────────────────────────────────────────────────
        public string  RouletteChips   { get; set; } = "[{\"value\":\"colorBlack\",\"amount\":0.0001}]";

        // ─── BACCARAT ─────────────────────────────────────────────────────
        public decimal BaccaratBanker  { get; set; } = 0;
        public decimal BaccaratPlayer  { get; set; } = 0;
        public decimal BaccaratTie     { get; set; } = 0;

        // ─── PUMP ─────────────────────────────────────────────────────────
        public int     PumpPumps       { get; set; } = 1;
        public string  PumpDifficulty  { get; set; } = "easy";

        // ─── DRAGON TOWER ─────────────────────────────────────────────────
        public string  DragonDifficulty{ get; set; } = "easy";
        public string  DragonEggs      { get; set; } = "0";

        // ─── BARS ─────────────────────────────────────────────────────────
        public string  BarsDifficulty  { get; set; } = "easy";
        public string  BarsTiles       { get; set; } = "2";

        // ─── SLOTS (Tome / Scarab) ────────────────────────────────────────
        public int     SlotLines       { get; set; } = 1;

        // ─── DIAMONDS ─────────────────────────────────────────────────────
        public string  DiamondsColors  { get; set; } = "";

        // ─── CASES ────────────────────────────────────────────────────────
        public string  CasesDifficulty { get; set; } = "easy";

        // ─── RPS / FLIP ───────────────────────────────────────────────────
        public string  RpsGuesses      { get; set; } = "[\"rock\"]";
        public string  FlipGuesses     { get; set; } = "[\"heads\"]";

        // ─── SNAKES ───────────────────────────────────────────────────────
        public string  SnakesDifficulty{ get; set; } = "easy";
        public int     SnakesRolls     { get; set; } = 1;

        // ─── CHICKEN ──────────────────────────────────────────────────────
        public string  ChickenDifficulty { get; set; } = "easy";
        public int     ChickenSteps    { get; set; } = 1;

        // ─── TAROT ────────────────────────────────────────────────────────
        public string  TarotDifficulty { get; set; } = "easy";

        // ─── DARTS ────────────────────────────────────────────────────────
        public string  DartsDifficulty { get; set; } = "easy";

        // ─── DRILL ────────────────────────────────────────────────────────
        public decimal DrillTarget     { get; set; } = 1.01m;
        public int     DrillPick       { get; set; } = 1;

        // ─── PRIMEDICE ────────────────────────────────────────────────────
        public decimal PDTarget1       { get; set; } = 2m;
        public decimal PDTarget2       { get; set; } = 24m;
        public decimal PDTarget3       { get; set; } = 34m;
        public decimal PDTarget4       { get; set; } = 68m;
        public string  PDCondition     { get; set; } = "rollBetweenTwo";
        /// <summary>Stop when the Primedice X roll result equals this value (2 dp). 0 = disabled.</summary>
        public decimal PDStopRoll1     { get; set; } = 0m;
        /// <summary>Stop when the Primedice X roll result equals this value (2 dp). 0 = disabled.</summary>
        public decimal PDStopRoll2     { get; set; } = 0m;
        public bool    PDStopOnRoll1   { get; set; } = false;
        public bool    PDStopOnRoll2   { get; set; } = false;

        // ─── MOLES ────────────────────────────────────────────────────────
        public int     MolesMoles      { get; set; } = 3;
        public string  MolesPicks      { get; set; } = "0";

   
        public string  VideoPokerStrategy { get; set; } = "optimal";

    }
}
