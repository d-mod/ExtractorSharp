using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System.Drawing;

namespace ExtractorSharp.Draw.Paint {
    public class Canvas : IPaint {
        public string Name { set; get; }
        public Bitmap Image { set; get; }
        public Rectangle Rectangle => new Rectangle(_Location, Size);

        private Point _Location {
            get {
                var location = Location;
                if (RealPosition) {
                    if (Tag is Sprite sprite) {
                        location = location.Add(sprite.Location);
                    }
                } 
                return location;
            }
        }

        public bool Contains(Point point) => Rectangle.Contains(point);
        public Point Offset { set; get; } = Point.Empty;
        public Size CanvasSize { set; get; }
        public void Draw(Graphics g) {
            if (Tag != null && Image != null) {
                g.DrawImage(Image, Rectangle);
            }
        }

        public Point Location { set; get; } = Point.Empty;
        public Size Size { set; get; }
        public object Tag { set; get; }
        public bool Visible { set; get; }
        public bool Locked { set; get; }

        public bool RealPosition {
            set {
                _realPostion = value;
                if (!value) {
                    Location = Point.Empty;
                }
            }
            get {
                return _realPostion;
            }
        }

        private bool _realPostion;

        public override string ToString() {
            return $"{Language.Default[Name]},{Language.Default["Position"]}({_Location.GetString()}),{Language.Default["Size"]}({Size.GetString()})";
        }
    }
}
