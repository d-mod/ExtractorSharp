using System.Drawing;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.Effect {
    internal class TrimImageEffect : IEffect {
        public string Name => "TrimImage";

        public bool Enable { set; get; }
        public int Index { set; get; } = -1;

        public void Handle(Core.Model.Sprite sprite, ref Bitmap bmp) {
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