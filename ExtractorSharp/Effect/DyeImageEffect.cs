using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Effect {

    [ExportMetadata("Name", "Dye")]
    [Export(typeof(IEffect))]
    public class DyeImageEffect : IEffect {

        [Import]
        private IConfig Config { get; }


        public string Name => "Dye";

        public bool Enable { set; get; }

        public int Index { set; get; }

        public void Handle(Sprite sprite, ref Bitmap bmp) {
            if(Config["Dye"].Boolean) {
                //  bmp = bmp.Dye(Drawer.Color);
            }
        }
    }
}
