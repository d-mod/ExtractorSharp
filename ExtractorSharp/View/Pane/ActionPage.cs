using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.EventArguments;
using ExtractorSharp.Command;
using ExtractorSharp.Data;
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
            var range = actionList.GetCheckItems();
            Controller.Delete(range);
        }

        private void Record(object sender, EventArgs e) {
            if (MessageBox.Show(Language["Record"]) == DialogResult.OK)
                Controller.Record();
        }

        private void Pause(object sender, EventArgs e) {
            if (MessageBox.Show(Language["Pause"]) == DialogResult.OK)
                Controller.Pause();
        }

        private void Run(object sender, EventArgs e) {
            var allImage = MessageBox.Show(Language["ActionTips"], "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
            Controller.Run(allImage, Controller.CheckedAlbum);
        }

        private void Refresh(object sender, ActionEventArgs e) => Refresh();

        public override void Refresh() {
            actionList.Items.Clear();
            foreach (var item in Controller.Macro)
                actionList.Items.Add(item);
            if (Controller.Index < actionList.Items.Count)
                actionList.SelectedIndex = Controller.Index;
            base.Refresh();
        }

    }

}
