using ExtractorSharp.Composition;
using ExtractorSharp.Config;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp {
    partial class MainForm {
        private class RealPositionEffect : IEffect {
            private IConfig Config;

            public string Name => "RealPosition";

            public bool Enable { set; get; }
            public int Index { set; get; }

            internal RealPositionEffect(IConfig Config) {
                this.Config = Config;
                Enable = Config["LinearDodgeSpriteConverter"].Boolean;
            }

            public void Handle(Sprite sprite, ref Bitmap bmp) {
                if (Config["RealPosition"].Boolean) {
                    bmp = bmp.Canvas(new Rectangle(sprite.Location, Config["CanvasImageSize"].Size));
                }
            }
        }
    }
}
