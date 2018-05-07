using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;

namespace ExtractorSharp.Draw.Paint {
    class Ruler : IPaint {
        public string Name { set; get; } = "Ruler";
        public Bitmap Image { set; get; }
        public Size Size { set; get; }
        public Point Location { set; get; }

        public Rectangle Rectangle { set; get; }

        public object Tag { set; get; }

        public bool FullCanvas { set; get; }= true;
        public bool Visible { set; get; }
        public bool Locked { set; get; }


        public bool DrawSpan { set; get; } = true;
        public int SmallSpan { set; get; } = 5;
        public int BigSpan { set; get; } = 200;
        public int Span { set; get; } = 50;

        private int rule_radius = 20;

        public bool Contains(Point point) {
            var rp = Location.Minus(point);
            if ((rp.X * rp.X + rp.Y * rp.Y) < rule_radius * rule_radius) {
                return true;
            }
            return false;
        }

        private void DrawSpans(Graphics g) {
            var rp = Location;
            var font = SystemFonts.DefaultFont;

            for (var i = rp.X % SmallSpan; i < Size.Width; i += SmallSpan) {
                g.DrawLine(Pens.White, new Point(i, rp.Y), new Point(i, rp.Y - 5));
            }
            for (var i = 0; i < rp.X; i += Span) {
                var h = (i % (BigSpan)) == 0 ? 15 : 10;
                g.DrawString($"{-i}px", font, Brushes.White, new Point(rp.X - i, rp.Y));
                g.DrawLine(Pens.White, new Point(rp.X - i, rp.Y), new Point(rp.X - i, rp.Y - h));
            }
            for (var i = rp.X; i < Size.Width; i += Span) {
                var h = ((i - rp.X) % (BigSpan)) == 0 ? 15 : 10;
                g.DrawString($"{i - rp.X}px", font, Brushes.White, new Point(i, rp.Y));
                g.DrawLine(Pens.White, new Point(i, rp.Y), new Point(i, rp.Y - h));
            }

            for (var i = rp.Y % SmallSpan; i < Size.Height; i += SmallSpan) {
                g.DrawLine(Pens.White, new Point(rp.X, i), new Point(rp.X - 5, i));
            }
            for (var i = 0; i < rp.Y; i += Span) {
                var h = (i % (BigSpan)) == 0 ? 15 : 10;
                g.DrawString($"{-i}px", font, Brushes.White, new Point(rp.X, rp.Y - i));
                g.DrawLine(Pens.White, new Point(rp.X, rp.Y - i), new Point(rp.X - h, rp.Y - i));
            }

            for (var i = rp.Y; i < Size.Height; i += Span) {
                var h = ((i - rp.Y) % (BigSpan)) == 0 ? 15 : 10;
                g.DrawString($"{i - rp.Y}px", font, Brushes.White, new Point(rp.X, i));
                g.DrawLine(Pens.White, new Point(rp.X, i), new Point(rp.X - h, i));
            }
        }

        public void Draw(Graphics g) {
            var rp = Location;
            var rule_point = (Point)Tag;
            var font = SystemFonts.DefaultFont;
            var x = rp.X - rule_radius;
            var y = rp.Y - rule_radius;
            if (DrawSpan) {
                DrawSpans(g);
            }
            g.DrawLine(Pens.White, new Point(rp.X, 0), new Point(rp.X, Size.Height));
            g.DrawLine(Pens.White, new Point(0, rp.Y), new Point(Size.Width, rp.Y));
            g.DrawEllipse(Pens.WhiteSmoke, x, y, rule_radius * 2, rule_radius * 2);
        }

        public override string ToString() {
            return $"{Language.Default[Name]},{Language.Default["Position"]}({Location.X},{Location.Y})";
        }
    }
}
