using System;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Config;
using ExtractorSharp.Core;

namespace ExtractorSharp.View {
    public partial class BugDialog : EaseDialog {
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

        public void Submit(object sender, EventArgs e) {
            Program.UploadBug(box.Text, textBox2.Text, Error, Mode);
            DialogResult = DialogResult.OK;
        }
        
    }
}
