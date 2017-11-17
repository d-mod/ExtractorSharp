using System;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.UI;
using ExtractorSharp.Users;
using ExtractorSharp.Core;

namespace ExtractorSharp.View{
    public partial class EncryptDialog : EaseDialog {
        private Controller Controller { get; }
        public EncryptDialog(){
            Controller = Program.Controller;
            InitializeComponent();
            submitButton.Click += Encrypt;
        }

        public void Encrypt(object sender, EventArgs e) {
            var array = Controller.CheckedAlbum;
            if (nameBox.Text.Trim().Equals(string.Empty)) {
                Messager.ShowMessage(Msg_Type.Warning, "名称不能为空");
                return;
            }
            if (authorBox.Text.Trim().Equals(string.Empty)) {
                Messager.ShowMessage(Msg_Type.Warning, "作者不能为空");
                return;
            }
            if (pwdBox.Text.ToString().Equals(string.Empty)) {
                Messager.ShowMessage(Msg_Type.Warning, "密码不能为空");
                return;
            }
            if (!checkBox.Checked) {
                Messager.ShowMessage(Msg_Type.Warning, "只有原创或者取得原作者许可才能使用本功能");
                return;
            }
            var Name = nameBox.Text.Trim();
            var AuthorName = authorBox.Text.Trim();
            var Key = pwdBox.Text.Trim();
            var Remark = remarkBox.Text.Trim();
            var Update = updatePicker.Value;
            var Expire = expirePicker.Value;
            var CanRead = canReadCheck.Checked;
            var CanExtract = canExtractCheck.Checked;
            Work.CreateNewWork(Name, AuthorName, Key, Remark, Update, Expire, CanExtract, CanRead, array);
            Controller.ImageFlush();
            Visible = false;
        }

        public override DialogResult Show(params object[] args) {
            var Work = args[0] as Work;
            pwdBox.Text = string.Empty;
            if (Work != null) {
                authorBox.Text = Work.Author.ToString();
                nameBox.Text = Work.Name;
                remarkBox.Text = Work.Remark;
                updatePicker.Value = Work.Update;
                expirePicker.Value = Work.Expire;
                canReadCheck.Checked = Work.CanRead;
                canExtractCheck.Checked = Work.CanExtract;
            }
            return ShowDialog();
        }
    }
}
