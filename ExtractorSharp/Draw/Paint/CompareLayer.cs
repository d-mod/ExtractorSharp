using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Draw.Paint {
    class CompareLayer : ILayer {
        public string Name { set; get; }
        public bool Visible { set; get; }
        public bool Locked { set; get; }
        public Bitmap Image { set { } get => Sprite?.Picture; }
        public Size Size {
            get {
                if (Sprite == null) {
                    return Size.Empty;
                }
                return Sprite.Size.Star(ImageScale);
            }
            set { }
        }
        public Point Location { set; get; }

        public Rectangle Rectangle => new Rectangle(RealLocation, Size);

        public  object Tag { set; get; }

        public decimal ImageScale { set; get; }

        public Album File => Tag as Album;

        public Sprite Sprite {
            get {
                if (File != null && Index > -1 && Index < File.List.Count) {
                    return File[Index];
                }
                return null;
            }
        }

        private Point RealLocation {
            get {
                var location = Location;
                if (RealPosition) {
                    if (Sprite!=null) {
                        location = location.Add(Sprite.Location);
                    }
                }
                return location;
            }
        }


        public int Index { set; get; }
        public bool RealPosition {
            set {
                _realPosition = value;
                if (!value) {
                    Location = Point.Empty;
                }
            }
            get => _realPosition;
        }

        private bool _realPosition;

        public bool Contains(Point point) {
            return Rectangle.Contains(point);
        }


        public void Draw(Graphics g) {
            if (Image == null) {
                return;
            }
            g.DrawImage(Image, Rectangle);
        }

        public override string ToString() {
            return $"{Name},{Language.Default["Position"]}{Location.GetString()},{Language.Default["Size"]}{Size.GetString()}";
        }
    }
}
