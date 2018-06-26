using System;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.View.Dialog {
    public partial class NewImgDialog : ESDialog {
        public NewImgDialog(IConnector Connector) : base(Connector) {
            InitializeComponent();
            pathBox.KeyDown += EnterDownRun;
            indexBox.KeyDown += EnterDownRun;
            countBox.KeyDown += EnterDownRun;
            yesButton.Click += NewImg;
        }

        /// <summary>
        ///     Enter快捷键执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterDownRun(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) NewImg(sender, e);
        }

        public override DialogResult Show(params object[] args) {
            var count = (int) args[0];
            indexBox.Maximum = count;
            indexBox.Value = count;
            return ShowDialog();
        }

        /// <summary>
        ///     新建一个img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewImg(object sender, EventArgs e) {
            var path = pathBox.Text.Trim();
            if (path.GetSuffix().Equals(string.Empty)) {
                Connector.SendError("FileNameCannotEmpty");
                return;
            }

            var count = (int) countBox.Value;
            var index = (int) indexBox.Value;
            Connector.Do("newImg", null, path, count, index);
            pathBox.Text = pathBox.Text.Replace(path.GetSuffix(), "");
            DialogResult = DialogResult.OK;
        }
    }
}