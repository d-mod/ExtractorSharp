using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Properties;

namespace ExtractorSharp.Draw.Brush {
    internal class Pencil : IBrush {
        public string Name => "Pencil";
        public Cursor Cursor => new Cursor(Resources.pencil.GetHicon());
        public int Radius { set; get; } = 1;
        public Point Location { set; get; }

        public void Draw(IPaint paint, Point point, decimal scale) {
            point = point.Minus(paint.Location).Divide(scale);
            var image = paint.Image;
            if (paint.Tag != null) {
                Program.Connector.Do("pencil", paint.Tag, point, Program.Drawer.Color);
            }
        }
    }
}