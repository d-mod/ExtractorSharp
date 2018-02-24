using ExtractorSharp.Composition;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.Plugin.Searcher {
    [ExportMetadata("Guid", "87844cf0-a062-4f34-8c3b-d8e6bda28daa")]
    [Export(typeof(IMenuItem))]
    class MainItem : IMenuItem {
        public string Name => "FileDownload";

        public string Command => "fileDownload";

        public MenuItemType Parent => MenuItemType.MODEL;

        public List<IMenuItem> Childrens { get; }

        public ClickType Click => ClickType.View;

        public Image Image => null;

        public Keys ShortcutKeys => Keys.None;
    }
}
