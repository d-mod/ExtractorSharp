using System.Windows.Forms;
using ExtractorSharp.Core.Composition;

namespace ExtractorSharp.View.Pane {
    /// <summary>
    ///     历史记录/动作界面
    /// </summary>
    public partial class DropPanel : TabControl {
        public DropPanel(IConnector Connector) {
            this.Connector = Connector;
            InitializeComponent();
            TabPages.Add(historyPanel);
            TabPages.Add(actionPanel);
            TabPages.Add(new PalattePanel());
            TabPages.Add(new TexturePanel());
        }

        private IConnector Connector { get; }

        public override void Refresh() {
            SelectedTab.Refresh();
            base.Refresh();
        }
    }
}