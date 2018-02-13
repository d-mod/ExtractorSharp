using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.Properties;
using ExtractorSharp.Data;

namespace ExtractorSharp.Draw.Brush {
    class Pencil : IBrush {
        public string Name => "Pencil";
        public Cursor Cursor => new Cursor(Resources.pencil.GetHicon());
        public int Radius { set; get; } = 1;
        public Point Location { set; get; }
        public void Draw(IPaint paint, Point point, decimal scale) {
            point = point.Minus(paint.Location).Divide(scale);
            var image = paint.Image;
            if (paint.Tag != null && paint.Rectangle.Contains(point)) {
                Program.Controller.Do("pencil", paint.Tag, point, Program.Drawer.Color);
            }
        }

    }
}
