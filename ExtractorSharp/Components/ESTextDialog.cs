using System;
using System.Text;
using System.Windows.Forms;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Components {
    public partial class ESTextDialog : BaseForm {

        public Language Language { set; get; } = Language.Empty;

        public ESTextDialog() {
            InitializeComponent();
            CancelButton = cancelButton;
            textBox.KeyPress += EnterKeyPress;
            yesButton.Click += Submit;
        }


        public bool CanEmpty { set; get; }


        public string InputText {
            get => textBox.Text;
            set {
                textBox.Text = value;
                var name = value.GetSuffix();
                var i = value.LastIndexOf(name);
                if(i > -1) {
                    textBox.Select(i, name.Length);
                }
            }
        }

        protected void OnEscape() {
            // base.OnEscape();
            Dispose();
        }


        public void EnterKeyPress(object sender, KeyPressEventArgs e) {
            if(e.KeyChar == 13) {
                Submit(sender, e);
            }
        }

        /// <summary>
        ///     重命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Submit(object sender, EventArgs e) {
            var i = textBox.Text.LastIndexOf("/") + 1;
            var text = textBox.Text.Substring(i);
            /*            if (Connector != null && !CanEmpty && text.Trim().Equals(string.Empty)) {
                            Messager.Warning("InputCannotEmpty");
                            return;
                        }*/
            DialogResult = DialogResult.OK;
        }

        public new DialogResult ShowDialog() {
            this.yesButton.Text = Language["OK"];
            this.cancelButton.Text = Language["Cancel"];
            return base.ShowDialog();
        }

    }
}