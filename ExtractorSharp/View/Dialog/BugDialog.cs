using System;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.UI;

namespace ExtractorSharp.View {
    public partial class BugDialog : EaseDialog {
        string error = "";
        int mode;
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
                error = error.Replace("&", "");
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
            var error = "无";
            if (submitCheck.Checked) {
                error = this.error;
            }
            Program.UploadBug(box.Text, textBox2.Text, error);
            Visible = false;
        }
        
    }
}
