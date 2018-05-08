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
        private class DyeEffect : IEffect {
            private IConfig Config;
            internal DyeEffect(IConfig Config) {
                this.Config = Config;
                Enable = Config["DyeSpriteConverter"].Boolean;
            }

            public string Name => "Dye";

            public bool Enable { set; get; }

            public int Index { set; get; }

            public void Handle(Sprite sprite, ref Bitmap bmp) {
                if (Config["Dye"].Boolean) {
                    bmp = bmp.Dye(Program.Drawer.Color);
                }
            }
        }
    }
}
