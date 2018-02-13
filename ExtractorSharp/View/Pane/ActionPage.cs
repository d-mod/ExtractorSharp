using System;
using System.Windows.Forms;
using ExtractorSharp.EventArguments;
using ExtractorSharp.Data;
using ExtractorSharp.Core.Control;
using ExtractorSharp.Core;

namespace ExtractorSharp.View.Pane {
    partial class ActionPage : TabPage {
        private Controller Controller;
        private Language Language;
        public ActionPage() {
            Language = Language.Default;
            InitializeComponent();
            Controller = Program.Controller;
            Controller.ActionChanged += Refresh;
            recordButton.Click += Record;
            pauseButton.Click += Pause;
            runButton.Click += Run;
            deleteButton.Click += (o, e) => Delete();
            actionList.Deleted += Delete;
            actionList.Cleared += ()=>Controller.ClearMacro();
        }

        private void Delete() {
            Controller.Delete(actionList.SelectItems);
        }

        private void Record(object sender, EventArgs e) {
            Messager.ShowMessage(Msg_Type.Operate, Language["ActionRecord"]);
            Controller.Record();
        }

        private void Pause(object sender, EventArgs e) {
            Messager.ShowMessage(Msg_Type.Operate, Language["ActionPause"]);
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
                actionList.Items.Add(Language[item.Name]);
            }
            if (Controller.Index < actionList.Items.Count) {
                actionList.SelectedIndex = Controller.Index;
            }
            base.Refresh();
        }

    }

}
