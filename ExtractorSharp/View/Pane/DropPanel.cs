using System.Windows.Forms;
using ExtractorSharp.Core;
using ExtractorSharp.View.Pane;

namespace ExtractorSharp.View {
    /// <summary>
    /// 历史记录/动作界面
    /// </summary>
    public partial class DropPanel : TabControl {
        private IConnector Connector { get; }
        public DropPanel(IConnector Connector) {
            this.Connector = Connector;
            InitializeComponent();
            TabPages.Add(historyPanel);
            TabPages.Add(actionPanel);
            TabPages.Add(new PalattePanel());
        }

        public override void Refresh() {
            SelectedTab.Refresh();
            base.Refresh();
        }





    }
}
