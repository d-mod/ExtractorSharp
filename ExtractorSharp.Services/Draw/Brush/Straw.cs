using System;
using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Draw.Paint;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Draw.Brush {
    /// <summary>
    ///     吸管
    /// </summary>
    [Export(typeof(IBrush))]
    internal class Straw : IBrush {

        public Color Color { get; set; } = Color.Aqua;

        public string Name => "Straw";

        public IntPtr Cursor { get; }

        public int Radius { set; get; } = 1;

        public Point Location { set; get; }

        public void Draw(IPaint layer, Point point, decimal scale) {
            var image = layer.Image;
            if(image != null && layer.Contains(point)) {
                if(layer is Canvas canvas && canvas.RealPosition && canvas.Tag is Sprite sprite) {
                    point = point.Minus(sprite.Location);
                }
                //得到鼠标相对图片的坐标
                var p = point.Minus(layer.Location).Divide(scale);
                var color = image.GetPixel(p.X, p.Y);
                // Drawer.Color = color;
            }
        }
    }
}