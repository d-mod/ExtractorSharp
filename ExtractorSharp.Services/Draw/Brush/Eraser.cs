using System;
using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Draw.Paint;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Draw.Brush {

    [Export(typeof(IBrush))]
    internal class Eraser : InjectService, IBrush {

        public Eraser() {
            this.RefreshCursor();
        }

        public string Name => "Eraser";

        public int Radius { set; get; } = 10;

        public IntPtr Cursor { set; get; }

        public Point Location { set; get; }


        public void Draw(IPaint paint, Point point, decimal scale) {
            point = point.Minus(paint.Location);
            if(paint is Canvas canvas && canvas.RealPosition) {
                if(canvas.Tag is Sprite sprite) {
                    point = point.Minus(sprite.Location);
                }
            }
            point = point.Divide(scale);
            if(paint.Tag != null) {
                // Controller.Do("eraser", paint.Tag, point, Drawer.Color, Radius);
            }
        }

        public void RefreshCursor() {
            var image = new Bitmap(this.Radius * 2, this.Radius * 2);
            using(var g = Graphics.FromImage(image)) {
                g.DrawEllipse(new Pen(Color.Black, 2), new Rectangle(0, 0, this.Radius * 2, this.Radius * 2));
            }
            this.Cursor = image.GetHicon();
        }
    }
}