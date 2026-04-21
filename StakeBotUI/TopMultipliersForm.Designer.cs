namespace StakeBotUI
{
    partial class TopMultipliersForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lvTop    = new System.Windows.Forms.ListView();
            this.colBetId = new System.Windows.Forms.ColumnHeader();
            this.colGame  = new System.Windows.Forms.ColumnHeader();
            this.colMulti = new System.Windows.Forms.ColumnHeader();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // ── lblTitle ──────────────────────────────────────────
            this.lblTitle.Text      = "🏆  Top 10 Highest Multipliers";
            this.lblTitle.Font      = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(40, 40, 80);
            this.lblTitle.Location  = new System.Drawing.Point(8, 8);
            this.lblTitle.Size      = new System.Drawing.Size(380, 22);
            this.lblTitle.AutoSize  = false;

            // ── lvTop ─────────────────────────────────────────────
            this.lvTop.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {
                this.colBetId,
                this.colGame,
                this.colMulti
            });
            this.colBetId.Text  = "Bet ID";
            this.colBetId.Width = 145;
            this.colGame.Text   = "Game";
            this.colGame.Width  = 90;
            this.colMulti.Text  = "Multiplier";
            this.colMulti.Width = 90;

            this.lvTop.FullRowSelect     = true;
            this.lvTop.GridLines         = true;
            this.lvTop.HeaderStyle       = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTop.Location          = new System.Drawing.Point(8, 36);
            this.lvTop.Name              = "lvTop";
            this.lvTop.OwnerDraw         = true;
            this.lvTop.Size              = new System.Drawing.Size(352, 252);
            this.lvTop.TabIndex          = 0;
            this.lvTop.View              = System.Windows.Forms.View.Details;
            this.lvTop.Font              = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lvTop.Cursor            = System.Windows.Forms.Cursors.Hand;
            // Inflate row height so the 11pt multiplier text isn't clipped
            this.lvTop.SmallImageList    = new System.Windows.Forms.ImageList { ImageSize = new System.Drawing.Size(1, 22) };

            this.lvTop.DrawColumnHeader  += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.LvTop_DrawColumnHeader);
            this.lvTop.DrawItem          += new System.Windows.Forms.DrawListViewItemEventHandler(this.LvTop_DrawItem);
            this.lvTop.DrawSubItem       += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.LvTop_DrawSubItem);
            this.lvTop.MouseDown         += new System.Windows.Forms.MouseEventHandler(this.LvTop_MouseDown);
            this.lvTop.MouseUp           += new System.Windows.Forms.MouseEventHandler(this.LvTop_MouseUp);

            // ── btnClear ──────────────────────────────────────────
            this.btnClear.Text      = "🗑  Clear List";
            this.btnClear.Font      = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnClear.Location  = new System.Drawing.Point(8, 296);
            this.btnClear.Size      = new System.Drawing.Size(352, 28);
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 50, 50);
            this.btnClear.ForeColor = System.Drawing.Color.FromArgb(180, 30, 30);
            this.btnClear.TabIndex  = 1;
            this.btnClear.Click    += new System.EventHandler(this.BtnClear_Click);

            // ── Form ──────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(368, 334);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lvTop);
            this.Controls.Add(this.btnClear);
            this.Font            = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.MinimizeBox     = true;
            this.Name            = "TopMultipliersForm";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.Manual;
            this.Text            = "Top Multipliers";
            this.BackColor       = System.Drawing.Color.FromArgb(245, 245, 250);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ListView   lvTop;
        private System.Windows.Forms.ColumnHeader colBetId;
        private System.Windows.Forms.ColumnHeader colGame;
        private System.Windows.Forms.ColumnHeader colMulti;
        private System.Windows.Forms.Button     btnClear;
        private System.Windows.Forms.Label      lblTitle;
    }
}
