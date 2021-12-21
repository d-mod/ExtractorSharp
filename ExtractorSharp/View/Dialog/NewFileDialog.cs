using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.Dialog {

    [ExportMetadata("Name", "NewFile")]
    [Export(typeof(IView))]
    public partial class NewFileDialog : BaseDialog, IPartImportsSatisfiedNotification {

        public NewFileDialog() {
        }

        public void OnImportsSatisfied() {
            InitializeComponent();
            pathBox.KeyDown += EnterDownRun;
            indexBox.KeyDown += EnterDownRun;
            countBox.KeyDown += EnterDownRun;
            yesButton.Click += NewImg;
            var list = Handler.Versions;
            var array = new object[list.Count];
            for(var i = 0; i < list.Count; i++) {
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
            if(e.KeyCode == Keys.Enter) {
                NewImg(sender, e);
            }
        }

        public override object ShowView(params object[] args) {
            var index = (int)args[0];
            Store.Get("/data/files", out List<Album> files);
            indexBox.Items.Clear();
            indexBox.Items.AddRange(files.ToArray());
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
            if(path.GetSuffix().Equals(string.Empty)) {
                Messager.Error("FileNameCannotEmpty");
                return;
            }
            var count = (int)countBox.Value;
            var index = indexBox.SelectedIndex;
            var version = (ImgVersion)versionBox.SelectedItem;
            var file = new Album {
                Path = path,
                Count = count,
                Version = version
            };
            Controller.Do("NewFile", new CommandContext(file){
                {"Index",index }
            });
            pathBox.Text = pathBox.Text.Replace(path.GetSuffix(), "");
            DialogResult = DialogResult.OK;
        }
    }
}