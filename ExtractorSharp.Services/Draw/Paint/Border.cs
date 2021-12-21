using System.Drawing;
using ExtractorSharp.Core.Draw;

namespace ExtractorSharp.Draw.Paint {

    public class Border : AbstractPaint {

        public override string Name { set; get; } = "Border";

        public override Size Size {
            set { }
            get => this.Rectangle.Size;
        }

        public override Point Location {
            set { }
            get => this.Rectangle.Location;
        }

        public override Rectangle Rectangle {
            set { }
            get => this.Tag == null ? Rectangle.Empty : (Rectangle)this.Tag;
        }

        public override bool Contains(Point point) {
            return false;
        }

        public override void Draw(Graphics g) {
            g.DrawRectangle(Pens.White, this.Rectangle);
        }
    }
}