using ExtractorSharp.Composition;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ExtractorSharp.Plugin.DressingRoom {
    [Export(typeof(IMenuItem))]
    class PluginMenuItem : IMenuItem {
        public string Name => "DressingRoom";

        public PluginItemType Parent => PluginItemType.MODEL;

        public string Command => "dressingRoom";

        public Dictionary<string, string> Children { get; }
    }
}
