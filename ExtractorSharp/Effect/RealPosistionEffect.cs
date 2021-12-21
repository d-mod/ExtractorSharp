using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Effect {

    [ExportMetadata("Name", "RealPosition")]
    [Export(typeof(IEffect))]
    class RealPositionEffect : IEffect {

        [Import]
        private IConfig Config;

        public string Name => "RealPosition";

        public bool Enable { set; get; }

        public int Index { set; get; }

        public void Handle(Sprite sprite, ref Bitmap bmp) {
            if(Config["RealPosition"].Boolean) {
                bmp = bmp.Canvas(new Rectangle(sprite.Location, Config["CanvasImageSize"].Size));
            }
        }
    }
}
