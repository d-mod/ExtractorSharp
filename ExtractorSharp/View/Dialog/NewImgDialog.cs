using System;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.View.Dialog {
    public partial class NewImgDialog : ESDialog {
        public NewImgDialog(IConnector Connector) : base(Connector) {
            InitializeComponent();
            pathBox.KeyDown += EnterDownRun;
            indexBox.KeyDown += EnterDownRun;
            countBox.KeyDown += EnterDownRun;
            yesButton.Click += NewImg;
            var list = Handler.Versions;
            var array = new object[list.Count];
            for (var i = 0; i < list.Count; i++) {
                array[i] = list[i];
            }
            versionBox.Items.AddRange(array);
            versionBox.SelectedItem = ImgVersion.Ver2;
        }

        /// <summary>
        ///     Enter快捷键执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterDownRun(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                NewImg(sender, e);
            }
        }

        public override DialogResult Show(params object[] args) {
            var count = (int) args[0];
            var index = (int)args[1];
            indexBox.Items.Clear();
            indexBox.Items.AddRange(Connector.FileArray);
            index = Math.Max(-1, index);
            index = Math.Min(index, indexBox.Items.Count);
            indexBox.SelectedIndex = index;
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
            var count = (int)countBox.Value;
            var index = indexBox.SelectedIndex;
            var version = versionBox.SelectedItem;
            Connector.Do("newImg", null, path, count, index,version);
            pathBox.Text = pathBox.Text.Replace(path.GetSuffix(), "");
            DialogResult = DialogResult.OK;
        }
    }
}