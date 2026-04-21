using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StakeBotUI
{
    public class ConditionBuilderForm : Form
    {
        public List<ConditionBlockData> Result { get; private set; }
        private List<ConditionBlockData> _blocks;

        private ListView lvBlocks;
        private Button btnAdd, btnEdit, btnRemove, btnUp, btnDown, btnOK, btnCancel;

        public ConditionBuilderForm(List<ConditionBlockData> existing)
        {
            _blocks         = existing.Select(b => Clone(b)).ToList();
            Text            = "Condition Builder";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition   = FormStartPosition.CenterParent;
            MaximizeBox     = false;
            Size            = new Size(700, 420);

            // ── ListView ─────────────────────────────────────────
            lvBlocks = new ListView
            {
                Location      = new Point(8, 8),
                Size          = new Size(560, 360),
                View          = View.Details,
                FullRowSelect = true,
                GridLines     = true,
                MultiSelect   = false,
            };
            lvBlocks.Columns.Add("#",       28);
            lvBlocks.Columns.Add("Type",    52);
            lvBlocks.Columns.Add("When",   200);
            lvBlocks.Columns.Add("Then",   260);
            lvBlocks.DoubleClick += (s, e) => EditSelected();
            Controls.Add(lvBlocks);

            // ── Buttons ───────────────────────────────────────────
            int bx = 576, by = 8;
            btnAdd    = Btn("Add",    bx, by);    by += 34; btnAdd.Click    += (s, e) => AddBlock();
            btnEdit   = Btn("Edit",   bx, by);    by += 34; btnEdit.Click   += (s, e) => EditSelected();
            btnRemove = Btn("Remove", bx, by);    by += 34; btnRemove.Click += (s, e) => RemoveSelected();
            btnUp     = Btn("▲ Up",   bx, by);    by += 34; btnUp.Click     += (s, e) => MoveSelected(-1);
            btnDown   = Btn("▼ Down", bx, by);    by += 42; btnDown.Click   += (s, e) => MoveSelected(1);

            btnOK     = Btn("OK",     bx, by);    by += 34; btnOK.Click     += BtnOK_Click;
            btnCancel = Btn("Cancel", bx, by);              btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            AcceptButton = btnOK;
            CancelButton = btnCancel;

            RefreshList();
        }

        private Button Btn(string text, int x, int y)
        {
            var b = new Button { Text = text, Location = new Point(x, y), Size = new Size(100, 28), UseVisualStyleBackColor = true };
            Controls.Add(b);
            return b;
        }

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

        private string DescribeOn(ConditionBlockData b)
        {
            if (b.Type == "bets")
                return $"{b.OnType} {b.BetType} streak of {b.OnValue}";
            else
                return $"{b.ProfitType} {b.OnType} {b.OnValue}";
        }

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
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    _blocks.Add(f.Result);
                    RefreshList();
                    lvBlocks.Items[_blocks.Count - 1].Selected = true;
                }
            }
        }

        private void EditSelected()
        {
            if (lvBlocks.SelectedItems.Count == 0) return;
            int idx = (int)lvBlocks.SelectedItems[0].Tag;
            using (var f = new BlockEditorForm(_blocks[idx]))
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    _blocks[idx] = f.Result;
                    RefreshList();
                    lvBlocks.Items[idx].Selected = true;
                }
            }
        }

        private void RemoveSelected()
        {
            if (lvBlocks.SelectedItems.Count == 0) return;
            int idx = (int)lvBlocks.SelectedItems[0].Tag;
            _blocks.RemoveAt(idx);
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
            Result       = _blocks;
            DialogResult = DialogResult.OK;
            Close();
        }

        private static ConditionBlockData Clone(ConditionBlockData b) => new ConditionBlockData
        {
            Id = b.Id, Type = b.Type, OnType = b.OnType, OnValue = b.OnValue,
            BetType = b.BetType, ProfitType = b.ProfitType, DoType = b.DoType, DoValue = b.DoValue
        };
    }
}
