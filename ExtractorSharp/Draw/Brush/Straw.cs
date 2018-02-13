using ExtractorSharp.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.Draw.Brush{
    /// <summary>
    /// 吸管
    /// </summary>
    class Straw : IBrush {
        public string Name => "Straw";

        public Cursor Cursor => Cursors.Cross;

        public Color Color { get; set; } = Color.Aqua;
        public int Radius { set; get; } = 1;
        public Point Location { set; get; }
        private Drawer Drawer => Program.Drawer;

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
