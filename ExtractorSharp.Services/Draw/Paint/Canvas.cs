using System.Drawing;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Draw.Paint {
    public class Canvas : AbstractPaint, IAttractable {

        public Point RealLocation {
            get {
                var location = this.Location;
                if(this.RealPosition && this.Tag is Sprite sprite) {
                    location = location.Add(sprite.Location);
                }
                return location;
            }
            set {
                var location = value;
                if(this.RealPosition && this.Tag is Sprite sprite) {
                    location = location.Minus(sprite.Location);
                }
                this.Location = location;
            }
        }

        public Point Offset { set; get; } = Point.Empty;

        public Size CanvasSize { set; get; }

        public bool RealPosition { set; get; }

        public override string Name { set; get; }

        public override Bitmap Image { set; get; }

        public override Rectangle Rectangle => new Rectangle(this.RealLocation, this.Size);

        public override bool Contains(Point point) {
            return this.Rectangle.Contains(point);
        }

        public override void Draw(Graphics g) {
            if(this.Tag != null && this.Image != null) {
                g.DrawImage(this.Image, this.Rectangle);
            }
        }


        public int Range => 20;


    }
}