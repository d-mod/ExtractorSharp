using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Effect {
    public class RealPositionEffect : IEffect {
        private readonly IConfig Config;

        internal RealPositionEffect(IConfig Config) {
            this.Config = Config;
        }

        public string Name => "RealPosition";

        public bool Enable { set; get; }
        public int Index { set; get; }

        public void Handle(Sprite sprite, ref Bitmap bmp) {
            if (Config["RealPosition"].Boolean) {
                bmp = bmp.Canvas(new Rectangle(sprite.Location, Config["CanvasImageSize"].Size));
            }
        }
    }
}
