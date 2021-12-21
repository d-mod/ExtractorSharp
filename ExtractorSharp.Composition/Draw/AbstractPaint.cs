using System;
using System.Drawing;

namespace ExtractorSharp.Core.Draw {
    public abstract class AbstractPaint : IPaint, IFormattable {


        public virtual bool Visible { set; get; }
        public virtual bool Locked { set; get; }
        public virtual Bitmap Image { set; get; }


        public virtual Size Size { set; get; }

        public virtual Point Location { set; get; }

        public virtual Rectangle Rectangle { set; get; }

        public virtual object Tag { set; get; }

        public abstract string Name { set; get; }

        public abstract bool Contains(Point point);

        public abstract void Draw(Graphics g);

        public override string ToString() {
            return $"<{this.Name}>,<Position>{this.Location.GetString()},<Size>{this.Size.GetString()}";
        }

        public string ToString(string formatString, IFormatProvider formatProvider) {
            if(formatProvider.GetFormat(this.GetType()) is ICustomFormatter formatter) {
                return formatter.Format(formatString, this, formatProvider);
            }
            return null;
        }
    }
}
