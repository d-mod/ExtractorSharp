using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using ExtractorSharp.Properties;

namespace ExtractorSharp.Draw.Brush{
    class Pencil : IBrush {
        public Cursor Cursor => new Cursor(Resources.pencil.GetHicon());
        public int Radius { set; get; } = 1;
        public Point Location { set; get; }
        public void Draw(IPaint paint, Point newPoint,decimal scale) {
            newPoint = newPoint.Minus(paint.Location).Divide(scale);
            if (paint.Tag != null) {
                Program.Controller.Do("pencil", paint.Tag, newPoint, Program.Drawer.Color);
            }
        }
        
    }
}
