using ExtractorSharp.Composition;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.Plugin.Searcher {
    [ExportMetadata("Guid", "D72DF478-FAFF-43DF-B904-9EB338A08B54")]
    [Export(typeof(IMenuItem))]
    class MainItem : IMenuItem {
        public string Name => "SearchModel";

        public string Command => "searchModel";

        public MenuItemType Parent => MenuItemType.MODEL;

        public List<IMenuItem> Childrens { get; }

        public ClickType Click => ClickType.View;

        public Image Image => null;

        public Keys ShortcutKeys => Keys.None;
    }
}
