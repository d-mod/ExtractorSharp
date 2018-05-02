using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Data;
using ExtractorSharp.Draw;

namespace ExtractorSharp.Draw.Paint {
    class Grid : IPaint {
        public string Name { set; get; } = "Grid";
        public Bitmap Image { set; get; }
        public Size Size { set; get; }
        public Point Location { set; get; }

        public Rectangle Rectangle { set; get; }

        public object Tag { set; get; }
        public bool FullCanvas { set; get; }
        public bool Visible { set; get; }
        public bool Locked { set; get; }

        public bool Contains(Point point) {
            return false;
        }

        public void Draw(Graphics g) {
            var gap = (int)Tag;
            gap = Math.Max(1, gap);
            for (var i = 0; i < Size.Width || i < Size.Height; i += gap) {
                if (i < Size.Width) {
                    g.DrawLine(Pens.White, new Point(i, 0), new Point(i, Size.Height));
                }
                if (i < Size.Height) {
                    g.DrawLine(Pens.White, new Point(0, i), new Point(Size.Width, i));
                }
            }
        }
        public override string ToString() {
            return $"{Language.Default[Name]}";
        }
    }
}
