namespace StakeBotUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.lblApiKeyLabel = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.cmbFetchMode = new System.Windows.Forms.ComboBox();
            this.btnGetCookie = new System.Windows.Forms.Button();
            this.lblCookieStatus = new System.Windows.Forms.Label();
            this.lblWsIndicator = new System.Windows.Forms.Label();
            this.lblWsStatus = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.cmbCurrency = new System.Windows.Forms.ComboBox();
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblMirrorLabel = new System.Windows.Forms.Label();
            this.txtMirror = new System.Windows.Forms.TextBox();
            this.sepTop = new System.Windows.Forms.Label();
            this.lblGameLabel = new System.Windows.Forms.Label();
            this.cmbGame = new System.Windows.Forms.ComboBox();
            this.lblBaseBetLabel = new System.Windows.Forms.Label();
            this.nudBaseBet = new System.Windows.Forms.NumericUpDown();
            this.btnCondition = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblMultSep = new System.Windows.Forms.Label();
            this.chkStopMultiplier = new System.Windows.Forms.CheckBox();
            this.nudStopMultiplier = new System.Windows.Forms.NumericUpDown();
            this.lblMultX = new System.Windows.Forms.Label();
            this.sepToolbar = new System.Windows.Forms.Label();
            this.pnlGameContainer = new System.Windows.Forms.Panel();
            this.lvHistory = new System.Windows.Forms.ListView();
            this.colBetId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGame = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBet = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMulti = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPayout = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colProfit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRoll = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlHiloCards = new System.Windows.Forms.Panel();
            this.lvHiloCards = new System.Windows.Forms.ListView();
            this.sepVertical = new System.Windows.Forms.Label();
            this.sepMid = new System.Windows.Forms.Label();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.panelDice = new System.Windows.Forms.Panel();
            this.lblDiceTitle = new System.Windows.Forms.Label();
            this.lblDiceDir = new System.Windows.Forms.Label();
            this.rbDiceOver = new System.Windows.Forms.RadioButton();
            this.rbDiceUnder = new System.Windows.Forms.RadioButton();
            this.lblDiceChanceLbl = new System.Windows.Forms.Label();
            this.nudDiceChance = new System.Windows.Forms.NumericUpDown();
            this.lblDiceInfo = new System.Windows.Forms.Label();
            this.chkStopDiceResult = new System.Windows.Forms.CheckBox();
            this.nudStopDiceResult = new System.Windows.Forms.NumericUpDown();
            this.lblDiceResultHint = new System.Windows.Forms.Label();
            this.panelLimbo = new System.Windows.Forms.Panel();
            this.lblLimboTitle = new System.Windows.Forms.Label();
            this.lblLimboTarget = new System.Windows.Forms.Label();
            this.nudLimboTarget = new System.Windows.Forms.NumericUpDown();
            this.panelHilo = new System.Windows.Forms.Panel();
            this.lblHiloTitle = new System.Windows.Forms.Label();
            this.lblHiloPatternLbl = new System.Windows.Forms.Label();
            this.txtHiloPattern = new System.Windows.Forms.TextBox();
            this.btnHiloClear = new System.Windows.Forms.Button();
            this.lblHiloHelp = new System.Windows.Forms.Label();
            this.lblHiloStartLbl = new System.Windows.Forms.Label();
            this.lblHiloRankLbl = new System.Windows.Forms.Label();
            this.cmbHiloStartCard = new System.Windows.Forms.ComboBox();
            this.lblHiloSuitLbl = new System.Windows.Forms.Label();
            this.cmbHiloSuit = new System.Windows.Forms.ComboBox();
            this.lblHiloCardHint = new System.Windows.Forms.Label();
            this.lblManualSep = new System.Windows.Forms.Label();
            this.btnManualHigh = new System.Windows.Forms.Button();
            this.btnManualLow = new System.Windows.Forms.Button();
            this.btnManualEqual = new System.Windows.Forms.Button();
            this.btnManualSkip = new System.Windows.Forms.Button();
            this.btnManualCashout = new System.Windows.Forms.Button();
            this.btnManualNewGame = new System.Windows.Forms.Button();
            this.panelMines = new System.Windows.Forms.Panel();
            this.lblMinesTitle = new System.Windows.Forms.Label();
            this.lblMinesCountLbl = new System.Windows.Forms.Label();
            this.nudMinesMines = new System.Windows.Forms.NumericUpDown();
            this.lblMinesFieldsLbl = new System.Windows.Forms.Label();
            this.txtMinesFields = new System.Windows.Forms.TextBox();
            this.lblMinesHint = new System.Windows.Forms.Label();
            this.panelKeno = new System.Windows.Forms.Panel();
            this.lblKenoTitle = new System.Windows.Forms.Label();
            this.lblKenoNumLbl = new System.Windows.Forms.Label();
            this.txtKenoNumbers = new System.Windows.Forms.TextBox();
            this.lblKenoRiskLbl = new System.Windows.Forms.Label();
            this.cmbKenoRisk = new System.Windows.Forms.ComboBox();
            this.panelPlinko = new System.Windows.Forms.Panel();
            this.lblPlinkoTitle = new System.Windows.Forms.Label();
            this.lblPlinkoRowsLbl = new System.Windows.Forms.Label();
            this.nudPlinkoRows = new System.Windows.Forms.NumericUpDown();
            this.lblPlinkoRiskLbl = new System.Windows.Forms.Label();
            this.cmbPlinkoRisk = new System.Windows.Forms.ComboBox();
            this.panelWheel = new System.Windows.Forms.Panel();
            this.lblWheelTitle = new System.Windows.Forms.Label();
            this.lblWheelSegsLbl = new System.Windows.Forms.Label();
            this.nudWheelSegs = new System.Windows.Forms.NumericUpDown();
            this.lblWheelRiskLbl = new System.Windows.Forms.Label();
            this.cmbWheelRisk = new System.Windows.Forms.ComboBox();
            this.panelBaccarat = new System.Windows.Forms.Panel();
            this.lblBaccaratTitle = new System.Windows.Forms.Label();
            this.lblBacBankerLbl = new System.Windows.Forms.Label();
            this.nudBacBanker = new System.Windows.Forms.NumericUpDown();
            this.lblBacPlayerLbl = new System.Windows.Forms.Label();
            this.nudBacPlayer = new System.Windows.Forms.NumericUpDown();
            this.lblBacTieLbl = new System.Windows.Forms.Label();
            this.nudBacTie = new System.Windows.Forms.NumericUpDown();
            this.panelRoulette = new System.Windows.Forms.Panel();
            this.lblRouletteTitle = new System.Windows.Forms.Label();
            this.lblRouletteChipsLbl = new System.Windows.Forms.Label();
            this.txtRouletteChips = new System.Windows.Forms.TextBox();
            this.panelPump = new System.Windows.Forms.Panel();
            this.lblPumpTitle = new System.Windows.Forms.Label();
            this.lblPumpPumpsLbl = new System.Windows.Forms.Label();
            this.nudPumpPumps = new System.Windows.Forms.NumericUpDown();
            this.lblPumpDiffLbl = new System.Windows.Forms.Label();
            this.cmbPumpDiff = new System.Windows.Forms.ComboBox();
            this.panelDragonTower = new System.Windows.Forms.Panel();
            this.lblDragonTitle = new System.Windows.Forms.Label();
            this.lblDragonDiffLbl = new System.Windows.Forms.Label();
            this.cmbDragonDiff = new System.Windows.Forms.ComboBox();
            this.lblDragonEggsLbl = new System.Windows.Forms.Label();
            this.txtDragonEggs = new System.Windows.Forms.TextBox();
            this.panelBars = new System.Windows.Forms.Panel();
            this.lblBarsTitle = new System.Windows.Forms.Label();
            this.lblBarsDiffLbl = new System.Windows.Forms.Label();
            this.cmbBarsDiff = new System.Windows.Forms.ComboBox();
            this.lblBarsTilesLbl = new System.Windows.Forms.Label();
            this.txtBarsTiles = new System.Windows.Forms.TextBox();
            this.panelTomeOfLife = new System.Windows.Forms.Panel();
            this.lblTomeTitle = new System.Windows.Forms.Label();
            this.lblTomeLinesLbl = new System.Windows.Forms.Label();
            this.nudTomeLines = new System.Windows.Forms.NumericUpDown();
            this.panelScarabSpin = new System.Windows.Forms.Panel();
            this.lblScarabTitle = new System.Windows.Forms.Label();
            this.lblScarabLinesLbl = new System.Windows.Forms.Label();
            this.nudScarabLines = new System.Windows.Forms.NumericUpDown();
            this.panelDiamonds = new System.Windows.Forms.Panel();
            this.lblDiamondsTitle = new System.Windows.Forms.Label();
            this.lblDiamondsSelLbl = new System.Windows.Forms.Label();
            this.btnDiamondRed = new System.Windows.Forms.Button();
            this.btnDiamondYellow = new System.Windows.Forms.Button();
            this.btnDiamondGreen = new System.Windows.Forms.Button();
            this.btnDiamondBlue = new System.Windows.Forms.Button();
            this.btnDiamondOrange = new System.Windows.Forms.Button();
            this.btnDiamondPurple = new System.Windows.Forms.Button();
            this.btnDiamondCyan = new System.Windows.Forms.Button();
            this.btnDiamondsClear = new System.Windows.Forms.Button();
            this.lblDiamondColLbl = new System.Windows.Forms.Label();
            this.txtDiamondColors = new System.Windows.Forms.TextBox();
            this.lblDiamondCount = new System.Windows.Forms.Label();
            this.chkStopDiamondsWin = new System.Windows.Forms.CheckBox();
            this.panelCases = new System.Windows.Forms.Panel();
            this.lblCasesTitle = new System.Windows.Forms.Label();
            this.lblCasesDiffLbl = new System.Windows.Forms.Label();
            this.cmbCasesDiff = new System.Windows.Forms.ComboBox();
            this.panelRps = new System.Windows.Forms.Panel();
            this.lblRpsTitle = new System.Windows.Forms.Label();
            this.lblRpsGuessLbl = new System.Windows.Forms.Label();
            this.txtRpsGuesses = new System.Windows.Forms.TextBox();
            this.panelFlip = new System.Windows.Forms.Panel();
            this.lblFlipTitle = new System.Windows.Forms.Label();
            this.lblFlipGuessLbl = new System.Windows.Forms.Label();
            this.txtFlipGuesses = new System.Windows.Forms.TextBox();
            this.panelSnakes = new System.Windows.Forms.Panel();
            this.lblSnakesTitle = new System.Windows.Forms.Label();
            this.lblSnakesDiffLbl = new System.Windows.Forms.Label();
            this.cmbSnakesDiff = new System.Windows.Forms.ComboBox();
            this.lblSnakesRollsLbl = new System.Windows.Forms.Label();
            this.nudSnakesRolls = new System.Windows.Forms.NumericUpDown();
            this.panelDarts = new System.Windows.Forms.Panel();
            this.lblDartsTitle = new System.Windows.Forms.Label();
            this.lblDartsDiffLbl = new System.Windows.Forms.Label();
            this.cmbDartsDiff = new System.Windows.Forms.ComboBox();
            this.panelPacks = new System.Windows.Forms.Panel();
            this.lblPacksTitle = new System.Windows.Forms.Label();
            this.lblPacksNote = new System.Windows.Forms.Label();
            this.panelMoles = new System.Windows.Forms.Panel();
            this.lblMolesTitle = new System.Windows.Forms.Label();
            this.lblMolesMolesLbl = new System.Windows.Forms.Label();
            this.nudMolesMoles = new System.Windows.Forms.NumericUpDown();
            this.lblMolesPicksLbl = new System.Windows.Forms.Label();
            this.txtMolesPicks = new System.Windows.Forms.TextBox();
            this.panelChicken = new System.Windows.Forms.Panel();
            this.lblChickenTitle = new System.Windows.Forms.Label();
            this.lblChickenDiffLbl = new System.Windows.Forms.Label();
            this.cmbChickenDiff = new System.Windows.Forms.ComboBox();
            this.lblChickenStepsLbl = new System.Windows.Forms.Label();
            this.nudChickenSteps = new System.Windows.Forms.NumericUpDown();
            this.panelTarot = new System.Windows.Forms.Panel();
            this.lblTarotTitle = new System.Windows.Forms.Label();
            this.lblTarotDiffLbl = new System.Windows.Forms.Label();
            this.cmbTarotDiff = new System.Windows.Forms.ComboBox();
            this.panelDrill = new System.Windows.Forms.Panel();
            this.lblDrillTitle = new System.Windows.Forms.Label();
            this.lblDrillTargetLbl = new System.Windows.Forms.Label();
            this.nudDrillTarget = new System.Windows.Forms.NumericUpDown();
            this.lblDrillPickLbl = new System.Windows.Forms.Label();
            this.nudDrillPick = new System.Windows.Forms.NumericUpDown();
            this.panelPrimedice = new System.Windows.Forms.Panel();
            this.lblPrimediceTitle = new System.Windows.Forms.Label();
            this.lblPDCondLbl = new System.Windows.Forms.Label();
            this.cmbPDCond = new System.Windows.Forms.ComboBox();
            this.lblPDT1 = new System.Windows.Forms.Label();
            this.nudPDT1 = new System.Windows.Forms.NumericUpDown();
            this.lblPDT2 = new System.Windows.Forms.Label();
            this.nudPDT2 = new System.Windows.Forms.NumericUpDown();
            this.lblPDT3 = new System.Windows.Forms.Label();
            this.nudPDT3 = new System.Windows.Forms.NumericUpDown();
            this.lblPDT4 = new System.Windows.Forms.Label();
            this.nudPDT4 = new System.Windows.Forms.NumericUpDown();
            this.panelBlueSamurai = new System.Windows.Forms.Panel();
            this.lblBlueSamuraiTitle = new System.Windows.Forms.Label();
            this.lblBlueSamuraiNote = new System.Windows.Forms.Label();
            this.panelVideoPoker = new System.Windows.Forms.Panel();
            this.lblVideoPokerTitle = new System.Windows.Forms.Label();
            this.lblVideoPokerNote = new System.Windows.Forms.Label();
            this.lblVideoPokerStratLbl = new System.Windows.Forms.Label();
            this.cmbVideoPokerStrat = new System.Windows.Forms.ComboBox();
            this.lblVideoPokerHelp = new System.Windows.Forms.Label();
            this.panelBlackjack = new System.Windows.Forms.Panel();
            this.lblBlackjackTitle = new System.Windows.Forms.Label();
            this.lblBlackjackNote = new System.Windows.Forms.Label();
            this.FastModeBox = new System.Windows.Forms.ComboBox();
            this.BetDelayNumericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lblBetsPerSec = new System.Windows.Forms.Label();
            this.ResetSeedBtn = new System.Windows.Forms.Button();
            this.HighestMultiplierButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaseBet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStopMultiplier)).BeginInit();
            this.pnlHiloCards.SuspendLayout();
            this.panelDice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDiceChance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStopDiceResult)).BeginInit();
            this.panelLimbo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimboTarget)).BeginInit();
            this.panelHilo.SuspendLayout();
            this.panelMines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinesMines)).BeginInit();
            this.panelKeno.SuspendLayout();
            this.panelPlinko.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlinkoRows)).BeginInit();
            this.panelWheel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWheelSegs)).BeginInit();
            this.panelBaccarat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBacBanker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBacPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBacTie)).BeginInit();
            this.panelRoulette.SuspendLayout();
            this.panelPump.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPumpPumps)).BeginInit();
            this.panelDragonTower.SuspendLayout();
            this.panelBars.SuspendLayout();
            this.panelTomeOfLife.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTomeLines)).BeginInit();
            this.panelScarabSpin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScarabLines)).BeginInit();
            this.panelDiamonds.SuspendLayout();
            this.panelCases.SuspendLayout();
            this.panelRps.SuspendLayout();
            this.panelFlip.SuspendLayout();
            this.panelSnakes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSnakesRolls)).BeginInit();
            this.panelDarts.SuspendLayout();
            this.panelPacks.SuspendLayout();
            this.panelMoles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMolesMoles)).BeginInit();
            this.panelChicken.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudChickenSteps)).BeginInit();
            this.panelTarot.SuspendLayout();
            this.panelDrill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDrillTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDrillPick)).BeginInit();
            this.panelPrimedice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPDT1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPDT2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPDT3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPDT4)).BeginInit();
            this.panelBlueSamurai.SuspendLayout();
            this.panelVideoPoker.SuspendLayout();
            this.panelBlackjack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BetDelayNumericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblApiKeyLabel
            // 
            this.lblApiKeyLabel.AutoSize = true;
            this.lblApiKeyLabel.Location = new System.Drawing.Point(235, 36);
            this.lblApiKeyLabel.Name = "lblApiKeyLabel";
            this.lblApiKeyLabel.Size = new System.Drawing.Size(51, 16);
            this.lblApiKeyLabel.TabIndex = 0;
            this.lblApiKeyLabel.Text = "apikey:";
            // 
            // txtApiKey
            // 
            this.txtApiKey.Location = new System.Drawing.Point(306, 34);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(174, 22);
            this.txtApiKey.TabIndex = 0;
            this.txtApiKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtApiKey.TextChanged += new System.EventHandler(this.txtApiKey_TextChanged);
            // 
            // cmbFetchMode
            // 
            this.cmbFetchMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFetchMode.Items.AddRange(new object[] {
            "Use Cookie",
            "Use Extension"});
            this.cmbFetchMode.Location = new System.Drawing.Point(4, 0);
            this.cmbFetchMode.Name = "cmbFetchMode";
            this.cmbFetchMode.Size = new System.Drawing.Size(151, 24);
            this.cmbFetchMode.TabIndex = 1;
            this.cmbFetchMode.SelectedIndexChanged += new System.EventHandler(this.CmbFetchMode_Changed);
            // 
            // btnGetCookie
            // 
            this.btnGetCookie.Location = new System.Drawing.Point(161, 0);
            this.btnGetCookie.Name = "btnGetCookie";
            this.btnGetCookie.Size = new System.Drawing.Size(112, 32);
            this.btnGetCookie.TabIndex = 2;
            this.btnGetCookie.Text = "Get Cookie";
            this.btnGetCookie.UseVisualStyleBackColor = true;
            this.btnGetCookie.Click += new System.EventHandler(this.BtnGetCookie_Click);
            // 
            // lblCookieStatus
            // 
            this.lblCookieStatus.AutoSize = true;
            this.lblCookieStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblCookieStatus.Location = new System.Drawing.Point(278, 7);
            this.lblCookieStatus.Name = "lblCookieStatus";
            this.lblCookieStatus.Size = new System.Drawing.Size(69, 16);
            this.lblCookieStatus.TabIndex = 3;
            this.lblCookieStatus.Text = "No cookie";
            // 
            // lblWsIndicator
            // 
            this.lblWsIndicator.AutoSize = true;
            this.lblWsIndicator.ForeColor = System.Drawing.Color.Gray;
            this.lblWsIndicator.Location = new System.Drawing.Point(161, 5);
            this.lblWsIndicator.Name = "lblWsIndicator";
            this.lblWsIndicator.Size = new System.Drawing.Size(15, 16);
            this.lblWsIndicator.TabIndex = 4;
            this.lblWsIndicator.Text = "⬤";
            this.lblWsIndicator.Visible = false;
            // 
            // lblWsStatus
            // 
            this.lblWsStatus.AutoSize = true;
            this.lblWsStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblWsStatus.Location = new System.Drawing.Point(180, 6);
            this.lblWsStatus.Name = "lblWsStatus";
            this.lblWsStatus.Size = new System.Drawing.Size(94, 16);
            this.lblWsStatus.TabIndex = 5;
            this.lblWsStatus.Text = "Not connected";
            this.lblWsStatus.Visible = false;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(379, 0);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(66, 29);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // cmbCurrency
            // 
            this.cmbCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCurrency.Location = new System.Drawing.Point(449, 0);
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.Size = new System.Drawing.Size(100, 24);
            this.cmbCurrency.TabIndex = 4;
            this.cmbCurrency.SelectedIndexChanged += new System.EventHandler(this.CmbCurrency_Changed);
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBalance.Location = new System.Drawing.Point(764, 8);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(86, 20);
            this.lblBalance.TabIndex = 6;
            this.lblBalance.Text = "Balance: —";
            // 
            // lblMirrorLabel
            // 
            this.lblMirrorLabel.AutoSize = true;
            this.lblMirrorLabel.Location = new System.Drawing.Point(1, 38);
            this.lblMirrorLabel.Name = "lblMirrorLabel";
            this.lblMirrorLabel.Size = new System.Drawing.Size(44, 16);
            this.lblMirrorLabel.TabIndex = 7;
            this.lblMirrorLabel.Text = "Mirror:";
            // 
            // txtMirror
            // 
            this.txtMirror.Location = new System.Drawing.Point(69, 35);
            this.txtMirror.Name = "txtMirror";
            this.txtMirror.Size = new System.Drawing.Size(147, 22);
            this.txtMirror.TabIndex = 5;
            this.txtMirror.Text = "stake.com";
            this.txtMirror.TextChanged += new System.EventHandler(this.txtMirror_TextChanged);
            // 
            // sepTop
            // 
            this.sepTop.BackColor = System.Drawing.Color.Silver;
            this.sepTop.Location = new System.Drawing.Point(0, 62);
            this.sepTop.Name = "sepTop";
            this.sepTop.Size = new System.Drawing.Size(1100, 1);
            this.sepTop.TabIndex = 8;
            // 
            // lblGameLabel
            // 
            this.lblGameLabel.AutoSize = true;
            this.lblGameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblGameLabel.Location = new System.Drawing.Point(3, 71);
            this.lblGameLabel.Name = "lblGameLabel";
            this.lblGameLabel.Size = new System.Drawing.Size(54, 20);
            this.lblGameLabel.TabIndex = 9;
            this.lblGameLabel.Text = "Game:";
            // 
            // cmbGame
            // 
            this.cmbGame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGame.Location = new System.Drawing.Point(69, 66);
            this.cmbGame.Name = "cmbGame";
            this.cmbGame.Size = new System.Drawing.Size(147, 24);
            this.cmbGame.TabIndex = 6;
            this.cmbGame.SelectedIndexChanged += new System.EventHandler(this.CmbGame_Changed);
            // 
            // lblBaseBetLabel
            // 
            this.lblBaseBetLabel.AutoSize = true;
            this.lblBaseBetLabel.Location = new System.Drawing.Point(235, 72);
            this.lblBaseBetLabel.Name = "lblBaseBetLabel";
            this.lblBaseBetLabel.Size = new System.Drawing.Size(65, 16);
            this.lblBaseBetLabel.TabIndex = 10;
            this.lblBaseBetLabel.Text = "Base Bet:";
            // 
            // nudBaseBet
            // 
            this.nudBaseBet.DecimalPlaces = 8;
            this.nudBaseBet.Location = new System.Drawing.Point(324, 69);
            this.nudBaseBet.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudBaseBet.Name = "nudBaseBet";
            this.nudBaseBet.Size = new System.Drawing.Size(144, 22);
            this.nudBaseBet.TabIndex = 7;
            this.nudBaseBet.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // btnCondition
            // 
            this.btnCondition.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnCondition.Location = new System.Drawing.Point(482, 67);
            this.btnCondition.Name = "btnCondition";
            this.btnCondition.Size = new System.Drawing.Size(145, 33);
            this.btnCondition.TabIndex = 8;
            this.btnCondition.Text = "⚙ Conditions";
            this.btnCondition.UseVisualStyleBackColor = true;
            this.btnCondition.Click += new System.EventHandler(this.BtnCondition_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.White;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(630, 68);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(110, 28);
            this.btnStart.TabIndex = 9;
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Visible = false;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.White;
            this.btnStop.Enabled = false;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(746, 68);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(110, 28);
            this.btnStop.TabIndex = 10;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Visible = false;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // lblMultSep
            // 
            this.lblMultSep.AutoSize = true;
            this.lblMultSep.ForeColor = System.Drawing.Color.Silver;
            this.lblMultSep.Location = new System.Drawing.Point(971, 11);
            this.lblMultSep.Name = "lblMultSep";
            this.lblMultSep.Size = new System.Drawing.Size(16, 16);
            this.lblMultSep.TabIndex = 11;
            this.lblMultSep.Text = "│";
            // 
            // chkStopMultiplier
            // 
            this.chkStopMultiplier.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.chkStopMultiplier.Location = new System.Drawing.Point(877, 71);
            this.chkStopMultiplier.Name = "chkStopMultiplier";
            this.chkStopMultiplier.Size = new System.Drawing.Size(119, 22);
            this.chkStopMultiplier.TabIndex = 12;
            this.chkStopMultiplier.Text = "Stop on X";
            this.chkStopMultiplier.CheckedChanged += new System.EventHandler(this.QueueSave);
            // 
            // nudStopMultiplier
            // 
            this.nudStopMultiplier.DecimalPlaces = 2;
            this.nudStopMultiplier.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudStopMultiplier.Location = new System.Drawing.Point(1002, 70);
            this.nudStopMultiplier.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudStopMultiplier.Name = "nudStopMultiplier";
            this.nudStopMultiplier.Size = new System.Drawing.Size(94, 22);
            this.nudStopMultiplier.TabIndex = 11;
            this.nudStopMultiplier.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudStopMultiplier.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblMultX
            // 
            this.lblMultX.AutoSize = true;
            this.lblMultX.Location = new System.Drawing.Point(1082, 70);
            this.lblMultX.Name = "lblMultX";
            this.lblMultX.Size = new System.Drawing.Size(14, 16);
            this.lblMultX.TabIndex = 13;
            this.lblMultX.Text = "×";
            // 
            // sepToolbar
            // 
            this.sepToolbar.BackColor = System.Drawing.Color.Silver;
            this.sepToolbar.Location = new System.Drawing.Point(0, 100);
            this.sepToolbar.Name = "sepToolbar";
            this.sepToolbar.Size = new System.Drawing.Size(1100, 1);
            this.sepToolbar.TabIndex = 14;
            // 
            // pnlGameContainer
            // 
            this.pnlGameContainer.Location = new System.Drawing.Point(4, 105);
            this.pnlGameContainer.Name = "pnlGameContainer";
            this.pnlGameContainer.Size = new System.Drawing.Size(358, 461);
            this.pnlGameContainer.TabIndex = 15;
            // 
            // lvHistory
            // 
            this.lvHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colBetId,
            this.colNum,
            this.colGame,
            this.colBet,
            this.colMulti,
            this.colPayout,
            this.colProfit,
            this.colRoll,
            this.colInfo});
            this.lvHistory.FullRowSelect = true;
            this.lvHistory.GridLines = true;
            this.lvHistory.HideSelection = false;
            this.lvHistory.Location = new System.Drawing.Point(370, 105);
            this.lvHistory.Name = "lvHistory";
            this.lvHistory.OwnerDraw = true;
            this.lvHistory.Size = new System.Drawing.Size(726, 305);
            this.lvHistory.TabIndex = 50;
            this.lvHistory.UseCompatibleStateImageBehavior = false;
            this.lvHistory.View = System.Windows.Forms.View.Details;
            this.lvHistory.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.LvHistory_DrawColumnHeader);
            this.lvHistory.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.LvHistory_DrawItem);
            this.lvHistory.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.LvHistory_DrawSubItem);
            this.lvHistory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LvHistory_MouseDown);
            this.lvHistory.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LvHistory_MouseUp);
            // 
            // colBetId
            // 
            this.colBetId.Text = "View";
            this.colBetId.Width = 52;
            // 
            // colNum
            // 
            this.colNum.Text = "#";
            this.colNum.Width = 36;
            // 
            // colGame
            // 
            this.colGame.Text = "Game";
            this.colGame.Width = 54;
            // 
            // colBet
            // 
            this.colBet.Text = "Bet";
            this.colBet.Width = 84;
            // 
            // colMulti
            // 
            this.colMulti.Text = "Multi";
            this.colMulti.Width = 56;
            // 
            // colPayout
            // 
            this.colPayout.Text = "Payout";
            this.colPayout.Width = 84;
            // 
            // colProfit
            // 
            this.colProfit.Text = "Profit";
            this.colProfit.Width = 88;
            // 
            // colRoll
            // 
            this.colRoll.Text = "Roll";
            this.colRoll.Width = 80;
            // 
            // colInfo
            // 
            this.colInfo.Text = "Info";
            this.colInfo.Width = 192;
            // 
            // pnlHiloCards
            // 
            this.pnlHiloCards.AutoScroll = true;
            this.pnlHiloCards.BackColor = System.Drawing.Color.White;
            this.pnlHiloCards.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHiloCards.Controls.Add(this.lvHiloCards);
            this.pnlHiloCards.Location = new System.Drawing.Point(370, 105);
            this.pnlHiloCards.Name = "pnlHiloCards";
            this.pnlHiloCards.Size = new System.Drawing.Size(726, 123);
            this.pnlHiloCards.TabIndex = 52;
            this.pnlHiloCards.Visible = false;
            // 
            // lvHiloCards
            // 
            this.lvHiloCards.AutoArrange = false;
            this.lvHiloCards.BackColor = System.Drawing.Color.White;
            this.lvHiloCards.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvHiloCards.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lvHiloCards.ForeColor = System.Drawing.Color.Black;
            this.lvHiloCards.HideSelection = false;
            this.lvHiloCards.LabelWrap = false;
            this.lvHiloCards.Location = new System.Drawing.Point(5, 2);
            this.lvHiloCards.Name = "lvHiloCards";
            this.lvHiloCards.Scrollable = false;
            this.lvHiloCards.Size = new System.Drawing.Size(13, 94);
            this.lvHiloCards.TabIndex = 0;
            this.lvHiloCards.UseCompatibleStateImageBehavior = false;
            this.lvHiloCards.View = System.Windows.Forms.View.Details;
            // 
            // sepVertical
            // 
            this.sepVertical.BackColor = System.Drawing.Color.Silver;
            this.sepVertical.Location = new System.Drawing.Point(366, 105);
            this.sepVertical.Name = "sepVertical";
            this.sepVertical.Size = new System.Drawing.Size(1, 461);
            this.sepVertical.TabIndex = 16;
            // 
            // sepMid
            // 
            this.sepMid.BackColor = System.Drawing.Color.Silver;
            this.sepMid.Location = new System.Drawing.Point(370, 414);
            this.sepMid.Name = "sepMid";
            this.sepMid.Size = new System.Drawing.Size(726, 1);
            this.sepMid.TabIndex = 51;
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.rtbLog.Font = new System.Drawing.Font("Consolas", 8F);
            this.rtbLog.Location = new System.Drawing.Point(370, 465);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLog.Size = new System.Drawing.Size(726, 101);
            this.rtbLog.TabIndex = 51;
            this.rtbLog.Text = "";
            // 
            // panelDice
            // 
            this.panelDice.Controls.Add(this.lblDiceTitle);
            this.panelDice.Controls.Add(this.lblDiceDir);
            this.panelDice.Controls.Add(this.rbDiceOver);
            this.panelDice.Controls.Add(this.rbDiceUnder);
            this.panelDice.Controls.Add(this.lblDiceChanceLbl);
            this.panelDice.Controls.Add(this.nudDiceChance);
            this.panelDice.Controls.Add(this.lblDiceInfo);
            this.panelDice.Controls.Add(this.chkStopDiceResult);
            this.panelDice.Controls.Add(this.nudStopDiceResult);
            this.panelDice.Controls.Add(this.lblDiceResultHint);
            this.panelDice.Location = new System.Drawing.Point(0, 0);
            this.panelDice.Name = "panelDice";
            this.panelDice.Size = new System.Drawing.Size(358, 461);
            this.panelDice.TabIndex = 0;
            // 
            // lblDiceTitle
            // 
            this.lblDiceTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDiceTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblDiceTitle.Location = new System.Drawing.Point(8, 6);
            this.lblDiceTitle.Name = "lblDiceTitle";
            this.lblDiceTitle.Size = new System.Drawing.Size(342, 20);
            this.lblDiceTitle.TabIndex = 0;
            this.lblDiceTitle.Text = "DICE";
            // 
            // lblDiceDir
            // 
            this.lblDiceDir.Location = new System.Drawing.Point(8, 32);
            this.lblDiceDir.Name = "lblDiceDir";
            this.lblDiceDir.Size = new System.Drawing.Size(70, 18);
            this.lblDiceDir.TabIndex = 1;
            this.lblDiceDir.Text = "Direction:";
            // 
            // rbDiceOver
            // 
            this.rbDiceOver.Location = new System.Drawing.Point(80, 30);
            this.rbDiceOver.Name = "rbDiceOver";
            this.rbDiceOver.Size = new System.Drawing.Size(64, 22);
            this.rbDiceOver.TabIndex = 2;
            this.rbDiceOver.Text = "Over";
            this.rbDiceOver.CheckedChanged += new System.EventHandler(this.rbDice_Changed);
            // 
            // rbDiceUnder
            // 
            this.rbDiceUnder.Checked = true;
            this.rbDiceUnder.Location = new System.Drawing.Point(148, 30);
            this.rbDiceUnder.Name = "rbDiceUnder";
            this.rbDiceUnder.Size = new System.Drawing.Size(64, 22);
            this.rbDiceUnder.TabIndex = 3;
            this.rbDiceUnder.TabStop = true;
            this.rbDiceUnder.Text = "Under";
            this.rbDiceUnder.CheckedChanged += new System.EventHandler(this.rbDice_Changed);
            // 
            // lblDiceChanceLbl
            // 
            this.lblDiceChanceLbl.Location = new System.Drawing.Point(8, 58);
            this.lblDiceChanceLbl.Name = "lblDiceChanceLbl";
            this.lblDiceChanceLbl.Size = new System.Drawing.Size(80, 18);
            this.lblDiceChanceLbl.TabIndex = 4;
            this.lblDiceChanceLbl.Text = "Win Chance (%):";
            // 
            // nudDiceChance
            // 
            this.nudDiceChance.DecimalPlaces = 2;
            this.nudDiceChance.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudDiceChance.Location = new System.Drawing.Point(120, 55);
            this.nudDiceChance.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            131072});
            this.nudDiceChance.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudDiceChance.Name = "nudDiceChance";
            this.nudDiceChance.Size = new System.Drawing.Size(110, 22);
            this.nudDiceChance.TabIndex = 5;
            this.nudDiceChance.Value = new decimal(new int[] {
            495,
            0,
            0,
            65536});
            this.nudDiceChance.ValueChanged += new System.EventHandler(this.nudDiceChance_Changed);
            // 
            // lblDiceInfo
            // 
            this.lblDiceInfo.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblDiceInfo.Location = new System.Drawing.Point(8, 83);
            this.lblDiceInfo.Name = "lblDiceInfo";
            this.lblDiceInfo.Size = new System.Drawing.Size(340, 18);
            this.lblDiceInfo.TabIndex = 6;
            // 
            // chkStopDiceResult
            // 
            this.chkStopDiceResult.Location = new System.Drawing.Point(8, 107);
            this.chkStopDiceResult.Name = "chkStopDiceResult";
            this.chkStopDiceResult.Size = new System.Drawing.Size(140, 22);
            this.chkStopDiceResult.TabIndex = 7;
            this.chkStopDiceResult.Text = "Stop on exact roll:";
            this.chkStopDiceResult.CheckedChanged += new System.EventHandler(this.QueueSave);
            // 
            // nudStopDiceResult
            // 
            this.nudStopDiceResult.DecimalPlaces = 2;
            this.nudStopDiceResult.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudStopDiceResult.Location = new System.Drawing.Point(152, 105);
            this.nudStopDiceResult.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            131072});
            this.nudStopDiceResult.Name = "nudStopDiceResult";
            this.nudStopDiceResult.Size = new System.Drawing.Size(100, 22);
            this.nudStopDiceResult.TabIndex = 8;
            this.nudStopDiceResult.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblDiceResultHint
            // 
            this.lblDiceResultHint.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblDiceResultHint.ForeColor = System.Drawing.Color.Gray;
            this.lblDiceResultHint.Location = new System.Drawing.Point(8, 130);
            this.lblDiceResultHint.Name = "lblDiceResultHint";
            this.lblDiceResultHint.Size = new System.Drawing.Size(340, 16);
            this.lblDiceResultHint.TabIndex = 9;
            this.lblDiceResultHint.Text = "(matches rolled number, e.g. 55.67)";
            // 
            // panelLimbo
            // 
            this.panelLimbo.Controls.Add(this.lblLimboTitle);
            this.panelLimbo.Controls.Add(this.lblLimboTarget);
            this.panelLimbo.Controls.Add(this.nudLimboTarget);
            this.panelLimbo.Location = new System.Drawing.Point(0, 0);
            this.panelLimbo.Name = "panelLimbo";
            this.panelLimbo.Size = new System.Drawing.Size(358, 461);
            this.panelLimbo.TabIndex = 0;
            // 
            // lblLimboTitle
            // 
            this.lblLimboTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLimboTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblLimboTitle.Location = new System.Drawing.Point(8, 6);
            this.lblLimboTitle.Name = "lblLimboTitle";
            this.lblLimboTitle.Size = new System.Drawing.Size(342, 20);
            this.lblLimboTitle.TabIndex = 0;
            this.lblLimboTitle.Text = "LIMBO";
            // 
            // lblLimboTarget
            // 
            this.lblLimboTarget.Location = new System.Drawing.Point(8, 32);
            this.lblLimboTarget.Name = "lblLimboTarget";
            this.lblLimboTarget.Size = new System.Drawing.Size(70, 18);
            this.lblLimboTarget.TabIndex = 1;
            this.lblLimboTarget.Text = "Target Multiplier:";
            // 
            // nudLimboTarget
            // 
            this.nudLimboTarget.DecimalPlaces = 4;
            this.nudLimboTarget.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudLimboTarget.Location = new System.Drawing.Point(130, 29);
            this.nudLimboTarget.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudLimboTarget.Minimum = new decimal(new int[] {
            101,
            0,
            0,
            131072});
            this.nudLimboTarget.Name = "nudLimboTarget";
            this.nudLimboTarget.Size = new System.Drawing.Size(110, 22);
            this.nudLimboTarget.TabIndex = 2;
            this.nudLimboTarget.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudLimboTarget.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelHilo
            // 
            this.panelHilo.Controls.Add(this.lblHiloTitle);
            this.panelHilo.Controls.Add(this.lblHiloPatternLbl);
            this.panelHilo.Controls.Add(this.txtHiloPattern);
            this.panelHilo.Controls.Add(this.btnHiloClear);
            this.panelHilo.Controls.Add(this.lblHiloHelp);
            this.panelHilo.Controls.Add(this.lblHiloStartLbl);
            this.panelHilo.Controls.Add(this.lblHiloRankLbl);
            this.panelHilo.Controls.Add(this.cmbHiloStartCard);
            this.panelHilo.Controls.Add(this.lblHiloSuitLbl);
            this.panelHilo.Controls.Add(this.cmbHiloSuit);
            this.panelHilo.Controls.Add(this.lblHiloCardHint);
            this.panelHilo.Controls.Add(this.lblManualSep);
            this.panelHilo.Controls.Add(this.btnManualHigh);
            this.panelHilo.Controls.Add(this.btnManualLow);
            this.panelHilo.Controls.Add(this.btnManualEqual);
            this.panelHilo.Controls.Add(this.btnManualSkip);
            this.panelHilo.Controls.Add(this.btnManualCashout);
            this.panelHilo.Controls.Add(this.btnManualNewGame);
            this.panelHilo.Location = new System.Drawing.Point(0, 0);
            this.panelHilo.Name = "panelHilo";
            this.panelHilo.Size = new System.Drawing.Size(358, 461);
            this.panelHilo.TabIndex = 0;
            // 
            // lblHiloTitle
            // 
            this.lblHiloTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblHiloTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblHiloTitle.Location = new System.Drawing.Point(8, 6);
            this.lblHiloTitle.Name = "lblHiloTitle";
            this.lblHiloTitle.Size = new System.Drawing.Size(342, 20);
            this.lblHiloTitle.TabIndex = 0;
            this.lblHiloTitle.Text = "HILO";
            // 
            // lblHiloPatternLbl
            // 
            this.lblHiloPatternLbl.Location = new System.Drawing.Point(8, 32);
            this.lblHiloPatternLbl.Name = "lblHiloPatternLbl";
            this.lblHiloPatternLbl.Size = new System.Drawing.Size(340, 18);
            this.lblHiloPatternLbl.TabIndex = 1;
            this.lblHiloPatternLbl.Text = "Pattern (comma-sep):";
            // 
            // txtHiloPattern
            // 
            this.txtHiloPattern.Location = new System.Drawing.Point(8, 50);
            this.txtHiloPattern.Name = "txtHiloPattern";
            this.txtHiloPattern.Size = new System.Drawing.Size(290, 22);
            this.txtHiloPattern.TabIndex = 2;
            this.txtHiloPattern.Text = "5,5,5";
            this.txtHiloPattern.TextChanged += new System.EventHandler(this.QueueSave);
            // 
            // btnHiloClear
            // 
            this.btnHiloClear.Location = new System.Drawing.Point(302, 49);
            this.btnHiloClear.Name = "btnHiloClear";
            this.btnHiloClear.Size = new System.Drawing.Size(46, 24);
            this.btnHiloClear.TabIndex = 3;
            this.btnHiloClear.Text = "Clear";
            this.btnHiloClear.UseVisualStyleBackColor = true;
            this.btnHiloClear.Click += new System.EventHandler(this.BtnHiloClear_Click);
            // 
            // lblHiloHelp
            // 
            this.lblHiloHelp.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblHiloHelp.ForeColor = System.Drawing.Color.Gray;
            this.lblHiloHelp.Location = new System.Drawing.Point(8, 78);
            this.lblHiloHelp.Name = "lblHiloHelp";
            this.lblHiloHelp.Size = new System.Drawing.Size(340, 36);
            this.lblHiloHelp.TabIndex = 4;
            this.lblHiloHelp.Text = "0=Low  1=High  2=Equal  3=Random\n4=Lowest Odds (best payout)  5=Highest Odds (saf" +
    "est)  7=Skip";
            // 
            // lblHiloStartLbl
            // 
            this.lblHiloStartLbl.Location = new System.Drawing.Point(8, 120);
            this.lblHiloStartLbl.Name = "lblHiloStartLbl";
            this.lblHiloStartLbl.Size = new System.Drawing.Size(80, 18);
            this.lblHiloStartLbl.TabIndex = 5;
            this.lblHiloStartLbl.Text = "Start Card:";
            // 
            // lblHiloRankLbl
            // 
            this.lblHiloRankLbl.Location = new System.Drawing.Point(8, 144);
            this.lblHiloRankLbl.Name = "lblHiloRankLbl";
            this.lblHiloRankLbl.Size = new System.Drawing.Size(36, 18);
            this.lblHiloRankLbl.TabIndex = 6;
            this.lblHiloRankLbl.Text = "Rank:";
            // 
            // cmbHiloStartCard
            // 
            this.cmbHiloStartCard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHiloStartCard.Items.AddRange(new object[] {
            "A",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "J",
            "Q",
            "K"});
            this.cmbHiloStartCard.Location = new System.Drawing.Point(48, 141);
            this.cmbHiloStartCard.Name = "cmbHiloStartCard";
            this.cmbHiloStartCard.Size = new System.Drawing.Size(80, 24);
            this.cmbHiloStartCard.TabIndex = 7;
            this.cmbHiloStartCard.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblHiloSuitLbl
            // 
            this.lblHiloSuitLbl.Location = new System.Drawing.Point(136, 144);
            this.lblHiloSuitLbl.Name = "lblHiloSuitLbl";
            this.lblHiloSuitLbl.Size = new System.Drawing.Size(30, 18);
            this.lblHiloSuitLbl.TabIndex = 8;
            this.lblHiloSuitLbl.Text = "Suit:";
            // 
            // cmbHiloSuit
            // 
            this.cmbHiloSuit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHiloSuit.Items.AddRange(new object[] {
            "C",
            "H",
            "D",
            "S"});
            this.cmbHiloSuit.Location = new System.Drawing.Point(168, 141);
            this.cmbHiloSuit.Name = "cmbHiloSuit";
            this.cmbHiloSuit.Size = new System.Drawing.Size(90, 24);
            this.cmbHiloSuit.TabIndex = 9;
            this.cmbHiloSuit.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblHiloCardHint
            // 
            this.lblHiloCardHint.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblHiloCardHint.ForeColor = System.Drawing.Color.Gray;
            this.lblHiloCardHint.Location = new System.Drawing.Point(8, 172);
            this.lblHiloCardHint.Name = "lblHiloCardHint";
            this.lblHiloCardHint.Size = new System.Drawing.Size(340, 32);
            this.lblHiloCardHint.TabIndex = 10;
            this.lblHiloCardHint.Text = "Leave Rank blank for random start card.\nSuits: C=Clubs H=Hearts D=Diamonds S=Spad" +
    "es";
            // 
            // lblManualSep
            // 
            this.lblManualSep.AutoSize = true;
            this.lblManualSep.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblManualSep.ForeColor = System.Drawing.Color.Gray;
            this.lblManualSep.Location = new System.Drawing.Point(8, 210);
            this.lblManualSep.Name = "lblManualSep";
            this.lblManualSep.Size = new System.Drawing.Size(296, 17);
            this.lblManualSep.TabIndex = 11;
            this.lblManualSep.Text = "─── Manual Play ───────────────────────────";
            // 
            // btnManualHigh
            // 
            this.btnManualHigh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(230)))), ((int)(((byte)(200)))));
            this.btnManualHigh.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnManualHigh.Location = new System.Drawing.Point(8, 230);
            this.btnManualHigh.Name = "btnManualHigh";
            this.btnManualHigh.Size = new System.Drawing.Size(62, 30);
            this.btnManualHigh.TabIndex = 12;
            this.btnManualHigh.Text = "High";
            this.btnManualHigh.UseVisualStyleBackColor = false;
            this.btnManualHigh.Click += new System.EventHandler(this.BtnManualHigh_Click);
            // 
            // btnManualLow
            // 
            this.btnManualLow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnManualLow.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnManualLow.Location = new System.Drawing.Point(74, 230);
            this.btnManualLow.Name = "btnManualLow";
            this.btnManualLow.Size = new System.Drawing.Size(62, 30);
            this.btnManualLow.TabIndex = 13;
            this.btnManualLow.Text = "Low";
            this.btnManualLow.UseVisualStyleBackColor = false;
            this.btnManualLow.Click += new System.EventHandler(this.BtnManualLow_Click);
            // 
            // btnManualEqual
            // 
            this.btnManualEqual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(245)))));
            this.btnManualEqual.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnManualEqual.Location = new System.Drawing.Point(140, 230);
            this.btnManualEqual.Name = "btnManualEqual";
            this.btnManualEqual.Size = new System.Drawing.Size(62, 30);
            this.btnManualEqual.TabIndex = 14;
            this.btnManualEqual.Text = "Equal";
            this.btnManualEqual.UseVisualStyleBackColor = false;
            this.btnManualEqual.Click += new System.EventHandler(this.BtnManualEqual_Click);
            // 
            // btnManualSkip
            // 
            this.btnManualSkip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(235)))), ((int)(((byte)(200)))));
            this.btnManualSkip.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnManualSkip.Location = new System.Drawing.Point(206, 230);
            this.btnManualSkip.Name = "btnManualSkip";
            this.btnManualSkip.Size = new System.Drawing.Size(62, 30);
            this.btnManualSkip.TabIndex = 15;
            this.btnManualSkip.Text = "Skip";
            this.btnManualSkip.UseVisualStyleBackColor = false;
            this.btnManualSkip.Click += new System.EventHandler(this.BtnManualSkip_Click);
            // 
            // btnManualCashout
            // 
            this.btnManualCashout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(220)))), ((int)(((byte)(200)))));
            this.btnManualCashout.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnManualCashout.ForeColor = System.Drawing.Color.Black;
            this.btnManualCashout.Location = new System.Drawing.Point(92, 268);
            this.btnManualCashout.Name = "btnManualCashout";
            this.btnManualCashout.Size = new System.Drawing.Size(80, 30);
            this.btnManualCashout.TabIndex = 16;
            this.btnManualCashout.Text = "Cashout";
            this.btnManualCashout.UseVisualStyleBackColor = false;
            this.btnManualCashout.Click += new System.EventHandler(this.BtnManualCashout_Click);
            // 
            // btnManualNewGame
            // 
            this.btnManualNewGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(240)))), ((int)(((byte)(255)))));
            this.btnManualNewGame.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnManualNewGame.ForeColor = System.Drawing.Color.Black;
            this.btnManualNewGame.Location = new System.Drawing.Point(8, 268);
            this.btnManualNewGame.Name = "btnManualNewGame";
            this.btnManualNewGame.Size = new System.Drawing.Size(80, 30);
            this.btnManualNewGame.TabIndex = 17;
            this.btnManualNewGame.Text = "New Game";
            this.btnManualNewGame.UseVisualStyleBackColor = false;
            this.btnManualNewGame.Click += new System.EventHandler(this.BtnManualNewGame_Click);
            // 
            // panelMines
            // 
            this.panelMines.Controls.Add(this.lblMinesTitle);
            this.panelMines.Controls.Add(this.lblMinesCountLbl);
            this.panelMines.Controls.Add(this.nudMinesMines);
            this.panelMines.Controls.Add(this.lblMinesFieldsLbl);
            this.panelMines.Controls.Add(this.txtMinesFields);
            this.panelMines.Controls.Add(this.lblMinesHint);
            this.panelMines.Location = new System.Drawing.Point(0, 0);
            this.panelMines.Name = "panelMines";
            this.panelMines.Size = new System.Drawing.Size(358, 461);
            this.panelMines.TabIndex = 0;
            // 
            // lblMinesTitle
            // 
            this.lblMinesTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMinesTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMinesTitle.Location = new System.Drawing.Point(8, 6);
            this.lblMinesTitle.Name = "lblMinesTitle";
            this.lblMinesTitle.Size = new System.Drawing.Size(342, 20);
            this.lblMinesTitle.TabIndex = 0;
            this.lblMinesTitle.Text = "MINES";
            // 
            // lblMinesCountLbl
            // 
            this.lblMinesCountLbl.Location = new System.Drawing.Point(8, 32);
            this.lblMinesCountLbl.Name = "lblMinesCountLbl";
            this.lblMinesCountLbl.Size = new System.Drawing.Size(92, 18);
            this.lblMinesCountLbl.TabIndex = 1;
            this.lblMinesCountLbl.Text = "Mines count:";
            // 
            // nudMinesMines
            // 
            this.nudMinesMines.Location = new System.Drawing.Point(100, 29);
            this.nudMinesMines.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.nudMinesMines.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMinesMines.Name = "nudMinesMines";
            this.nudMinesMines.Size = new System.Drawing.Size(110, 22);
            this.nudMinesMines.TabIndex = 2;
            this.nudMinesMines.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMinesMines.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblMinesFieldsLbl
            // 
            this.lblMinesFieldsLbl.Location = new System.Drawing.Point(8, 57);
            this.lblMinesFieldsLbl.Name = "lblMinesFieldsLbl";
            this.lblMinesFieldsLbl.Size = new System.Drawing.Size(340, 18);
            this.lblMinesFieldsLbl.TabIndex = 3;
            this.lblMinesFieldsLbl.Text = "Tiles to reveal (1-based, comma-sep):";
            // 
            // txtMinesFields
            // 
            this.txtMinesFields.Location = new System.Drawing.Point(8, 75);
            this.txtMinesFields.Name = "txtMinesFields";
            this.txtMinesFields.Size = new System.Drawing.Size(340, 22);
            this.txtMinesFields.TabIndex = 4;
            this.txtMinesFields.Text = "1,2,3";
            this.txtMinesFields.TextChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblMinesHint
            // 
            this.lblMinesHint.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblMinesHint.ForeColor = System.Drawing.Color.Gray;
            this.lblMinesHint.Location = new System.Drawing.Point(8, 102);
            this.lblMinesHint.Name = "lblMinesHint";
            this.lblMinesHint.Size = new System.Drawing.Size(340, 18);
            this.lblMinesHint.TabIndex = 5;
            this.lblMinesHint.Text = "Grid: 1=top-left, 25=bottom-right (5×5, row by row)";
            // 
            // panelKeno
            // 
            this.panelKeno.Controls.Add(this.lblKenoTitle);
            this.panelKeno.Controls.Add(this.lblKenoNumLbl);
            this.panelKeno.Controls.Add(this.txtKenoNumbers);
            this.panelKeno.Controls.Add(this.lblKenoRiskLbl);
            this.panelKeno.Controls.Add(this.cmbKenoRisk);
            this.panelKeno.Location = new System.Drawing.Point(0, 0);
            this.panelKeno.Name = "panelKeno";
            this.panelKeno.Size = new System.Drawing.Size(358, 461);
            this.panelKeno.TabIndex = 0;
            // 
            // lblKenoTitle
            // 
            this.lblKenoTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblKenoTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblKenoTitle.Location = new System.Drawing.Point(8, 6);
            this.lblKenoTitle.Name = "lblKenoTitle";
            this.lblKenoTitle.Size = new System.Drawing.Size(342, 20);
            this.lblKenoTitle.TabIndex = 0;
            this.lblKenoTitle.Text = "KENO";
            // 
            // lblKenoNumLbl
            // 
            this.lblKenoNumLbl.Location = new System.Drawing.Point(8, 32);
            this.lblKenoNumLbl.Name = "lblKenoNumLbl";
            this.lblKenoNumLbl.Size = new System.Drawing.Size(340, 18);
            this.lblKenoNumLbl.TabIndex = 1;
            this.lblKenoNumLbl.Text = "Numbers (1-40, comma-sep):";
            // 
            // txtKenoNumbers
            // 
            this.txtKenoNumbers.Location = new System.Drawing.Point(8, 50);
            this.txtKenoNumbers.Name = "txtKenoNumbers";
            this.txtKenoNumbers.Size = new System.Drawing.Size(340, 22);
            this.txtKenoNumbers.TabIndex = 2;
            this.txtKenoNumbers.Text = "1,2,3,4,5";
            this.txtKenoNumbers.TextChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblKenoRiskLbl
            // 
            this.lblKenoRiskLbl.Location = new System.Drawing.Point(8, 80);
            this.lblKenoRiskLbl.Name = "lblKenoRiskLbl";
            this.lblKenoRiskLbl.Size = new System.Drawing.Size(42, 18);
            this.lblKenoRiskLbl.TabIndex = 3;
            this.lblKenoRiskLbl.Text = "Risk:";
            // 
            // cmbKenoRisk
            // 
            this.cmbKenoRisk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKenoRisk.Items.AddRange(new object[] {
            "low",
            "classic",
            "medium",
            "high"});
            this.cmbKenoRisk.Location = new System.Drawing.Point(50, 77);
            this.cmbKenoRisk.Name = "cmbKenoRisk";
            this.cmbKenoRisk.Size = new System.Drawing.Size(120, 24);
            this.cmbKenoRisk.TabIndex = 4;
            this.cmbKenoRisk.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelPlinko
            // 
            this.panelPlinko.Controls.Add(this.lblPlinkoTitle);
            this.panelPlinko.Controls.Add(this.lblPlinkoRowsLbl);
            this.panelPlinko.Controls.Add(this.nudPlinkoRows);
            this.panelPlinko.Controls.Add(this.lblPlinkoRiskLbl);
            this.panelPlinko.Controls.Add(this.cmbPlinkoRisk);
            this.panelPlinko.Location = new System.Drawing.Point(0, 0);
            this.panelPlinko.Name = "panelPlinko";
            this.panelPlinko.Size = new System.Drawing.Size(358, 461);
            this.panelPlinko.TabIndex = 0;
            // 
            // lblPlinkoTitle
            // 
            this.lblPlinkoTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPlinkoTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblPlinkoTitle.Location = new System.Drawing.Point(8, 6);
            this.lblPlinkoTitle.Name = "lblPlinkoTitle";
            this.lblPlinkoTitle.Size = new System.Drawing.Size(342, 20);
            this.lblPlinkoTitle.TabIndex = 0;
            this.lblPlinkoTitle.Text = "PLINKO";
            // 
            // lblPlinkoRowsLbl
            // 
            this.lblPlinkoRowsLbl.Location = new System.Drawing.Point(8, 32);
            this.lblPlinkoRowsLbl.Name = "lblPlinkoRowsLbl";
            this.lblPlinkoRowsLbl.Size = new System.Drawing.Size(50, 18);
            this.lblPlinkoRowsLbl.TabIndex = 1;
            this.lblPlinkoRowsLbl.Text = "Rows:";
            // 
            // nudPlinkoRows
            // 
            this.nudPlinkoRows.Location = new System.Drawing.Point(58, 29);
            this.nudPlinkoRows.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nudPlinkoRows.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nudPlinkoRows.Name = "nudPlinkoRows";
            this.nudPlinkoRows.Size = new System.Drawing.Size(110, 22);
            this.nudPlinkoRows.TabIndex = 2;
            this.nudPlinkoRows.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nudPlinkoRows.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblPlinkoRiskLbl
            // 
            this.lblPlinkoRiskLbl.Location = new System.Drawing.Point(8, 58);
            this.lblPlinkoRiskLbl.Name = "lblPlinkoRiskLbl";
            this.lblPlinkoRiskLbl.Size = new System.Drawing.Size(42, 18);
            this.lblPlinkoRiskLbl.TabIndex = 3;
            this.lblPlinkoRiskLbl.Text = "Risk:";
            // 
            // cmbPlinkoRisk
            // 
            this.cmbPlinkoRisk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlinkoRisk.Items.AddRange(new object[] {
            "low",
            "medium",
            "high",
            "expert"});
            this.cmbPlinkoRisk.Location = new System.Drawing.Point(50, 55);
            this.cmbPlinkoRisk.Name = "cmbPlinkoRisk";
            this.cmbPlinkoRisk.Size = new System.Drawing.Size(120, 24);
            this.cmbPlinkoRisk.TabIndex = 4;
            this.cmbPlinkoRisk.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelWheel
            // 
            this.panelWheel.Controls.Add(this.lblWheelTitle);
            this.panelWheel.Controls.Add(this.lblWheelSegsLbl);
            this.panelWheel.Controls.Add(this.nudWheelSegs);
            this.panelWheel.Controls.Add(this.lblWheelRiskLbl);
            this.panelWheel.Controls.Add(this.cmbWheelRisk);
            this.panelWheel.Location = new System.Drawing.Point(0, 0);
            this.panelWheel.Name = "panelWheel";
            this.panelWheel.Size = new System.Drawing.Size(358, 461);
            this.panelWheel.TabIndex = 0;
            // 
            // lblWheelTitle
            // 
            this.lblWheelTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblWheelTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblWheelTitle.Location = new System.Drawing.Point(8, 6);
            this.lblWheelTitle.Name = "lblWheelTitle";
            this.lblWheelTitle.Size = new System.Drawing.Size(342, 20);
            this.lblWheelTitle.TabIndex = 0;
            this.lblWheelTitle.Text = "WHEEL";
            // 
            // lblWheelSegsLbl
            // 
            this.lblWheelSegsLbl.Location = new System.Drawing.Point(8, 32);
            this.lblWheelSegsLbl.Name = "lblWheelSegsLbl";
            this.lblWheelSegsLbl.Size = new System.Drawing.Size(68, 18);
            this.lblWheelSegsLbl.TabIndex = 1;
            this.lblWheelSegsLbl.Text = "Segments:";
            // 
            // nudWheelSegs
            // 
            this.nudWheelSegs.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudWheelSegs.Location = new System.Drawing.Point(76, 29);
            this.nudWheelSegs.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudWheelSegs.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudWheelSegs.Name = "nudWheelSegs";
            this.nudWheelSegs.Size = new System.Drawing.Size(110, 22);
            this.nudWheelSegs.TabIndex = 2;
            this.nudWheelSegs.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudWheelSegs.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblWheelRiskLbl
            // 
            this.lblWheelRiskLbl.Location = new System.Drawing.Point(8, 58);
            this.lblWheelRiskLbl.Name = "lblWheelRiskLbl";
            this.lblWheelRiskLbl.Size = new System.Drawing.Size(42, 18);
            this.lblWheelRiskLbl.TabIndex = 3;
            this.lblWheelRiskLbl.Text = "Risk:";
            // 
            // cmbWheelRisk
            // 
            this.cmbWheelRisk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWheelRisk.Items.AddRange(new object[] {
            "low",
            "medium",
            "high"});
            this.cmbWheelRisk.Location = new System.Drawing.Point(50, 55);
            this.cmbWheelRisk.Name = "cmbWheelRisk";
            this.cmbWheelRisk.Size = new System.Drawing.Size(120, 24);
            this.cmbWheelRisk.TabIndex = 4;
            this.cmbWheelRisk.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelBaccarat
            // 
            this.panelBaccarat.Controls.Add(this.lblBaccaratTitle);
            this.panelBaccarat.Controls.Add(this.lblBacBankerLbl);
            this.panelBaccarat.Controls.Add(this.nudBacBanker);
            this.panelBaccarat.Controls.Add(this.lblBacPlayerLbl);
            this.panelBaccarat.Controls.Add(this.nudBacPlayer);
            this.panelBaccarat.Controls.Add(this.lblBacTieLbl);
            this.panelBaccarat.Controls.Add(this.nudBacTie);
            this.panelBaccarat.Location = new System.Drawing.Point(0, 0);
            this.panelBaccarat.Name = "panelBaccarat";
            this.panelBaccarat.Size = new System.Drawing.Size(358, 461);
            this.panelBaccarat.TabIndex = 0;
            // 
            // lblBaccaratTitle
            // 
            this.lblBaccaratTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBaccaratTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblBaccaratTitle.Location = new System.Drawing.Point(8, 6);
            this.lblBaccaratTitle.Name = "lblBaccaratTitle";
            this.lblBaccaratTitle.Size = new System.Drawing.Size(342, 20);
            this.lblBaccaratTitle.TabIndex = 0;
            this.lblBaccaratTitle.Text = "BACCARAT";
            // 
            // lblBacBankerLbl
            // 
            this.lblBacBankerLbl.Location = new System.Drawing.Point(8, 32);
            this.lblBacBankerLbl.Name = "lblBacBankerLbl";
            this.lblBacBankerLbl.Size = new System.Drawing.Size(54, 18);
            this.lblBacBankerLbl.TabIndex = 1;
            this.lblBacBankerLbl.Text = "Banker:";
            // 
            // nudBacBanker
            // 
            this.nudBacBanker.DecimalPlaces = 8;
            this.nudBacBanker.Increment = new decimal(new int[] {
            1,
            0,
            0,
            524288});
            this.nudBacBanker.Location = new System.Drawing.Point(62, 29);
            this.nudBacBanker.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudBacBanker.Name = "nudBacBanker";
            this.nudBacBanker.Size = new System.Drawing.Size(140, 22);
            this.nudBacBanker.TabIndex = 2;
            this.nudBacBanker.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblBacPlayerLbl
            // 
            this.lblBacPlayerLbl.Location = new System.Drawing.Point(8, 58);
            this.lblBacPlayerLbl.Name = "lblBacPlayerLbl";
            this.lblBacPlayerLbl.Size = new System.Drawing.Size(54, 18);
            this.lblBacPlayerLbl.TabIndex = 3;
            this.lblBacPlayerLbl.Text = "Player:";
            // 
            // nudBacPlayer
            // 
            this.nudBacPlayer.DecimalPlaces = 8;
            this.nudBacPlayer.Increment = new decimal(new int[] {
            1,
            0,
            0,
            524288});
            this.nudBacPlayer.Location = new System.Drawing.Point(62, 55);
            this.nudBacPlayer.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudBacPlayer.Name = "nudBacPlayer";
            this.nudBacPlayer.Size = new System.Drawing.Size(140, 22);
            this.nudBacPlayer.TabIndex = 4;
            this.nudBacPlayer.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblBacTieLbl
            // 
            this.lblBacTieLbl.Location = new System.Drawing.Point(8, 84);
            this.lblBacTieLbl.Name = "lblBacTieLbl";
            this.lblBacTieLbl.Size = new System.Drawing.Size(54, 18);
            this.lblBacTieLbl.TabIndex = 5;
            this.lblBacTieLbl.Text = "Tie:";
            // 
            // nudBacTie
            // 
            this.nudBacTie.DecimalPlaces = 8;
            this.nudBacTie.Increment = new decimal(new int[] {
            1,
            0,
            0,
            524288});
            this.nudBacTie.Location = new System.Drawing.Point(62, 81);
            this.nudBacTie.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudBacTie.Name = "nudBacTie";
            this.nudBacTie.Size = new System.Drawing.Size(140, 22);
            this.nudBacTie.TabIndex = 6;
            this.nudBacTie.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelRoulette
            // 
            this.panelRoulette.Controls.Add(this.lblRouletteTitle);
            this.panelRoulette.Controls.Add(this.lblRouletteChipsLbl);
            this.panelRoulette.Controls.Add(this.txtRouletteChips);
            this.panelRoulette.Location = new System.Drawing.Point(0, 0);
            this.panelRoulette.Name = "panelRoulette";
            this.panelRoulette.Size = new System.Drawing.Size(358, 461);
            this.panelRoulette.TabIndex = 0;
            // 
            // lblRouletteTitle
            // 
            this.lblRouletteTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRouletteTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblRouletteTitle.Location = new System.Drawing.Point(8, 6);
            this.lblRouletteTitle.Name = "lblRouletteTitle";
            this.lblRouletteTitle.Size = new System.Drawing.Size(342, 20);
            this.lblRouletteTitle.TabIndex = 0;
            this.lblRouletteTitle.Text = "ROULETTE";
            // 
            // lblRouletteChipsLbl
            // 
            this.lblRouletteChipsLbl.Location = new System.Drawing.Point(8, 32);
            this.lblRouletteChipsLbl.Name = "lblRouletteChipsLbl";
            this.lblRouletteChipsLbl.Size = new System.Drawing.Size(340, 18);
            this.lblRouletteChipsLbl.TabIndex = 1;
            this.lblRouletteChipsLbl.Text = "Chips JSON:";
            // 
            // txtRouletteChips
            // 
            this.txtRouletteChips.Location = new System.Drawing.Point(8, 50);
            this.txtRouletteChips.Name = "txtRouletteChips";
            this.txtRouletteChips.Size = new System.Drawing.Size(340, 22);
            this.txtRouletteChips.TabIndex = 2;
            this.txtRouletteChips.Text = "[{\"value\":\"colorBlack\",\"amount\":0.0001}]";
            this.txtRouletteChips.TextChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelPump
            // 
            this.panelPump.Controls.Add(this.lblPumpTitle);
            this.panelPump.Controls.Add(this.lblPumpPumpsLbl);
            this.panelPump.Controls.Add(this.nudPumpPumps);
            this.panelPump.Controls.Add(this.lblPumpDiffLbl);
            this.panelPump.Controls.Add(this.cmbPumpDiff);
            this.panelPump.Location = new System.Drawing.Point(0, 0);
            this.panelPump.Name = "panelPump";
            this.panelPump.Size = new System.Drawing.Size(358, 461);
            this.panelPump.TabIndex = 0;
            // 
            // lblPumpTitle
            // 
            this.lblPumpTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPumpTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblPumpTitle.Location = new System.Drawing.Point(8, 6);
            this.lblPumpTitle.Name = "lblPumpTitle";
            this.lblPumpTitle.Size = new System.Drawing.Size(342, 20);
            this.lblPumpTitle.TabIndex = 0;
            this.lblPumpTitle.Text = "PUMP";
            // 
            // lblPumpPumpsLbl
            // 
            this.lblPumpPumpsLbl.Location = new System.Drawing.Point(8, 32);
            this.lblPumpPumpsLbl.Name = "lblPumpPumpsLbl";
            this.lblPumpPumpsLbl.Size = new System.Drawing.Size(52, 18);
            this.lblPumpPumpsLbl.TabIndex = 1;
            this.lblPumpPumpsLbl.Text = "Pumps:";
            // 
            // nudPumpPumps
            // 
            this.nudPumpPumps.Location = new System.Drawing.Point(60, 29);
            this.nudPumpPumps.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudPumpPumps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPumpPumps.Name = "nudPumpPumps";
            this.nudPumpPumps.Size = new System.Drawing.Size(110, 22);
            this.nudPumpPumps.TabIndex = 2;
            this.nudPumpPumps.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPumpPumps.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblPumpDiffLbl
            // 
            this.lblPumpDiffLbl.Location = new System.Drawing.Point(8, 58);
            this.lblPumpDiffLbl.Name = "lblPumpDiffLbl";
            this.lblPumpDiffLbl.Size = new System.Drawing.Size(70, 18);
            this.lblPumpDiffLbl.TabIndex = 3;
            this.lblPumpDiffLbl.Text = "Difficulty:";
            // 
            // cmbPumpDiff
            // 
            this.cmbPumpDiff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPumpDiff.Items.AddRange(new object[] {
            "easy",
            "medium",
            "hard",
            "expert"});
            this.cmbPumpDiff.Location = new System.Drawing.Point(78, 55);
            this.cmbPumpDiff.Name = "cmbPumpDiff";
            this.cmbPumpDiff.Size = new System.Drawing.Size(130, 24);
            this.cmbPumpDiff.TabIndex = 4;
            this.cmbPumpDiff.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelDragonTower
            // 
            this.panelDragonTower.Controls.Add(this.lblDragonTitle);
            this.panelDragonTower.Controls.Add(this.lblDragonDiffLbl);
            this.panelDragonTower.Controls.Add(this.cmbDragonDiff);
            this.panelDragonTower.Controls.Add(this.lblDragonEggsLbl);
            this.panelDragonTower.Controls.Add(this.txtDragonEggs);
            this.panelDragonTower.Location = new System.Drawing.Point(0, 0);
            this.panelDragonTower.Name = "panelDragonTower";
            this.panelDragonTower.Size = new System.Drawing.Size(358, 461);
            this.panelDragonTower.TabIndex = 0;
            // 
            // lblDragonTitle
            // 
            this.lblDragonTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDragonTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblDragonTitle.Location = new System.Drawing.Point(8, 6);
            this.lblDragonTitle.Name = "lblDragonTitle";
            this.lblDragonTitle.Size = new System.Drawing.Size(342, 20);
            this.lblDragonTitle.TabIndex = 0;
            this.lblDragonTitle.Text = "DRAGON TOWER";
            // 
            // lblDragonDiffLbl
            // 
            this.lblDragonDiffLbl.Location = new System.Drawing.Point(8, 32);
            this.lblDragonDiffLbl.Name = "lblDragonDiffLbl";
            this.lblDragonDiffLbl.Size = new System.Drawing.Size(70, 18);
            this.lblDragonDiffLbl.TabIndex = 1;
            this.lblDragonDiffLbl.Text = "Difficulty:";
            // 
            // cmbDragonDiff
            // 
            this.cmbDragonDiff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDragonDiff.Items.AddRange(new object[] {
            "easy",
            "medium",
            "hard",
            "expert",
            "master"});
            this.cmbDragonDiff.Location = new System.Drawing.Point(78, 29);
            this.cmbDragonDiff.Name = "cmbDragonDiff";
            this.cmbDragonDiff.Size = new System.Drawing.Size(130, 24);
            this.cmbDragonDiff.TabIndex = 2;
            this.cmbDragonDiff.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblDragonEggsLbl
            // 
            this.lblDragonEggsLbl.Location = new System.Drawing.Point(8, 58);
            this.lblDragonEggsLbl.Name = "lblDragonEggsLbl";
            this.lblDragonEggsLbl.Size = new System.Drawing.Size(110, 18);
            this.lblDragonEggsLbl.TabIndex = 3;
            this.lblDragonEggsLbl.Text = "Eggs (0-based):";
            // 
            // txtDragonEggs
            // 
            this.txtDragonEggs.Location = new System.Drawing.Point(118, 55);
            this.txtDragonEggs.Name = "txtDragonEggs";
            this.txtDragonEggs.Size = new System.Drawing.Size(180, 22);
            this.txtDragonEggs.TabIndex = 4;
            this.txtDragonEggs.Text = "0";
            this.txtDragonEggs.TextChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelBars
            // 
            this.panelBars.Controls.Add(this.lblBarsTitle);
            this.panelBars.Controls.Add(this.lblBarsDiffLbl);
            this.panelBars.Controls.Add(this.cmbBarsDiff);
            this.panelBars.Controls.Add(this.lblBarsTilesLbl);
            this.panelBars.Controls.Add(this.txtBarsTiles);
            this.panelBars.Location = new System.Drawing.Point(0, 0);
            this.panelBars.Name = "panelBars";
            this.panelBars.Size = new System.Drawing.Size(358, 461);
            this.panelBars.TabIndex = 0;
            // 
            // lblBarsTitle
            // 
            this.lblBarsTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBarsTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblBarsTitle.Location = new System.Drawing.Point(8, 6);
            this.lblBarsTitle.Name = "lblBarsTitle";
            this.lblBarsTitle.Size = new System.Drawing.Size(342, 20);
            this.lblBarsTitle.TabIndex = 0;
            this.lblBarsTitle.Text = "BARS";
            // 
            // lblBarsDiffLbl
            // 
            this.lblBarsDiffLbl.Location = new System.Drawing.Point(8, 32);
            this.lblBarsDiffLbl.Name = "lblBarsDiffLbl";
            this.lblBarsDiffLbl.Size = new System.Drawing.Size(70, 18);
            this.lblBarsDiffLbl.TabIndex = 1;
            this.lblBarsDiffLbl.Text = "Difficulty:";
            // 
            // cmbBarsDiff
            // 
            this.cmbBarsDiff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBarsDiff.Items.AddRange(new object[] {
            "easy",
            "medium",
            "hard",
            "expert"});
            this.cmbBarsDiff.Location = new System.Drawing.Point(78, 29);
            this.cmbBarsDiff.Name = "cmbBarsDiff";
            this.cmbBarsDiff.Size = new System.Drawing.Size(130, 24);
            this.cmbBarsDiff.TabIndex = 2;
            this.cmbBarsDiff.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblBarsTilesLbl
            // 
            this.lblBarsTilesLbl.Location = new System.Drawing.Point(8, 58);
            this.lblBarsTilesLbl.Name = "lblBarsTilesLbl";
            this.lblBarsTilesLbl.Size = new System.Drawing.Size(44, 18);
            this.lblBarsTilesLbl.TabIndex = 3;
            this.lblBarsTilesLbl.Text = "Tiles:";
            // 
            // txtBarsTiles
            // 
            this.txtBarsTiles.Location = new System.Drawing.Point(52, 55);
            this.txtBarsTiles.Name = "txtBarsTiles";
            this.txtBarsTiles.Size = new System.Drawing.Size(240, 22);
            this.txtBarsTiles.TabIndex = 4;
            this.txtBarsTiles.Text = "2";
            this.txtBarsTiles.TextChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelTomeOfLife
            // 
            this.panelTomeOfLife.Controls.Add(this.lblTomeTitle);
            this.panelTomeOfLife.Controls.Add(this.lblTomeLinesLbl);
            this.panelTomeOfLife.Controls.Add(this.nudTomeLines);
            this.panelTomeOfLife.Location = new System.Drawing.Point(0, 0);
            this.panelTomeOfLife.Name = "panelTomeOfLife";
            this.panelTomeOfLife.Size = new System.Drawing.Size(358, 461);
            this.panelTomeOfLife.TabIndex = 0;
            // 
            // lblTomeTitle
            // 
            this.lblTomeTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTomeTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblTomeTitle.Location = new System.Drawing.Point(8, 6);
            this.lblTomeTitle.Name = "lblTomeTitle";
            this.lblTomeTitle.Size = new System.Drawing.Size(342, 20);
            this.lblTomeTitle.TabIndex = 0;
            this.lblTomeTitle.Text = "TOME OF LIFE";
            // 
            // lblTomeLinesLbl
            // 
            this.lblTomeLinesLbl.Location = new System.Drawing.Point(8, 32);
            this.lblTomeLinesLbl.Name = "lblTomeLinesLbl";
            this.lblTomeLinesLbl.Size = new System.Drawing.Size(46, 18);
            this.lblTomeLinesLbl.TabIndex = 1;
            this.lblTomeLinesLbl.Text = "Lines:";
            // 
            // nudTomeLines
            // 
            this.nudTomeLines.Location = new System.Drawing.Point(54, 29);
            this.nudTomeLines.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudTomeLines.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTomeLines.Name = "nudTomeLines";
            this.nudTomeLines.Size = new System.Drawing.Size(110, 22);
            this.nudTomeLines.TabIndex = 2;
            this.nudTomeLines.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTomeLines.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelScarabSpin
            // 
            this.panelScarabSpin.Controls.Add(this.lblScarabTitle);
            this.panelScarabSpin.Controls.Add(this.lblScarabLinesLbl);
            this.panelScarabSpin.Controls.Add(this.nudScarabLines);
            this.panelScarabSpin.Location = new System.Drawing.Point(0, 0);
            this.panelScarabSpin.Name = "panelScarabSpin";
            this.panelScarabSpin.Size = new System.Drawing.Size(358, 461);
            this.panelScarabSpin.TabIndex = 0;
            // 
            // lblScarabTitle
            // 
            this.lblScarabTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblScarabTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblScarabTitle.Location = new System.Drawing.Point(8, 6);
            this.lblScarabTitle.Name = "lblScarabTitle";
            this.lblScarabTitle.Size = new System.Drawing.Size(342, 20);
            this.lblScarabTitle.TabIndex = 0;
            this.lblScarabTitle.Text = "SCARAB SPIN";
            // 
            // lblScarabLinesLbl
            // 
            this.lblScarabLinesLbl.Location = new System.Drawing.Point(8, 32);
            this.lblScarabLinesLbl.Name = "lblScarabLinesLbl";
            this.lblScarabLinesLbl.Size = new System.Drawing.Size(46, 18);
            this.lblScarabLinesLbl.TabIndex = 1;
            this.lblScarabLinesLbl.Text = "Lines:";
            // 
            // nudScarabLines
            // 
            this.nudScarabLines.Location = new System.Drawing.Point(54, 29);
            this.nudScarabLines.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudScarabLines.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudScarabLines.Name = "nudScarabLines";
            this.nudScarabLines.Size = new System.Drawing.Size(110, 22);
            this.nudScarabLines.TabIndex = 2;
            this.nudScarabLines.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudScarabLines.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelDiamonds
            // 
            this.panelDiamonds.Controls.Add(this.lblDiamondsTitle);
            this.panelDiamonds.Controls.Add(this.lblDiamondsSelLbl);
            this.panelDiamonds.Controls.Add(this.btnDiamondRed);
            this.panelDiamonds.Controls.Add(this.btnDiamondYellow);
            this.panelDiamonds.Controls.Add(this.btnDiamondGreen);
            this.panelDiamonds.Controls.Add(this.btnDiamondBlue);
            this.panelDiamonds.Controls.Add(this.btnDiamondOrange);
            this.panelDiamonds.Controls.Add(this.btnDiamondPurple);
            this.panelDiamonds.Controls.Add(this.btnDiamondCyan);
            this.panelDiamonds.Controls.Add(this.btnDiamondsClear);
            this.panelDiamonds.Controls.Add(this.lblDiamondColLbl);
            this.panelDiamonds.Controls.Add(this.txtDiamondColors);
            this.panelDiamonds.Controls.Add(this.lblDiamondCount);
            this.panelDiamonds.Controls.Add(this.chkStopDiamondsWin);
            this.panelDiamonds.Location = new System.Drawing.Point(0, 0);
            this.panelDiamonds.Name = "panelDiamonds";
            this.panelDiamonds.Size = new System.Drawing.Size(358, 461);
            this.panelDiamonds.TabIndex = 0;
            // 
            // lblDiamondsTitle
            // 
            this.lblDiamondsTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDiamondsTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblDiamondsTitle.Location = new System.Drawing.Point(8, 6);
            this.lblDiamondsTitle.Name = "lblDiamondsTitle";
            this.lblDiamondsTitle.Size = new System.Drawing.Size(342, 20);
            this.lblDiamondsTitle.TabIndex = 0;
            this.lblDiamondsTitle.Text = "DIAMONDS";
            // 
            // lblDiamondsSelLbl
            // 
            this.lblDiamondsSelLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDiamondsSelLbl.Location = new System.Drawing.Point(8, 30);
            this.lblDiamondsSelLbl.Name = "lblDiamondsSelLbl";
            this.lblDiamondsSelLbl.Size = new System.Drawing.Size(200, 18);
            this.lblDiamondsSelLbl.TabIndex = 1;
            this.lblDiamondsSelLbl.Text = "Select colors in order:";
            // 
            // btnDiamondRed
            // 
            this.btnDiamondRed.BackColor = System.Drawing.Color.Crimson;
            this.btnDiamondRed.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnDiamondRed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiamondRed.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.btnDiamondRed.ForeColor = System.Drawing.Color.White;
            this.btnDiamondRed.Location = new System.Drawing.Point(4, 52);
            this.btnDiamondRed.Name = "btnDiamondRed";
            this.btnDiamondRed.Size = new System.Drawing.Size(80, 26);
            this.btnDiamondRed.TabIndex = 2;
            this.btnDiamondRed.Tag = "red";
            this.btnDiamondRed.Text = "Red";
            this.btnDiamondRed.UseVisualStyleBackColor = false;
            this.btnDiamondRed.Click += new System.EventHandler(this.DiamondBtn_Click);
            // 
            // btnDiamondYellow
            // 
            this.btnDiamondYellow.BackColor = System.Drawing.Color.Gold;
            this.btnDiamondYellow.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnDiamondYellow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiamondYellow.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.btnDiamondYellow.ForeColor = System.Drawing.Color.Black;
            this.btnDiamondYellow.Location = new System.Drawing.Point(92, 52);
            this.btnDiamondYellow.Name = "btnDiamondYellow";
            this.btnDiamondYellow.Size = new System.Drawing.Size(80, 26);
            this.btnDiamondYellow.TabIndex = 3;
            this.btnDiamondYellow.Tag = "yellow";
            this.btnDiamondYellow.Text = "Yellow";
            this.btnDiamondYellow.UseVisualStyleBackColor = false;
            this.btnDiamondYellow.Click += new System.EventHandler(this.DiamondBtn_Click);
            // 
            // btnDiamondGreen
            // 
            this.btnDiamondGreen.BackColor = System.Drawing.Color.ForestGreen;
            this.btnDiamondGreen.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnDiamondGreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiamondGreen.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.btnDiamondGreen.ForeColor = System.Drawing.Color.White;
            this.btnDiamondGreen.Location = new System.Drawing.Point(180, 52);
            this.btnDiamondGreen.Name = "btnDiamondGreen";
            this.btnDiamondGreen.Size = new System.Drawing.Size(80, 26);
            this.btnDiamondGreen.TabIndex = 4;
            this.btnDiamondGreen.Tag = "green";
            this.btnDiamondGreen.Text = "Green";
            this.btnDiamondGreen.UseVisualStyleBackColor = false;
            this.btnDiamondGreen.Click += new System.EventHandler(this.DiamondBtn_Click);
            // 
            // btnDiamondBlue
            // 
            this.btnDiamondBlue.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnDiamondBlue.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnDiamondBlue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiamondBlue.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.btnDiamondBlue.ForeColor = System.Drawing.Color.White;
            this.btnDiamondBlue.Location = new System.Drawing.Point(4, 82);
            this.btnDiamondBlue.Name = "btnDiamondBlue";
            this.btnDiamondBlue.Size = new System.Drawing.Size(80, 26);
            this.btnDiamondBlue.TabIndex = 5;
            this.btnDiamondBlue.Tag = "blue";
            this.btnDiamondBlue.Text = "Blue";
            this.btnDiamondBlue.UseVisualStyleBackColor = false;
            this.btnDiamondBlue.Click += new System.EventHandler(this.DiamondBtn_Click);
            // 
            // btnDiamondOrange
            // 
            this.btnDiamondOrange.BackColor = System.Drawing.Color.Pink;
            this.btnDiamondOrange.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnDiamondOrange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiamondOrange.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.btnDiamondOrange.ForeColor = System.Drawing.Color.White;
            this.btnDiamondOrange.Location = new System.Drawing.Point(92, 82);
            this.btnDiamondOrange.Name = "btnDiamondOrange";
            this.btnDiamondOrange.Size = new System.Drawing.Size(80, 26);
            this.btnDiamondOrange.TabIndex = 6;
            this.btnDiamondOrange.Tag = "orange";
            this.btnDiamondOrange.Text = "Pink";
            this.btnDiamondOrange.UseVisualStyleBackColor = false;
            this.btnDiamondOrange.Click += new System.EventHandler(this.DiamondBtn_Click);
            // 
            // btnDiamondPurple
            // 
            this.btnDiamondPurple.BackColor = System.Drawing.Color.MediumPurple;
            this.btnDiamondPurple.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnDiamondPurple.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiamondPurple.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.btnDiamondPurple.ForeColor = System.Drawing.Color.White;
            this.btnDiamondPurple.Location = new System.Drawing.Point(180, 82);
            this.btnDiamondPurple.Name = "btnDiamondPurple";
            this.btnDiamondPurple.Size = new System.Drawing.Size(80, 26);
            this.btnDiamondPurple.TabIndex = 7;
            this.btnDiamondPurple.Tag = "purple";
            this.btnDiamondPurple.Text = "Purple";
            this.btnDiamondPurple.UseVisualStyleBackColor = false;
            this.btnDiamondPurple.Click += new System.EventHandler(this.DiamondBtn_Click);
            // 
            // btnDiamondCyan
            // 
            this.btnDiamondCyan.BackColor = System.Drawing.Color.DarkCyan;
            this.btnDiamondCyan.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnDiamondCyan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiamondCyan.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this.btnDiamondCyan.ForeColor = System.Drawing.Color.White;
            this.btnDiamondCyan.Location = new System.Drawing.Point(4, 112);
            this.btnDiamondCyan.Name = "btnDiamondCyan";
            this.btnDiamondCyan.Size = new System.Drawing.Size(80, 26);
            this.btnDiamondCyan.TabIndex = 8;
            this.btnDiamondCyan.Tag = "cyan";
            this.btnDiamondCyan.Text = "Cyan";
            this.btnDiamondCyan.UseVisualStyleBackColor = false;
            this.btnDiamondCyan.Click += new System.EventHandler(this.DiamondBtn_Click);
            // 
            // btnDiamondsClear
            // 
            this.btnDiamondsClear.Location = new System.Drawing.Point(92, 112);
            this.btnDiamondsClear.Name = "btnDiamondsClear";
            this.btnDiamondsClear.Size = new System.Drawing.Size(168, 26);
            this.btnDiamondsClear.TabIndex = 9;
            this.btnDiamondsClear.Text = "Clear";
            this.btnDiamondsClear.UseVisualStyleBackColor = true;
            this.btnDiamondsClear.Click += new System.EventHandler(this.BtnDiamondsClear_Click);
            // 
            // lblDiamondColLbl
            // 
            this.lblDiamondColLbl.Location = new System.Drawing.Point(4, 142);
            this.lblDiamondColLbl.Name = "lblDiamondColLbl";
            this.lblDiamondColLbl.Size = new System.Drawing.Size(50, 18);
            this.lblDiamondColLbl.TabIndex = 10;
            this.lblDiamondColLbl.Text = "Colors:";
            // 
            // txtDiamondColors
            // 
            this.txtDiamondColors.Location = new System.Drawing.Point(54, 139);
            this.txtDiamondColors.Name = "txtDiamondColors";
            this.txtDiamondColors.ReadOnly = true;
            this.txtDiamondColors.Size = new System.Drawing.Size(296, 22);
            this.txtDiamondColors.TabIndex = 11;
            // 
            // lblDiamondCount
            // 
            this.lblDiamondCount.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblDiamondCount.Location = new System.Drawing.Point(4, 166);
            this.lblDiamondCount.Name = "lblDiamondCount";
            this.lblDiamondCount.Size = new System.Drawing.Size(346, 18);
            this.lblDiamondCount.TabIndex = 12;
            this.lblDiamondCount.Text = "0 / 5 selected";
            // 
            // chkStopDiamondsWin
            // 
            this.chkStopDiamondsWin.Location = new System.Drawing.Point(4, 190);
            this.chkStopDiamondsWin.Name = "chkStopDiamondsWin";
            this.chkStopDiamondsWin.Size = new System.Drawing.Size(346, 22);
            this.chkStopDiamondsWin.TabIndex = 13;
            this.chkStopDiamondsWin.Text = "Stop on pattern";
            this.chkStopDiamondsWin.CheckedChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelCases
            // 
            this.panelCases.Controls.Add(this.lblCasesTitle);
            this.panelCases.Controls.Add(this.lblCasesDiffLbl);
            this.panelCases.Controls.Add(this.cmbCasesDiff);
            this.panelCases.Location = new System.Drawing.Point(0, 0);
            this.panelCases.Name = "panelCases";
            this.panelCases.Size = new System.Drawing.Size(358, 461);
            this.panelCases.TabIndex = 0;
            // 
            // lblCasesTitle
            // 
            this.lblCasesTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCasesTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblCasesTitle.Location = new System.Drawing.Point(8, 6);
            this.lblCasesTitle.Name = "lblCasesTitle";
            this.lblCasesTitle.Size = new System.Drawing.Size(342, 20);
            this.lblCasesTitle.TabIndex = 0;
            this.lblCasesTitle.Text = "CASES";
            // 
            // lblCasesDiffLbl
            // 
            this.lblCasesDiffLbl.Location = new System.Drawing.Point(8, 32);
            this.lblCasesDiffLbl.Name = "lblCasesDiffLbl";
            this.lblCasesDiffLbl.Size = new System.Drawing.Size(70, 18);
            this.lblCasesDiffLbl.TabIndex = 1;
            this.lblCasesDiffLbl.Text = "Difficulty:";
            // 
            // cmbCasesDiff
            // 
            this.cmbCasesDiff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCasesDiff.Items.AddRange(new object[] {
            "easy",
            "medium",
            "hard",
            "expert"});
            this.cmbCasesDiff.Location = new System.Drawing.Point(78, 29);
            this.cmbCasesDiff.Name = "cmbCasesDiff";
            this.cmbCasesDiff.Size = new System.Drawing.Size(130, 24);
            this.cmbCasesDiff.TabIndex = 2;
            this.cmbCasesDiff.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelRps
            // 
            this.panelRps.Controls.Add(this.lblRpsTitle);
            this.panelRps.Controls.Add(this.lblRpsGuessLbl);
            this.panelRps.Controls.Add(this.txtRpsGuesses);
            this.panelRps.Location = new System.Drawing.Point(0, 0);
            this.panelRps.Name = "panelRps";
            this.panelRps.Size = new System.Drawing.Size(358, 461);
            this.panelRps.TabIndex = 0;
            // 
            // lblRpsTitle
            // 
            this.lblRpsTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRpsTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblRpsTitle.Location = new System.Drawing.Point(8, 6);
            this.lblRpsTitle.Name = "lblRpsTitle";
            this.lblRpsTitle.Size = new System.Drawing.Size(342, 20);
            this.lblRpsTitle.TabIndex = 0;
            this.lblRpsTitle.Text = "ROCK PAPER SCISSORS";
            // 
            // lblRpsGuessLbl
            // 
            this.lblRpsGuessLbl.Location = new System.Drawing.Point(8, 32);
            this.lblRpsGuessLbl.Name = "lblRpsGuessLbl";
            this.lblRpsGuessLbl.Size = new System.Drawing.Size(340, 18);
            this.lblRpsGuessLbl.TabIndex = 1;
            this.lblRpsGuessLbl.Text = "Guesses (rock,paper,scissors):";
            // 
            // txtRpsGuesses
            // 
            this.txtRpsGuesses.Location = new System.Drawing.Point(8, 50);
            this.txtRpsGuesses.Name = "txtRpsGuesses";
            this.txtRpsGuesses.Size = new System.Drawing.Size(340, 22);
            this.txtRpsGuesses.TabIndex = 2;
            this.txtRpsGuesses.Text = "rock";
            this.txtRpsGuesses.TextChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelFlip
            // 
            this.panelFlip.Controls.Add(this.lblFlipTitle);
            this.panelFlip.Controls.Add(this.lblFlipGuessLbl);
            this.panelFlip.Controls.Add(this.txtFlipGuesses);
            this.panelFlip.Location = new System.Drawing.Point(0, 0);
            this.panelFlip.Name = "panelFlip";
            this.panelFlip.Size = new System.Drawing.Size(358, 461);
            this.panelFlip.TabIndex = 0;
            // 
            // lblFlipTitle
            // 
            this.lblFlipTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFlipTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblFlipTitle.Location = new System.Drawing.Point(8, 6);
            this.lblFlipTitle.Name = "lblFlipTitle";
            this.lblFlipTitle.Size = new System.Drawing.Size(342, 20);
            this.lblFlipTitle.TabIndex = 0;
            this.lblFlipTitle.Text = "COIN FLIP";
            // 
            // lblFlipGuessLbl
            // 
            this.lblFlipGuessLbl.Location = new System.Drawing.Point(8, 32);
            this.lblFlipGuessLbl.Name = "lblFlipGuessLbl";
            this.lblFlipGuessLbl.Size = new System.Drawing.Size(340, 18);
            this.lblFlipGuessLbl.TabIndex = 1;
            this.lblFlipGuessLbl.Text = "Guesses (heads,tails):";
            // 
            // txtFlipGuesses
            // 
            this.txtFlipGuesses.Location = new System.Drawing.Point(8, 50);
            this.txtFlipGuesses.Name = "txtFlipGuesses";
            this.txtFlipGuesses.Size = new System.Drawing.Size(340, 22);
            this.txtFlipGuesses.TabIndex = 2;
            this.txtFlipGuesses.Text = "heads";
            this.txtFlipGuesses.TextChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelSnakes
            // 
            this.panelSnakes.Controls.Add(this.lblSnakesTitle);
            this.panelSnakes.Controls.Add(this.lblSnakesDiffLbl);
            this.panelSnakes.Controls.Add(this.cmbSnakesDiff);
            this.panelSnakes.Controls.Add(this.lblSnakesRollsLbl);
            this.panelSnakes.Controls.Add(this.nudSnakesRolls);
            this.panelSnakes.Location = new System.Drawing.Point(0, 0);
            this.panelSnakes.Name = "panelSnakes";
            this.panelSnakes.Size = new System.Drawing.Size(358, 461);
            this.panelSnakes.TabIndex = 0;
            // 
            // lblSnakesTitle
            // 
            this.lblSnakesTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSnakesTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblSnakesTitle.Location = new System.Drawing.Point(8, 6);
            this.lblSnakesTitle.Name = "lblSnakesTitle";
            this.lblSnakesTitle.Size = new System.Drawing.Size(342, 20);
            this.lblSnakesTitle.TabIndex = 0;
            this.lblSnakesTitle.Text = "SNAKES";
            // 
            // lblSnakesDiffLbl
            // 
            this.lblSnakesDiffLbl.Location = new System.Drawing.Point(8, 32);
            this.lblSnakesDiffLbl.Name = "lblSnakesDiffLbl";
            this.lblSnakesDiffLbl.Size = new System.Drawing.Size(70, 18);
            this.lblSnakesDiffLbl.TabIndex = 1;
            this.lblSnakesDiffLbl.Text = "Difficulty:";
            // 
            // cmbSnakesDiff
            // 
            this.cmbSnakesDiff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSnakesDiff.Items.AddRange(new object[] {
            "easy",
            "medium",
            "hard",
            "expert"});
            this.cmbSnakesDiff.Location = new System.Drawing.Point(78, 29);
            this.cmbSnakesDiff.Name = "cmbSnakesDiff";
            this.cmbSnakesDiff.Size = new System.Drawing.Size(130, 24);
            this.cmbSnakesDiff.TabIndex = 2;
            this.cmbSnakesDiff.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblSnakesRollsLbl
            // 
            this.lblSnakesRollsLbl.Location = new System.Drawing.Point(8, 58);
            this.lblSnakesRollsLbl.Name = "lblSnakesRollsLbl";
            this.lblSnakesRollsLbl.Size = new System.Drawing.Size(44, 18);
            this.lblSnakesRollsLbl.TabIndex = 3;
            this.lblSnakesRollsLbl.Text = "Rolls:";
            // 
            // nudSnakesRolls
            // 
            this.nudSnakesRolls.Location = new System.Drawing.Point(52, 55);
            this.nudSnakesRolls.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudSnakesRolls.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSnakesRolls.Name = "nudSnakesRolls";
            this.nudSnakesRolls.Size = new System.Drawing.Size(110, 22);
            this.nudSnakesRolls.TabIndex = 4;
            this.nudSnakesRolls.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSnakesRolls.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelDarts
            // 
            this.panelDarts.Controls.Add(this.lblDartsTitle);
            this.panelDarts.Controls.Add(this.lblDartsDiffLbl);
            this.panelDarts.Controls.Add(this.cmbDartsDiff);
            this.panelDarts.Location = new System.Drawing.Point(0, 0);
            this.panelDarts.Name = "panelDarts";
            this.panelDarts.Size = new System.Drawing.Size(358, 461);
            this.panelDarts.TabIndex = 0;
            // 
            // lblDartsTitle
            // 
            this.lblDartsTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDartsTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblDartsTitle.Location = new System.Drawing.Point(8, 6);
            this.lblDartsTitle.Name = "lblDartsTitle";
            this.lblDartsTitle.Size = new System.Drawing.Size(342, 20);
            this.lblDartsTitle.TabIndex = 0;
            this.lblDartsTitle.Text = "DARTS";
            // 
            // lblDartsDiffLbl
            // 
            this.lblDartsDiffLbl.Location = new System.Drawing.Point(8, 32);
            this.lblDartsDiffLbl.Name = "lblDartsDiffLbl";
            this.lblDartsDiffLbl.Size = new System.Drawing.Size(70, 18);
            this.lblDartsDiffLbl.TabIndex = 1;
            this.lblDartsDiffLbl.Text = "Difficulty:";
            // 
            // cmbDartsDiff
            // 
            this.cmbDartsDiff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDartsDiff.Items.AddRange(new object[] {
            "easy",
            "medium",
            "hard",
            "expert"});
            this.cmbDartsDiff.Location = new System.Drawing.Point(78, 29);
            this.cmbDartsDiff.Name = "cmbDartsDiff";
            this.cmbDartsDiff.Size = new System.Drawing.Size(130, 24);
            this.cmbDartsDiff.TabIndex = 2;
            this.cmbDartsDiff.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelPacks
            // 
            this.panelPacks.Controls.Add(this.lblPacksTitle);
            this.panelPacks.Controls.Add(this.lblPacksNote);
            this.panelPacks.Location = new System.Drawing.Point(0, 0);
            this.panelPacks.Name = "panelPacks";
            this.panelPacks.Size = new System.Drawing.Size(358, 461);
            this.panelPacks.TabIndex = 0;
            // 
            // lblPacksTitle
            // 
            this.lblPacksTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPacksTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblPacksTitle.Location = new System.Drawing.Point(8, 6);
            this.lblPacksTitle.Name = "lblPacksTitle";
            this.lblPacksTitle.Size = new System.Drawing.Size(342, 20);
            this.lblPacksTitle.TabIndex = 0;
            this.lblPacksTitle.Text = "PACKS";
            // 
            // lblPacksNote
            // 
            this.lblPacksNote.Location = new System.Drawing.Point(8, 32);
            this.lblPacksNote.Name = "lblPacksNote";
            this.lblPacksNote.Size = new System.Drawing.Size(340, 18);
            this.lblPacksNote.TabIndex = 1;
            this.lblPacksNote.Text = "No extra parameters — base bet only.";
            // 
            // panelMoles
            // 
            this.panelMoles.Controls.Add(this.lblMolesTitle);
            this.panelMoles.Controls.Add(this.lblMolesMolesLbl);
            this.panelMoles.Controls.Add(this.nudMolesMoles);
            this.panelMoles.Controls.Add(this.lblMolesPicksLbl);
            this.panelMoles.Controls.Add(this.txtMolesPicks);
            this.panelMoles.Location = new System.Drawing.Point(0, 0);
            this.panelMoles.Name = "panelMoles";
            this.panelMoles.Size = new System.Drawing.Size(358, 461);
            this.panelMoles.TabIndex = 0;
            // 
            // lblMolesTitle
            // 
            this.lblMolesTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMolesTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMolesTitle.Location = new System.Drawing.Point(8, 6);
            this.lblMolesTitle.Name = "lblMolesTitle";
            this.lblMolesTitle.Size = new System.Drawing.Size(342, 20);
            this.lblMolesTitle.TabIndex = 0;
            this.lblMolesTitle.Text = "MOLES";
            // 
            // lblMolesMolesLbl
            // 
            this.lblMolesMolesLbl.Location = new System.Drawing.Point(8, 32);
            this.lblMolesMolesLbl.Name = "lblMolesMolesLbl";
            this.lblMolesMolesLbl.Size = new System.Drawing.Size(46, 18);
            this.lblMolesMolesLbl.TabIndex = 1;
            this.lblMolesMolesLbl.Text = "Moles:";
            // 
            // nudMolesMoles
            // 
            this.nudMolesMoles.Location = new System.Drawing.Point(54, 29);
            this.nudMolesMoles.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMolesMoles.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMolesMoles.Name = "nudMolesMoles";
            this.nudMolesMoles.Size = new System.Drawing.Size(110, 22);
            this.nudMolesMoles.TabIndex = 2;
            this.nudMolesMoles.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudMolesMoles.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblMolesPicksLbl
            // 
            this.lblMolesPicksLbl.Location = new System.Drawing.Point(8, 58);
            this.lblMolesPicksLbl.Name = "lblMolesPicksLbl";
            this.lblMolesPicksLbl.Size = new System.Drawing.Size(104, 18);
            this.lblMolesPicksLbl.TabIndex = 3;
            this.lblMolesPicksLbl.Text = "Picks (0-based):";
            // 
            // txtMolesPicks
            // 
            this.txtMolesPicks.Location = new System.Drawing.Point(112, 55);
            this.txtMolesPicks.Name = "txtMolesPicks";
            this.txtMolesPicks.Size = new System.Drawing.Size(190, 22);
            this.txtMolesPicks.TabIndex = 4;
            this.txtMolesPicks.Text = "0";
            this.txtMolesPicks.TextChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelChicken
            // 
            this.panelChicken.Controls.Add(this.lblChickenTitle);
            this.panelChicken.Controls.Add(this.lblChickenDiffLbl);
            this.panelChicken.Controls.Add(this.cmbChickenDiff);
            this.panelChicken.Controls.Add(this.lblChickenStepsLbl);
            this.panelChicken.Controls.Add(this.nudChickenSteps);
            this.panelChicken.Location = new System.Drawing.Point(0, 0);
            this.panelChicken.Name = "panelChicken";
            this.panelChicken.Size = new System.Drawing.Size(358, 461);
            this.panelChicken.TabIndex = 0;
            // 
            // lblChickenTitle
            // 
            this.lblChickenTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblChickenTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblChickenTitle.Location = new System.Drawing.Point(8, 6);
            this.lblChickenTitle.Name = "lblChickenTitle";
            this.lblChickenTitle.Size = new System.Drawing.Size(342, 20);
            this.lblChickenTitle.TabIndex = 0;
            this.lblChickenTitle.Text = "CHICKEN";
            // 
            // lblChickenDiffLbl
            // 
            this.lblChickenDiffLbl.Location = new System.Drawing.Point(8, 32);
            this.lblChickenDiffLbl.Name = "lblChickenDiffLbl";
            this.lblChickenDiffLbl.Size = new System.Drawing.Size(70, 18);
            this.lblChickenDiffLbl.TabIndex = 1;
            this.lblChickenDiffLbl.Text = "Difficulty:";
            // 
            // cmbChickenDiff
            // 
            this.cmbChickenDiff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChickenDiff.Items.AddRange(new object[] {
            "easy",
            "medium",
            "hard",
            "expert"});
            this.cmbChickenDiff.Location = new System.Drawing.Point(78, 29);
            this.cmbChickenDiff.Name = "cmbChickenDiff";
            this.cmbChickenDiff.Size = new System.Drawing.Size(130, 24);
            this.cmbChickenDiff.TabIndex = 2;
            this.cmbChickenDiff.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblChickenStepsLbl
            // 
            this.lblChickenStepsLbl.Location = new System.Drawing.Point(8, 58);
            this.lblChickenStepsLbl.Name = "lblChickenStepsLbl";
            this.lblChickenStepsLbl.Size = new System.Drawing.Size(44, 18);
            this.lblChickenStepsLbl.TabIndex = 3;
            this.lblChickenStepsLbl.Text = "Steps:";
            // 
            // nudChickenSteps
            // 
            this.nudChickenSteps.Location = new System.Drawing.Point(52, 55);
            this.nudChickenSteps.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudChickenSteps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudChickenSteps.Name = "nudChickenSteps";
            this.nudChickenSteps.Size = new System.Drawing.Size(110, 22);
            this.nudChickenSteps.TabIndex = 4;
            this.nudChickenSteps.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudChickenSteps.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelTarot
            // 
            this.panelTarot.Controls.Add(this.lblTarotTitle);
            this.panelTarot.Controls.Add(this.lblTarotDiffLbl);
            this.panelTarot.Controls.Add(this.cmbTarotDiff);
            this.panelTarot.Location = new System.Drawing.Point(0, 0);
            this.panelTarot.Name = "panelTarot";
            this.panelTarot.Size = new System.Drawing.Size(358, 461);
            this.panelTarot.TabIndex = 0;
            // 
            // lblTarotTitle
            // 
            this.lblTarotTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTarotTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblTarotTitle.Location = new System.Drawing.Point(8, 6);
            this.lblTarotTitle.Name = "lblTarotTitle";
            this.lblTarotTitle.Size = new System.Drawing.Size(342, 20);
            this.lblTarotTitle.TabIndex = 0;
            this.lblTarotTitle.Text = "TAROT";
            // 
            // lblTarotDiffLbl
            // 
            this.lblTarotDiffLbl.Location = new System.Drawing.Point(8, 32);
            this.lblTarotDiffLbl.Name = "lblTarotDiffLbl";
            this.lblTarotDiffLbl.Size = new System.Drawing.Size(70, 18);
            this.lblTarotDiffLbl.TabIndex = 1;
            this.lblTarotDiffLbl.Text = "Difficulty:";
            // 
            // cmbTarotDiff
            // 
            this.cmbTarotDiff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTarotDiff.Items.AddRange(new object[] {
            "easy",
            "medium",
            "hard",
            "expert"});
            this.cmbTarotDiff.Location = new System.Drawing.Point(78, 29);
            this.cmbTarotDiff.Name = "cmbTarotDiff";
            this.cmbTarotDiff.Size = new System.Drawing.Size(130, 24);
            this.cmbTarotDiff.TabIndex = 2;
            this.cmbTarotDiff.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelDrill
            // 
            this.panelDrill.Controls.Add(this.lblDrillTitle);
            this.panelDrill.Controls.Add(this.lblDrillTargetLbl);
            this.panelDrill.Controls.Add(this.nudDrillTarget);
            this.panelDrill.Controls.Add(this.lblDrillPickLbl);
            this.panelDrill.Controls.Add(this.nudDrillPick);
            this.panelDrill.Location = new System.Drawing.Point(0, 0);
            this.panelDrill.Name = "panelDrill";
            this.panelDrill.Size = new System.Drawing.Size(358, 461);
            this.panelDrill.TabIndex = 0;
            // 
            // lblDrillTitle
            // 
            this.lblDrillTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDrillTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblDrillTitle.Location = new System.Drawing.Point(8, 6);
            this.lblDrillTitle.Name = "lblDrillTitle";
            this.lblDrillTitle.Size = new System.Drawing.Size(342, 20);
            this.lblDrillTitle.TabIndex = 0;
            this.lblDrillTitle.Text = "DRILL";
            // 
            // lblDrillTargetLbl
            // 
            this.lblDrillTargetLbl.Location = new System.Drawing.Point(8, 32);
            this.lblDrillTargetLbl.Name = "lblDrillTargetLbl";
            this.lblDrillTargetLbl.Size = new System.Drawing.Size(80, 18);
            this.lblDrillTargetLbl.TabIndex = 1;
            this.lblDrillTargetLbl.Text = "Target Multi:";
            // 
            // nudDrillTarget
            // 
            this.nudDrillTarget.DecimalPlaces = 2;
            this.nudDrillTarget.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudDrillTarget.Location = new System.Drawing.Point(88, 29);
            this.nudDrillTarget.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudDrillTarget.Minimum = new decimal(new int[] {
            101,
            0,
            0,
            131072});
            this.nudDrillTarget.Name = "nudDrillTarget";
            this.nudDrillTarget.Size = new System.Drawing.Size(110, 22);
            this.nudDrillTarget.TabIndex = 2;
            this.nudDrillTarget.Value = new decimal(new int[] {
            101,
            0,
            0,
            131072});
            this.nudDrillTarget.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblDrillPickLbl
            // 
            this.lblDrillPickLbl.Location = new System.Drawing.Point(8, 58);
            this.lblDrillPickLbl.Name = "lblDrillPickLbl";
            this.lblDrillPickLbl.Size = new System.Drawing.Size(64, 18);
            this.lblDrillPickLbl.TabIndex = 3;
            this.lblDrillPickLbl.Text = "Pick (col):";
            // 
            // nudDrillPick
            // 
            this.nudDrillPick.Location = new System.Drawing.Point(72, 55);
            this.nudDrillPick.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudDrillPick.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDrillPick.Name = "nudDrillPick";
            this.nudDrillPick.Size = new System.Drawing.Size(110, 22);
            this.nudDrillPick.TabIndex = 4;
            this.nudDrillPick.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDrillPick.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelPrimedice
            // 
            this.panelPrimedice.Controls.Add(this.lblPrimediceTitle);
            this.panelPrimedice.Controls.Add(this.lblPDCondLbl);
            this.panelPrimedice.Controls.Add(this.cmbPDCond);
            this.panelPrimedice.Controls.Add(this.lblPDT1);
            this.panelPrimedice.Controls.Add(this.nudPDT1);
            this.panelPrimedice.Controls.Add(this.lblPDT2);
            this.panelPrimedice.Controls.Add(this.nudPDT2);
            this.panelPrimedice.Controls.Add(this.lblPDT3);
            this.panelPrimedice.Controls.Add(this.nudPDT3);
            this.panelPrimedice.Controls.Add(this.lblPDT4);
            this.panelPrimedice.Controls.Add(this.nudPDT4);
            this.panelPrimedice.Location = new System.Drawing.Point(0, 0);
            this.panelPrimedice.Name = "panelPrimedice";
            this.panelPrimedice.Size = new System.Drawing.Size(358, 461);
            this.panelPrimedice.TabIndex = 0;
            // 
            // lblPrimediceTitle
            // 
            this.lblPrimediceTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPrimediceTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblPrimediceTitle.Location = new System.Drawing.Point(8, 6);
            this.lblPrimediceTitle.Name = "lblPrimediceTitle";
            this.lblPrimediceTitle.Size = new System.Drawing.Size(342, 20);
            this.lblPrimediceTitle.TabIndex = 0;
            this.lblPrimediceTitle.Text = "PRIMEDICE X";
            // 
            // lblPDCondLbl
            // 
            this.lblPDCondLbl.Location = new System.Drawing.Point(8, 32);
            this.lblPDCondLbl.Name = "lblPDCondLbl";
            this.lblPDCondLbl.Size = new System.Drawing.Size(70, 18);
            this.lblPDCondLbl.TabIndex = 1;
            this.lblPDCondLbl.Text = "Condition:";
            // 
            // cmbPDCond
            // 
            this.cmbPDCond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPDCond.Items.AddRange(new object[] {
            "rollBetween",
            "rollOutside",
            "rollBetweenTwo"});
            this.cmbPDCond.Location = new System.Drawing.Point(78, 29);
            this.cmbPDCond.Name = "cmbPDCond";
            this.cmbPDCond.Size = new System.Drawing.Size(200, 24);
            this.cmbPDCond.TabIndex = 2;
            this.cmbPDCond.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblPDT1
            // 
            this.lblPDT1.Location = new System.Drawing.Point(8, 58);
            this.lblPDT1.Name = "lblPDT1";
            this.lblPDT1.Size = new System.Drawing.Size(22, 18);
            this.lblPDT1.TabIndex = 3;
            this.lblPDT1.Text = "T1:";
            // 
            // nudPDT1
            // 
            this.nudPDT1.DecimalPlaces = 2;
            this.nudPDT1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudPDT1.Location = new System.Drawing.Point(30, 55);
            this.nudPDT1.Name = "nudPDT1";
            this.nudPDT1.Size = new System.Drawing.Size(88, 22);
            this.nudPDT1.TabIndex = 4;
            this.nudPDT1.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudPDT1.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblPDT2
            // 
            this.lblPDT2.Location = new System.Drawing.Point(126, 58);
            this.lblPDT2.Name = "lblPDT2";
            this.lblPDT2.Size = new System.Drawing.Size(22, 18);
            this.lblPDT2.TabIndex = 5;
            this.lblPDT2.Text = "T2:";
            // 
            // nudPDT2
            // 
            this.nudPDT2.DecimalPlaces = 2;
            this.nudPDT2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudPDT2.Location = new System.Drawing.Point(148, 55);
            this.nudPDT2.Name = "nudPDT2";
            this.nudPDT2.Size = new System.Drawing.Size(88, 22);
            this.nudPDT2.TabIndex = 6;
            this.nudPDT2.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.nudPDT2.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblPDT3
            // 
            this.lblPDT3.Location = new System.Drawing.Point(8, 84);
            this.lblPDT3.Name = "lblPDT3";
            this.lblPDT3.Size = new System.Drawing.Size(22, 18);
            this.lblPDT3.TabIndex = 7;
            this.lblPDT3.Text = "T3:";
            // 
            // nudPDT3
            // 
            this.nudPDT3.DecimalPlaces = 2;
            this.nudPDT3.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudPDT3.Location = new System.Drawing.Point(30, 81);
            this.nudPDT3.Name = "nudPDT3";
            this.nudPDT3.Size = new System.Drawing.Size(88, 22);
            this.nudPDT3.TabIndex = 8;
            this.nudPDT3.Value = new decimal(new int[] {
            34,
            0,
            0,
            0});
            this.nudPDT3.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblPDT4
            // 
            this.lblPDT4.Location = new System.Drawing.Point(126, 84);
            this.lblPDT4.Name = "lblPDT4";
            this.lblPDT4.Size = new System.Drawing.Size(22, 18);
            this.lblPDT4.TabIndex = 9;
            this.lblPDT4.Text = "T4:";
            // 
            // nudPDT4
            // 
            this.nudPDT4.DecimalPlaces = 2;
            this.nudPDT4.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudPDT4.Location = new System.Drawing.Point(148, 81);
            this.nudPDT4.Name = "nudPDT4";
            this.nudPDT4.Size = new System.Drawing.Size(88, 22);
            this.nudPDT4.TabIndex = 10;
            this.nudPDT4.Value = new decimal(new int[] {
            68,
            0,
            0,
            0});
            this.nudPDT4.ValueChanged += new System.EventHandler(this.QueueSave);
            // 
            // panelBlueSamurai
            // 
            this.panelBlueSamurai.Controls.Add(this.lblBlueSamuraiTitle);
            this.panelBlueSamurai.Controls.Add(this.lblBlueSamuraiNote);
            this.panelBlueSamurai.Location = new System.Drawing.Point(0, 0);
            this.panelBlueSamurai.Name = "panelBlueSamurai";
            this.panelBlueSamurai.Size = new System.Drawing.Size(358, 461);
            this.panelBlueSamurai.TabIndex = 0;
            // 
            // lblBlueSamuraiTitle
            // 
            this.lblBlueSamuraiTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBlueSamuraiTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblBlueSamuraiTitle.Location = new System.Drawing.Point(8, 6);
            this.lblBlueSamuraiTitle.Name = "lblBlueSamuraiTitle";
            this.lblBlueSamuraiTitle.Size = new System.Drawing.Size(342, 20);
            this.lblBlueSamuraiTitle.TabIndex = 0;
            this.lblBlueSamuraiTitle.Text = "BLUE SAMURAI";
            // 
            // lblBlueSamuraiNote
            // 
            this.lblBlueSamuraiNote.Location = new System.Drawing.Point(8, 32);
            this.lblBlueSamuraiNote.Name = "lblBlueSamuraiNote";
            this.lblBlueSamuraiNote.Size = new System.Drawing.Size(340, 18);
            this.lblBlueSamuraiNote.TabIndex = 1;
            this.lblBlueSamuraiNote.Text = "No extra parameters — base bet only.";
            // 
            // panelVideoPoker
            // 
            this.panelVideoPoker.Controls.Add(this.lblVideoPokerTitle);
            this.panelVideoPoker.Controls.Add(this.lblVideoPokerNote);
            this.panelVideoPoker.Controls.Add(this.lblVideoPokerStratLbl);
            this.panelVideoPoker.Controls.Add(this.cmbVideoPokerStrat);
            this.panelVideoPoker.Controls.Add(this.lblVideoPokerHelp);
            this.panelVideoPoker.Location = new System.Drawing.Point(0, 0);
            this.panelVideoPoker.Name = "panelVideoPoker";
            this.panelVideoPoker.Size = new System.Drawing.Size(358, 461);
            this.panelVideoPoker.TabIndex = 0;
            // 
            // lblVideoPokerTitle
            // 
            this.lblVideoPokerTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblVideoPokerTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblVideoPokerTitle.Location = new System.Drawing.Point(8, 6);
            this.lblVideoPokerTitle.Name = "lblVideoPokerTitle";
            this.lblVideoPokerTitle.Size = new System.Drawing.Size(342, 20);
            this.lblVideoPokerTitle.TabIndex = 0;
            this.lblVideoPokerTitle.Text = "VIDEO POKER";
            // 
            // lblVideoPokerNote
            // 
            this.lblVideoPokerNote.Location = new System.Drawing.Point(8, 30);
            this.lblVideoPokerNote.Name = "lblVideoPokerNote";
            this.lblVideoPokerNote.Size = new System.Drawing.Size(340, 18);
            this.lblVideoPokerNote.TabIndex = 1;
            // 
            // lblVideoPokerStratLbl
            // 
            this.lblVideoPokerStratLbl.AutoSize = true;
            this.lblVideoPokerStratLbl.Location = new System.Drawing.Point(8, 58);
            this.lblVideoPokerStratLbl.Name = "lblVideoPokerStratLbl";
            this.lblVideoPokerStratLbl.Size = new System.Drawing.Size(90, 16);
            this.lblVideoPokerStratLbl.TabIndex = 2;
            this.lblVideoPokerStratLbl.Text = "Hold strategy:";
            // 
            // cmbVideoPokerStrat
            // 
            this.cmbVideoPokerStrat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVideoPokerStrat.Items.AddRange(new object[] {
            "optimal"});
            this.cmbVideoPokerStrat.Location = new System.Drawing.Point(105, 55);
            this.cmbVideoPokerStrat.Name = "cmbVideoPokerStrat";
            this.cmbVideoPokerStrat.Size = new System.Drawing.Size(130, 24);
            this.cmbVideoPokerStrat.TabIndex = 3;
            this.cmbVideoPokerStrat.SelectedIndexChanged += new System.EventHandler(this.QueueSave);
            // 
            // lblVideoPokerHelp
            // 
            this.lblVideoPokerHelp.ForeColor = System.Drawing.Color.DimGray;
            this.lblVideoPokerHelp.Location = new System.Drawing.Point(8, 86);
            this.lblVideoPokerHelp.Name = "lblVideoPokerHelp";
            this.lblVideoPokerHelp.Size = new System.Drawing.Size(340, 80);
            this.lblVideoPokerHelp.TabIndex = 4;
            // 
            // panelBlackjack
            // 
            this.panelBlackjack.Controls.Add(this.lblBlackjackTitle);
            this.panelBlackjack.Controls.Add(this.lblBlackjackNote);
            this.panelBlackjack.Location = new System.Drawing.Point(0, 0);
            this.panelBlackjack.Name = "panelBlackjack";
            this.panelBlackjack.Size = new System.Drawing.Size(358, 461);
            this.panelBlackjack.TabIndex = 0;
            // 
            // lblBlackjackTitle
            // 
            this.lblBlackjackTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBlackjackTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblBlackjackTitle.Location = new System.Drawing.Point(8, 6);
            this.lblBlackjackTitle.Name = "lblBlackjackTitle";
            this.lblBlackjackTitle.Size = new System.Drawing.Size(342, 20);
            this.lblBlackjackTitle.TabIndex = 0;
            this.lblBlackjackTitle.Text = "BLACKJACK";
            // 
            // lblBlackjackNote
            // 
            this.lblBlackjackNote.Location = new System.Drawing.Point(8, 32);
            this.lblBlackjackNote.Name = "lblBlackjackNote";
            this.lblBlackjackNote.Size = new System.Drawing.Size(340, 18);
            this.lblBlackjackNote.TabIndex = 1;
            this.lblBlackjackNote.Text = "Uses base bet only. Actions handled by bot logic.";
            // 
            // FastModeBox
            // 
            this.FastModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FastModeBox.FormattingEnabled = true;
            this.FastModeBox.Items.AddRange(new object[] {
            "Fast mode OFF",
            "Fast mode ON"});
            this.FastModeBox.Location = new System.Drawing.Point(926, 35);
            this.FastModeBox.Name = "FastModeBox";
            this.FastModeBox.Size = new System.Drawing.Size(170, 24);
            this.FastModeBox.TabIndex = 52;
            this.FastModeBox.SelectedIndexChanged += new System.EventHandler(this.FastModeBox_Changed);
            // 
            // BetDelayNumericUpDown1
            // 
            this.BetDelayNumericUpDown1.Location = new System.Drawing.Point(849, 35);
            this.BetDelayNumericUpDown1.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.BetDelayNumericUpDown1.Name = "BetDelayNumericUpDown1";
            this.BetDelayNumericUpDown1.Size = new System.Drawing.Size(68, 22);
            this.BetDelayNumericUpDown1.TabIndex = 53;
            this.BetDelayNumericUpDown1.Value = new decimal(new int[] {
            131,
            0,
            0,
            0});
            this.BetDelayNumericUpDown1.ValueChanged += new System.EventHandler(this.BetDelayNumericUpDown1_Changed);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(762, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 16);
            this.label1.TabIndex = 54;
            this.label1.Text = "Bet delay";
            // 
            // lblBetsPerSec
            // 
            this.lblBetsPerSec.AutoSize = true;
            this.lblBetsPerSec.ForeColor = System.Drawing.Color.Gray;
            this.lblBetsPerSec.Location = new System.Drawing.Point(710, 9);
            this.lblBetsPerSec.Name = "lblBetsPerSec";
            this.lblBetsPerSec.Size = new System.Drawing.Size(35, 16);
            this.lblBetsPerSec.TabIndex = 55;
            this.lblBetsPerSec.Text = "0.0/s";
            // 
            // ResetSeedBtn
            // 
            this.ResetSeedBtn.Location = new System.Drawing.Point(608, 32);
            this.ResetSeedBtn.Name = "ResetSeedBtn";
            this.ResetSeedBtn.Size = new System.Drawing.Size(132, 30);
            this.ResetSeedBtn.TabIndex = 56;
            this.ResetSeedBtn.Text = "Reset Seed";
            this.ResetSeedBtn.UseVisualStyleBackColor = true;
            this.ResetSeedBtn.Click += new System.EventHandler(this.BtnResetSeed_Click);
            // 
            // HighestMultiplierButton
            // 
            this.HighestMultiplierButton.Location = new System.Drawing.Point(497, 33);
            this.HighestMultiplierButton.Name = "HighestMultiplierButton";
            this.HighestMultiplierButton.Size = new System.Drawing.Size(105, 30);
            this.HighestMultiplierButton.TabIndex = 57;
            this.HighestMultiplierButton.Text = "Top 10";
            this.HighestMultiplierButton.UseVisualStyleBackColor = true;
            this.HighestMultiplierButton.Click += new System.EventHandler(this.HighestMultiplierButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 570);
            this.Controls.Add(this.HighestMultiplierButton);
            this.Controls.Add(this.ResetSeedBtn);
            this.Controls.Add(this.lblBetsPerSec);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BetDelayNumericUpDown1);
            this.Controls.Add(this.FastModeBox);
            this.Controls.Add(this.nudStopMultiplier);
            this.Controls.Add(this.lblApiKeyLabel);
            this.Controls.Add(this.txtApiKey);
            this.Controls.Add(this.cmbFetchMode);
            this.Controls.Add(this.btnGetCookie);
            this.Controls.Add(this.lblCookieStatus);
            this.Controls.Add(this.lblWsIndicator);
            this.Controls.Add(this.lblWsStatus);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.cmbCurrency);
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.lblMirrorLabel);
            this.Controls.Add(this.txtMirror);
            this.Controls.Add(this.sepTop);
            this.Controls.Add(this.lblGameLabel);
            this.Controls.Add(this.cmbGame);
            this.Controls.Add(this.lblBaseBetLabel);
            this.Controls.Add(this.nudBaseBet);
            this.Controls.Add(this.btnCondition);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lblMultSep);
            this.Controls.Add(this.chkStopMultiplier);
            this.Controls.Add(this.lblMultX);
            this.Controls.Add(this.sepToolbar);
            this.Controls.Add(this.pnlGameContainer);
            this.Controls.Add(this.sepVertical);
            this.Controls.Add(this.pnlHiloCards);
            this.Controls.Add(this.lvHistory);
            this.Controls.Add(this.sepMid);
            this.Controls.Add(this.rtbLog);
            this.MinimumSize = new System.Drawing.Size(1100, 570);
            this.Name = "Form1";
            this.Text = "Stake BOT UI";
            ((System.ComponentModel.ISupportInitialize)(this.nudBaseBet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStopMultiplier)).EndInit();
            this.pnlHiloCards.ResumeLayout(false);
            this.panelDice.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudDiceChance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStopDiceResult)).EndInit();
            this.panelLimbo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudLimboTarget)).EndInit();
            this.panelHilo.ResumeLayout(false);
            this.panelHilo.PerformLayout();
            this.panelMines.ResumeLayout(false);
            this.panelMines.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinesMines)).EndInit();
            this.panelKeno.ResumeLayout(false);
            this.panelKeno.PerformLayout();
            this.panelPlinko.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudPlinkoRows)).EndInit();
            this.panelWheel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudWheelSegs)).EndInit();
            this.panelBaccarat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudBacBanker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBacPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBacTie)).EndInit();
            this.panelRoulette.ResumeLayout(false);
            this.panelRoulette.PerformLayout();
            this.panelPump.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudPumpPumps)).EndInit();
            this.panelDragonTower.ResumeLayout(false);
            this.panelDragonTower.PerformLayout();
            this.panelBars.ResumeLayout(false);
            this.panelBars.PerformLayout();
            this.panelTomeOfLife.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudTomeLines)).EndInit();
            this.panelScarabSpin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudScarabLines)).EndInit();
            this.panelDiamonds.ResumeLayout(false);
            this.panelDiamonds.PerformLayout();
            this.panelCases.ResumeLayout(false);
            this.panelRps.ResumeLayout(false);
            this.panelRps.PerformLayout();
            this.panelFlip.ResumeLayout(false);
            this.panelFlip.PerformLayout();
            this.panelSnakes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudSnakesRolls)).EndInit();
            this.panelDarts.ResumeLayout(false);
            this.panelPacks.ResumeLayout(false);
            this.panelMoles.ResumeLayout(false);
            this.panelMoles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMolesMoles)).EndInit();
            this.panelChicken.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudChickenSteps)).EndInit();
            this.panelTarot.ResumeLayout(false);
            this.panelDrill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudDrillTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDrillPick)).EndInit();
            this.panelPrimedice.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudPDT1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPDT2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPDT3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPDT4)).EndInit();
            this.panelBlueSamurai.ResumeLayout(false);
            this.panelVideoPoker.ResumeLayout(false);
            this.panelVideoPoker.PerformLayout();
            this.panelBlackjack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BetDelayNumericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        // ── Field declarations ─────────────────────────────────────
        // Login
        private System.Windows.Forms.Label     lblApiKeyLabel, lblMirrorLabel, lblCookieStatus, lblWsIndicator, lblWsStatus, lblBalance;
        private System.Windows.Forms.TextBox   txtApiKey, txtMirror;
        private System.Windows.Forms.ComboBox  cmbFetchMode, cmbCurrency;
        private System.Windows.Forms.Button    btnGetCookie, btnLogin;
        // Separators & layout
        private System.Windows.Forms.Label     sepTop, sepToolbar, sepVertical, sepMid;
        private System.Windows.Forms.Label     lblGameLabel, lblBaseBetLabel, lblMultSep, lblMultX;
        // Toolbar
        private System.Windows.Forms.ComboBox  cmbGame;
        private System.Windows.Forms.NumericUpDown nudBaseBet;
        private System.Windows.Forms.Button    btnCondition, btnStart, btnStop;
        private System.Windows.Forms.CheckBox  chkStopMultiplier;
        private System.Windows.Forms.NumericUpDown nudStopMultiplier;
        // Body
        private System.Windows.Forms.Panel     pnlGameContainer;
        private System.Windows.Forms.ListView  lvHistory;
        private System.Windows.Forms.Panel    pnlHiloCards;
        private System.Windows.Forms.ListView lvHiloCards;
        private System.Windows.Forms.ColumnHeader colNum, colGame, colBet, colMulti, colPayout, colProfit, colRoll, colInfo, colBetId;
        private System.Windows.Forms.RichTextBox rtbLog;
        // Game panels
        private System.Windows.Forms.Panel panelDice, panelLimbo, panelHilo, panelMines, panelKeno;
        private System.Windows.Forms.Panel panelPlinko, panelWheel, panelBaccarat, panelRoulette, panelPump;
        private System.Windows.Forms.Panel panelDragonTower, panelBars, panelTomeOfLife, panelScarabSpin;
        private System.Windows.Forms.Panel panelDiamonds, panelCases, panelRps, panelFlip, panelSnakes;
        private System.Windows.Forms.Panel panelDarts, panelPacks, panelMoles, panelChicken, panelTarot;
        private System.Windows.Forms.Panel panelDrill, panelPrimedice, panelBlueSamurai, panelVideoPoker, panelBlackjack;
        // DICE
        private System.Windows.Forms.Label     lblDiceTitle, lblDiceDir, lblDiceChanceLbl, lblDiceInfo, lblDiceResultHint;
        private System.Windows.Forms.RadioButton rbDiceOver, rbDiceUnder;
        private System.Windows.Forms.NumericUpDown nudDiceChance;
        private System.Windows.Forms.CheckBox  chkStopDiceResult;
        private System.Windows.Forms.NumericUpDown nudStopDiceResult;
        // LIMBO
        private System.Windows.Forms.Label     lblLimboTitle, lblLimboTarget;
        private System.Windows.Forms.NumericUpDown nudLimboTarget;
        // HILO
        private System.Windows.Forms.Label     lblHiloTitle, lblHiloPatternLbl, lblHiloHelp, lblHiloStartLbl, lblHiloRankLbl, lblHiloSuitLbl, lblHiloCardHint;
        private System.Windows.Forms.Button    btnHiloClear;
        private System.Windows.Forms.TextBox   txtHiloPattern;
        private System.Windows.Forms.ComboBox  cmbHiloStartCard;
        private System.Windows.Forms.ComboBox  cmbHiloSuit;
        // MINES
        private System.Windows.Forms.Label     lblMinesTitle, lblMinesCountLbl, lblMinesFieldsLbl, lblMinesHint;
        private System.Windows.Forms.NumericUpDown nudMinesMines;
        private System.Windows.Forms.TextBox   txtMinesFields;
        // KENO
        private System.Windows.Forms.Label     lblKenoTitle, lblKenoNumLbl, lblKenoRiskLbl;
        private System.Windows.Forms.TextBox   txtKenoNumbers;
        private System.Windows.Forms.ComboBox  cmbKenoRisk;
        // PLINKO
        private System.Windows.Forms.Label     lblPlinkoTitle, lblPlinkoRowsLbl, lblPlinkoRiskLbl;
        private System.Windows.Forms.NumericUpDown nudPlinkoRows;
        private System.Windows.Forms.ComboBox  cmbPlinkoRisk;
        // WHEEL
        private System.Windows.Forms.Label     lblWheelTitle, lblWheelSegsLbl, lblWheelRiskLbl;
        private System.Windows.Forms.NumericUpDown nudWheelSegs;
        private System.Windows.Forms.ComboBox  cmbWheelRisk;
        // BACCARAT
        private System.Windows.Forms.Label     lblBaccaratTitle, lblBacBankerLbl, lblBacPlayerLbl, lblBacTieLbl;
        private System.Windows.Forms.NumericUpDown nudBacBanker, nudBacPlayer, nudBacTie;
        // ROULETTE
        private System.Windows.Forms.Label     lblRouletteTitle, lblRouletteChipsLbl;
        private System.Windows.Forms.TextBox   txtRouletteChips;
        // PUMP
        private System.Windows.Forms.Label     lblPumpTitle, lblPumpPumpsLbl, lblPumpDiffLbl;
        private System.Windows.Forms.NumericUpDown nudPumpPumps;
        private System.Windows.Forms.ComboBox  cmbPumpDiff;
        // DRAGON TOWER
        private System.Windows.Forms.Label     lblDragonTitle, lblDragonDiffLbl, lblDragonEggsLbl;
        private System.Windows.Forms.ComboBox  cmbDragonDiff;
        private System.Windows.Forms.TextBox   txtDragonEggs;
        // BARS
        private System.Windows.Forms.Label     lblBarsTitle, lblBarsDiffLbl, lblBarsTilesLbl;
        private System.Windows.Forms.ComboBox  cmbBarsDiff;
        private System.Windows.Forms.TextBox   txtBarsTiles;
        // TOME / SCARAB
        private System.Windows.Forms.Label     lblTomeTitle, lblTomeLinesLbl;
        private System.Windows.Forms.NumericUpDown nudTomeLines;
        private System.Windows.Forms.Label     lblScarabTitle, lblScarabLinesLbl;
        private System.Windows.Forms.NumericUpDown nudScarabLines;
        // DIAMONDS
        private System.Windows.Forms.Label     lblDiamondsTitle, lblDiamondsSelLbl, lblDiamondColLbl, lblDiamondCount;
        private System.Windows.Forms.Button    btnDiamondRed, btnDiamondYellow, btnDiamondGreen, btnDiamondCyan, btnDiamondBlue, btnDiamondOrange, btnDiamondPurple, btnDiamondsClear;
        private System.Windows.Forms.TextBox   txtDiamondColors;
        private System.Windows.Forms.CheckBox  chkStopDiamondsWin;
        // CASES
        private System.Windows.Forms.Label     lblCasesTitle, lblCasesDiffLbl;
        private System.Windows.Forms.ComboBox  cmbCasesDiff;
        // RPS / FLIP
        private System.Windows.Forms.Label     lblRpsTitle, lblRpsGuessLbl;
        private System.Windows.Forms.TextBox   txtRpsGuesses;
        private System.Windows.Forms.Label     lblFlipTitle, lblFlipGuessLbl;
        private System.Windows.Forms.TextBox   txtFlipGuesses;
        // SNAKES
        private System.Windows.Forms.Label     lblSnakesTitle, lblSnakesDiffLbl, lblSnakesRollsLbl;
        private System.Windows.Forms.ComboBox  cmbSnakesDiff;
        private System.Windows.Forms.NumericUpDown nudSnakesRolls;
        // DARTS
        private System.Windows.Forms.Label     lblDartsTitle, lblDartsDiffLbl;
        private System.Windows.Forms.ComboBox  cmbDartsDiff;
        // PACKS
        private System.Windows.Forms.Label     lblPacksTitle, lblPacksNote;
        // MOLES
        private System.Windows.Forms.Label     lblMolesTitle, lblMolesMolesLbl, lblMolesPicksLbl;
        private System.Windows.Forms.NumericUpDown nudMolesMoles;
        private System.Windows.Forms.TextBox   txtMolesPicks;
        // CHICKEN
        private System.Windows.Forms.Label     lblChickenTitle, lblChickenDiffLbl, lblChickenStepsLbl;
        private System.Windows.Forms.ComboBox  cmbChickenDiff;
        private System.Windows.Forms.NumericUpDown nudChickenSteps;
        // TAROT
        private System.Windows.Forms.Label     lblTarotTitle, lblTarotDiffLbl;
        private System.Windows.Forms.ComboBox  cmbTarotDiff;
        // DRILL
        private System.Windows.Forms.Label     lblDrillTitle, lblDrillTargetLbl, lblDrillPickLbl;
        private System.Windows.Forms.NumericUpDown nudDrillTarget, nudDrillPick;
        // PRIMEDICE
        private System.Windows.Forms.Label     lblPrimediceTitle, lblPDCondLbl, lblPDT1, lblPDT2, lblPDT3, lblPDT4;
        private System.Windows.Forms.ComboBox  cmbPDCond;
        private System.Windows.Forms.NumericUpDown nudPDT1, nudPDT2, nudPDT3, nudPDT4;
        // Note panels
        private System.Windows.Forms.Label     lblBlueSamuraiTitle, lblBlueSamuraiNote;
        private System.Windows.Forms.Label     lblVideoPokerTitle,  lblVideoPokerNote;
        private System.Windows.Forms.Label     lblVideoPokerStratLbl, lblVideoPokerHelp;
        private System.Windows.Forms.ComboBox  cmbVideoPokerStrat;
        private System.Windows.Forms.Label     lblBlackjackTitle,   lblBlackjackNote;
        private System.Windows.Forms.ComboBox FastModeBox;
        private System.Windows.Forms.NumericUpDown BetDelayNumericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblBetsPerSec;
        private System.Windows.Forms.Button ResetSeedBtn;
        // HILO manual-play buttons
        private System.Windows.Forms.Label  lblManualSep;
        private System.Windows.Forms.Button btnManualHigh;
        private System.Windows.Forms.Button btnManualLow;
        private System.Windows.Forms.Button btnManualEqual;
        private System.Windows.Forms.Button btnManualSkip;
        private System.Windows.Forms.Button btnManualCashout;
        private System.Windows.Forms.Button btnManualNewGame;
        private System.Windows.Forms.Button HighestMultiplierButton;
    }
}
