using System.Windows.Forms;
using ExtractorSharp.Core.Composition;

namespace ExtractorSharp.View.Pane {
    /// <summary>
    ///     历史记录/动作界面
    /// </summary>
    public partial class DropPanel : TabControl {
        public DropPanel(IConnector Connector) {
            InitializeComponent();
            TabPages.Add(new HistoryPage());
            TabPages.Add(new ActionPage(Connector));
            TabPages.Add(new PalettePage());
            //TabPages.Add(new TexturePage());
        }

        public override void Refresh() {
            SelectedTab.Refresh();
            base.Refresh();
        }
    }
}