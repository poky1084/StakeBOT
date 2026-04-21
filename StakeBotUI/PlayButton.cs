using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace StakeBotUI
{
    /// <summary>
    /// A clickable green gradient play-triangle button, drawn entirely with GDI+.
    /// Drop it on any form or panel — no image file required.
    /// </summary>
    public class PlayButton : Control
    {
        // ── State ────────────────────────────────────────────────────────
        private bool _hovered  = false;
        private bool _pressed  = false;

        // ── Colours (top-left highlight → bottom-right shadow) ───────────
        private static readonly Color _colorTop    = Color.FromArgb(120, 230, 60);   // bright lime
        private static readonly Color _colorBottom = Color.FromArgb(40,  150, 10);   // deep green
        private static readonly Color _colorHover  = Color.FromArgb(150, 255, 80);   // lighter on hover
        private static readonly Color _colorPress  = Color.FromArgb(30,  120,  5);   // darker on press

        public PlayButton()
        {
            // Flicker-free rendering
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

            // ── 1. Rounded-rect button background ────────────────────────
            float radius  = 5f;
            var   btnRect = new RectangleF(0.5f, 0.5f, w - 1f, h - 1f);

            using (GraphicsPath bgPath = RoundedRect(btnRect, radius))
            {
                // Base color sampled from the provided image: RGB(253, 253, 253)
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

            // ── 2. Centered play triangle ────────────────────────────────
            int   side = (int)(Math.Min(w, h) * 0.52f);
            float cx   = w / 2f;
            float cy   = h / 2f;
            float half = side / 2f;

            PointF[] triangle =
            {
                new PointF(cx - half, cy - half),
                new PointF(cx + half, cy),
                new PointF(cx - half, cy + half)
            };

            using (GraphicsPath triPath = RoundedTriangle(triangle, cornerRadius: side * 0.08f))
            {
                Color gradTop = _pressed ? _colorPress
                              : _hovered ? _colorHover
                              : _colorTop;

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new PointF(cx - half, cy - half),
                    new PointF(cx + half, cy + half),
                    gradTop, _colorBottom))
                {
                    ColorBlend blend = new ColorBlend(3);
                    blend.Colors    = new[] { gradTop, Color.FromArgb(70, 190, 30), _colorBottom };
                    blend.Positions = new[] { 0f, 0.45f, 1f };
                    brush.InterpolationColors = blend;
                    g.FillPath(brush, triPath);
                }

                using (Pen highlight = new Pen(Color.FromArgb(80, 255, 255, 255), 1.0f))
                    g.DrawPath(highlight, triPath);

                if (!Enabled)
                    using (SolidBrush dim = new SolidBrush(Color.FromArgb(130, 30, 30, 30)))
                        g.FillPath(dim, triPath);
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

        /// <summary>
        /// Returns a GraphicsPath for a triangle with softly rounded corners.
        /// </summary>
        private static GraphicsPath RoundedTriangle(PointF[] pts, float cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();
            float r = Math.Max(1f, cornerRadius);

            for (int i = 0; i < 3; i++)
            {
                PointF prev = pts[(i + 2) % 3];
                PointF curr = pts[i];
                PointF next = pts[(i + 1) % 3];

                // Unit vectors for the two edges meeting at curr
                PointF v1 = Normalize(Sub(prev, curr));
                PointF v2 = Normalize(Sub(next, curr));

                PointF arcStart = Add(curr, Scale(v1, r));
                PointF arcEnd   = Add(curr, Scale(v2, r));

                if (i == 0) path.StartFigure();
                path.AddLine(arcStart, arcEnd);      // straight segment (arc approximation)
                // For a proper arc use AddBezier; a simple line gives slightly softer look for small r
            }

            path.CloseFigure();
            return path;
        }

        private static PointF Sub(PointF a, PointF b) => new PointF(a.X - b.X, a.Y - b.Y);
        private static PointF Add(PointF a, PointF b) => new PointF(a.X + b.X, a.Y + b.Y);
        private static PointF Scale(PointF v, float s) => new PointF(v.X * s, v.Y * s);
        private static PointF Normalize(PointF v)
        {
            float len = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
            return len < 0.0001f ? v : new PointF(v.X / len, v.Y / len);
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
