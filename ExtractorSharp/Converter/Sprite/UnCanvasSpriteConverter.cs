using ExtractorSharp.Composition;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Converter.Sprite {
    class UnCanvasSpriteConverter : ISpriteConverter {
        public string Name => "UnCanvasImage";

        public bool Enable { set; get; }

        public void Convert(Data.Sprite sprite, ref Bitmap bmp) {
            var rct = bmp.Scan();
            var image = new Bitmap(rct.Width, rct.Height);
            using (var g = Graphics.FromImage(image)) {
                var empty = new Rectangle(Point.Empty, rct.Size);
                g.DrawImage(bmp, empty, rct, GraphicsUnit.Pixel);
            }
            bmp = image;
        }
    }
}
