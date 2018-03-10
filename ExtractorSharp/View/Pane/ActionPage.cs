using System;
using System.Windows.Forms;
using ExtractorSharp.EventArguments;
using ExtractorSharp.Data;
using ExtractorSharp.Core;
using ExtractorSharp.Command;
using ExtractorSharp.Composition;

namespace ExtractorSharp.View.Pane {
    partial class ActionPage : TabPage {
        private Controller Controller;
        private Language Language;
        private IConnector Connector;
        public ActionPage() {
            Language = Language.Default;
            InitializeComponent();
            Controller = Program.Controller;
            Connector=Program.Connector;
            Controller.ActionChanged += Refresh;
            recordButton.Click += Record;
            pauseButton.Click += Pause;
            runButton.Click += Run;
            deleteButton.Click += (o, e) => Delete();
            actionList.Deleted += Delete;
            actionList.Cleared += ()=>Controller.ClearMacro();
        }

        private void Delete() {
            var items = actionList.SelectItems;
            var array = Array.ConvertAll(items, item => item.Action);
            Controller.Delete(array);
        }

        private void Record(object sender, EventArgs e) {
            Connector.SendMessage(MessageType.Success, Language["ActionRecord"]);
            Controller.Record();
        }

        private void Pause(object sender, EventArgs e) {
            Connector.SendMessage(MessageType.Success, Language["ActionPause"]);
            Controller.Pause();          
        }

        private void Run(object sender, EventArgs e) {
           var result= MessageBox.Show(Language["ActionTips"], "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result != DialogResult.Cancel) {
                Controller.Run(result==DialogResult.Yes, Program.Connector.CheckedFiles);
            }
        }

        private void Refresh(object sender, ActionEventArgs e) => Refresh();

        public override void Refresh() {
            actionList.Items.Clear();
            foreach (var item in Controller.Macro) {
                actionList.Items.Add(new ActionItem(item));
            }
            if (Controller.Index < actionList.Items.Count) {
                actionList.SelectedIndex = Controller.Index;
            }
            base.Refresh();
        }

        private class ActionItem {
            public IAction Action { get; }
            private string Label;

            public ActionItem(IAction action) {
                Action = action;
                Label = Language.Default[action.Name];
            }

            public override string ToString() => Label;
        }

    }

}
