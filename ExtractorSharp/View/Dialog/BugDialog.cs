using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Config;
using ExtractorSharp.Core;
using ExtractorSharp.Json;

namespace ExtractorSharp.View {
    public partial class BugDialog : ESDialog {
        private string Error { set; get; }
        private string Mode { set; get; }
        public BugDialog(IConnector Data) : base(Data) {
            InitializeComponent();
            CancelButton = cancelButton;
            yesButton.Click += Submit;
        }

        public override DialogResult Show(params object[] args) {
            Mode = args[0] as string;
            box.Text = string.Empty;
            if ("debug".Equals(Mode)) {
                Error = args[1] as string;
                label1.Text = Language["ProgramExceptions"];
                submitCheck.Checked = true;
                submitCheck.Visible = true;
            } else {
                label1.Text = Language["SubmitFeedback"];
                submitCheck.Checked = false;
                submitCheck.Visible = false;
            }
            return ShowDialog();
        }

        private void Submit(object sender, EventArgs e) {
            UploadBug(box.Text, textBox2.Text, Error, Mode);
            DialogResult = DialogResult.OK;
        }

        private bool UploadBug(string remark, string contact, string log, string type) {
            try {
                var data = new Dictionary<string, object>() {
                    { "remark",remark },
                    { "contact",contact},
                    { "log", log },
                    { "type",type},
                    { "version",Config["Version"]}
                };
                var builder = new LSBuilder();
                var resultObj = builder.Post(Config["DebugUrl"].Value, data);
                var result = (bool)resultObj.GetValue(typeof(bool));
                return result;
            } catch (Exception e) {
                Console.Write(e.StackTrace);
            }
            return false;
        }

    }
}
