using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StakeBotUI
{
    /// <summary>
    /// Floating window that tracks and displays the top-10 highest payout
    /// multipliers seen during the current session.
    /// </summary>
    public partial class TopMultipliersForm : Form
    {
        // ── Entry model ────────────────────────────────────────────
        private class MultiplierEntry
        {
            public string BetId { get; set; }
            public string Game { get; set; }
            public decimal Multiplier { get; set; }
            public int InsertionOrder { get; set; }
            /// <summary>Resolved "casino:xxx" IID — null until the user clicks the row.</summary>
            public string ResolvedIid { get; set; }
        }

        // ── State ──────────────────────────────────────────────────
        private const int MaxEntries = 10;
        private readonly List<MultiplierEntry> _entries = new List<MultiplierEntry>();
        private int _insertionCounter = 0;

        /// <summary>
        /// Delegate that resolves a numeric betId → IID string via GraphQL.
        /// Supplied by Form1 so this form doesn't need its own HTTP stack.
        /// </summary>
        private readonly Func<string, Task<string>> _fetchIid;

        /// <summary>Mirror hostname — reserved for future use.</summary>
        private readonly Func<string> _getSite;

        // ── IID cache (item → resolved IID) ───────────────────────
        private readonly Dictionary<ListViewItem, string> _resolvedIids =
            new Dictionary<ListViewItem, string>();

        /// <summary>BetId of the most recently inserted entry — highlighted in light green.</summary>
        private string _lastAddedBetId;

        // ── Constructor ────────────────────────────────────────────
        public TopMultipliersForm(Func<string, Task<string>> fetchIid, Func<string> getSite)
        {
            _fetchIid = fetchIid;
            _getSite = getSite;
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            lvTop.SetDoubleBuffered(true);
        }

        // ── Public API: called from Form1 on every bet result ──────
        /// <summary>
        /// Thread-safe. Inserts the result into the top-10 list if it qualifies.
        /// </summary>
        public void TryAdd(string betId, string game, decimal multiplier)
        {
            if (string.IsNullOrEmpty(betId) || multiplier <= 0) return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => TryAdd(betId, game, multiplier)));
                return;
            }

            // Already in list? skip
            if (_entries.Any(e => e.BetId == betId)) return;

            bool shouldAdd = _entries.Count < MaxEntries ||
                             multiplier > _entries[_entries.Count - 1].Multiplier;

            // Also add if multiplier is equal to the lowest in the top 10
            if (!shouldAdd && _entries.Count == MaxEntries && multiplier == _entries[_entries.Count - 1].Multiplier)
            {
                shouldAdd = true;
            }

            if (!shouldAdd) return;

            // If list is full and we're adding a new entry (including equal case), remove the lowest
            if (_entries.Count == MaxEntries)
            {
                _entries.RemoveAt(_entries.Count - 1);
            }

            _entries.Add(new MultiplierEntry
            {
                BetId = betId,
                Game = game,
                Multiplier = multiplier,
                InsertionOrder = ++_insertionCounter
            });
            _lastAddedBetId = betId;

            // Sort descending by multiplier; newest first within equal multipliers
            _entries.Sort((a, b) =>
            {
                int cmp = b.Multiplier.CompareTo(a.Multiplier);
                return cmp != 0 ? cmp : b.InsertionOrder.CompareTo(a.InsertionOrder);
            });
            if (_entries.Count > MaxEntries)
                _entries.RemoveAt(_entries.Count - 1);

            RebuildListView();
        }

        // ── Rebuild ListView from _entries ─────────────────────────
        private void RebuildListView()
        {
            lvTop.BeginUpdate();
            lvTop.Items.Clear();
            _resolvedIids.Clear();

            foreach (var entry in _entries)
            {
                var item = new ListViewItem("");          // col 0: BetId (shown as "View")
                item.Tag = entry;
                item.SubItems.Add(entry.Game);            // col 1
                item.SubItems.Add(FormatMultiplier(entry.Multiplier)); // col 2

                // Restore previously resolved IID if available
                if (!string.IsNullOrEmpty(entry.ResolvedIid))
                    _resolvedIids[item] = entry.ResolvedIid;

                lvTop.Items.Add(item);
            }

            lvTop.EndUpdate();
            lvTop.Invalidate();
        }

        private static string FormatMultiplier(decimal m) =>
            m >= 1000 ? m.ToString("#") + "x" : m.ToString("0.####") + "x";

        // ── Owner-draw: header ──────────────────────────────────────
        private void LvTop_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawBackground();
            using (var brush = new SolidBrush(SystemColors.ControlText))
            using (var sf = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.NoWrap
            })
            {
                var r = new Rectangle(e.Bounds.X + 4, e.Bounds.Y, e.Bounds.Width - 4, e.Bounds.Height);
                e.Graphics.DrawString(e.Header.Text, e.Font, brush, r, sf);
            }
        }

        // ── Owner-draw: row background ──────────────────────────────
        private void LvTop_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // sub-item drawing handles everything; just suppress default
        }

        // ── Owner-draw: sub-items ───────────────────────────────────
        private void LvTop_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // Row background — light green for the most recently added entry
            var entry = e.Item.Tag as MultiplierEntry;
            bool isNew = entry != null && entry.BetId == _lastAddedBetId;

            Color rowBg = isNew
                ? Color.FromArgb(210, 245, 215)
                : e.Item.Index % 2 == 0
                    ? Color.White
                    : Color.FromArgb(248, 248, 252);

            using (var bg = new SolidBrush(rowBg))
                e.Graphics.FillRectangle(bg, e.Bounds);

            Color fg = Color.Black;

            if (e.ColumnIndex == 0)  // BetId column
            {
                bool resolved = _resolvedIids.TryGetValue(e.Item, out string iid);
                string display = resolved ? iid : "View";
                Color link = resolved ? Color.DarkSlateBlue : Color.RoyalBlue;

                using (var f = new Font(e.SubItem.Font.FontFamily, resolved ? 10f : e.SubItem.Font.Size, FontStyle.Underline))
                using (var b = new SolidBrush(link))
                {
                    var fmt = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.EllipsisCharacter,
                        FormatFlags = StringFormatFlags.NoWrap
                    };
                    var rect = new Rectangle(e.Bounds.X + 4, e.Bounds.Y, e.Bounds.Width - 4, e.Bounds.Height);
                    e.Graphics.DrawString(display, f, b, rect, fmt);
                }
            }
            else if (e.ColumnIndex == 2)  // Multiplier — colour-code by rank
            {
                Color mc = e.Item.Index == 0 ? Color.FromArgb(184, 134, 11)   // gold  #1
                         : e.Item.Index == 1 ? Color.FromArgb(108, 108, 108)  // silver #2
                         : e.Item.Index == 2 ? Color.FromArgb(149, 87, 56)    // bronze #3
                         : Color.FromArgb(50, 120, 50);                        // green rest

                using (var b = new SolidBrush(mc))
                using (var boldFont = new Font(e.SubItem.Font.FontFamily, 11f, FontStyle.Bold))
                {
                    var fmt = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString(e.SubItem.Text, boldFont, b, e.Bounds, fmt);
                }
            }
            else
            {
                using (var b = new SolidBrush(fg))
                {
                    var fmt = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.EllipsisCharacter,
                        FormatFlags = StringFormatFlags.NoWrap
                    };
                    var rect = new Rectangle(e.Bounds.X + 4, e.Bounds.Y, e.Bounds.Width - 4, e.Bounds.Height);
                    e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, b, rect, fmt);
                }
            }
        }

        // ── Left-click on BetId column → resolve and reveal IID only ──
        private async void LvTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var hit = lvTop.HitTest(e.X, e.Y);
            if (hit.Item == null) return;

            int col = hit.SubItem != null
                ? hit.Item.SubItems.IndexOf(hit.SubItem)
                : GetColumnAtX(e.X);

            if (col != 0) return;

            var entry = hit.Item.Tag as MultiplierEntry;
            if (entry == null || string.IsNullOrEmpty(entry.BetId)) return;

            // Already revealed — nothing to do
            if (!string.IsNullOrEmpty(entry.ResolvedIid)) return;

            lvTop.Cursor = Cursors.WaitCursor;
            try
            {
                string raw = await _fetchIid(entry.BetId);
                if (!string.IsNullOrEmpty(raw))
                    entry.ResolvedIid = raw.Replace("house:", "casino:");
            }
            catch { }
            finally { lvTop.Cursor = Cursors.Default; }

            if (!string.IsNullOrEmpty(entry.ResolvedIid))
            {
                _resolvedIids[hit.Item] = entry.ResolvedIid;
                int needed = TextRenderer.MeasureText(entry.ResolvedIid, lvTop.Font).Width + 16;
                if (needed > lvTop.Columns[0].Width)
                    lvTop.Columns[0].Width = Math.Min(needed, 260);
                lvTop.Invalidate();
            }
        }

        // ── Right-click anywhere on a row → Copy ID menu only ───────
        private async void LvTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            var hit = lvTop.HitTest(e.X, e.Y);
            if (hit.Item == null) return;

            var entry = hit.Item.Tag as MultiplierEntry;
            if (entry == null || string.IsNullOrEmpty(entry.BetId)) return;

            // Resolve IID on demand so the copy item shows the real IID
            if (string.IsNullOrEmpty(entry.ResolvedIid))
            {
                lvTop.Cursor = Cursors.WaitCursor;
                try
                {
                    string raw = await _fetchIid(entry.BetId);
                    if (!string.IsNullOrEmpty(raw))
                        entry.ResolvedIid = raw.Replace("house:", "casino:");
                }
                catch { }
                finally { lvTop.Cursor = Cursors.Default; }

                if (!string.IsNullOrEmpty(entry.ResolvedIid))
                {
                    _resolvedIids[hit.Item] = entry.ResolvedIid;
                    int needed = TextRenderer.MeasureText(entry.ResolvedIid, lvTop.Font).Width + 16;
                    if (needed > lvTop.Columns[0].Width)
                        lvTop.Columns[0].Width = Math.Min(needed, 260);
                    lvTop.Invalidate();
                }
            }

            string copyText = !string.IsNullOrEmpty(entry.ResolvedIid) ? entry.ResolvedIid : entry.BetId;
            string copyLabel = !string.IsNullOrEmpty(entry.ResolvedIid)
                ? $"📋  Copy IID:  {entry.ResolvedIid}"
                : $"📋  Copy Bet ID:  {entry.BetId}";

            string site = _getSite?.Invoke() ?? "stake.com";
            string openUrl = !string.IsNullOrEmpty(entry.ResolvedIid)
                ? $"https://{site}/?modal=bet&iid={entry.ResolvedIid}"
                : $"https://{site}/?betId={entry.BetId}&modal=bet";

            var menu = new ContextMenuStrip();
            var copyItem = new ToolStripMenuItem(copyLabel);
            copyItem.Click += (s, ev) => Clipboard.SetText(copyText);
            menu.Items.Add(copyItem);

            var openItem = new ToolStripMenuItem("🌐  Open in browser");
            openItem.Click += (s, ev) =>
            {
                try { Process.Start(new ProcessStartInfo(openUrl) { UseShellExecute = true }); }
                catch { }
            };
            menu.Items.Add(openItem);
            menu.Show(lvTop, new Point(e.X, e.Y));
        }

        private int GetColumnAtX(int x)
        {
            int accum = 0;
            for (int i = 0; i < lvTop.Columns.Count; i++)
            {
                accum += lvTop.Columns[i].Width;
                if (x < accum) return i;
            }
            return -1;
        }

        // ── Clear button ────────────────────────────────────────────
        private void BtnClear_Click(object sender, EventArgs e)
        {
            _entries.Clear();
            _resolvedIids.Clear();
            _lastAddedBetId = null;
            _insertionCounter = 0;
            lvTop.Items.Clear();
        }

        // ── Keep window alive on X — just hide it ──────────────────
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                return;
            }
            base.OnFormClosing(e);
        }
    }
}