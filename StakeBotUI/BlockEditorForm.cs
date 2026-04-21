using System;
using System.Drawing;
using System.Windows.Forms;

namespace StakeBotUI
{
    public class BlockEditorForm : Form
    {
        public ConditionBlockData Result { get; private set; }

        private ComboBox cmbType, cmbOnType, cmbBetType, cmbProfitType, cmbDoType;
        private NumericUpDown nudOnValue, nudDoValue;
        private Label lblBetType, lblProfitType;
        private Button btnOK, btnCancel;

        private static readonly string[] BetOnTypes    = { "every","everyStreakOf","firstStreakOf","streakGreaterThan","streakLowerThan" };
        private static readonly string[] ProfitOnTypes  = { "greaterThan","greaterThanOrEqualTo","lowerThan","lowerThanOrEqualTo" };
        private static readonly string[] BetTypes       = { "bet","win","lose" };
        private static readonly string[] ProfitTypes    = { "profit","loss","balance" };
        private static readonly string[] DoTypes        = {
            "increaseByPercentage","decreaseByPercentage","increaseWinChanceBy","decreaseWinChanceBy",
            "addToAmount","subtractFromAmount","addToWinChance","subtractFromWinChance",
            "setAmount","setWinChance","switchOverUnder","resetAmount","resetWinChance","stop"
        };

        public BlockEditorForm(ConditionBlockData existing = null)
        {
            Text            = existing == null ? "Add Condition Block" : "Edit Condition Block";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition   = FormStartPosition.CenterParent;
            MaximizeBox     = false;
            MinimizeBox     = false;
            Size            = new Size(420, 280);

            int y = 12;

            // ── Condition Type ────────────────────────────────────
            AddLabel("Condition type:", 12, y);
            cmbType = AddCombo(new[] { "bets", "profit" }, 130, y);
            cmbType.SelectedIndexChanged += (s, e) => UpdateOnSection();
            y += 32;

            // ── ON ────────────────────────────────────────────────
            AddLabel("When:", 12, y);
            cmbOnType = AddCombo(BetOnTypes, 130, y, 180);
            y += 32;

            lblBetType = AddLabel("Bet type:", 12, y);
            cmbBetType = AddCombo(BetTypes, 130, y);

            lblProfitType = AddLabel("Profit of:", 12, y);
            lblProfitType.Visible = false;
            cmbProfitType = AddCombo(ProfitTypes, 130, y);
            cmbProfitType.Visible = false;
            y += 32;

            AddLabel("Value:", 12, y);
            nudOnValue = AddNud(130, y, -99999999m, 99999999m, 0, 8);
            y += 32;

            // ── DO ────────────────────────────────────────────────
            var sep = new Label { Location = new Point(12, y), Size = new Size(380, 1), BackColor = Color.Silver };
            Controls.Add(sep); y += 8;

            AddLabel("Then:", 12, y);
            cmbDoType = AddCombo(DoTypes, 130, y, 220);
            y += 32;

            AddLabel("With value:", 12, y);
            nudDoValue = AddNud(130, y, -99999999m, 99999999m, 0, 4);
            y += 40;

            // ── Buttons ───────────────────────────────────────────
            btnOK = new Button { Text = "OK", Location = new Point(220, y), Size = new Size(80, 28), UseVisualStyleBackColor = true };
            btnOK.Click += BtnOK_Click;
            Controls.Add(btnOK);
            AcceptButton = btnOK;

            btnCancel = new Button { Text = "Cancel", Location = new Point(308, y), Size = new Size(80, 28), UseVisualStyleBackColor = true };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(btnCancel);
            CancelButton = btnCancel;

            // Populate from existing block
            if (existing != null) Populate(existing);
            else { cmbType.SelectedIndex = 0; cmbDoType.SelectedIndex = 0; }
            UpdateOnSection();
        }

        private void Populate(ConditionBlockData b)
        {
            SetCombo(cmbType,       b.Type);
            SetCombo(cmbBetType,    b.BetType);
            SetCombo(cmbProfitType, b.ProfitType);
            SetCombo(cmbDoType,     b.DoType);
            nudOnValue.Value = Clamp(b.OnValue, nudOnValue);
            nudDoValue.Value = Clamp(b.DoValue, nudDoValue);

            // OnType combo changes based on type
            string[] onOpts = b.Type == "bets" ? BetOnTypes : ProfitOnTypes;
            cmbOnType.Items.Clear();
            cmbOnType.Items.AddRange(onOpts);
            SetCombo(cmbOnType, b.OnType);
        }

        private void UpdateOnSection()
        {
            bool isBets = cmbType.SelectedItem?.ToString() == "bets";
            lblBetType.Visible    = isBets;
            cmbBetType.Visible    = isBets;
            lblProfitType.Visible = !isBets;
            cmbProfitType.Visible = !isBets;

            string cur = cmbOnType.SelectedItem?.ToString();
            cmbOnType.Items.Clear();
            cmbOnType.Items.AddRange(isBets ? (object[])BetOnTypes : ProfitOnTypes);
            SetCombo(cmbOnType, cur);
            if (cmbOnType.SelectedIndex < 0 && cmbOnType.Items.Count > 0) cmbOnType.SelectedIndex = 0;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            bool doNeedsValue = cmbDoType.SelectedItem?.ToString() != "switchOverUnder"
                             && cmbDoType.SelectedItem?.ToString() != "resetAmount"
                             && cmbDoType.SelectedItem?.ToString() != "resetWinChance"
                             && cmbDoType.SelectedItem?.ToString() != "stop";
            Result = new ConditionBlockData
            {
                Type       = cmbType.SelectedItem?.ToString() ?? "bets",
                OnType     = cmbOnType.SelectedItem?.ToString() ?? "every",
                OnValue    = nudOnValue.Value,
                BetType    = cmbBetType.SelectedItem?.ToString() ?? "lose",
                ProfitType = cmbProfitType.SelectedItem?.ToString() ?? "profit",
                DoType     = cmbDoType.SelectedItem?.ToString() ?? "stop",
                DoValue    = doNeedsValue ? nudDoValue.Value : 0,
            };
            DialogResult = DialogResult.OK;
            Close();
        }

        // ── Helpers ───────────────────────────────────────────────
        private Label AddLabel(string text, int x, int y)
        {
            var l = new Label { Text = text, Location = new Point(x, y + 3), AutoSize = true };
            Controls.Add(l);
            return l;
        }

        private ComboBox AddCombo(string[] items, int x, int y, int w = 160)
        {
            var cb = new ComboBox { Location = new Point(x, y), Size = new Size(w, 24), DropDownStyle = ComboBoxStyle.DropDownList };
            cb.Items.AddRange(items);
            if (cb.Items.Count > 0) cb.SelectedIndex = 0;
            Controls.Add(cb);
            return cb;
        }

        private NumericUpDown AddNud(int x, int y, decimal min, decimal max, decimal val, int dec)
        {
            var n = new NumericUpDown { Location = new Point(x, y), Size = new Size(150, 24), Minimum = min, Maximum = max, DecimalPlaces = dec };
            n.Value = Clamp(val, n);
            Controls.Add(n);
            return n;
        }

        private static void SetCombo(ComboBox cb, string val)
        {
            if (val == null) return;
            foreach (var item in cb.Items) if (item.ToString() == val) { cb.SelectedItem = item; return; }
        }

        private static decimal Clamp(decimal v, NumericUpDown n) => Math.Max(n.Minimum, Math.Min(n.Maximum, v));
    }
}
