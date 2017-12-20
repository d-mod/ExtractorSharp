using System.Windows.Forms;
using System;
using ExtractorSharp.Core;
using ExtractorSharp.Data;

namespace ExtractorSharp.Component {
    public partial class EaseTextDialog : EaseDialog {
        public bool CanEmpty { set; get; }

        public new Language Language  => Language.Default;

        public string InputText {
            get => textBox.Text;
            set {
                textBox.Text = value;
                var name = value.GetName();
                var i = value.LastIndexOf(name);
                if (i > -1) {
                    textBox.Select(i, name.Length);
                }
            }
        }

        public EaseTextDialog():this(null){

        }

        public EaseTextDialog(IConnector Connector) : base(Connector) {
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
            if (!CanEmpty && text.Trim().Equals(string.Empty)) {
                Messager.ShowWarnning("InputCannotEmpty");
                DialogResult = DialogResult.Retry;
                return;
            }
            DialogResult = DialogResult.OK;
        }


    }
}
