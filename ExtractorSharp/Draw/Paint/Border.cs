using System.Drawing;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Draw.Paint {
    internal class Border : IPaint {
        public string Name { set; get; } = "Border";
        public bool Locked { set; get; }
        public bool Visible { set; get; }
        public Bitmap Image { set; get; }

        public Size Size {
            set { }
            get => Rectangle.Size;
        }

        public Point Location {
            set { }
            get => Rectangle.Location;
        }

        public Rectangle Rectangle {
            set { }
            get => Tag == null ? Rectangle.Empty : (Rectangle) Tag;
        }

        public object Tag { set; get; }


        public bool Contains(Point point) {
            return false;
        }

        public void Draw(Graphics g) {
            g.DrawRectangle(Pens.White, Rectangle);
        }

        public override string ToString() {
            return
                $"{Language.Default[Name]},{Language.Default["Position"]}({Location.X},{Location.Y}),{Language.Default["Size"]}:({Size.Width},{Size.Height})";
        }
    }
}