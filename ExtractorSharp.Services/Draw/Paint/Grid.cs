using System;
using System.Drawing;
using ExtractorSharp.Core.Draw;

namespace ExtractorSharp.Draw.Paint {
    public class Grid : IPaint, IFormattable {

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
            var gap = (int)this.Tag;
            gap = Math.Max(1, gap);
            for(var i = 0; i < this.Size.Width || i < this.Size.Height; i += gap) {
                if(i < this.Size.Width) {
                    g.DrawLine(Pens.White, new Point(i, 0), new Point(i, this.Size.Height));
                }
                if(i < this.Size.Height) {
                    g.DrawLine(Pens.White, new Point(0, i), new Point(this.Size.Width, i));
                }
            }
        }

        public string ToString(string formatString, IFormatProvider formatProvider) {
            if(formatProvider.GetFormat(this.GetType()) is ICustomFormatter formatter) {
                return formatter.Format(formatString, this, formatProvider);
            }
            return null;
        }

        public override string ToString() {
            return $"<{this.Name}>";
        }
    }
}