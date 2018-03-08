using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;

namespace ExtractorSharp.Draw.Paint {
    class Rule : IPaint {
        public Bitmap Image { set; get; }
        public Size Size { set; get; }
        public Point Location { set; get; }

        public Rectangle Rectangle { set; get; }

        public object Tag { set; get; }

        public bool FullCanvas { set; get; }= true;
        public bool Visible { set; get; }
        public bool Locked { set; get; }


        private int rule_radius = 20;

        public bool Contains(Point point) {
            var rp = Location.Minus(point);
            if ((rp.X * rp.X + rp.Y * rp.Y) < rule_radius * rule_radius) {
                return true;
            }
            return false;
        }
        public void Draw(Graphics g) {
            var rp = Location;
            var rule_point = (Point)Tag;
            var font = SystemFonts.DefaultFont;
            g.DrawString($"{Language.Default["AbsolutePosition"]}:{rp.GetString()}", font, Brushes.White, new Point(rp.X + rule_radius, rp.Y - rule_radius - font.Height));
            g.DrawString($"{Language.Default["RealativePosition"]}:{rule_point.Reverse().GetString()}", font, Brushes.White, new Point(rp.X + rule_radius, rp.Y - rule_radius - font.Height * 2));
            g.DrawLine(Pens.White, new Point(rp.X, 0), new Point(rp.X, Size.Height));
            g.DrawLine(Pens.White, new Point(0, rp.Y), new Point(Size.Width, rp.Y));
            var x = rp.X - rule_radius;
            var y = rp.Y - rule_radius;
            g.DrawEllipse(Pens.WhiteSmoke, x, y, rule_radius * 2, rule_radius * 2);
        }
    }
}
