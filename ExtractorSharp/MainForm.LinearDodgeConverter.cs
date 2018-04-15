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
        private class LinearDodgeEffect : IEffect {
            private IConfig Config;
            internal LinearDodgeEffect(IConfig Config) {
                this.Config = Config;
                Enable = Config["LinearDodgeSpriteConverter"].Boolean;
            }

            public string Name => "LinearDodge";

            public bool Enable { set; get; }

            public int Index { set; get; }

            public void Handle(Sprite sprite, ref Bitmap bmp) {
                if (Config["LinearDodge"].Boolean) {
                    bmp = bmp.LinearDodge();
                }
            }
        }
    }
}
