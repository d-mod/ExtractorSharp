using System;
using System.Windows.Forms;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.EventArguments;

namespace ExtractorSharp.View.Pane {
    internal partial class HistoryPage : TabPage {
        private readonly Controller Controller;

        public HistoryPage() {
            InitializeComponent();
            Controller = Program.Controller;
            historyList.MouseDoubleClick += Goto;
            Controller.CommandDid += Refresh;
            Controller.CommandUndid += RefreshList;
            Controller.CommandRedid += RefreshList;
            Controller.CommandCleared += Refresh;
            gotoItem.Click += Move;
            addItem.Click += Add;
            historyList.Items.Add("...");
            historyList.Items.AddRange(Controller.History);
            historyList.SelectedIndex = 0;
        }

        private Language Language => Language.Default;

        private void Add(object sender, EventArgs e) {
            var index = historyList.SelectedIndex - 1;
            if (index > -1) {
                var command = Controller.History[index];
                Controller.AddMacro(command);
            }
        }

        protected override void OnTabIndexChanged(EventArgs e) {
            Refresh();
            base.OnTabIndexChanged(e);
        }


        private void RefreshList(object sender, EventArgs e) {
            if (Parent.Visible) historyList.SelectedIndex = Controller.Index;
        }

        public void Refresh(object sender, CommandEventArgs e) {
            if (e.Type == CommandEventType.Clear || e.Command.CanUndo) Refresh();
        }

        public override void Refresh() {
            if (Parent.Visible) {
                historyList.Items.Clear();
                historyList.Items.Add("...");
                var history = Controller.History;
                for (var i = 0; i < history.Length; i++) {
                    historyList.Items.Add($"{(i == Controller.Index - 1 ? "-> " : "")}{Language[history[i].Name]}");
                }
                if (Controller.Index < historyList.Items.Count) historyList.SelectedIndex = Controller.Index;
                base.Refresh();
            }
        }

        private void Move(object sender, EventArgs e) {
            if (historyList.SelectedIndex > -1) {
                Controller.Move(historyList.SelectedIndex - Controller.Index);
                Refresh();
            }
        }

        private void Goto(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && e.Clicks == 2) Move(sender, e);
        }
    }
}