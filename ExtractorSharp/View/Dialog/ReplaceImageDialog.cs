using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.Dialog {


    [ExportMetadata("Name", "replaceImage")]
    [Export(typeof(IView))]
    public partial class ReplaceImageDialog : BaseDialog, IPartImportsSatisfiedNotification {

        public ReplaceImageDialog() {

        }

        public void Replace(object sender, EventArgs e) {
            Store.Get(seletImageRadio.Checked ? "/imagelist/checked-items" : "/imagelist/items", out Sprite[] array);
            var indices = new int[array.Length];
            for(var i = 0; i < array.Length; i++) {
                indices[i] = array[i].Index;
            }
            var type = ColorFormats.UNKNOWN;
            if(_1555_Radio.Checked) {
                type = ColorFormats.ARGB_1555;
            } else if(_4444_Radio.Checked) {
                type = ColorFormats.ARGB_4444;
            } else if(_8888_Radio.Checked) {
                type = ColorFormats.ARGB_8888;
            }
            var path = string.Empty;
            var mode = 0;
            if(array.Length == 1) {
                var dialog = new OpenFileDialog {
                    Filter = $"{Language["ImageResource"]}|*.jpg;*.png;*.bmp"
                };
                if(dialog.ShowDialog() == DialogResult.OK) {
                    path = dialog.FileName;
                }
            } else if(fromGifBox.Checked) {
                var dialog = new OpenFileDialog {
                    Filter = "GIF|*.gif"
                };
                if(dialog.ShowDialog() == DialogResult.OK) {
                    path = dialog.FileName;
                }
                mode = 1;
            } else {
                var dialog = new FolderBrowserDialog();
                if(dialog.ShowDialog() == DialogResult.OK) {
                    path = dialog.SelectedPath;
                }
                mode = 2;
            }
            if(!string.IsNullOrEmpty(path)) {
                Store.Get("/filelist/selected-item", out Album file);
                Controller.Do("ReplaceImage", new CommandContext {
                    {"type",type },
                    {"isAdjust",adjustPositionBox.Checked },
                    {"path",path },
                    {"mode",mode },
                    {"file" ,file},
                    {"indices",indices }
                }); ;
                DialogResult = DialogResult.OK;
            }
        }

        public void Cancel(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
        }

        public void OnImportsSatisfied() {
            InitializeComponent();
            yesButton.Click += Replace;
            cancelButton.Click += Cancel;
        }
    }
}