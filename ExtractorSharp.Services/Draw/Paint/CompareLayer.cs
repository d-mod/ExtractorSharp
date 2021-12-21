using System.Drawing;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Draw.Paint {
    public class CompareLayer : AbstractPaint {

        public override string Name { set; get; }

        public override Bitmap Image { set { } get => this.Sprite?.Image; }

        public override Size Size {
            get {
                if(this.Sprite == null) {
                    return Size.Empty;
                }
                return this.Sprite.Size.Star(this.ImageScale);
            }
            set { }
        }

        public override Rectangle Rectangle => new Rectangle(this.RealLocation, this.Size);

        public decimal ImageScale { set; get; }

        public Album File => this.Tag as Album;

        public Language Language { set; get; } = Language.Empty;

        public Sprite Sprite {
            get {
                if(this.File != null && this.Index > -1 && this.Index < this.File.List.Count) {
                    return this.File[this.Index];
                }
                return null;
            }
        }

        private Point RealLocation {
            get {
                var location = this.Location;
                if(this.RealPosition) {
                    if(this.Sprite != null) {
                        location = location.Add(this.Sprite.Location);
                    }
                }
                return location;
            }
        }


        public int Index { set; get; }
        public bool RealPosition {
            set {
                this._realPosition = value;
                if(!value) {
                    this.Location = Point.Empty;
                }
            }
            get => this._realPosition;
        }

        private bool _realPosition;

        public override bool Contains(Point point) {
            return this.Rectangle.Contains(point);
        }


        public override void Draw(Graphics g) {
            if(this.Image == null) {
                return;
            }
            g.DrawImage(this.Image, this.Rectangle);
        }


    }
}
