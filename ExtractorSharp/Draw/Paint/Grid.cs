using System;
using System.Drawing;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Draw.Paint {
    internal class Grid : IPaint {
        public string Name { set; get; } = "Grid";
        public Bitmap Image { set; get; }
        public Size Size { set; get; }
        public Point Location { set; get; }

        public Rectangle Rectangle { set; get; }

        public object Tag { set; get; } = 100;
        public bool Visible { set; get; }

        public bool Locked {
            set { }
            get => true;
        }

        public bool Contains(Point point) {
            return false;
        }

        public void Draw(Graphics g) {
            var gap = (int) Tag;
            gap = Math.Max(1, gap);
            for (var i = 0; i < Size.Width || i < Size.Height; i += gap) {
                if (i < Size.Width) g.DrawLine(Pens.White, new Point(i, 0), new Point(i, Size.Height));
                if (i < Size.Height) g.DrawLine(Pens.White, new Point(0, i), new Point(Size.Width, i));
            }
        }

        public override string ToString() {
            return $"{Language.Default[Name]}";
        }
    }
}