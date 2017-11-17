using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Draw.Paint {
    class Cavas : IPaint {
        public Bitmap Image { set; get; }
        public Rectangle Rectangle => new Rectangle(Location, Size);
        public bool Contains(Point point) => Rectangle.Contains(point);
        public void Move(Point point) => Location = Location.Add(point);
        public void Draw(Graphics g) {
            if (Image != null) {
                g.DrawImage(Image, Rectangle);
            }
        }

        public Point Location { set; get; } = Point.Empty;
        public Size Size { set; get; }
        public object Tag { set; get; }
    }
}
