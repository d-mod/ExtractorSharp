using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Effect {

    [ExportMetadata("Name", "LinearDodge")]
    [Export(typeof(IEffect))]
    public class LinearDodgeEffect : IEffect {

        [Import]
        private IConfig Config;

        public string Name => "LinearDodge";

        public bool Enable { set; get; }

        public int Index { set; get; }

        public void Handle(Sprite sprite, ref Bitmap bmp) {
            if(Config["LinearDodge"].Boolean) {
                bmp = bmp.LinearDodge();
            }
        }
    }
}
