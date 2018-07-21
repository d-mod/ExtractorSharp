using System.Drawing;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp {
    partial class MainForm {
        private class LinearDodgeEffect : IEffect {
            private readonly IConfig Config;

            internal LinearDodgeEffect(IConfig Config) {
                this.Config = Config;
                Enable = Config["LinearDodgeSpriteConverter"].Boolean;
            }

            public string Name => "LinearDodge";

            public bool Enable { set; get; }

            public int Index { set; get; }

            public void Handle(Sprite sprite, ref Bitmap bmp) {
                if (Config["LinearDodge"].Boolean) bmp = bmp.LinearDodge();
            }
        }
    }
}