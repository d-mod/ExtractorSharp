using System.Drawing;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp {
    partial class MainForm {
        private class DyeEffect : IEffect {
            private readonly IConfig Config;

            internal DyeEffect(IConfig Config) {
                this.Config = Config;
                Enable = Config["DyeSpriteConverter"].Boolean;
            }

            public string Name => "Dye";

            public bool Enable { set; get; }

            public int Index { set; get; }

            public void Handle(Sprite sprite, ref Bitmap bmp) {
                if (Config["Dye"].Boolean) bmp = bmp.Dye(Program.Drawer.Color);
            }
        }
    }
}