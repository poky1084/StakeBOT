using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StakeBotUI
{
    public class ConditionBuilderForm : Form
    {
        // ── Results ───────────────────────────────────────────────
        public List<ConditionBlockData>                      Result             { get; private set; }
        public Dictionary<string, List<ConditionBlockData>> ResultStrategies   { get; private set; }
        public string                                        ResultStrategyName { get; private set; }

        // ── State ─────────────────────────────────────────────────
        private List<ConditionBlockData> _blocks;
        private Dictionary<string, List<ConditionBlockData>> _strategies;

        // ── Controls ──────────────────────────────────────────────
        private TextBox  txtStrategyName;
        private ComboBox cmbStrategies;
        private Button   btnSaveStrategy, btnDeleteStrategy, btnNewStrategy;
        private Button   btnExport, btnImport, btnDeleteAll;
        private ListView lvBlocks;
        private Button   btnAdd, btnEdit, btnRemove, btnUp, btnDown, btnOK, btnCancel;

        // ── Layout constants ──────────────────────────────────────
        private const int StrategyBarHeight = 72;
        private const int ListTop           = StrategyBarHeight + 4;
        private const int ListHeight        = 328;

        public ConditionBuilderForm(
            List<ConditionBlockData> existing,
            Dictionary<string, List<ConditionBlockData>> namedStrategies = null,
            string activeStrategyName = "")
        {
            _blocks     = existing.Select(Clone).ToList();
            _strategies = namedStrategies != null
                ? namedStrategies.ToDictionary(kv => kv.Key, kv => kv.Value.Select(Clone).ToList())
                : new Dictionary<string, List<ConditionBlockData>>();

            Text            = "Condition Builder";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition   = FormStartPosition.CenterParent;
            MaximizeBox     = false;
            Size            = new Size(700, 460);

            BuildStrategyBar(activeStrategyName);
            BuildListView();
            BuildSideButtons();

            RefreshList();
            RefreshStrategyCombo(activeStrategyName);
            
        }

        // ═══════════════════════════════════════════════════════════
        //  Strategy bar (two rows)
        // ═══════════════════════════════════════════════════════════
        private void BuildStrategyBar(string activeName)
        {
            int y1 = 8;
            AddLabel("Strategy:", 8, y1 + 3);

            txtStrategyName = new TextBox
            {
                Location        = new Point(72, y1),
                Size            = new Size(180, 24),
                Text            = activeName
            };
            Controls.Add(txtStrategyName);

            AddLabel("Load:", 260, y1 + 3);

            cmbStrategies = new ComboBox
            {
                Location      = new Point(300, y1),
                Size          = new Size(172, 24),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStrategies.SelectedIndexChanged += CmbStrategies_SelectedIndexChanged;
            Controls.Add(cmbStrategies);

            btnSaveStrategy = SmallBtn("Save",   490, y1, 56);
            btnSaveStrategy.Click += BtnSaveStrategy_Click;

            btnDeleteStrategy = SmallBtn("Delete", 550, y1, 56);
            btnDeleteStrategy.Click += BtnDeleteStrategy_Click;

            btnNewStrategy = SmallBtn("New", 610, y1, 56);
            btnNewStrategy.Click += BtnNewStrategy_Click;

            // Row 2: Export (dropdown) | Import | Delete All
            int y2 = y1 + 32;
            btnExport = SmallBtn("⬆  Export ▾", 8, y2, 120);
            btnExport.Click += BtnExport_Click;

            btnImport = SmallBtn("⬇  Import", 134, y2, 110);
            btnImport.Click += BtnImport_Click;

            btnDeleteAll = SmallBtn("✕  Delete All", 250, y2, 110);
            btnDeleteAll.ForeColor = Color.DarkRed;
            btnDeleteAll.Click += BtnDeleteAll_Click;

            Controls.Add(new Label
            {
                Location  = new Point(8, StrategyBarHeight),
                Size      = new Size(676, 1),
                BackColor = Color.Silver
            });
        }

        // ═══════════════════════════════════════════════════════════
        //  Export — dropdown with two options, both produce [] array
        // ═══════════════════════════════════════════════════════════
        private void BtnExport_Click(object sender, EventArgs e)
        {
            var menu = new ContextMenuStrip();

            // ── Export Current ────────────────────────────────────
            var itemCurrent = new ToolStripMenuItem("EXPORT SELECTED");
            itemCurrent.Click += (s, ev) => ExportCurrent();
            menu.Items.Add(itemCurrent);

            // ── Export All ────────────────────────────────────────
            var itemAll = new ToolStripMenuItem(
                $"EXPORT ALL");
            itemAll.Enabled = _strategies.Count > 0;
            itemAll.Click  += (s, ev) => ExportAll();
            menu.Items.Add(itemAll);

            menu.Show(btnExport, new Point(0, btnExport.Height));
        }

        // ── Export Current: wraps only the active block list in [] ─
        private void ExportCurrent()
        {
            string label = txtStrategyName.Text.Trim();

            using (var dlg = new SaveFileDialog
            {
                Title      = "Export Current Strategy",
                Filter     = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                DefaultExt = "json",
                FileName   = string.IsNullOrEmpty(label) ? "strategy" : label
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    // Single entry in the array
                    var single = new Dictionary<string, List<ConditionBlockData>>
                    {
                        [string.IsNullOrEmpty(label) ? "(unnamed)" : label] = _blocks
                    };
                    File.WriteAllText(dlg.FileName, SerializeCollection(single));
                    //MessageBox.Show($"Exported current strategy to:\n{dlg.FileName}",
                        //"Export Current", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Export failed:\n" + ex.Message,
                        //"Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ── Export All: all saved strategies in [] ─────────────────
        private void ExportAll()
        {
            using (var dlg = new SaveFileDialog
            {
                Title      = "Export All Strategies",
                Filter     = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                DefaultExt = "json",
                FileName   = "strategies"
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    File.WriteAllText(dlg.FileName, SerializeCollection(_strategies));
//MessageBox.Show(
                       //$"{_strategies.Count} strategies exported to:\n{dlg.FileName}",
                        //"Export All", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Export failed:\n" + ex.Message,
                       // "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ═══════════════════════════════════════════════════════════
        //  JSON serialization
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Always produces:
        /// [
        ///   { "label": "...", "blocks": [...], "isDefault": false },
        ///   ...
        /// ]
        /// </summary>
        private static string SerializeCollection(
            Dictionary<string, List<ConditionBlockData>> strategies)
        {
            var arr = new JArray();
            foreach (var kv in strategies)
            {
                arr.Add(new JObject
                {
                    ["label"]     = kv.Key,
                    ["blocks"]    = BlocksToJArray(kv.Value),
                    ["isDefault"] = false
                });
            }
            return arr.ToString(Formatting.Indented);
        }

        private static JArray BlocksToJArray(List<ConditionBlockData> blocks)
        {
            var arr = new JArray();
            foreach (var b in blocks)
            {
                arr.Add(new JObject
                {
                    ["id"]   = b.Id,
                    ["type"] = b.Type,
                    ["on"]   = new JObject
                    {
                        ["type"]       = b.OnType,
                        ["value"]      = b.OnValue,
                        ["betType"]    = b.BetType,
                        ["profitType"] = b.ProfitType
                    },
                    ["do"] = new JObject
                    {
                        ["type"]  = b.DoType,
                        ["value"] = b.DoValue
                    }
                });
            }
            return arr;
        }

        // ═══════════════════════════════════════════════════════════
        //  Import  (handles {} single and [] array formats)
        // ═══════════════════════════════════════════════════════════
        private void BtnImport_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog
            {
                Title  = "Import Strategy",
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    string raw    = File.ReadAllText(dlg.FileName);
                    var    parsed = ParseStrategyFile(raw);

                    if (parsed.Count == 0)
                    {
                        MessageBox.Show("No valid strategies found in the file.",
                            "Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (parsed.Count == 1)
                        ImportSingle(parsed[0].label, parsed[0].blocks);
                    else
                        ImportMultiple(parsed);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Import failed:\n" + ex.Message,
                        "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ImportSingle(string label, List<ConditionBlockData> blocks)
        {
            if (_blocks.Count > 0)
            {
                //var ans = MessageBox.Show(
                   // $"Load \"{label}\" from file?\nThe current block list will be replaced.",
                    //"Import Strategy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //if (ans != DialogResult.Yes) return;
            }

            // Add to the saved-strategy list so it appears in the Load combo
            _strategies[label] = blocks.Select(Clone).ToList();
            RefreshStrategyCombo(label);
            AutoSaveStrategies();

            // Load as the active block list
            _blocks = blocks;
            txtStrategyName.Text = label;
            RefreshList();
            //MessageBox.Show($"Imported \"{label}\" — {blocks.Count} block(s).",
                //"Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ImportMultiple(List<(string label, List<ConditionBlockData> blocks)> parsed)
        {
            string chosen = ShowStrategyPicker(
                parsed.Select(p => p.label).ToList(),
                $"File contains {parsed.Count} strategies.\n" +
                "Select one to load, or click \"Import All\" to add all to the Load dropdown.");

            if (chosen != null)
            {
                // Only import the selected strategy
                var (label, blocks) = parsed.First(p => p.label == chosen);

                if (_strategies.ContainsKey(label))
                {
                    //var ans = MessageBox.Show(
                        //$"Strategy \"{label}\" already exists. Overwrite?",
                        //"Import — Name Conflict", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //if (ans != DialogResult.Yes) return;
                }

                _strategies[label] = blocks.Select(Clone).ToList();
                RefreshStrategyCombo(label);
                AutoSaveStrategies();

                _blocks = blocks.Select(Clone).ToList();
                txtStrategyName.Text = label;
                RefreshList();
                //MessageBox.Show($"Imported \"{label}\" — {blocks.Count} block(s).",
                    //"Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Import All — add every strategy in the file
                var clashes = parsed.Select(p => p.label)
                                    .Where(n => _strategies.ContainsKey(n))
                                    .ToList();

                if (clashes.Count > 0)
                {
                    string list = string.Join(", ", clashes.Select(n => $"\"{n}\""));
                    //var ans = MessageBox.Show(
                        //$"The following strategies already exist and will be overwritten:\n{list}\n\nContinue?",
                        //"Import All — Name Conflict", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //if (ans != DialogResult.Yes) return;
                }

                foreach (var (label, blocks) in parsed)
                    _strategies[label] = blocks.Select(Clone).ToList();

                RefreshStrategyCombo();
                AutoSaveStrategies();
                //MessageBox.Show($"{parsed.Count} strategies added to the Load dropdown.",
                    //"Import All Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ═══════════════════════════════════════════════════════════
        //  Strategy picker dialog
        // ═══════════════════════════════════════════════════════════
        private string ShowStrategyPicker(List<string> names, string prompt)
        {
            string chosen = null;

            using (var f = new Form
            {
                Text            = "Select Strategy",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition   = FormStartPosition.CenterParent,
                MaximizeBox     = false,
                MinimizeBox     = false,
                Size            = new Size(380, 420)
            })
            {
                f.Controls.Add(new Label
                {
                    Text     = prompt,
                    Location = new Point(12, 8),
                    Size     = new Size(344, 48),
                    AutoSize = false
                });

                var lb = new ListBox
                {
                    Location      = new Point(12, 60),
                    Size          = new Size(344, 260),
                    SelectionMode = SelectionMode.One
                };
                foreach (var n in names) lb.Items.Add(n);
                if (lb.Items.Count > 0) lb.SelectedIndex = 0;
                f.Controls.Add(lb);

                Action pick = () =>
                {
                    if (lb.SelectedItem == null) return;
                    chosen = lb.SelectedItem.ToString();
                    f.DialogResult = DialogResult.OK;
                    f.Close();
                };

                var btnLoad = new Button
                {
                    Text = "Load Selected", Location = new Point(12, 332),
                    Size = new Size(160, 30), UseVisualStyleBackColor = true
                };
                btnLoad.Click  += (s, ev) => pick();
                lb.DoubleClick += (s, ev) => pick();
                f.Controls.Add(btnLoad);
                f.AcceptButton = btnLoad;

                var btnSkip = new Button
                {
                    Text = "Import All", Location = new Point(196, 332),
                    Size = new Size(160, 30), UseVisualStyleBackColor = true
                };
                btnSkip.Click += (s, ev) => { f.DialogResult = DialogResult.Cancel; f.Close(); };
                f.Controls.Add(btnSkip);
                f.CancelButton = btnSkip;

                f.ShowDialog(this);
            }

            return chosen;
        }

        // ═══════════════════════════════════════════════════════════
        //  JSON deserialization
        // ═══════════════════════════════════════════════════════════
        private static List<(string label, List<ConditionBlockData> blocks)>
            ParseStrategyFile(string json)
        {
            var result = new List<(string, List<ConditionBlockData>)>();
            var token  = JToken.Parse(json);

            if (token is JArray arr)
                foreach (var item in arr)
                    if (item is JObject obj) result.Add(ParseSingleStrategyObject(obj));
            else if (token is JObject single)
                result.Add(ParseSingleStrategyObject(single));

            return result;
        }

        private static (string label, List<ConditionBlockData> blocks)
            ParseSingleStrategyObject(JObject root)
        {
            string label = root["label"]?.ToString() ?? "(unnamed)";
            var    arr   = root["blocks"] as JArray ?? new JArray();
            var    list  = new List<ConditionBlockData>();

            foreach (var item in arr)
            {
                if (!(item is JObject block)) continue;

                var on    = block["on"]  as JObject ?? new JObject();
                var doObj = block["do"]  as JObject ?? new JObject();

                decimal onVal = 1;
                var onValTok  = on["value"];
                if (onValTok != null && onValTok.Type != JTokenType.Null && onValTok.ToString() != "")
                    try { onVal = onValTok.ToObject<decimal>(); } catch { onVal = 1; }

                list.Add(new ConditionBlockData
                {
                    Id         = block["id"]?.ToString()              ?? Guid.NewGuid().ToString("N").Substring(0, 8),
                    Type       = block["type"]?.ToString()            ?? "bets",
                    OnType     = on["type"]?.ToString()               ?? "every",
                    OnValue    = onVal,
                    BetType    = on["betType"]?.ToString()            ?? "bet",
                    ProfitType = on["profitType"]?.ToString()         ?? "profit",
                    DoType     = doObj["type"]?.ToString()            ?? "stop",
                    DoValue    = doObj["value"]?.ToObject<decimal?>() ?? 0
                });
            }

            return (label, list);
        }

        // ═══════════════════════════════════════════════════════════
        //  Strategy combo
        // ═══════════════════════════════════════════════════════════
        private void RefreshStrategyCombo(string selectName = null)
        {
            cmbStrategies.SelectedIndexChanged -= CmbStrategies_SelectedIndexChanged;
            cmbStrategies.Items.Clear();
            foreach (var name in _strategies.Keys.OrderBy(k => k))
                cmbStrategies.Items.Add(name);

            if (selectName != null && cmbStrategies.Items.Contains(selectName))
                cmbStrategies.SelectedItem = selectName;
            else
                cmbStrategies.SelectedIndex = -1;

            cmbStrategies.SelectedIndexChanged += CmbStrategies_SelectedIndexChanged;
        }

        private void CmbStrategies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStrategies.SelectedIndex < 0) return;
            string name = cmbStrategies.SelectedItem.ToString();

            if (_blocks.Count > 0)
            {
                //var ans = MessageBox.Show(
                   // $"Load strategy \"{name}\"?\nUnsaved changes to the current block list will be replaced.",
                   // "Load Strategy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //if (ans != DialogResult.Yes) return;
            }

            _blocks = _strategies[name].Select(Clone).ToList();
            txtStrategyName.Text = name;
            RefreshList();
        }

        private void BtnSaveStrategy_Click(object sender, EventArgs e)
        {
            string name = txtStrategyName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a strategy name.", "Save Strategy",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStrategyName.Focus();
                return;
            }

            if (_strategies.ContainsKey(name))
            {
               // var ans = MessageBox.Show($"Strategy \"{name}\" already exists. Overwrite?",
                    //"Save Strategy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //if (ans != DialogResult.Yes) return;
            }

            int savedCount = _blocks.Count;
            _strategies[name] = _blocks.Select(Clone).ToList();
            RefreshStrategyCombo(name);
            AutoSaveStrategies();
            //MessageBox.Show($"Strategy \"{name}\" saved ({savedCount} blocks).",
                //"Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Clear the active block list and name ready for a new strategy
            _blocks = new List<ConditionBlockData>();
            txtStrategyName.Text = "";
            RefreshList();
        }

        private void BtnDeleteStrategy_Click(object sender, EventArgs e)
        {
            if (cmbStrategies.SelectedIndex < 0)
            {
               ///MessageBox.Show("Select a strategy from the list to delete.", "Delete Strategy",
                   // MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = cmbStrategies.SelectedItem.ToString();
            //var answer  = MessageBox.Show($"Delete strategy \"{name}\"?",
                //"Delete Strategy", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //if (answer != DialogResult.Yes) return;

            _strategies.Remove(name);
            if (txtStrategyName.Text.Trim() == name) txtStrategyName.Text = "";
            RefreshStrategyCombo();
            AutoSaveStrategies();
        }

        private void BtnDeleteAll_Click(object sender, EventArgs e)
        {
            if (_strategies.Count == 0)
            {
                //MessageBox.Show("There are no saved strategies to delete.", "Delete All",
                    //MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ans = MessageBox.Show(
                $"Delete all {_strategies.Count} saved strategies? This cannot be undone.",
                "Delete All Strategies", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ans != DialogResult.Yes) return;

            _strategies.Clear();
            RefreshStrategyCombo();
            AutoSaveStrategies();
        }

        private void BtnNewStrategy_Click(object sender, EventArgs e)
        {
            if (_blocks.Count > 0)
            {
                //var ans = MessageBox.Show(
                    //"Clear the current block list and start a new strategy?",
                    //"New Strategy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //if (ans != DialogResult.Yes) return;
            }

            _blocks = new List<ConditionBlockData>();
            txtStrategyName.Text = "";
            cmbStrategies.SelectedIndex = -1;
            RefreshList();
        }

        // ═══════════════════════════════════════════════════════════
        //  ListView
        // ═══════════════════════════════════════════════════════════
        private void BuildListView()
        {
            lvBlocks = new ListView
            {
                Location      = new Point(8, ListTop),
                Size          = new Size(560, ListHeight),
                View          = View.Details,
                FullRowSelect = true,
                GridLines     = true,
                MultiSelect   = false,
            };
            lvBlocks.Columns.Add("#",      28);
            lvBlocks.Columns.Add("Type",   52);
            lvBlocks.Columns.Add("When",  200);
            lvBlocks.Columns.Add("Then",  260);
            lvBlocks.DoubleClick += (s, e) => EditSelected();
            Controls.Add(lvBlocks);
        }

        // ═══════════════════════════════════════════════════════════
        //  Side buttons
        // ═══════════════════════════════════════════════════════════
        private void BuildSideButtons()
        {
            int bx = 576, by = ListTop;

            btnAdd    = SideBtn("Add",    bx, by); by += 34; btnAdd.Click    += (s, e) => AddBlock();
            btnEdit   = SideBtn("Edit",   bx, by); by += 34; btnEdit.Click   += (s, e) => EditSelected();
            btnRemove = SideBtn("Remove", bx, by); by += 34; btnRemove.Click += (s, e) => RemoveSelected();
            btnUp     = SideBtn("▲ Up",   bx, by); by += 34; btnUp.Click     += (s, e) => MoveSelected(-1);
            btnDown   = SideBtn("▼ Down", bx, by); by += 42; btnDown.Click   += (s, e) => MoveSelected(1);
            btnOK     = SideBtn("OK",     bx, by); by += 34; btnOK.Click     += BtnOK_Click;
            btnCancel = SideBtn("Cancel", bx, by);            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            AcceptButton = btnOK;
            CancelButton = btnCancel;
        }

        // ═══════════════════════════════════════════════════════════
        //  Block list CRUD
        // ═══════════════════════════════════════════════════════════
        private void RefreshList()
        {
            lvBlocks.Items.Clear();
            for (int i = 0; i < _blocks.Count; i++)
            {
                var b    = _blocks[i];
                var item = new ListViewItem((i + 1).ToString());
                item.SubItems.Add(b.Type);
                item.SubItems.Add(DescribeOn(b));
                item.SubItems.Add(DescribeDo(b));
                item.Tag = i;
                lvBlocks.Items.Add(item);
            }
        }

        private string DescribeOn(ConditionBlockData b) =>
            b.Type == "bets"
                ? $"{b.OnType} {b.BetType} streak of {b.OnValue}"
                : $"{b.ProfitType} {b.OnType} {b.OnValue}";

        private string DescribeDo(ConditionBlockData b)
        {
            switch (b.DoType)
            {
                case "increaseByPercentage":  return $"Increase bet by {b.DoValue}%";
                case "decreaseByPercentage":  return $"Decrease bet by {b.DoValue}%";
                case "increaseWinChanceBy":   return $"Increase chance by {b.DoValue}%";
                case "decreaseWinChanceBy":   return $"Decrease chance by {b.DoValue}%";
                case "addToAmount":           return $"Add {b.DoValue} to bet";
                case "subtractFromAmount":    return $"Subtract {b.DoValue} from bet";
                case "addToWinChance":        return $"Add {b.DoValue} to chance";
                case "subtractFromWinChance": return $"Subtract {b.DoValue} from chance";
                case "setAmount":             return $"Set bet to {b.DoValue}";
                case "setWinChance":          return $"Set chance to {b.DoValue}";
                case "switchOverUnder":       return "Switch Over/Under";
                case "resetAmount":           return "Reset bet to base";
                case "resetWinChance":        return "Reset chance to base";
                case "stop":                  return "Stop bot";
                default:                      return b.DoType;
            }
        }

        private void AddBlock()
        {
            using (var f = new BlockEditorForm())
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    _blocks.Add(f.Result);
                    RefreshList();
                    lvBlocks.Items[_blocks.Count - 1].Selected = true;
                }
        }

        private void EditSelected()
        {
            if (lvBlocks.SelectedItems.Count == 0) return;
            int idx = (int)lvBlocks.SelectedItems[0].Tag;
            using (var f = new BlockEditorForm(_blocks[idx]))
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    _blocks[idx] = f.Result;
                    RefreshList();
                    lvBlocks.Items[idx].Selected = true;
                }
        }

        private void RemoveSelected()
        {
            if (lvBlocks.SelectedItems.Count == 0) return;
            _blocks.RemoveAt((int)lvBlocks.SelectedItems[0].Tag);
            RefreshList();
        }

        private void MoveSelected(int dir)
        {
            if (lvBlocks.SelectedItems.Count == 0) return;
            int idx    = (int)lvBlocks.SelectedItems[0].Tag;
            int newIdx = idx + dir;
            if (newIdx < 0 || newIdx >= _blocks.Count) return;
            var tmp         = _blocks[idx];
            _blocks[idx]    = _blocks[newIdx];
            _blocks[newIdx] = tmp;
            RefreshList();
            lvBlocks.Items[newIdx].Selected = true;
        }
        
        private void BtnOK_Click(object sender, EventArgs e)
        {
            Result             = _blocks;
            ResultStrategies   = _strategies;
            ResultStrategyName = txtStrategyName.Text.Trim();
            DialogResult       = DialogResult.OK;
            Close();
        }

        // ═══════════════════════════════════════════════════════════
        //  Auto-save strategies to disk
        // ═══════════════════════════════════════════════════════════
        private static readonly string AutoSavePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "strategies.json");

        /// <summary>
        /// Persists all saved strategies to strategies.json next to the exe.
        /// Called silently after every add / delete operation.
        /// </summary>
        private void AutoSaveStrategies()
        {
            try
            {
                File.WriteAllText(AutoSavePath, SerializeCollection(_strategies));
            }
            catch { /* never crash the UI for a background save */ }
        }

        // ═══════════════════════════════════════════════════════════
        //  UI helpers
        // ═══════════════════════════════════════════════════════════
        private Button SideBtn(string text, int x, int y)
        {
            var b = new Button { Text = text, Location = new Point(x, y), Size = new Size(100, 28), UseVisualStyleBackColor = true };
            Controls.Add(b);
            return b;
        }

        private Button SmallBtn(string text, int x, int y, int w = 80)
        {
            var b = new Button { Text = text, Location = new Point(x, y), Size = new Size(w, 24), UseVisualStyleBackColor = true };
            Controls.Add(b);
            return b;
        }

        private void AddLabel(string text, int x, int y)
            => Controls.Add(new Label { Text = text, Location = new Point(x, y), AutoSize = true });

        private static ConditionBlockData Clone(ConditionBlockData b) => new ConditionBlockData
        {
            Id = b.Id, Type = b.Type, OnType = b.OnType, OnValue = b.OnValue,
            BetType = b.BetType, ProfitType = b.ProfitType, DoType = b.DoType, DoValue = b.DoValue
        };
    }
}
