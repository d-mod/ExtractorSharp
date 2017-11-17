using System;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.Users;
using ExtractorSharp.Core;

namespace ExtractorSharp.View {
    public partial class DecryptPanel : Panel {
        Work Work;
        Controller Controller;
        public DecryptPanel() {
            Controller = Program.Controller;
            InitializeComponent();
            decryptButton.Click += Decrypt;
            pwdBox.KeyDown += KeyDecrypt;
        }

        internal void Show(Work Work) {
            this.Work = Work;
            Visible = true;
            msgBox.Text = Work.ToString();
            readbutton.Enabled = !Work.IsValid||Work.CanRead;
        }

        public void Decrypt() {
            if (!Work.IsDecrypt) {
                bool result = Work.Decrypt(pwdBox.Text);
                if (result) {
                    Messager.ShowOperate("Decrypt");
                    Visible = false;
                    Controller.ImageFlush();
                } else
                    Messager.ShowError("PasswordError");             
            }
        }

        public void Decrypt(object sender,EventArgs e) {
            Decrypt();
        }

        public void KeyDecrypt(object sender,KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) 
                Decrypt();        
        }

   
        
    }
}
