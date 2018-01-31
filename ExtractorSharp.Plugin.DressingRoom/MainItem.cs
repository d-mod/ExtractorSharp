using ExtractorSharp.Composition;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.Plugin.DressingRoom {
    [Export(typeof(IMenuItem))]
    [ExportMetadata("Guid", "57951442-F28C-4D84-8677-0DE4105BFBD1")]
    class MainItem : IMenuItem {
        public string Name => "DressingRoom";

        public string Command => "dressingRoom";

        public List<IMenuItem> Childrens { get; }

        public MenuItemType Parent => MenuItemType.MODEL;

        public ClickType Click => ClickType.View;

        public Image Image { get; }

        public Keys ShortcutKeys => Keys.None;
    }
}
