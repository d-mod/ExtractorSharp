using System.Windows.Forms;
using ExtractorSharp.Component.EventArguments;
using ExtractorSharp.Core.Composition;

namespace ExtractorSharp.Component {
    public partial class ESDialog : ESForm {
        public ESDialog(IConnector connector) : base(connector) {
            InitializeComponent();
        }

        /// <summary>
        ///     向视图层传递数据的事件
        /// </summary>
        protected event DialogDataEvent DataSent;

        protected void OnDataSent(DialogDataEventArgs e) {
            DataSent?.Invoke(this, e);
        }

        /// <summary>
        ///     向视图层传递数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddAttribute(string key, object value) {
            var e = new DialogDataEventArgs {
                Key = key,
                Value = value
            };
            OnDataSent(e);
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (msg.Msg == 256 || msg.Msg == 260) {
                if (keyData == Keys.Escape) {
                    OnEscape();
                    return true;
                }
            }
            return false;
        }

        protected virtual void OnEscape() {
            DialogResult = DialogResult.Cancel;
        }


        public virtual DialogResult Show(params object[] args) {
            return ShowDialog();
        }

        protected delegate void DialogDataEvent(object sender, DialogDataEventArgs e);
    }
}