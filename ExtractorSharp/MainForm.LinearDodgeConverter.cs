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
        private class LinearDodgeSpriteConverter : ISpriteConverter {
            private IConfig Config;
            internal LinearDodgeSpriteConverter(IConfig Config) {
                this.Config = Config;
                Enable = Config["LinearDodgeSpriteConverter"].Boolean;
            }

            public string Name => "LinearDodge";

            public bool Enable { set; get; }


            public void Convert(Sprite sprite, ref Bitmap bmp) {
                if (Config["LinearDodge"].Boolean) {
                    bmp = bmp.LinearDodge();
                }
            }
        }
    }
}
