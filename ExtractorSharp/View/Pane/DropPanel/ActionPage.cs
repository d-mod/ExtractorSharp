using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.EventArguments;

namespace ExtractorSharp.View.Pane {

    [Export(typeof(DropPage))]
    internal partial class ActionPage : DropPage, IPartImportsSatisfiedNotification {

        [Import]
        private Controller Controller;

        [Import]
        private Language Language;

        [Import]
        private Store Store;

        [Import]
        private Messager Messager;

        public ActionPage() {
        }

        public void OnImportsSatisfied() {
            InitializeComponent();
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
            Messager.Success("ActionRecord");
            Controller.Record();
        }

        private void Pause(object sender, EventArgs e) {
            Messager.Success("ActionPause");
            Controller.Pause();
        }

        private void Run(object sender, EventArgs e) {
            var result = MessageBox.Show(Language["ActionTips"], "", MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            if(result != DialogResult.Cancel) {
                Controller.Run(result == DialogResult.Yes, Store.Get<Album[]>("/filelist/checked-items"));
            }
        }

        private void Refresh(object sender, ActionEventArgs e) {
            Refresh();
        }

        public override void Refresh() {
            actionList.Items.Clear();
            foreach(var action in Controller.Macro) {
                actionList.Items.Add(new ActionItem(action, Language[action.ToString()]));
            }
            if(Controller.Index < actionList.Items.Count) {
                actionList.SelectedIndex = Controller.Index;
            }
        }


        private class ActionItem {
            private readonly string Title;

            public ActionItem(IMacro action, string Title) {
                Action = action;
                this.Title = Title;
            }

            public IMacro Action { get; }

            public override string ToString() {
                return Title;
            }
        }
    }
}