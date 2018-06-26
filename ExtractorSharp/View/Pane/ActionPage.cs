using System;
using System.Windows.Forms;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Model;
using ExtractorSharp.EventArguments;

namespace ExtractorSharp.View.Pane {
    internal partial class ActionPage : TabPage {
        private readonly IConnector Connector;
        private readonly Controller Controller;
        private readonly Language Language;

        public ActionPage(IConnector Connector) {
            this.Connector = Connector;
            Language = Connector.Language;
            InitializeComponent();
            Controller = Program.Controller;
            Controller.ActionChanged += Refresh;
            recordButton.Click += Record;
            pauseButton.Click += Pause;
            runButton.Click += Run;
            deleteButton.Click += Delete;
            actionList.ItemDeleted += Delete;
            actionList.ItemCleared += (o, e) => Controller.ClearMacro();
        }

        private void Delete(object sender, EventArgs e) {
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
            var result = MessageBox.Show(Language["ActionTips"], "", MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            if (result != DialogResult.Cancel) {
                Controller.Run(result == DialogResult.Yes, Program.Connector.CheckedFiles);
            }
        }

        private void Refresh(object sender, ActionEventArgs e) {
            Refresh();
        }

        public override void Refresh() {
            actionList.Items.Clear();
            foreach (var item in Controller.Macro) actionList.Items.Add(new ActionItem(item));
            if (Controller.Index < actionList.Items.Count) actionList.SelectedIndex = Controller.Index;
            base.Refresh();
        }

        private class ActionItem {
            private readonly string Label;

            public ActionItem(IAction action) {
                Action = action;
                Label = Language.Default[action.Name];
            }

            public IAction Action { get; }

            public override string ToString() {
                return Label;
            }
        }
    }
}