using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Draw.Paint {
    class Border : IPaint {
        public string Name { set; get; }="Border";
        public bool FullCanvas { set; get; }
        public bool Locked { set; get; }
        public bool Visible { set; get; }
        public Bitmap Image { set; get; }
        public Size Size { set; get; }
        public Point Location { set; get; }
        public Rectangle Rectangle { set { }
            get {
                return  (Rectangle)Tag;
            }
        }

        public object Tag { set; get; }
        

        public bool Contains(Point point) {
            return false;
        }

        public void Draw(Graphics g) {
            g.DrawRectangle(Pens.White,Rectangle);
        }
    }
}
