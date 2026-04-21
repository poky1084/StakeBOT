using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace StakeBotUI
{
    /// <summary>
    /// A clickable stop button drawn entirely with GDI+.
    /// Matches PlayButton's style: same rounded-rect shell, same background,
    /// but shows a red gradient square (■) instead of the green triangle.
    /// </summary>
    public class StopButton : Control
    {
        // ── State ────────────────────────────────────────────────────────
        private bool _hovered = false;
        private bool _pressed = false;

        // ── Square colours ───────────────────────────────────────────────
        private static readonly Color _colorTop    = Color.FromArgb(255, 80,  60);   // bright red
        private static readonly Color _colorBottom = Color.FromArgb(160, 10,  10);   // deep crimson
        private static readonly Color _colorHover  = Color.FromArgb(255, 110, 90);   // lighter on hover
        private static readonly Color _colorPress  = Color.FromArgb(130,  5,   5);   // darker on press

        public StopButton()
        {
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint  |
                ControlStyles.UserPaint             |
                ControlStyles.ResizeRedraw          |
                ControlStyles.SupportsTransparentBackColor,
                true);

            BackColor = Color.Transparent;
            Size      = new Size(48, 48);
            Cursor    = Cursors.Hand;
        }

        // ── Mouse tracking ───────────────────────────────────────────────
        protected override void OnMouseEnter(EventArgs e) { if (Enabled) { _hovered = true;  Invalidate(); } base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hovered = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { if (Enabled && e.Button == MouseButtons.Left) { _pressed = true;  Invalidate(); } base.OnMouseDown(e); }
        protected override void OnMouseUp  (MouseEventArgs e) { _pressed = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnEnabledChanged(EventArgs e) { _hovered = false; _pressed = false; Cursor = Enabled ? Cursors.Hand : Cursors.Default; Invalidate(); base.OnEnabledChanged(e); }

        // ── Paint ────────────────────────────────────────────────────────
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = ClientSize.Width;
            int h = ClientSize.Height;

            // ── 1. Rounded-rect button background (same as PlayButton) ───
            float radius  = 5f;
            var   btnRect = new RectangleF(0.5f, 0.5f, w - 1f, h - 1f);

            using (GraphicsPath bgPath = RoundedRect(btnRect, radius))
            {
                Color bgColor = !Enabled ? Color.FromArgb(220, 220, 220)   // light gray when disabled
                              : _pressed ? Color.FromArgb(225, 225, 225)
                              : _hovered ? Color.FromArgb(240, 240, 240)
                              : Color.FromArgb(253, 253, 253);

                using (SolidBrush bgBrush = new SolidBrush(bgColor))
                    g.FillPath(bgBrush, bgPath);

                Color borderColor = !Enabled ? Color.FromArgb(190, 190, 190)
                                  : _pressed ? Color.FromArgb(160, 160, 160)
                                  : _hovered ? Color.FromArgb(180, 180, 180)
                                  : Color.FromArgb(200, 200, 200);

                using (Pen borderPen = new Pen(borderColor, 1.5f))
                    g.DrawPath(borderPen, bgPath);
            }

            // ── 2. Centered stop square ──────────────────────────────────
            int   side = (int)(Math.Min(w, h) * 0.46f);
            float cx   = w / 2f;
            float cy   = h / 2f;
            float half = side / 2f;

            float sqLeft   = cx - half;
            float sqTop    = cy - half;
            float sqRadius = side * 0.12f;   // slightly rounded square corners

            var squareRect = new RectangleF(sqLeft, sqTop, side, side);

            using (GraphicsPath sqPath = RoundedRect(squareRect, sqRadius))
            {
                Color gradTop = _pressed ? _colorPress
                              : _hovered ? _colorHover
                              : _colorTop;

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new PointF(sqLeft,        sqTop),
                    new PointF(sqLeft + side, sqTop + side),
                    gradTop, _colorBottom))
                {
                    ColorBlend blend = new ColorBlend(3);
                    blend.Colors    = new[] { gradTop, Color.FromArgb(220, 40, 30), _colorBottom };
                    blend.Positions = new[] { 0f, 0.45f, 1f };
                    brush.InterpolationColors = blend;
                    g.FillPath(brush, sqPath);
                }

                using (Pen highlight = new Pen(Color.FromArgb(80, 255, 255, 255), 1.0f))
                    g.DrawPath(highlight, sqPath);

                // Dim when disabled
                if (!Enabled)
                    using (SolidBrush dim = new SolidBrush(Color.FromArgb(130, 30, 30, 30)))
                        g.FillPath(dim, sqPath);
            }
        }

        // ── Helpers ──────────────────────────────────────────────────────

        private static GraphicsPath RoundedRect(RectangleF r, float radius)
        {
            float d = radius * 2f;
            GraphicsPath p = new GraphicsPath();
            p.AddArc(r.Left,      r.Top,      d, d, 180, 90);
            p.AddArc(r.Right - d, r.Top,      d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom-d, d, d,   0, 90);
            p.AddArc(r.Left,      r.Bottom-d, d, d,  90, 90);
            p.CloseFigure();
            return p;
        }

        // Transparent background needs parent repaint
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT
                return cp;
            }
        }
    }
}
