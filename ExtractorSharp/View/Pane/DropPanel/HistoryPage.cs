using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.Pane {

    [Export(typeof(DropPage))]
    internal partial class HistoryPage : DropPage, IPartImportsSatisfiedNotification {

        [Import]
        private Controller Controller;

        [Import]
        private Language Language;

        [Import]
        private Messager Messager;


        public HistoryPage() {

        }

        public void OnImportsSatisfied() {

            InitializeComponent();
            historyList.MouseDoubleClick += Goto;
            Controller.CommandChanged += Refresh;

            gotoItem.Click += Move;
            addItem.Click += Add;
            clearItem.Click += Clear;
            historyList.Items.Add("...");
            historyList.Items.AddRange(Controller.History.Select(e => e.Key).ToArray());
            historyList.SelectedIndex = 0;
        }

        private void Clear(object sender, EventArgs e) {
            Controller.ClearCommand();
        }


        private void Add(object sender, EventArgs e) {
            var index = historyList.SelectedIndex - 1;
            if(index > -1) {
                var command = Controller.History.ElementAt(index).Key;
                if(command is IMacro action) {
                    Controller.AddMacro(action);
                    return;
                }
                Messager.Error("CantAddAction");
            }
        }


        private void RefreshList(object sender, EventArgs e) {
            if(Parent.Visible) {
                historyList.SelectedIndex = Controller.Index;
            }
        }

        public void Refresh(object sender, CommandEventArgs e) {
            if(e.Type == CommandEventType.Clear || e.Command is IRollback) {
                Refresh();
            }
        }

        public override void Refresh() {
            if(Parent.Visible) {
                historyList.Items.Clear();
                historyList.Items.Add("...");
                var history = Controller.History;
                for(var i = 0; i < history.Count(); i++) {
                    var pair = history.ElementAt(i);
                    historyList.Items.Add($"{(i == Controller.Index - 1 ? "-> " : "")}{Language[pair.Value.Name]}");
                }
                if(Controller.Index < historyList.Items.Count) {
                    historyList.SelectedIndex = Controller.Index;
                }
                base.Refresh();
            }
        }

        private void Move(object sender, EventArgs e) {
            if(historyList.SelectedIndex > -1) {
                Controller.Move(historyList.SelectedIndex - Controller.Index);
                Refresh();
            }
        }

        private void Goto(object sender, MouseEventArgs e) {
            if(e.Button == MouseButtons.Left && e.Clicks == 2) {
                var point = e.Location;
                var index = this.historyList.IndexFromPoint(e.Location);
                if(index > -1) {
                    Controller.Move(index - Controller.Index);
                    Refresh();
                }
            }
        }

    }
}