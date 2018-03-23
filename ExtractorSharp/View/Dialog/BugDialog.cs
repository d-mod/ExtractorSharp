using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Config;
using ExtractorSharp.Core;
using ExtractorSharp.Data;
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
                Error = null;
                label1.Text = Language["SubmitFeedback"];
                submitCheck.Checked = false;
                submitCheck.Visible = false;
            }
            return ShowDialog();
        }

        private void Submit(object sender, EventArgs e) {
            UploadBug(box.Text, Error);
            DialogResult = DialogResult.OK;
        }

        private Result UploadBug(string message, string log) {
            var result = new Result() {
                Status = "error",
                Message = "提交失败!"
            };
            try {
                var data = new Dictionary<string, object>() {
                    { "projectId" , Config["ProgramId"].Integer },
                    { "message",message },
                    { "log", log },
                    { "version", Config["Version"].Value}
                };
                var builder = new LSBuilder();
                var path = $"{Config["ApiHost"].Value}/{Config["FeedbackUrl"].Value}";
                var resultObj = builder.Post(path, data);
                resultObj.GetValue(ref result);
            } catch (Exception e) {
                Console.Write(e.StackTrace);
            }
            return result;
        }

    }
}
