using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.Draw.Brush {
    /// <summary>
    ///     吸管
    /// </summary>
    internal class Straw : IBrush {
        public Color Color { get; set; } = Color.Aqua;
        private Drawer Drawer => Program.Drawer;
        public string Name => "Straw";

        public Cursor Cursor => Cursors.Cross;
        public int Radius { set; get; } = 1;
        public Point Location { set; get; }

        public void Draw(IPaint layer, Point point, decimal scale) {
            var image = layer.Image;
            if (image != null && layer.Contains(point)) {
                //得到鼠标相对图片的坐标
                var p = point.Minus(layer.Location).Divide(scale);
                var color = image.GetPixel(p.X, p.Y);
                Drawer.Color = color;
            }
        }
    }
}