using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.Draw.Brush {
    internal class Eraser : IBrush {
        public Eraser() {
            RefreshCursor();
        }

        public string Name => "Eraser";

        public int Radius { set; get; } = 10;

        public Cursor Cursor { set; get; }
        public Point Location { set; get; }

        public void Draw(IPaint paint, Point point, decimal scale) {
            point = point.Minus(paint.Location).Divide(scale);
            if (paint.Tag != null) Program.Connector.Do("eraser", paint.Tag, point, Program.Drawer.Color, Radius);
        }

        public void RefreshCursor() {
            var image = new Bitmap(Radius * 2, Radius * 2);
            using (var g = Graphics.FromImage(image)) {
                g.DrawEllipse(new Pen(Color.Black, 2), new Rectangle(0, 0, Radius * 2, Radius * 2));
            }

            Cursor = new Cursor(image.GetHicon());
        }
    }
}