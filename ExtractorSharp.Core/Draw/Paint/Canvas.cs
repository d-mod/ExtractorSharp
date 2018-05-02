using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System.Drawing;

namespace ExtractorSharp.Draw.Paint {
    public class Canvas : IPaint {
        public string Name { set; get; }
        public Bitmap Image { set; get; }
        public Rectangle Rectangle => new Rectangle(Location, Size);
        public bool Contains(Point point) => Rectangle.Contains(point);
        public Point Offset { set; get; } = Point.Empty;
        public Size CanvasSize { set; get; }
        public void Draw(Graphics g) {
            if (Image != null) {
                g.DrawImage(Image, Rectangle);
            }
        }

        public Point Location { set; get; } = Point.Empty;
        public Size Size { set; get; }
        public object Tag { set; get; }
        public bool FullCanvas { set; get; }
        public bool Visible { set; get; }
        public bool Locked { set; get; }

        public override string ToString() {
            return $"{Language.Default[Name]},{Language.Default["Position"]}({Location.X},{Location.Y}),{Language.Default["Size"]}({Size.Width},{Size.Height})";
        }
    }
}
