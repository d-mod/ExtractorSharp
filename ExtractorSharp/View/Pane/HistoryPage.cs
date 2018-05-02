using ExtractorSharp.Core;
using ExtractorSharp.Data;
using System;
using System.Windows.Forms;

namespace ExtractorSharp.View.Pane {
    partial class HistoryPage : TabPage {
        private Controller Controller;
        
        public HistoryPage() {
            InitializeComponent();
            Controller = Program.Controller;
            historyList.MouseDoubleClick += Goto;
            Controller.CommandDid += Refresh;
            Controller.CommandUndid += RefreshList;
            Controller.CommandRedid += RefreshList;
            Controller.CommandCleared += Refresh;
            historyList.Items.Add("...");
            historyList.Items.AddRange(Controller.History);
            historyList.SelectedIndex = 0;
        }

        protected override void OnTabIndexChanged(EventArgs e) {
            Refresh();
            base.OnTabIndexChanged(e);
        }




        private void RefreshList(object sender, EventArgs e) {
            if (Parent.Visible) {
                historyList.SelectedIndex = Controller.Index;
            }
        }

        public void Refresh(object sender, CommandEventArgs e) {
            if (e.Command.CanUndo) {
                Refresh();
            }
        }

        public override void Refresh() {
            if (Parent.Visible) {
                historyList.Items.Clear();
                historyList.Items.Add("...");
                foreach (var item in Controller.History) {
                    historyList.Items.Add(Language.Default[item.Name]);
                }
                if (Controller.Index < historyList.Items.Count) {
                    historyList.SelectedIndex = Controller.Index;
                }
                base.Refresh();
            }
        }

        private void Goto(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && e.Clicks == 2 && historyList.SelectedIndices.Count > 0)
                Controller.Move(historyList.SelectedIndices[0] - Controller.Index);
        }

    }
}
