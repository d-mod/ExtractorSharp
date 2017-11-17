using System;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.UI;
using System.Drawing;
using ExtractorSharp.View.Pane;
using ExtractorSharp.Core;

namespace ExtractorSharp.View {
    /// <summary>
    /// 历史记录/动作界面
    /// </summary>
    public partial class DropPanel : TabControl {
        Controller Controller { get; }
        public DropPanel() {
            InitializeComponent();
            Controller = Program.Controller;
            TabPages.Add(historyPanel);
            TabPages.Add(actionPanel);
            TabPages.Add(new ColorChartPanel());
        }

        public override void Refresh() {
            SelectedTab.Refresh();
            base.Refresh();
        }





    }
}
