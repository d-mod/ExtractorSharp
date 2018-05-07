using System.Windows.Forms;
using System;
using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.Component {
    public partial class ESTextDialog : ESDialog {
        public bool CanEmpty { set; get; }

        public new Language Language  => Language.Default;

        public string InputText {
            get => textBox.Text;
            set {
                textBox.Text = value;
                var name = value.GetSuffix();
                var i = value.LastIndexOf(name);
                if (i > -1) {
                    textBox.Select(i, name.Length);
                }
            }
        }

        public ESTextDialog():this(null){

        }

        public ESTextDialog(IConnector Connector) : base(Connector) {
            InitializeComponent();
            CancelButton = cancelButton;
            textBox.KeyPress += EnterKeyPress;
            yesButton.Click += Submit;
        }

        protected override void OnEscape() {
            base.OnEscape();
            Dispose();
        }


        public void EnterKeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 13)
                Submit(sender, e);
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Submit(object sender, EventArgs e) {
            var i = textBox.Text.LastIndexOf("/") + 1;
            var text = textBox.Text.Substring(i);
            if (Connector != null && !CanEmpty && text.Trim().Equals(string.Empty)) {
                Connector.SendWarning("InputCannotEmpty");
                return;
            }
            DialogResult = DialogResult.OK;
        }


    }
}
