using ExtractorSharp.Composition;
using ExtractorSharp.Core;
using System.ComponentModel.Composition;

namespace ExtractorSharp.Plugin.DressingRoom {
    [Export(typeof(IPlugin))]
    class MainPlugin : IPlugin {
        public void Initialize() {
        }
    }
}
