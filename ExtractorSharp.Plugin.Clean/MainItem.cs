using ExtractorSharp.Composition;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.Plugin.DressingRoom {
    [Export(typeof(IMenuItem))]
    [ExportMetadata("Guid", "A4BB046F-ACAB-44B5-A0F0-2DD84278B43D")]
    class MainItem : IMenuItem {
        public string Name => "CleanMod";

        public string Command => "cleanMod";

        public List<IMenuItem> Childrens { get; }

        public MenuItemType Parent => MenuItemType.MODEL;

        public ClickType Click => ClickType.View;

        public Image Image { get; }

        public Keys ShortcutKeys => Keys.None;
    }
}
