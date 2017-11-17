using System.Windows.Forms;
using ExtractorSharp.EaseUI;
using System;

namespace ExtractorSharp.View {
    public partial class RenameDialog : EaseDialog {
        IName IName;
        public RenameDialog(Controller Controller) : base(Controller) {
            InitializeComponent();
            CancelButton = cancelButton;
            textBox1.KeyPress += EnterKeyPress;
            // textBox1.MouseDoubleClick += SelectName;
            yesButton.Click += Rename;
        }

        public void EnterKeyPress(object sender,KeyPressEventArgs e) {
            if (e.KeyChar == 13)
                Rename(sender, e);
        }
        
        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Rename(object sender,EventArgs e) {
            var i = textBox1.Text.LastIndexOf("/") + 1;
            var text = textBox1.Text.Substring(i);
            if (text.Trim().Equals(string.Empty)) {
                MainForm.Message.Show(Msg_Type.Warning, "不能输入为空");
                return;
            }        
            IName.Name=textBox1.Text;
            DialogResult = DialogResult.OK;
        }

        public override DialogResult Show(params object[] args) {
            IName = args[0] as IName;
            if (IName != null) {
                textBox1.Text = IName.Name;
                var i = textBox1.Text.LastIndexOf("/")+1;
                if (i < textBox1.Text.Length) {
                    textBox1.SelectionStart = i;
                    textBox1.SelectionLength = textBox1.Text.Length - i;
                }
                return base.Show(args);
            } else
                return DialogResult.Cancel;
        }
    }
}
