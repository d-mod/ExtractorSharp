using System;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.UI;

namespace ExtractorSharp.View {
    public partial class BugDialog : EaseDialog {
        private string error = "";
        private int mode;
        public BugDialog(){
            InitializeComponent();
            CancelButton = cancelButton;
            yesButton.Click += Submit;
        }

        public override DialogResult Show(params object[] args) {
            mode = (int)args[0];
            box.Text = string.Empty;
            if (mode == 0) {
                error = (string)args[1];
                label1.Text = "程序出现了一些问题";
                submitCheck.Checked = true;
                submitCheck.Visible = true;
            } else {
                label1.Text = "向作者提交你的建议";
                submitCheck.Checked = false;
                submitCheck.Visible = false;
            }
            return ShowDialog();
        }

        public void Submit(object sender,EventArgs e) {
            Program.UploadBug(box.Text, textBox2.Text, error);
            DialogResult = DialogResult.OK;
        }
        
    }
}
