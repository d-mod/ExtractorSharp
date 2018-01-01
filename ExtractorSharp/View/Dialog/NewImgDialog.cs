using System;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.Component;
using ExtractorSharp.View;
using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Core.Control;
using ExtractorSharp.Config;

namespace ExtractorSharp {
    public partial class NewImgDialog : ESDialog {
        private Controller Controller;
        public NewImgDialog(IConnector Data) : base(Data) {
            Controller = Program.Controller;
            InitializeComponent();
            pathBox.KeyDown += EnterDownRun;
            indexBox.KeyDown += EnterDownRun;
            countBox.KeyDown += EnterDownRun;
            yesButton.Click += NewImg;

        }

        /// <summary>
        /// Enter快捷键执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterDownRun(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                NewImg(sender, e);
            }
        }

        public override DialogResult Show(params object[] args) {
            var count = (int)args[0];
            indexBox.Maximum = count;
            indexBox.Value = count;
            return ShowDialog();
        }
        
        /// <summary>
        /// 新建一个img
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NewImg(object sender, EventArgs e) {
            var path = pathBox.Text.Trim();
            if (path.GetName().Equals(string.Empty)) {
                Messager.ShowError("FileNameCannotEmpty");
                return;
            }
            var count = (int)countBox.Value;
            var index = (int)indexBox.Value;
            var album = new Album();
            Controller.Do("newImg",album, path, count, index);
            pathBox.Text = pathBox.Text.Replace(path.GetName(), "");
            DialogResult = DialogResult.OK;
        }

    }
}
