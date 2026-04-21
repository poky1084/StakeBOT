using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StakeBotUI
{
    public partial class Form1 : Form
    {
        // ─── Settings ───────────────────────────────────────────────
        private BotSettings _s    = new BotSettings();
        private const string SETTINGS_FILE = "bot_settings.json";
        private System.Windows.Forms.Timer _saveTimer;
        private System.Threading.Timer _balanceTimer;       // 10-second live balance refresh (thread-pool, not UI-thread)
        private decimal _sessionStartBalance = 0m;          // balance snapshot taken when bot STARTs

        // ─── Auth ───────────────────────────────────────────────────
        private CookieContainer cc = new CookieContainer();
        private string ClearanceCookie = "";
        private string UserAgent       = "";
        public  string StakeSite       = "stake.com";
        private Dictionary<string, decimal> _currencyBalances = new Dictionary<string, decimal>();

        // ─── Game panels map ────────────────────────────────────────
        private Dictionary<string, Panel> _gamePanels;
        private string _currentGame = "dice";
        private List<string> _selectedDiamondColors = new List<string>();

        // ─── HiLo card-strip state ──────────────────────────────────
        private List<string> _hiloList        = new List<string>();
        private string       _hiloRowStr       = "Start,";
        private bool         _hiloManualActive  = false;
        private volatile bool _hiloNextInFlight  = false;
        private string       _hiloActiveBetId   = null;

        // ─── Bot ────────────────────────────────────────────────────
        private BotEngine _engine = new BotEngine();
        private CancellationTokenSource _botCts;
        private int _histRow;
        private TopMultipliersForm _topMultiForm;    // top-10 multiplier tracker window
        public static bool isManual = false;
        private readonly Queue<DateTime> _betTimes = new Queue<DateTime>();
        // ─── lvHistory scroll-lock ───────────────────────────────────────
        private bool            _lvHistoryAutoScroll      = true;   // false = user has scrolled away from top
        private bool            _lvHistoryRestoringScroll = false;  // re-entrancy guard
        private LvScrollWatcher _lvScrollWatcher;
        private PlayButton _playBtn;   // green triangle replacing btnStart
        private StopButton _stopBtn;   // red square replacing btnStop
        PrivateFontCollection pfc = new PrivateFontCollection();
        private ImageList _gemImageList;
        public Form1()
        {
            InitializeComponent();
            pfc.AddFontFile("montserrat.semibold.ttf");
            Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            foreach (Control control in this.Controls)
            {
                if (control is Label || control is CheckBox || control is Button || control is ComboBox || control is ListView)
                {
                    control.Font = new Font(pfc.Families[0], 9, FontStyle.Regular);
                }
            }
            var appFont = new Font(pfc.Families[0], 9, FontStyle.Regular);
            //this.Font = appFont;                        // sets the form itself
            ApplyFontRecursive(this, appFont);
            // Do not execute runtime setup when opened in the VS designer
            if (System.ComponentModel.LicenseManager.UsageMode
                == System.ComponentModel.LicenseUsageMode.Designtime) return;
            InitializeGemImages();
            BuildGamePanelMap();
            PopulateGameCombo();

            lvHistory.SetDoubleBuffered(true);
            _lvScrollWatcher = new LvScrollWatcher(lvHistory, () =>
            {
                if (_lvHistoryRestoringScroll) return;           // ignore our own programmatic scrolls
                var top = lvHistory.TopItem;
                bool wasAuto = _lvHistoryAutoScroll;
                _lvHistoryAutoScroll = (top == null || top.Index == 0);

                // The moment auto-scroll re-enables, snap straight to row 0
                // so new items start appearing immediately instead of drifting further away.
                if (!wasAuto && _lvHistoryAutoScroll && lvHistory.Items.Count > 0)
                {
                    _lvHistoryRestoringScroll = true;
                    try   { lvHistory.TopItem = lvHistory.Items[0]; }
                    finally { _lvHistoryRestoringScroll = false; }
                }
            });

            lvHiloCards.SetDoubleBuffered(true);
            lvHiloCards.OwnerDraw = true;
            lvHiloCards.DrawColumnHeader += LvHiloCards_DrawColumnHeader;
            lvHiloCards.DrawItem         += LvHiloCards_DrawItem;
            lvHiloCards.DrawSubItem      += LvHiloCards_DrawSubItem;

            _saveTimer = new System.Windows.Forms.Timer { Interval = 400 };
            _saveTimer.Tick += (s, e) => { _saveTimer.Stop(); SaveSettings(); };

            // Balance check every 10 s on the thread pool — independent of the UI message pump
            // so it fires reliably even when the bot floods the UI thread with BeginInvoke calls.
            // RefreshBalanceAsync handles InvokeRequired internally.
            _balanceTimer = new System.Threading.Timer(
                async _ => { try { await RefreshBalanceAsync(); } catch { } },
                null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

            BrowserFetch.StartServer();
            BrowserFetch.Connected    += OnWsConnected;
            BrowserFetch.Disconnected += OnWsDisconnected;

            // ── PlayButton: same size & position as btnStart (which is hidden) ──
            // The triangle is drawn centered inside the button bounds, no text shown.
            _playBtn = new PlayButton
            {
                Size      = btnStart.Size,        // 110 × 28 — same footprint
                Location  = btnStart.Location,    // 584, 68  — same spot
                BackColor = Color.Transparent
            };
            _playBtn.Click += BtnStart_Click;
            // Mirror btnStart.Enabled so the icon dims when the bot is running
            btnStart.EnabledChanged += (s, e) => _playBtn.Enabled = btnStart.Enabled;
            this.Controls.Add(_playBtn);
            _playBtn.BringToFront();

            // ── StopButton: same size & position as btnStop (which is hidden) ──
            _stopBtn = new StopButton
            {
                Size      = btnStop.Size,
                Location  = btnStop.Location,
                BackColor = Color.Transparent,
                Enabled   = false            // starts disabled, just like btnStop
            };
            _stopBtn.Click += BtnStop_Click;
            btnStop.EnabledChanged += (s, e) => _stopBtn.Enabled = btnStop.Enabled;
            this.Controls.Add(_stopBtn);
            _stopBtn.BringToFront();
            this.FormClosing += Form1_FormClosing;
            this.Load += async (s, e) =>
            {
                LoadSettings();

                // Create the top-multiplier tracker immediately so it accumulates
                // entries from the very first bet, even before the user opens the window.
                _topMultiForm = new TopMultipliersForm(
                    fetchIid: FetchBetIid,
                    getSite:  () => StakeSite);
                _topMultiForm.Location = new System.Drawing.Point(this.Right + 8, this.Top);

                // Auto-login on startup if a cookie is already saved or extension is already connected
                bool hasCookie    = !string.IsNullOrWhiteSpace(ClearanceCookie);
                bool extConnected = cmbFetchMode.SelectedIndex == 1 && BrowserFetch.IsConnected;
                if ((hasCookie || extConnected) && !string.IsNullOrWhiteSpace(txtApiKey.Text.Trim()))
                    await DoLoginAsync();
            };
        }
        private void ApplyFontRecursive(Control parent, Font font)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (!(ctrl is ListView)) {
                    ctrl.Font = font;
                }

                if (ctrl.Controls.Count > 0)
                    ApplyFontRecursive(ctrl, font);
            }
        }
        private void InitializeGemImages()
        {
            _gemImageList = new ImageList();
            _gemImageList.ImageSize = new Size(12, 12); // Changed from 20x20 to 14x14
            _gemImageList.ColorDepth = ColorDepth.Depth32Bit;

            // Get the path to the img folder
            string imgPath = Path.Combine(Application.StartupPath, "img");

            var colorMap = new Dictionary<string, string>
    {
        { "red", "red.png" },
        { "green", "green.png" },
        { "yellow", "yellow.png" },
        { "blue", "blue.png" },
        { "cyan", "cyan.png" },
        { "orange", "orange.png" },
        { "purple", "purple.png" }
    };

            foreach (var color in colorMap)
            {
                string fullPath = Path.Combine(imgPath, color.Value);
                if (File.Exists(fullPath))
                {
                    try
                    {
                        using (var img = Image.FromFile(fullPath))
                        {
                            // Resize the image to fit the ImageList size
                            var resizedImg = new Bitmap(img, _gemImageList.ImageSize);
                            _gemImageList.Images.Add(color.Key, resizedImg);
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendLog($"[ERROR] Failed to load gem image {color.Value}: {ex.Message}");
                    }
                }
                else
                {
                    AppendLog($"[WARNING] Gem image not found: {fullPath}");
                }
            }
        }
        // ═══════════════════════════════════════════════════════════
        //  GAME PANEL MAP + SWITCHING
        // ═══════════════════════════════════════════════════════════
        private void BuildGamePanelMap()
        {
            _gamePanels = new Dictionary<string, Panel>
            {
                ["dice"]        = panelDice,
                ["limbo"]       = panelLimbo,
                ["hilo"]        = panelHilo,
                ["mines"]       = panelMines,
                ["keno"]        = panelKeno,
                ["plinko"]      = panelPlinko,
                ["wheel"]       = panelWheel,
                ["baccarat"]    = panelBaccarat,
                ["roulette"]    = panelRoulette,
                ["pump"]        = panelPump,
                ["dragontower"] = panelDragonTower,
                ["bars"]        = panelBars,
                ["tomeoflife"]  = panelTomeOfLife,
                ["scarabspin"]  = panelScarabSpin,
                ["diamonds"]    = panelDiamonds,
                ["cases"]       = panelCases,
                ["rps"]         = panelRps,
                ["flip"]        = panelFlip,
                ["snakes"]      = panelSnakes,
                ["darts"]       = panelDarts,
                ["packs"]       = panelPacks,
                ["moles"]       = panelMoles,
                ["chicken"]     = panelChicken,
                ["tarot"]       = panelTarot,
                ["drill"]       = panelDrill,
                ["primedice"]   = panelPrimedice,
                ["bluesamurai"] = panelBlueSamurai,
                ["videopoker"]  = panelVideoPoker,
                ["blackjack"]   = panelBlackjack,
            };
        }
     

        // Only one panel lives in pnlGameContainer at a time — eliminates greyed-out issue
        private void ShowGame(string key)
        {
            _currentGame = key;
            if (!_gamePanels.TryGetValue(key, out var panel)) return;
            pnlGameContainer.SuspendLayout();
            pnlGameContainer.Controls.Clear();
            panel.Visible = true;
            panel.Enabled = true;
            pnlGameContainer.Controls.Add(panel);
            pnlGameContainer.ResumeLayout(true);
            pnlGameContainer.Refresh();

            // Show the HiLo card-strip panel above lvHistory only when Hilo is selected
            bool isHilo = key == "hilo";
            pnlHiloCards.Visible = isHilo;
            if (isHilo)
            {
                // Shift lvHistory down to make room for the 123-px card strip (+ 5 px gap)
                lvHistory.Location = new System.Drawing.Point(277, 185);
                lvHistory.Size     = new System.Drawing.Size(546, 190);
            }
            else
            {
                // Restore lvHistory to its original full-height position
                lvHistory.Location = new System.Drawing.Point(277, 85);
                lvHistory.Size     = new System.Drawing.Size(546, 290);
            }

            UpdateDiceInfo();
        }

        private void PopulateGameCombo()
        {
            foreach (var (k, d) in new (string, string)[]
            {
                ("dice","Dice"),("limbo","Limbo"),("hilo","Hilo"),("mines","Mines"),("keno","Keno"),
                ("plinko","Plinko"),("wheel","Wheel"),("baccarat","Baccarat"),("roulette","Roulette"),
                ("pump","Pump"),("dragontower","Dragon Tower"),("bars","Bars"),
                ("tomeoflife","Tome of Life"),("scarabspin","Scarab Spin"),("diamonds","Diamonds"),
                ("cases","Cases"),("rps","Rock Paper Scissors"),("flip","Coin Flip"),
                ("snakes","Snakes"),("darts","Darts"),("packs","Packs"),("moles","Moles"),
                ("chicken","Chicken"),("tarot","Tarot"),("drill","Drill"),("primedice","Primedice X"),
                ("bluesamurai","Blue Samurai"),("videopoker","Video Poker"),("blackjack","Blackjack"),
            })
            cmbGame.Items.Add(new GameItem(k, d));
        }

        // ═══════════════════════════════════════════════════════════
        //  EVENT HANDLERS wired in Designer.cs
        // ═══════════════════════════════════════════════════════════
        private void CmbGame_Changed(object sender, EventArgs e)
        {
            if (cmbGame.SelectedItem is GameItem gi) ShowGame(gi.Key);
            QueueSave(sender, e);
        }

        private void txtApiKey_TextChanged(object sender, EventArgs e) => QueueSave(sender, e);
        private void txtMirror_TextChanged(object sender, EventArgs e)
        {
            StakeSite = txtMirror.Text.Trim().ToLower();
            QueueSave(sender, e);
        }

        private void nudDiceChance_Changed(object sender, EventArgs e) { UpdateDiceInfo(); QueueSave(sender, e); }
        private void rbDice_Changed(object sender, EventArgs e)        { UpdateDiceInfo(); QueueSave(sender, e); }

        private void UpdateDiceInfo()
        {
            if (nudDiceChance == null || lblDiceInfo == null) return;
            decimal ch = nudDiceChance.Value;
            decimal m  = ch > 0 ? Math.Round(99m / ch, 4) : 0m;
            lblDiceInfo.Text = $"Multi: {m:F4}x  |  Target: {(rbDiceOver.Checked ? 100m - ch : ch):F2}";
        }

        // Diamond buttons
        private void DiamondBtn_Click(object sender, EventArgs e)
        {
            if (_selectedDiamondColors.Count >= 5) return;
            _selectedDiamondColors.Add(((Button)sender).Tag.ToString());
            txtDiamondColors.Text  = string.Join(",", _selectedDiamondColors);
            lblDiamondCount.Text   = $"{_selectedDiamondColors.Count} / 5 selected";
            QueueSave(sender, e);
        }
        private void BtnHiloClear_Click(object sender, EventArgs e)
        {
            txtHiloPattern.Clear();
            QueueSave(sender, e);
        }

        // ═══════════════════════════════════════════════════════════
        //  HILO CARD-STRIP  (listView1 / panel2 from HiLo project)
        // ═══════════════════════════════════════════════════════════

        /// <summary>Maps a rank + suit code to a single card glyph string, e.g. "A♡".</summary>
        private string CardLayout(string rank, string suit)
        {
            switch (suit)
            {
                case "H": return rank + "\u2661";   // ♡
                case "C": return rank + "\u2663";   // ♣
                case "D": return rank + "\u2662";   // ♢
                case "S": return rank + "\u2660";   // ♠
                default:  return rank;
            }
        }

        /// <summary>Returns the display colour for a card suit (red for hearts/diamonds).</summary>
        private Color CardSuitColor(string suit)
        {
            return (suit == "H" || suit == "D") ? Color.Red : Color.Black;
        }

        // ── lvHiloCards owner-draw ───────────────────────────────────────────
        private void LvHiloCards_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawBackground();
            // Colour hearts ♡ and diamonds ♢ red; clubs/spades near-black
            bool isRed = e.Header.Text.IndexOf('\u2661') >= 0
                      || e.Header.Text.IndexOf('\u2662') >= 0;
            Color textColor = isRed ? Color.Crimson : Color.FromArgb(30, 30, 30);

            using (var brush = new SolidBrush(textColor))
            using (var sf = new StringFormat
            {
                Alignment     = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                Trimming      = StringTrimming.None,
                FormatFlags   = StringFormatFlags.NoWrap
            })
            {
                e.Graphics.TextRenderingHint =
                    System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                e.Graphics.DrawString(e.Header.Text, e.Font, brush, e.Bounds, sf);
            }
        }

        private void LvHiloCards_DrawItem(object sender, DrawListViewItemEventArgs e)
            => e.DrawDefault = true;

        private void LvHiloCards_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
            => e.DrawDefault = true;

        // ── Card-strip update helpers ────────────────────────────────────────

        /// <summary>
        /// Call when a HiloBet response arrives.
        /// Adds the starting card column and "Start" row to the card strip.
        /// </summary>
        public void AddHiloStartCard(string rank, string suit)
        {
            if (lvHiloCards.InvokeRequired)
            {
                lvHiloCards.Invoke(new Action(() => AddHiloStartCard(rank, suit)));
                return;
            }
            _hiloList.Add(rank);
            string startCard = CardLayout(rank, suit);
            var hdr = new ColumnHeader { Text = startCard };
            lvHiloCards.Columns.Add(hdr);
            lvHiloCards.Columns[0].Width = 80;
            lvHiloCards.Width += 80;
            var lvi = new ListViewItem("Start") { Font = new Font("Consolas", 12f) };
            lvHiloCards.Items.Insert(0, lvi);
        }

        /// <summary>
        /// Call when a HiloNext response arrives.
        /// Adds the next card column and updates the running multiplier row.
        /// </summary>
        public void AddHiloCard(string rank, string suit, double payoutMultiplier)
        {
            if (lvHiloCards.InvokeRequired)
            {
                lvHiloCards.Invoke(new Action(() => AddHiloCard(rank, suit, payoutMultiplier)));
                return;
            }
            string multi = payoutMultiplier >= 1000
                ? payoutMultiplier.ToString("#") + "x"
                : payoutMultiplier.ToString("0.##").Replace(",", ".") + "x";

            _hiloList.Add(rank);
            string card = CardLayout(rank, suit);

            var hdr = new ColumnHeader { Text = card };
            lvHiloCards.Columns.Add(hdr);
            int colIdx = lvHiloCards.Columns.Count - 1;
            lvHiloCards.Columns[colIdx].Width = 80;
            lvHiloCards.Width += 80;

            _hiloRowStr += multi + ",";
            string[] cells = _hiloRowStr.Split(',');
            var lvi = new ListViewItem(cells) { Font = new Font("Consolas", 12f) };
            lvHiloCards.Items.Insert(0, lvi);
            if (lvHiloCards.Items.Count > 1)
                lvHiloCards.Items[1].Remove();

            // Auto-scroll the panel so the latest card is always visible
            pnlHiloCards.AutoScrollPosition = new Point(lvHiloCards.Width, 0);
        }

        /// <summary>
        /// Resets the card strip at the start of each new Hilo round.
        /// Call this before placing a new HiloBet.
        /// </summary>
        public void ClearHiloCards()
        {
            if (lvHiloCards.InvokeRequired)
            {
                lvHiloCards.Invoke(new Action(ClearHiloCards));
                return;
            }
            _hiloList.Clear();
            _hiloRowStr    = "Start,";
            lvHiloCards.Width = 13;
            lvHiloCards.Columns.Clear();
            lvHiloCards.Items.Clear();
        }

        private void BtnDiamondsClear_Click(object sender, EventArgs e)
        {
            _selectedDiamondColors.Clear();
            txtDiamondColors.Text = "";
            lblDiamondCount.Text  = "0 / 5 selected";
            QueueSave(sender, e);
        }

        // ═══════════════════════════════════════════════════════════
        //  HISTORY LISTVIEW
        // ═══════════════════════════════════════════════════════════
        // ═══════════════════════════════════════════════════════════
        //  HISTORY LISTVIEW  (column 0 = "View" / BetId link)
        //  Column order: View(0) | #(1) | Game(2) | Bet(3) | Multi(4)
        //                Payout(5) | Profit(6) | Roll(7) | Info(8)
        // ═══════════════════════════════════════════════════════════
        private const int COL_BETID = 0; // View column index

        private readonly Dictionary<ListViewItem, string> _resolvedBetIids =
            new Dictionary<ListViewItem, string>();

        // ── Owner-draw: column header ──────────────────────────────
        private void LvHistory_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawBackground();
            using (var brush = new SolidBrush(SystemColors.ControlText))
            using (var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap })
            {
                var r = new Rectangle(e.Bounds.X + 4, e.Bounds.Y, e.Bounds.Width - 4, e.Bounds.Height);
                e.Graphics.DrawString(e.Header.Text, e.Font, brush, r, sf);
            }
        }

        // ── Owner-draw: row background ─────────────────────────────
        private void LvHistory_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // sub-item drawing handles everything
        }

        // ── Owner-draw: sub-items — View column is blue link ───────
        private void LvHistory_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // Row background preserves win/loss colour
            using (var bg = new SolidBrush(e.Item.BackColor))
                e.Graphics.FillRectangle(bg, e.Bounds);

            if (e.ColumnIndex == COL_BETID)
            {
                bool resolved = _resolvedBetIids.TryGetValue(e.Item, out string iid);
                string cellText = resolved ? iid : "View";
                Color linkColor = resolved ? Color.Black : Color.Blue;
                using (var brush = new SolidBrush(linkColor))
                {
                    var fmt = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString(cellText, e.SubItem.Font, brush, e.Bounds, fmt);
                }
            }
            else
            {
                // Special handling for Roll column (index 7) for diamonds game
                bool isDiamondsRoll = (e.ColumnIndex == 7) && // Roll column index
                                      e.Item.SubItems.Count > 2 &&
                                      e.Item.SubItems[2]?.Text == "diamonds" &&
                                      e.Item.Tag is Tuple<string, List<string>> tuple &&
                                      tuple.Item2 != null &&
                                      tuple.Item2.Count > 0;

                if (isDiamondsRoll && _gemImageList != null)
                {
                    // Draw gem images
                    var colors = ((Tuple<string, List<string>>)e.Item.Tag).Item2;
                    DrawGemImages(e.Graphics, e.Bounds, colors);
                }
                else
                {
                    // Normal text drawing
                    Color fg = e.ColumnIndex == 6   // Profit column gets green/red
                               ? (e.SubItem.ForeColor != Color.Empty ? e.SubItem.ForeColor : e.Item.ForeColor)
                               : e.Item.ForeColor;
                    using (var brush = new SolidBrush(fg))
                    {
                        var fmt = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit,
                            Trimming = StringTrimming.EllipsisCharacter
                        };
                        var rect = new Rectangle(e.Bounds.X + 2, e.Bounds.Y, e.Bounds.Width - 2, e.Bounds.Height);
                        e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, brush, rect, fmt);
                    }
                }
            }
        }
        private void DrawGemImages(Graphics g, Rectangle bounds, List<string> colors)
        {
            if (colors == null || colors.Count == 0 || _gemImageList == null) return;

            // Even smaller images
            int imageSize = 10; // 10x10 pixels
            int startX = bounds.X + 2;
            int startY = bounds.Y + (bounds.Height - imageSize) / 2;
            int spacing = 1;

            // Calculate total width needed
            int totalWidth = (colors.Count * imageSize) + ((colors.Count - 1) * spacing);

            // Center the images in the column if they fit
            if (totalWidth < bounds.Width - 4)
            {
                startX = bounds.X + (bounds.Width - totalWidth) / 2;
            }

            for (int i = 0; i < Math.Min(colors.Count, 5); i++)
            {
                string color = colors[i];
                if (_gemImageList.Images.ContainsKey(color))
                {
                    try
                    {
                        var destRect = new Rectangle(
                            startX + (i * (imageSize + spacing)),
                            startY,
                            imageSize,
                            imageSize);

                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(_gemImageList.Images[color], destRect);
                    }
                    catch { }
                }
            }
        }
        // ── Helper: extract betId from item Tag (handles plain string and diamonds Tuple) ──
        private static string GetBetId(ListViewItem item)
        {
            if (item.Tag is string s) return s;
            if (item.Tag is Tuple<string, List<string>> t) return t.Item1;
            return null;
        }

        // ── Helper: determine which column was clicked ──────────────
        private int GetClickedColumn(ListViewHitTestInfo info, int x)
        {
            if (info.SubItem != null)
                return info.Item.SubItems.IndexOf(info.SubItem);
            int accum = 0;
            for (int i = 0; i < lvHistory.Columns.Count; i++)
            {
                accum += lvHistory.Columns[i].Width;
                if (x < accum) return i;
            }
            return -1;
        }

        // ── Left-click column 0 → resolve and reveal IID ────────────
        private async void LvHistory_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var info = lvHistory.HitTest(e.X, e.Y);
            if (info.Item == null) return;
            if (GetClickedColumn(info, e.X) != COL_BETID) return;

            string betId = GetBetId(info.Item);
            if (string.IsNullOrEmpty(betId)) return;

            // Already resolved — nothing to do
            if (_resolvedBetIids.ContainsKey(info.Item)) return;

            lvHistory.Cursor = Cursors.WaitCursor;
            string iid = null;
            try   { iid = await FetchBetIid(betId); }
            catch { }
            finally { lvHistory.Cursor = Cursors.Default; }

            if (string.IsNullOrEmpty(iid)) return;

            string cleanIid = iid.Replace("house:", "casino:");
            _resolvedBetIids[info.Item] = cleanIid;

            int needed = TextRenderer.MeasureText(cleanIid, lvHistory.Font).Width + 10;
            if (needed > lvHistory.Columns[COL_BETID].Width)
                lvHistory.Columns[COL_BETID].Width = Math.Min(needed, 250);

            lvHistory.Invalidate();
        }

        // ── Right-click anywhere on a row → context menu ────────────
        private async void LvHistory_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            var info = lvHistory.HitTest(e.X, e.Y);
            if (info.Item == null) return;

            string betId = GetBetId(info.Item);
            if (string.IsNullOrEmpty(betId)) return;

            // Resolve IID on demand if not yet done
            if (!_resolvedBetIids.ContainsKey(info.Item))
            {
                lvHistory.Cursor = Cursors.WaitCursor;
                string iid = null;
                try   { iid = await FetchBetIid(betId); }
                catch { }
                finally { lvHistory.Cursor = Cursors.Default; }

                if (!string.IsNullOrEmpty(iid))
                {
                    string ci = iid.Replace("house:", "casino:");
                    _resolvedBetIids[info.Item] = ci;
                    int needed = TextRenderer.MeasureText(ci, lvHistory.Font).Width + 10;
                    if (needed > lvHistory.Columns[COL_BETID].Width)
                        lvHistory.Columns[COL_BETID].Width = Math.Min(needed, 250);
                    lvHistory.Invalidate();
                }
            }

            string cleanIid = _resolvedBetIids.TryGetValue(info.Item, out string resolved) ? resolved : null;
            string copyText  = cleanIid ?? betId;
            string copyLabel = cleanIid != null ? $"📋  Copy IID:  {cleanIid}" : $"📋  Copy Bet ID:  {betId}";
            string openUrl   = cleanIid != null
                ? $"https://{StakeSite}/?modal=bet&iid={cleanIid}"
                : $"https://{StakeSite}/?betId={betId}&modal=bet";

            var menu     = new ContextMenuStrip();
            var copyItem = new ToolStripMenuItem(copyLabel);
            var openItem = new ToolStripMenuItem("🌐  Open in browser");
            copyItem.Click += (s, ev) => Clipboard.SetText(copyText);
            openItem.Click += (s, ev) => { try { Process.Start(new ProcessStartInfo(openUrl) { UseShellExecute = true }); } catch { } };
            menu.Items.Add(copyItem);
            menu.Items.Add(openItem);
            menu.Show(lvHistory, new Point(e.X, e.Y));
        }

        // ── Resolve numeric betId → IID via GraphQL BetLookup ──────
        private async Task<string> FetchBetIid(string betId)
        {
            try
            {
                const string gql = "query BetLookup($betId:String){bet(betId:$betId){iid}}";
                using (var cts = new CancellationTokenSource(10000))
                {
                    var j = await ApiPost("_api/graphql",
                        new { operationName = "BetLookup", query = gql, variables = new { betId } },
                        cts.Token);
                    return j["data"]?["bet"]?["iid"]?.Value<string>();
                }
            }
            catch { return null; }
        }

        // ── Add row to history ─────────────────────────────────────
        private void AddBetToHistory(BetResult r)
        {
            if (r == null || r.HasError || string.IsNullOrEmpty(r.BetId)) return;
            if (lvHistory.InvokeRequired) { lvHistory.BeginInvoke(new Action(() => AddBetToHistory(r))); return; }
            _histRow++;

            var item = new ListViewItem("");
            item.Tag = r.BetId;

            item.SubItems.Add(_histRow.ToString());
            item.SubItems.Add(r.Game);
            item.SubItems.Add(r.Amount.ToString("F8"));
            item.SubItems.Add(r.PayoutMultiplier.ToString("F2") + "x");
            item.SubItems.Add(r.Payout.ToString("F8"));
            var profItem = new ListViewItem.ListViewSubItem(item,
                (r.Profit >= 0 ? "+" : "") + r.Profit.ToString("F8"))
            { ForeColor = r.Profit >= 0 ? Color.DarkGreen : Color.Crimson };
            item.SubItems.Add(profItem);

            // Store color list in the item for diamonds game
            if (r.Game == "diamonds" && r.ResultColors != null && r.ResultColors.Count > 0)
            {
                item.Tag = new Tuple<string, List<string>>(r.BetId, r.ResultColors);
                item.SubItems.Add(""); // Empty text for Roll column
            }
            else
            {
                string rollStr = !string.IsNullOrEmpty(r.RollDisplay) ? r.RollDisplay
                                 : r.RollResult > 0 ? r.RollResult.ToString("F2") : "";
                item.SubItems.Add(rollStr);
            }

            item.SubItems.Add(r.Extra ?? "");

            item.BackColor = r.TriggeredStop ? Color.FromArgb(255, 200, 100)
                           : r.Win ? Color.FromArgb(240, 255, 240)
                           : Color.White;
            item.ForeColor = r.Win ? Color.DarkGreen : Color.Crimson;

            // Remember which item was at the top before we insert, so we can re-pin it
            int topIndexBefore = _lvHistoryAutoScroll ? -1 : (lvHistory.TopItem?.Index ?? -1);

            lvHistory.BeginUpdate();
            lvHistory.Items.Insert(0, item);
            if (lvHistory.Items.Count > 500) lvHistory.Items.RemoveAt(lvHistory.Items.Count - 1);
            lvHistory.EndUpdate();

            // If the user has scrolled away from the top, compensate for the Insert(0,…) shift
            // so the view stays locked on the row they were reading.
            if (!_lvHistoryAutoScroll && topIndexBefore >= 0)
            {
                int newIndex = Math.Min(topIndexBefore + 1, lvHistory.Items.Count - 1);
                _lvHistoryRestoringScroll = true;
                try   { lvHistory.TopItem = lvHistory.Items[newIndex]; }
                finally { _lvHistoryRestoringScroll = false; }
            }

            // Force redraw to show images
            lvHistory.Invalidate();

            // ── Feed top-multiplier tracker ────────────────────────
            _topMultiForm?.TryAdd(r.BetId, r.Game, r.PayoutMultiplier);

            // Rolling 3-second bets/sec counter
            var now = DateTime.UtcNow;
            _betTimes.Enqueue(now);
            var cutoff = now.AddSeconds(-3);
            while (_betTimes.Count > 0 && _betTimes.Peek() < cutoff)
                _betTimes.Dequeue();
            double rate = _betTimes.Count / 3.0;
            lblBetsPerSec.Text = $"{rate:F1}/s";
            lblBetsPerSec.ForeColor = rate >= 1.0 ? Color.DarkGreen : Color.DimGray;
        }

        // ═══════════════════════════════════════════════════════════
        //  LOG
        // ═══════════════════════════════════════════════════════════
        private void AppendLog(string msg)
        {
            if (rtbLog == null) return;
            if (rtbLog.InvokeRequired) { rtbLog.BeginInvoke(new Action(() => AppendLog(msg))); return; }
            bool isErr = msg.StartsWith("[API ERROR]") || msg.StartsWith("[ERROR]");
            rtbLog.SelectionStart  = rtbLog.TextLength;
            rtbLog.SelectionLength = 0;
            rtbLog.SelectionColor  = isErr ? Color.Crimson : Color.DimGray;
            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\n");
            rtbLog.ScrollToCaret();
            if (rtbLog.Lines.Length > 500) rtbLog.Text = string.Join("\n", rtbLog.Lines, rtbLog.Lines.Length - 400, 400);
        }

        // ═══════════════════════════════════════════════════════════
        //  LOGIN  (Keno-style)
        // ═══════════════════════════════════════════════════════════
        private async void CmbCurrency_Changed(object sender, EventArgs e)
        {
            string cur = cmbCurrency.Text;
            lblBalance.Text = _currencyBalances.TryGetValue(cur, out decimal bal)
                ? $"Balance: {bal:F8} {cur}"
                : $"Balance: — {cur}";

            // Persist to both stores immediately
            _s.SelectedCurrency = cur;
            Properties.Settings.Default.SelectedCurrency = cur;
            Properties.Settings.Default.Save();
            SaveSettings();

            // Fetch live balance for the newly selected currency
            await RefreshBalanceAsync();
        }

        private void CmbFetchMode_Changed(object sender, EventArgs e)
        {
            // Persist immediately to the user-scoped settings store
            Properties.Settings.Default.FetchMode = cmbFetchMode.SelectedIndex;
            Properties.Settings.Default.Save();

            ApplyFetchModeUI(cmbFetchMode.SelectedIndex == 1);
            QueueSave(sender, e);
        }

        /// <summary>
        /// Applies all visibility / subscription side-effects for the chosen fetch mode.
        /// Extracted so it can be called both from the change handler AND from startup
        /// without re-triggering the change event.
        /// </summary>
        private void ApplyFetchModeUI(bool ext)
        {
            UpdateCookieStatus();
            if (ext)
            {
                BrowserFetch.StartServer();
                // Guard against double-subscription
                BrowserFetch.Connected    -= OnWsConnected;
                BrowserFetch.Disconnected -= OnWsDisconnected;
                BrowserFetch.Connected    += OnWsConnected;
                BrowserFetch.Disconnected += OnWsDisconnected;
                bool ok = BrowserFetch.IsConnected;
                lblWsIndicator.ForeColor = ok ? Color.LimeGreen : Color.Gray;
                lblWsStatus.ForeColor    = ok ? Color.LimeGreen : Color.Gray;
                lblWsStatus.Text         = ok ? "Extension OK" : "Extension OFF";
                btnGetCookie.Visible     = false;
                lblCookieStatus.Visible  = false;
                lblWsIndicator.Visible   = true;
                lblWsStatus.Visible      = true;
            }
            else
            {
                BrowserFetch.Connected    -= OnWsConnected;
                BrowserFetch.Disconnected -= OnWsDisconnected;
                lblWsStatus.Text          = "Extension OFF";
                lblWsIndicator.Visible    = false;
                lblWsStatus.Visible       = false;
                btnGetCookie.Visible      = true;
                lblCookieStatus.Visible   = true;
            }
        }

        private void BtnGetCookie_Click(object sender, EventArgs e)
        {
            using (var f = new WebViewLogin(StakeSite))
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    ClearanceCookie = f.CapturedClearance;
                    UserAgent       = f.CapturedUserAgent;
                    cc = new CookieContainer();
                    UpdateCookieStatus();
                    _s.ClearanceCookie = ClearanceCookie;
                    _s.UserAgent       = UserAgent;
                    SaveSettings();
                }
        }

        private void UpdateCookieStatus()
        {
            bool has = !string.IsNullOrWhiteSpace(ClearanceCookie);
            lblCookieStatus.Text      = has ? "Cookie OK" : "No cookie";
            lblCookieStatus.ForeColor = has ? Color.LimeGreen : Color.Gray;
        }

        private void OnWsConnected(object sender, EventArgs e)
        {
            void UpdateUI()
            {
                // Switch the combobox to "Extension" silently (no side-effect re-subscription)
                cmbFetchMode.SelectedIndexChanged -= CmbFetchMode_Changed;
                cmbFetchMode.SelectedIndex = 1;
                cmbFetchMode.SelectedIndexChanged += CmbFetchMode_Changed;

                // Show only the extension status labels; hide the cookie button/label
                btnGetCookie.Visible    = false;
                lblCookieStatus.Visible = false;
                lblWsIndicator.Visible  = true;
                lblWsStatus.Visible     = true;

                lblWsIndicator.ForeColor = Color.LimeGreen;
                lblWsStatus.ForeColor    = Color.LimeGreen;
                lblWsStatus.Text         = "Extension OK";
            }
            if (lblWsIndicator.InvokeRequired) lblWsIndicator.Invoke((MethodInvoker)UpdateUI); else UpdateUI();

            // Auto-login now that the extension is available
            _ = Task.Delay(300).ContinueWith(_ =>
            {
                if (InvokeRequired)
                    BeginInvoke(new Action(async () => await DoLoginAsync()));
                else
                    Task.Run(async () => await DoLoginAsync());
            });
        }
        private void OnWsDisconnected(object sender, EventArgs e)
        {
            void A() { lblWsIndicator.ForeColor = Color.Gray; lblWsStatus.ForeColor = Color.Gray; lblWsStatus.Text = "Extension OFF"; }
            if (lblWsIndicator.InvokeRequired) lblWsIndicator.Invoke((MethodInvoker)A); else A();
        }

        private async Task<JObject> ApiPost(string path, object body, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            bool ext; string key;
            if (InvokeRequired) { ext = false; key = ""; Invoke(new Action(() => { ext = cmbFetchMode.SelectedIndex == 1; key = txtApiKey.Text.Trim(); })); }
            else { ext = cmbFetchMode.SelectedIndex == 1; key = txtApiKey.Text.Trim(); }

            string url     = "https://" + StakeSite + "/" + path;
            string bodyStr = JsonConvert.SerializeObject(body);

            if (ext)
            {
                var opts = new { method = "POST", headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "x-access-token", key } }, body = bodyStr };
                string resp = await BrowserFetch.FetchAsync(url, opts, 15000);
                return TryParse(resp);
            }
            else
            {
                return await RestPostWithRetryAsync(url, bodyStr, key, ct);
            }
        }

        /// <summary>
        /// Mirrors fetchPlus() from index.js:
        ///   • 5-second timeout per attempt  (AbortSignal.timeout(5000))
        ///   • Up to 5 retries on timeout or any network/HTTP error
        ///   • 500 ms pause between retries  (setTimeout(res, 500))
        ///   • Attaches cf_clearance cookie and all required headers
        /// </summary>
        private async Task<JObject> RestPostWithRetryAsync(
            string url, string bodyStr, string key,
            CancellationToken ct, int retries = 0)
        {
            while (true)
            {
                ct.ThrowIfCancellationRequested();

                // Per-attempt 5-second timeout linked to the caller's cancellation token.
                // C# 7.3: must use the full using-block syntax, not "using var".
                using (var attemptCts = CancellationTokenSource.CreateLinkedTokenSource(ct))
                {
                    attemptCts.CancelAfter(TimeSpan.FromSeconds(5)); // ← AbortSignal.timeout(5000)

                    try
                    {
                        return await Task.Run(() =>
                        {
                            var client = new RestClient(url);
                            // client.Timeout (ms) is how old RestSharp enforces per-request
                            // timeouts — equivalent to passing the token to Execute().
                            client.Timeout  = 5000;
                            client.UserAgent = string.IsNullOrEmpty(UserAgent)
                                ? "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 Chrome/122.0.0.0 Safari/537.36"
                                : UserAgent;
                            client.CookieContainer = cc;
                            if (!string.IsNullOrEmpty(ClearanceCookie))
                                client.CookieContainer.Add(
                                    new Cookie("cf_clearance", ClearanceCookie, "/", StakeSite));

                            var req = new RestRequest(Method.POST);
                            req.AddHeader("Content-Type",   "application/json");
                            req.AddHeader("x-access-token", key);
                            req.AddHeader("Accept",         "application/json");
                            req.AddParameter("application/json", bodyStr, ParameterType.RequestBody);

                            var r = client.Execute(req);

                            if (!r.IsSuccessful)
                                throw new Exception(
                                    $"HTTP {(int)r.StatusCode}: {r.ErrorMessage ?? r.StatusCode.ToString()}");

                            return TryParse(r.Content);

                        }, attemptCts.Token);
                    }
                    catch (OperationCanceledException) when (!ct.IsCancellationRequested)
                    {
                        // ── Timed out (not cancelled by the caller) ──────────────────
                        // Mirrors: ['AbortError','TimeoutError'].includes(error.name) branch
                        if (retries > 0)
                        {
                            AppendLog($"[WARN] Request timed out — retrying ({retries} left)");
                            await Task.Delay(5000); // ← await new Promise(res => setTimeout(res, 500))
                            retries--;
                            continue;
                        }
                        throw new Exception("Request timed out after all retries.");
                    }
                    catch (OperationCanceledException)
                    {
                        // Caller cancelled (bot stopped) — propagate immediately, no retry
                        throw;
                    }
                    catch (Exception) when (retries > 0)
                    {
                        // ── Network / HTTP error ─────────────────────────────────────
                        // Mirrors: the second catch branch in fetchPlus
                        AppendLog($"[WARN] Network error — retrying ({retries} left)");
                        await Task.Delay(5000);
                        retries--;
                    }
                }
            }
        }

        private static JObject TryParse(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return new JObject();
            try { return JObject.Parse(s); } catch { return new JObject { ["_raw"] = s }; }
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtApiKey.Text.Trim())) { MessageBox.Show("Enter your API key.", "Error"); return; }
            await DoLoginAsync();
        }

        /// <summary>
        /// Performs the UserBalances query, populates the currency combobox and restores the
        /// previously selected currency.  Safe to call from any thread; marshals to the UI thread
        /// internally.  Does nothing if the API key field is empty.
        /// </summary>
        private async Task DoLoginAsync()
        {
            // Marshal to UI thread if needed
            if (InvokeRequired) { Invoke(new Action(async () => await DoLoginAsync())); return; }

            if (string.IsNullOrEmpty(txtApiKey.Text.Trim())) return;

            btnLogin.Enabled = true; btnLogin.Text = "...";
            try
            {
                using (var cts = new CancellationTokenSource(15000))
                {
                    var j    = await ApiPost("_api/graphql", new { operationName = "UserBalances", variables = new { }, query = "query UserBalances{user{balances{available{amount currency}}}}" }, cts.Token);
                    var bals = j["data"]?["user"]?["balances"];
                    if (bals == null) { AppendLog("[ERROR] Login failed: " + j); return; }

                    // Unhook the change handler so that the temporary index-0 selection
                    // that WinForms applies while Items.Add() runs does NOT overwrite
                    // _s.SelectedCurrency before we get a chance to restore it.
                    cmbCurrency.SelectedIndexChanged -= CmbCurrency_Changed;

                    cmbCurrency.Items.Clear();
                    _currencyBalances.Clear();
                    foreach (var b in bals)
                    {
                        string cur  = b["available"]?["currency"]?.Value<string>();
                        decimal amt = b["available"]?["amount"]?.Value<decimal>() ?? 0m;
                        if (!string.IsNullOrEmpty(cur)) { cmbCurrency.Items.Add(cur); _currencyBalances[cur] = amt; }
                    }

                    // Restore saved currency — prefer Properties.Settings (written instantly on
                    // every selection), fall back to the JSON bot_settings value.
                    string savedCur = Properties.Settings.Default.SelectedCurrency;
                    if (string.IsNullOrEmpty(savedCur)) savedCur = _s.SelectedCurrency;

                    // Case-insensitive match; fall back to first item
                    int ci = -1;
                    if (!string.IsNullOrEmpty(savedCur))
                        for (int i = 0; i < cmbCurrency.Items.Count; i++)
                            if (string.Equals(cmbCurrency.Items[i]?.ToString(), savedCur,
                                              StringComparison.OrdinalIgnoreCase))
                            { ci = i; break; }

                    cmbCurrency.SelectedIndex = ci >= 0 ? ci : (cmbCurrency.Items.Count > 0 ? 0 : -1);

                    // Re-hook and manually fire once so the balance label reflects the restored selection
                    cmbCurrency.SelectedIndexChanged += CmbCurrency_Changed;
                    CmbCurrency_Changed(cmbCurrency, EventArgs.Empty);

                    btnLogin.Text = "✔ OK";
                    if (cmbFetchMode.SelectedIndex == 0) { lblCookieStatus.Text = "Connected"; lblCookieStatus.ForeColor = Color.LimeGreen; }
                    AppendLog("[INFO] Logged in successfully.");
                    _balanceTimer.Change(10_000, 10_000);   // start / reset: fire every 10 s from now
                    await LoadActiveHiloGameAsync();
                }
            }
            catch (Exception ex) { AppendLog("[ERROR] Login: " + ex.Message); btnLogin.Text = "Login"; }
            finally { btnLogin.Enabled = true; }
        }

        // ═══════════════════════════════════════════════════════════
        //  PER-GAME CONDITION BUILDER
        // ═══════════════════════════════════════════════════════════
        private void BtnCondition_Click(object sender, EventArgs e)
        {
            var current = _s.GetConditions(_currentGame);
            using (var f = new ConditionBuilderForm(current))
            {
                f.Text = $"Condition Builder — {_currentGame.ToUpper()}";
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    _s.GameConditions[_currentGame] = f.Result;
                    SaveSettings();
                    AppendLog($"[INFO] Conditions saved for {_currentGame} ({f.Result.Count} blocks)");
                }
            }
        }

        // ═══════════════════════════════════════════════════════════
        //  BOT START / STOP
        // ═══════════════════════════════════════════════════════════
        private async void BtnStart_Click(object sender, EventArgs e)
        {
            CollectSettings();
            isManual = false;
            if (string.IsNullOrEmpty(_s.SelectedCurrency)) { MessageBox.Show("Login first to select a currency.", "Not logged in"); return; }

            // If an active HiLo game is already open, load its cards and let the
            // user finish it manually — do NOT start the bot on top of it.
            if (_currentGame == "hilo" && _hiloManualActive)
            {
                AppendLog("[HILO] Active game in progress — loading cards. Finish it before starting the bot.");
                await LoadActiveHiloGameAsync();
                return;
            }
            _botCts = new CancellationTokenSource();
            btnStart.Enabled = false; btnStop.Enabled = true;

            // Snapshot the real server balance at this exact moment so
            // UpdateEngineUI always has a stable base for its delta math.
            _sessionStartBalance = _currencyBalances.TryGetValue(_s.SelectedCurrency, out decimal _sb) ? _sb : 0m;
            // Keep the timer running so balance is refreshed every 10 s even while betting.
            //_histRow = 0; lvHistory.Items.Clear();
            //_betTimes.Clear();
            lblBetsPerSec.Text = "0.0/s";
            lblBetsPerSec.ForeColor = System.Drawing.Color.Gray;
            _engine.Init(_s, ApiPost, AppendLog, UpdateEngineUI, AddBetToHistory,
                onHiloCard: (rank, suit, multi) =>
                {
                    if (multi <= 1.0) { ClearHiloCards(); AddHiloStartCard(rank, suit); }
                    else              { AddHiloCard(rank, suit, multi); }
                });
            AppendLog($"[START] Game={_currentGame} BaseBet={_s.BaseBet} Currency={_s.SelectedCurrency}");

            bool completed = true;  // Track if bot completed naturally

            try
            {
                // Run the engine on a background task so the UI message pump
                // stays free — especially important in fast mode where the
                // loop fires requests as fast as the delay allows.
                await Task.Run(async () => await _engine.RunAsync(_botCts.Token, _sessionStartBalance));
            }
            catch (OperationCanceledException)
            {
                AppendLog("[INFO] Bot was manually stopped.");
                //completed = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bot error:\n" + ex.Message, "Error");
            }
            finally
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                lblBetsPerSec.Text = "0.0/s";
                lblBetsPerSec.ForeColor = System.Drawing.Color.Gray;
                AppendLog("[STOP] Bot stopped.");
                _balanceTimer.Change(10_000, 10_000);    // ensure timer keeps ticking after bot stops
                _ = RefreshBalanceAsync();       // immediate refresh so label is accurate right away

                // Only play beep if bot completed naturally (not cancelled, no error, and no HTTP error stop)
                if (completed && !isManual && !_engine.StoppedDueToError)
                {
                    
                    var beep = new AudioBeep();
                    await beep.BeepAsync();
                }
            }
        }

        private async void BtnStop_Click(object sender, EventArgs e)
        {
            isManual = true;
            if (_botCts != null)
            {
                _botCts.Cancel();  // This will trigger OperationCanceledException
                btnStop.Enabled = false;
            }
        }

        // ── Fast-mode toggle ──────────────────────────────────────
        private void FastModeBox_Changed(object sender, EventArgs e)
        {
            _s.FastMode = FastModeBox.SelectedIndex == 1;

            if (!_s.FastMode)
            {
                // Switching OFF → zero the delay field
                //BetDelayNumericUpDown1.Value = 0;
                _s.BetDelayMs = (int)BetDelayNumericUpDown1.Value;
            }
            else
            {
                // Switching ON → keep whatever delay the user has set
                _s.BetDelayMs = Math.Max(128, (int)BetDelayNumericUpDown1.Value);
                BetDelayNumericUpDown1.Value = 128;
            }

            QueueSave(sender, e);
        }

        private void BetDelayNumericUpDown1_Changed(object sender, EventArgs e)
        {
            if (!_s.FastMode)
            {
                // Switching OFF → zero the delay field
                //BetDelayNumericUpDown1.Value = 0;
                _s.BetDelayMs = (int)BetDelayNumericUpDown1.Value;
            }
            else
            {
                // Switching ON → keep whatever delay the user has set
                _s.BetDelayMs = Math.Max(128, (int)BetDelayNumericUpDown1.Value);
                BetDelayNumericUpDown1.Value = 128;
            }// Update live so the running bot uses the new delay immediately
            QueueSave(sender, e);
        }

        private void UpdateEngineUI(BotEngine eng)
        {
            if (lblBalance.InvokeRequired) { lblBalance.BeginInvoke(new Action(() => UpdateEngineUI(eng))); return; }
            // eng.balance = startBalance + cumulative profit — reflects the true running wallet value.
            string cur = cmbCurrency.Text;
            lblBalance.Text = $"Balance: {eng.balance:F8} {cur}";
        }

        /// <summary>
        /// Fetches live balances from the API, updates the cache, and refreshes
        /// the balance label.  Runs every 10 s whether the bot is idle or running —
        /// when running it overwrites the per-bet delta estimate with the true server value.
        /// Safe to call from any thread.
        /// </summary>
        private async Task RefreshBalanceAsync()
        {
            if (InvokeRequired) { BeginInvoke(new Action(async () => await RefreshBalanceAsync())); return; }

            try
            {
                using (var cts = new CancellationTokenSource(10_000))
                {
                    var j    = await ApiPost("_api/graphql",
                                   new { operationName = "UserBalances", variables = new { },
                                         query = "query UserBalances{user{balances{available{amount currency}}}}" },
                                   cts.Token);
                    var bals = j["data"]?["user"]?["balances"];
                    if (bals == null) return;

                    foreach (var b in bals)
                    {
                        string  cur = b["available"]?["currency"]?.Value<string>();
                        decimal amt = b["available"]?["amount"]?.Value<decimal>() ?? 0m;
                        if (!string.IsNullOrEmpty(cur))
                            _currencyBalances[cur] = amt;
                    }

                    // Always update the label — while the bot runs this corrects the
                    // per-bet delta estimate to the true server value every 10 seconds.
                    string sel = cmbCurrency.Text;
                    if (_currencyBalances.TryGetValue(sel, out decimal bal))
                    {
                        lblBalance.Text = $"Balance: {bal:F8} {sel}";
                        // Resync the engine's running balance to the server-confirmed value.
                        // Without this, UpdateEngineUI (called on every bet) immediately
                        // overwrites this label with the old stale delta value.
                        _engine.balance = bal;
                    }
                }
            }
            catch { /* silent — best-effort, timer will retry in 10 s */ }
        }

        // ═══════════════════════════════════════════════════════════
        //  AUTO-SAVE / LOAD
        // ═══════════════════════════════════════════════════════════
        private void QueueSave(object sender, EventArgs e) { _saveTimer.Stop(); _saveTimer.Start(); }

        private void SaveSettings()
        {
            CollectSettings();
            try { File.WriteAllText(SETTINGS_FILE, JsonConvert.SerializeObject(_s, Formatting.Indented)); }
            catch { }
        }

        private void LoadSettings()
        {
            try
            {
                if (!File.Exists(SETTINGS_FILE))
                {
                    if (cmbGame.Items.Count > 0) cmbGame.SelectedIndex = 0;
                    cmbFetchMode.SelectedIndex = 0; // first run → always Cookie mode
                    DefaultAllCombos();             // ensure every other combobox starts at item 0
                    return;
                }
                _s = JsonConvert.DeserializeObject<BotSettings>(File.ReadAllText(SETTINGS_FILE)) ?? new BotSettings();
                ApplySettings();
            }
            catch { if (cmbGame.Items.Count > 0) cmbGame.SelectedIndex = 0; DefaultAllCombos(); }
        }

        /// <summary>
        /// Selects index 0 on every game-settings combobox that has no selection yet.
        /// Called on first run and on error so the UI is never left in an indeterminate state.
        /// </summary>
        private void DefaultAllCombos()
        {
            foreach (var cmb in new ComboBox[]
            {
                cmbKenoRisk, cmbPlinkoRisk, cmbWheelRisk,
                cmbPumpDiff, cmbDragonDiff, cmbBarsDiff,
                cmbCasesDiff, cmbSnakesDiff, cmbDartsDiff,
                cmbChickenDiff, cmbTarotDiff, cmbPDCond,
                cmbHiloStartCard, cmbHiloSuit, FastModeBox,
                cmbVideoPokerStrat
            })
            {
                if (cmb != null && cmb.Items.Count > 0 && cmb.SelectedIndex < 0)
                    cmb.SelectedIndex = 0;
            }
        }

        private void CollectSettings()
        {
            _s.ApiKey           = txtApiKey.Text;
            _s.AuthMethod       = cmbFetchMode.SelectedIndex == 1 ? "Extension" : "Cookie";
            _s.SelectedCurrency = cmbCurrency.Text;
            _s.Mirror           = txtMirror.Text;
            _s.BaseBet          = nudBaseBet.Value;
            _s.SelectedGame     = _currentGame;
            _s.DiceBetHigh      = rbDiceOver.Checked;
            _s.DiceChance       = nudDiceChance.Value;
            _s.StopOnDiceResult     = chkStopDiceResult.Checked;
            _s.StopDiceResultValue  = nudStopDiceResult.Value;
            _s.LimboTarget      = nudLimboTarget.Value;
            _s.HiloPattern      = txtHiloPattern.Text;
            _s.HiloStartCardRank= cmbHiloStartCard.SelectedItem?.ToString() ?? "";
            _s.HiloStartCardSuit= cmbHiloSuit.SelectedItem?.ToString() ?? "";
            _s.MinesMines       = (int)nudMinesMines.Value;
            _s.MinesFields      = txtMinesFields.Text;
            _s.KenoNumbers      = txtKenoNumbers.Text;
            _s.KenoRisk         = cmbKenoRisk.SelectedItem?.ToString();
            _s.PlinkoRows       = (int)nudPlinkoRows.Value;
            _s.PlinkoRisk       = cmbPlinkoRisk.SelectedItem?.ToString();
            _s.WheelSegments    = (int)nudWheelSegs.Value;
            _s.WheelRisk        = cmbWheelRisk.SelectedItem?.ToString();
            _s.BaccaratBanker   = nudBacBanker.Value;
            _s.BaccaratPlayer   = nudBacPlayer.Value;
            _s.BaccaratTie      = nudBacTie.Value;
            _s.RouletteChips    = txtRouletteChips.Text;
            _s.PumpPumps        = (int)nudPumpPumps.Value;
            _s.PumpDifficulty   = cmbPumpDiff.SelectedItem?.ToString();
            _s.DragonDifficulty = cmbDragonDiff.SelectedItem?.ToString();
            _s.DragonEggs       = txtDragonEggs.Text;
            _s.BarsDifficulty   = cmbBarsDiff.SelectedItem?.ToString();
            _s.BarsTiles        = txtBarsTiles.Text;
            _s.SlotLines = _currentGame == "scarabspin"
								? (int)nudScarabLines.Value
								: (int)nudTomeLines.Value;
            _s.DiamondsColors   = txtDiamondColors.Text;
            _s.StopOnDiamondsWin= chkStopDiamondsWin.Checked;
            _s.CasesDifficulty  = cmbCasesDiff.SelectedItem?.ToString();
            _s.RpsGuesses       = "[\"" + txtRpsGuesses.Text.Replace(",", "\",\"") + "\"]";
            _s.FlipGuesses      = "[\"" + txtFlipGuesses.Text.Replace(",", "\",\"") + "\"]";
            _s.SnakesDifficulty = cmbSnakesDiff.SelectedItem?.ToString();
            _s.SnakesRolls      = (int)nudSnakesRolls.Value;
            _s.DartsDifficulty  = cmbDartsDiff.SelectedItem?.ToString();
            _s.MolesMoles       = (int)nudMolesMoles.Value;
            _s.MolesPicks       = txtMolesPicks.Text;
            _s.ChickenDifficulty= cmbChickenDiff.SelectedItem?.ToString();
            _s.ChickenSteps     = (int)nudChickenSteps.Value;
            _s.TarotDifficulty  = cmbTarotDiff.SelectedItem?.ToString();
            _s.DrillTarget      = nudDrillTarget.Value;
            _s.DrillPick        = (int)nudDrillPick.Value;
            _s.PDCondition      = cmbPDCond.SelectedItem?.ToString();
            _s.PDTarget1        = nudPDT1.Value; _s.PDTarget2 = nudPDT2.Value;
            _s.PDTarget3        = nudPDT3.Value; _s.PDTarget4 = nudPDT4.Value;
            _s.VideoPokerStrategy = cmbVideoPokerStrat.SelectedItem?.ToString() ?? "optimal";
            _s.StopOnMultiplier    = chkStopMultiplier.Checked;
            _s.StopMultiplierValue = nudStopMultiplier.Value;
            _s.FastMode            = FastModeBox.SelectedIndex == 1;
            _s.BetDelayMs          = (int)BetDelayNumericUpDown1.Value;
        }

        private void ApplySettings()
        {
            txtApiKey.Text         = _s.ApiKey;
            // Restore fetch mode from the user-scoped settings store (most reliable),
            // falling back to the JSON value.  Always apply the matching UI state after.
            int fetchMode = Properties.Settings.Default.FetchMode;
            // If the Properties store still has the default (0) and JSON says Extension, honour JSON.
            if (fetchMode == 0 && _s.AuthMethod == "Extension") fetchMode = 1;
            cmbFetchMode.SelectedIndexChanged -= CmbFetchMode_Changed;
            cmbFetchMode.SelectedIndex = fetchMode;
            cmbFetchMode.SelectedIndexChanged += CmbFetchMode_Changed;
            ApplyFetchModeUI(fetchMode == 1);   // apply visibility / subscriptions immediately
            if (!string.IsNullOrEmpty(_s.Mirror)) { txtMirror.Text = _s.Mirror; StakeSite = _s.Mirror; }
            ClearanceCookie = _s.ClearanceCookie ?? ""; UserAgent = _s.UserAgent ?? ""; UpdateCookieStatus();

            SS(nudBaseBet,  _s.BaseBet);
            rbDiceOver.Checked = _s.DiceBetHigh; rbDiceUnder.Checked = !_s.DiceBetHigh;
            SS(nudDiceChance, _s.DiceChance);
            chkStopDiceResult.Checked = _s.StopOnDiceResult; SS(nudStopDiceResult, _s.StopDiceResultValue);
            SS(nudLimboTarget, _s.LimboTarget);
            ST(txtHiloPattern, _s.HiloPattern);
            // Hilo rank/suit: default to "A"/"C"; an empty saved value (old settings
            // file) is also treated as "use default" so the blank item can never win.
            SC(cmbHiloStartCard, string.IsNullOrEmpty(_s.HiloStartCardRank) ? "A" : _s.HiloStartCardRank);
            SC(cmbHiloSuit,      string.IsNullOrEmpty(_s.HiloStartCardSuit) ? "C" : _s.HiloStartCardSuit);
            SS(nudMinesMines, _s.MinesMines); ST(txtMinesFields, _s.MinesFields);
            ST(txtKenoNumbers, _s.KenoNumbers); SC(cmbKenoRisk, _s.KenoRisk);
            SS(nudPlinkoRows, _s.PlinkoRows); SC(cmbPlinkoRisk, _s.PlinkoRisk);
            SS(nudWheelSegs, _s.WheelSegments); SC(cmbWheelRisk, _s.WheelRisk);
            SS(nudBacBanker, _s.BaccaratBanker); SS(nudBacPlayer, _s.BaccaratPlayer); SS(nudBacTie, _s.BaccaratTie);
            ST(txtRouletteChips, _s.RouletteChips);
            SS(nudPumpPumps, _s.PumpPumps); SC(cmbPumpDiff, _s.PumpDifficulty);
            SC(cmbDragonDiff, _s.DragonDifficulty); ST(txtDragonEggs, _s.DragonEggs);
            SC(cmbBarsDiff, _s.BarsDifficulty); ST(txtBarsTiles, _s.BarsTiles);
            SS(nudTomeLines, _s.SlotLines); SS(nudScarabLines, _s.SlotLines);
            if (!string.IsNullOrEmpty(_s.DiamondsColors))
            {
                txtDiamondColors.Text    = _s.DiamondsColors;
                _selectedDiamondColors   = new List<string>(_s.DiamondsColors.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                lblDiamondCount.Text     = $"{_selectedDiamondColors.Count} / 5 selected";
            }
            chkStopDiamondsWin.Checked = _s.StopOnDiamondsWin;
            SC(cmbCasesDiff, _s.CasesDifficulty);
            ST(txtRpsGuesses,  (_s.RpsGuesses  ?? "rock" ).Trim('[',']').Replace("\"",""));
            ST(txtFlipGuesses, (_s.FlipGuesses ?? "heads").Trim('[',']').Replace("\"",""));
            SC(cmbSnakesDiff, _s.SnakesDifficulty); SS(nudSnakesRolls, _s.SnakesRolls);
            SC(cmbDartsDiff, _s.DartsDifficulty);
            SS(nudMolesMoles, _s.MolesMoles); ST(txtMolesPicks, _s.MolesPicks);
            SC(cmbChickenDiff, _s.ChickenDifficulty); SS(nudChickenSteps, _s.ChickenSteps);
            SC(cmbTarotDiff, _s.TarotDifficulty);
            SS(nudDrillTarget, _s.DrillTarget); SS(nudDrillPick, _s.DrillPick);
            SC(cmbPDCond, _s.PDCondition);
            SS(nudPDT1, _s.PDTarget1); SS(nudPDT2, _s.PDTarget2);
            SS(nudPDT3, _s.PDTarget3); SS(nudPDT4, _s.PDTarget4);
            SC(cmbVideoPokerStrat, string.IsNullOrEmpty(_s.VideoPokerStrategy) ? "optimal" : _s.VideoPokerStrategy);
            chkStopMultiplier.Checked = _s.StopOnMultiplier;
            SS(nudStopMultiplier, _s.StopMultiplierValue > 0 ? _s.StopMultiplierValue : 500m);

            // Fast mode + bet delay
            FastModeBox.SelectedIndexChanged -= FastModeBox_Changed;
            FastModeBox.SelectedIndex = _s.FastMode ? 1 : 0;
            FastModeBox.SelectedIndexChanged += FastModeBox_Changed;
            SS(BetDelayNumericUpDown1, _s.BetDelayMs);

            // Select game — triggers ShowGame via CmbGame_Changed
            foreach (GameItem gi in cmbGame.Items)
                if (gi.Key == _s.SelectedGame) { cmbGame.SelectedItem = gi; goto done; }
            if (cmbGame.Items.Count > 0) cmbGame.SelectedIndex = 0;
            done:

            if (!string.IsNullOrEmpty(_s.SelectedCurrency))
            {
                for (int i = 0; i < cmbCurrency.Items.Count; i++)
                    if (string.Equals(cmbCurrency.Items[i]?.ToString(), _s.SelectedCurrency,
                                      StringComparison.OrdinalIgnoreCase))
                    { cmbCurrency.SelectedIndex = i; break; }
            }
        }

        // ─── Micro-helpers ────────────────────────────────────────
        private static void SS(NumericUpDown n, decimal v) { if (n == null) return; n.Value = Math.Max(n.Minimum, Math.Min(n.Maximum, v)); }
        private static void SC(ComboBox c, string t) { if (c == null || t == null) return; foreach (var i in c.Items) if (i.ToString().Equals(t, StringComparison.OrdinalIgnoreCase)) { c.SelectedItem = i; return; } if (c.Items.Count > 0) c.SelectedIndex = 0; }
        private static void ST(TextBox t, string v) { if (t != null && v != null) t.Text = v; }

        // ═══════════════════════════════════════════════════════════
        //  RESET SEED
        // ═══════════════════════════════════════════════════════════
        private async void BtnResetSeed_Click(object sender, EventArgs e)
        {
            ResetSeedBtn.Enabled = true;
            ResetSeedBtn.Text    = "...";
            try
            {
                string clientSeed = RandomString(10);

                // ── Step 1: query active bets before rotating ─────────────────────
                const string seedPairQuery =
                    "query UserSeedPair {\n  user {\n    id\n" +
                    "    activeClientSeed { id seed __typename }\n" +
                    "    activeServerSeed { id nonce seedHash nextSeedHash __typename }\n" +
                    "    activeCasinoBets { id amount currency game createdAt __typename }\n" +
                    "    __typename\n  }\n}";

                // Held in outer scope so Step 2 can use it in the error message
                string activeGameList = null;

                using (var cts = new CancellationTokenSource(15000))
                {
                    var seedPairResponse = await ApiPost(
                        "_api/graphql",
                        new { operationName = "UserSeedPair", query = seedPairQuery, variables = new { } },
                        cts.Token);

                    var activeBets = seedPairResponse["data"]?["user"]?["activeCasinoBets"] as Newtonsoft.Json.Linq.JArray;
                    if (activeBets != null && activeBets.Count > 0)
                    {
                        var gameNames = activeBets
                            .Select(b => b["game"]?.Value<string>())
                            .Where(g => !string.IsNullOrEmpty(g))
                            .Distinct()
                            .ToList();

                        activeGameList = string.Join(", ", gameNames);
                        AppendLog($"[SEED] Warning: {activeBets.Count} active bet(s) in: {activeGameList} — rotating anyway");
                    }
                }

                // ── Step 2: perform the rotation ──────────────────────────────────
                const string query =
                    @"mutation RotateSeedPair($seed: String!) {
                        rotateSeedPair(seed: $seed) {
                            clientSeed {
                                user {
                                    id
                                    activeClientSeed { id seed __typename }
                                    activeServerSeed { id nonce seedHash nextSeedHash __typename }
                                    __typename
                                }
                                __typename
                            }
                            __typename
                        }
                    }";

                using (var cts = new CancellationTokenSource(15000))
                {
                    var j = await ApiPost(
                        "_api/graphql",
                        new { operationName = "RotateSeedPair", query, variables = new { seed = clientSeed } },
                        cts.Token);

                    if (j["errors"] is Newtonsoft.Json.Linq.JArray errs && errs.Count > 0)
                    {
                        string msg = errs[0]["message"]?.Value<string>() ?? "Unknown error";
                        if (activeGameList != null)
                            msg = msg.Replace("existing games", activeGameList)
                                     .Replace("existing game",  activeGameList);
                        AppendLog($"[ERROR] Reset Seed failed: {msg}");
                        return;
                    }

                    var user       = j["data"]?["rotateSeedPair"]?["clientSeed"]?["user"];
                    string newClient = user?["activeClientSeed"]?["seed"]?.Value<string>()     ?? clientSeed;
                    string seedHash  = user?["activeServerSeed"]?["nextSeedHash"]?.Value<string>() ?? "(unknown)";
                    long   nonce     = user?["activeServerSeed"]?["nonce"]?.Value<long>()          ?? 0;
                    AppendLog($"[SEED] Rotated — Client: {newClient} | Nonce: {nonce}");
                }
            }
            catch (Exception ex)
            {
                AppendLog($"[ERROR] Reset Seed exception: {ex.Message}");
            }
            finally
            {
                ResetSeedBtn.Enabled = true;
                ResetSeedBtn.Text    = "Reset Seed";
            }
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var rng = new Random();
            return new string(Enumerable.Range(0, length).Select(_ => chars[rng.Next(chars.Length)]).ToArray());
        }

        // ═══════════════════════════════════════════════════════════
        //  HILO — ACTIVE GAME DETECTION
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Called after every successful login.  Queries HiloActiveBet; if an
        /// in-progress round exists it populates lvHiloCards with all revealed
        /// cards and enables the manual-play buttons so the user can finish
        /// (or cash out) without running the bot.
        /// </summary>
        private async Task LoadActiveHiloGameAsync()
        {
            try
            {
                // Full query with fragments — matches HILO-Form1.cs exactly so the
                // server always returns a complete state object including all rounds.
                const string gql =
                    "query HiloActiveBet {\n  user {\n    id\n    activeCasinoBet(game: hilo) {\n" +
                    "      ...CasinoBetFragment\n      state { ...HiloStateFragment __typename }\n" +
                    "      __typename\n    }\n    __typename\n  }\n}\n" +
                    "fragment CasinoBetFragment on CasinoBet {\n" +
                    "  id active payoutMultiplier amountMultiplier amount payout updatedAt currency game\n" +
                    "  user { id name __typename }\n  __typename\n}\n" +
                    "fragment HiloStateFragment on CasinoGameHilo {\n" +
                    "  startCard { suit rank __typename }\n" +
                    "  rounds { card { suit rank __typename } guess payoutMultiplier __typename }\n" +
                    "  __typename\n}\n";

                using (var cts = new CancellationTokenSource(15000))
                {
                    var j = await ApiPost("_api/graphql",
                        new { operationName = "HiloActiveBet", query = gql, variables = new { } },
                        cts.Token);

                    // Response: data → user → activeCasinoBet (null when no active round)
                    var bet = j["data"]?["user"]?["activeCasinoBet"];
                    if (bet == null || bet.Type == Newtonsoft.Json.Linq.JTokenType.Null)
                        return;

                    bool active = bet["active"]?.Value<bool>() ?? false;
                    if (!active) return;

                    _hiloActiveBetId  = bet["id"]?.Value<string>();
                    _hiloManualActive = true;

                    ClearHiloCards();

                    var state    = bet["state"];
                    string sRank = state?["startCard"]?["rank"]?.Value<string>() ?? "?";
                    string sSuit = state?["startCard"]?["suit"]?.Value<string>() ?? "?";
                    AddHiloStartCard(sRank, sSuit);

                    var rounds = state?["rounds"] as Newtonsoft.Json.Linq.JArray;
                    if (rounds != null)
                        foreach (var r in rounds)
                        {
                            string rank  = r["card"]?["rank"]?.Value<string>() ?? "?";
                            string suit  = r["card"]?["suit"]?.Value<string>() ?? "?";
                            double multi = r["payoutMultiplier"]?.Value<double>() ?? 0.0;
                            AddHiloCard(rank, suit, multi);
                        }

                    SetManualHiloButtons(true);
                    AppendLog($"[HILO] Active game loaded — {_hiloList.Count} card(s). Use manual buttons to finish or cash out.");
                }
            }
            catch (Exception ex)
            {
                AppendLog("[HILO] Could not check for active game: " + ex.Message);
            }
        }

        // ═══════════════════════════════════════════════════════════
        //  HILO — MANUAL BUTTON HELPERS
        // ═══════════════════════════════════════════════════════════

        /// <summary>All manual-play buttons are always enabled. Parameter kept for call-site compatibility.</summary>
        private void SetManualHiloButtons(bool enabled)
        {
            if (btnManualHigh.InvokeRequired)
            {
                btnManualHigh.Invoke(new Action(() => SetManualHiloButtons(enabled)));
                return;
            }
            btnManualHigh.Enabled    = true;
            btnManualLow.Enabled     = true;
            btnManualEqual.Enabled   = true;
            btnManualSkip.Enabled    = true;
            btnManualCashout.Enabled = true;
            btnManualNewGame.Enabled = true;
        }

        // ═══════════════════════════════════════════════════════════
        //  HILO — BUTTON CLICK HANDLERS
        // ═══════════════════════════════════════════════════════════

        private void BtnManualHigh_Click(object sender, EventArgs e)
        {
            //if (!_hiloManualActive || _hiloList.Count == 0) return;
            string last = _hiloList[_hiloList.Count - 1];
            if (last == "K") return;  // can't go higher than K — do nothing
            string guess = last == "A" ? "higher" : "higherEqual";
            _ = ManualHiloNextAsync(guess);
        }

        private void BtnManualLow_Click(object sender, EventArgs e)
        {
            //if (!_hiloManualActive || _hiloList.Count == 0) return;
            string last = _hiloList[_hiloList.Count - 1];
            if (last == "A") return;  // can't go lower than A — do nothing
            string guess = last == "K" ? "lower" : "lowerEqual";
            _ = ManualHiloNextAsync(guess);
        }

        private void BtnManualEqual_Click(object sender, EventArgs e)
        {
            //if (!_hiloManualActive || _hiloList.Count == 0) return;
            _ = ManualHiloNextAsync("equal");
        }

        private void BtnManualSkip_Click(object sender, EventArgs e)
        {
            //if (!_hiloManualActive || _hiloList.Count == 0) return;
            _ = ManualHiloNextAsync("skip");
        }

        private void BtnManualCashout_Click(object sender, EventArgs e)
        {
            //if (!_hiloManualActive) return;
            _ = ManualHiloCashoutAsync();
        }

        private void BtnManualNewGame_Click(object sender, EventArgs e)
        {
            // Guard: don't allow a new manual game while the bot is running
            if (btnStop.Enabled)
            {
                AppendLog("[HILO] Stop the bot before starting a manual game.");
                return;
            }
            _ = ManualHiloNewGameAsync();
        }

        // ═══════════════════════════════════════════════════════════
        //  HILO — CORE ASYNC MANUAL ACTIONS
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Places a fresh HiLo manual bet using the Rank/Suit configured in the
        /// HiLo panel.  Requires no active game to be open.
        /// </summary>
        private async Task ManualHiloNewGameAsync()
        {
            btnManualNewGame.Enabled = true;
            try
            {
                string scRank = cmbHiloStartCard.SelectedItem?.ToString()?.Trim().ToUpper() ?? "";
                string scSuit = cmbHiloSuit.SelectedItem?.ToString()?.Trim().ToUpper()      ?? "";

                const string gql =
                    "mutation HiloBet($amount: Float!, $currency: CurrencyEnum!, $startCard: HiloBetStartCardInput!) {\n" +
                    "  hiloBet(amount: $amount, currency: $currency, startCard: $startCard) {\n" +
                    "    ...CasinoBetFragment\n    state { ...HiloStateFragment __typename }\n    __typename\n  }\n}\n" +
                    HiloGqlFragments;

                var variables = new
                {
                    amount    = (double)nudBaseBet.Value,
                    currency  = cmbCurrency.Text,
                    startCard = (!string.IsNullOrEmpty(scRank) && !string.IsNullOrEmpty(scSuit))
                                    ? (object)new { rank = scRank, suit = scSuit }
                                    : null
                };

                using (var cts = new CancellationTokenSource(15000))
                {
                    var j = await ApiPost("_api/graphql",
                        new { operationName = "HiloBet", query = gql, variables }, cts.Token);
                    var (bet, err, errType) = ExtractHiloBet(j);

                    if (err != null)
                    {
                        if (errType == "existingGame" || errType == "emptyResponse")
                        {
                            AppendLog("[HILO] Active game found — loading cards into the strip...");
                            await LoadActiveHiloGameAsync();
                        }
                        else
                        {
                            AppendLog($"[HILO] New game error: {err} ({errType})");
                            btnManualNewGame.Enabled = true;
                        }
                        return;
                    }

                    bool active = bet?["active"]?.Value<bool>() ?? false;
                    if (!active)
                    {
                        AppendLog("[HILO] New game: instant loss on start card.");
                        AddManualBetToHistory(bet, cashout: false);
                        btnManualNewGame.Enabled = true;
                        return;
                    }

                    _hiloActiveBetId  = bet?["id"]?.Value<string>();
                    _hiloManualActive = true;

                    ClearHiloCards();
                    string rank = bet?["state"]?["startCard"]?["rank"]?.Value<string>() ?? "?";
                    string suit = bet?["state"]?["startCard"]?["suit"]?.Value<string>() ?? "?";
                    AddHiloStartCard(rank, suit);

                    SetManualHiloButtons(true);
                    AppendLog($"[HILO] New manual game started — start card: {rank}{suit}");
                }
            }
            catch (Exception ex)
            {
                AppendLog("[HILO] New game failed — checking for existing active game... (" + ex.Message + ")");
                await LoadActiveHiloGameAsync();
                if (!_hiloManualActive) btnManualNewGame.Enabled = true;
            }
        }

        /// <summary>Sends a guess to hiloNext (GraphQL) and updates the card strip.</summary>
        private async Task ManualHiloNextAsync(string guess)
        {
            //if (_hiloNextInFlight) return;
            _hiloNextInFlight = false;
            try
            {
                const string gql =
                    "mutation HiloNext($guess: CasinoGameHiloGuessEnum!) {\n" +
                    "  hiloNext(guess: $guess) {\n" +
                    "    ...CasinoBetFragment\n    state { ...HiloStateFragment __typename }\n    __typename\n  }\n}\n" +
                    HiloGqlFragments;

                using (var cts = new CancellationTokenSource(15000))
                {
                    var j = await ApiPost("_api/graphql",
                        new { operationName = "HiloNext", query = gql, variables = new { guess } },
                        cts.Token);

                    var (bet, err, errType) = ExtractHiloBet(j);
                    if (err != null)
                    {
                        if (errType == "notFound")
                        {
                            // ── CHANGED: do NOT clear the card strip; just mark the game as over
                            AppendLog("[HILO] Game not found — the round may have expired.");
                            _hiloManualActive = false;
                        }
                        else if (errType == "emptyResponse")
                        {
                            AppendLog("[HILO] Empty response from server — please try again.");
                        }
                        else
                        {
                            AppendLog($"[HILO] {err} ({errType})");
                        }
                        _hiloNextInFlight = false;
                        return;
                    }

                    bool   active = bet?["active"]?.Value<bool>()            ?? false;
                    double pm     = bet?["payoutMultiplier"]?.Value<double>() ?? 0.0;

                    var rounds = bet?["state"]?["rounds"] as Newtonsoft.Json.Linq.JArray;
                    if (rounds != null && rounds.Count > 0)
                    {
                        var last = rounds[rounds.Count - 1];
                        AddHiloCard(
                            last["card"]?["rank"]?.Value<string>() ?? "?",
                            last["card"]?["suit"]?.Value<string>() ?? "?",
                            last["payoutMultiplier"]?.Value<double>() ?? 0.0);
                    }

                    if (!active)
                    {
                        AppendLog($"[HILO] Bust! Multiplier: {pm:F4}x");
                        AddManualBetToHistory(bet, cashout: false);
                        ClearHiloCards();
                        // ── CHANGED: do NOT clear the card strip; leave history visible
                        _hiloManualActive = false;
                    }
                    else
                    {
                        AppendLog($"[HILO] {guess} → {pm:F4}x");
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog("[HILO] Next failed: " + ex.Message);
            }
            finally
            {
                _hiloNextInFlight = false;
            }
        }

        /// <summary>Cashes out the active HiLo round via GraphQL.</summary>
        private async Task ManualHiloCashoutAsync()
        {
            SetManualHiloButtons(false);
            try
            {
                const string gql =
                    "mutation HiloCashout($identifier: String!) {\n" +
                    "  hiloCashout(identifier: $identifier) {\n" +
                    "    ...CasinoBetFragment\n    state { ...HiloStateFragment __typename }\n    __typename\n  }\n}\n" +
                    HiloGqlFragments;

                using (var cts = new CancellationTokenSource(15000))
                {
                    var j = await ApiPost("_api/graphql",
                        new { operationName = "HiloCashout", query = gql, variables = new { identifier = RandomString(20) } },
                        cts.Token);

                    var (bet, err, errType) = ExtractHiloBet(j);
                    if (err != null)
                    {
                        if (errType == "hiloNoRoundsPlayed")
                        {
                            AppendLog("[HILO] Cashout not available until at least one guess is made.");
                            SetManualHiloButtons(true);
                        }
                        else if (errType == "emptyResponse")
                        {
                            AppendLog("[HILO] Empty response from server — please try again.");
                            SetManualHiloButtons(true);
                        }
                        else
                        {
                            AppendLog($"[HILO] Cashout error: {err} ({errType})");
                            ClearHiloCards(); _hiloManualActive = false;
                        }
                        return;
                    }

                    decimal payout = bet?["payout"]?.Value<decimal>()           ?? 0m;
                    decimal amount = bet?["amount"]?.Value<decimal>()            ?? 0m;
                    double  multi  = bet?["payoutMultiplier"]?.Value<double>()   ?? 0.0;
                    decimal profit = payout - amount;

                    AppendLog($"[HILO] Cashed out {multi:F4}x — Profit: {(profit >= 0 ? "+" : "")}{profit:F8}");
                    AddManualBetToHistory(bet, cashout: true);
                    ClearHiloCards();
                    _hiloManualActive = false;
                }
            }
            catch (Exception ex)
            {
                AppendLog("[HILO] Cashout failed: " + ex.Message);
                SetManualHiloButtons(true);
            }
        }
        

        // ── Shared GraphQL fragments used by all three HiLo mutations ──
        private const string HiloGqlFragments =
            "fragment CasinoBetFragment on CasinoBet {\n" +
            "  id active payoutMultiplier amountMultiplier amount payout updatedAt currency game\n" +
            "  user { id name __typename }\n  __typename\n}\n" +
            "fragment HiloStateFragment on CasinoGameHilo {\n" +
            "  startCard { suit rank __typename }\n" +
            "  rounds { card { suit rank __typename } guess payoutMultiplier __typename }\n" +
            "  __typename\n}\n";

        // ── Normalise a HiLo REST or GraphQL JSON response ─────────
        private (Newtonsoft.Json.Linq.JToken bet, string err, string errType)
            ExtractHiloBet(Newtonsoft.Json.Linq.JObject j)
        {
            // Error path (both REST and GraphQL surface errors the same way)
            var errs = j["errors"] as Newtonsoft.Json.Linq.JArray;
            if (errs != null && errs.Count > 0)
                return (null,
                        errs[0]["message"]?.Value<string>()   ?? "Unknown error",
                        errs[0]["errorType"]?.Value<string>() ?? "");

            // REST path: top-level { id, active, … }
            if (j["id"] != null) return (j, null, null);

            // GraphQL path: { data: { hiloNext | hiloCashout | hiloBet | … } }
            var data = j["data"];
            if (data != null)
            {
                var bet = data["hiloNext"]    ??
                          data["hiloCashout"] ??
                          data["hiloBet"];
                if (bet != null && bet.Type != Newtonsoft.Json.Linq.JTokenType.Null)
                    return (bet, null, null);
            }

            return (null, "Empty response", "emptyResponse");
        }

        // ── Record a manual bet in lvHistory via the existing helper ─
        private void AddManualBetToHistory(Newtonsoft.Json.Linq.JToken bet, bool cashout)
        {
            if (bet == null) return;
            decimal amount = bet["amount"]?.Value<decimal>()          ?? 0m;
            decimal payout = bet["payout"]?.Value<decimal>()          ?? 0m;
            double  multi  = bet["payoutMultiplier"]?.Value<double>() ?? 0.0;
            string  betId  = bet["id"]?.Value<string>()               ?? "";
            decimal profit = payout - amount;

            AddBetToHistory(new BetResult
            {
                Game             = "hilo",
                Amount           = amount,
                Payout           = payout,
                PayoutMultiplier = (decimal)multi,
                Profit           = profit,
                Win              = cashout && multi > 0,
                BetId            = betId,
                Extra            = cashout ? "manual-cashout" : "manual-bust",
                RollDisplay      = string.Join(" ", _hiloList)
            });
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _botCts?.Cancel();
            _balanceTimer?.Dispose();
            BrowserFetch.Connected    -= OnWsConnected;
            BrowserFetch.Disconnected -= OnWsDisconnected;
        }

        private void HighestMultiplierButton_Click(object sender, EventArgs e)
        {
            // Recreate only if somehow disposed (normally it just hides on close).
            if (_topMultiForm == null || _topMultiForm.IsDisposed)
            {
                _topMultiForm = new TopMultipliersForm(
                    fetchIid: FetchBetIid,
                    getSite:  () => StakeSite);
                _topMultiForm.Location = new System.Drawing.Point(this.Right + 8, this.Top);
            }

            if (_topMultiForm.Visible)
                _topMultiForm.BringToFront();
            else
                _topMultiForm.Show(this);
        }

        // ── Detects real user scroll events on lvHistory ─────────────────
        private sealed class LvScrollWatcher : NativeWindow, IDisposable
        {
            private const int WM_VSCROLL    = 0x0115;
            private const int WM_MOUSEWHEEL = 0x020A;
            private readonly Action _onScroll;

            public LvScrollWatcher(ListView lv, Action onScroll)
            {
                _onScroll = onScroll;
                AssignHandle(lv.Handle);
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (m.Msg == WM_VSCROLL || m.Msg == WM_MOUSEWHEEL)
                    _onScroll?.Invoke();
            }

            public void Dispose() => ReleaseHandle();
        }
    }

    public class GameItem { public string Key; private string D; public GameItem(string k, string d) { Key = k; D = d; } public override string ToString() => D; }

    public static class ListViewExtensions
    {
        public static void SetDoubleBuffered(this System.Windows.Forms.ListView listView, bool doubleBuffered = true)
        {
            listView.GetType()
                .GetProperty("DoubleBuffered",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(listView, doubleBuffered, null);
        }
    }
}
