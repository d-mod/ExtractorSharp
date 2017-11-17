using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.View {
    partial class DecryptPanel {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            pwdBox = new TextBox();
            msgBox = new TextBox();
            decryptButton = new Button();
            readbutton = new Button();
            SuspendLayout();
            msgBox.Location = new Point(0, 0);
            msgBox.Size = new Size(240,200);
            msgBox.Multiline = true;
            msgBox.ReadOnly = true;
            // 
            // pwdBox
            // 
            pwdBox.Location = new Point(40, 220);
            pwdBox.Size = new Size(160, 20);
            pwdBox.PasswordChar = '*';
            pwdBox.TabIndex = 0;
            pwdBox.ShortcutsEnabled = false;
            //
            //
            //
            decryptButton.Location = new Point(60, 250);
            decryptButton.Size = new Size(120, 40);
            decryptButton.Text = "解密";
            decryptButton.UseVisualStyleBackColor = true;
            //
            //
            //
            readbutton.Location = new Point(60,320);
            readbutton.Size = new Size(120, 40);
            readbutton.Text = "查看";
            readbutton.BackColor = Color.GreenYellow;
            readbutton.UseVisualStyleBackColor = true;

            Location = new Point(1050, 80);
            Size = new Size(240, 600);
            ResumeLayout(false);
            Visible = false;

            Controls.Add(msgBox);
            Controls.Add(pwdBox);
            Controls.Add(decryptButton);
            Controls.Add(readbutton);
        }

        #endregion

        private TextBox msgBox;
        private TextBox pwdBox;
        private Button decryptButton;
        public Button readbutton;
    }
}
