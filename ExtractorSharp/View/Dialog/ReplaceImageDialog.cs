using System;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.Dialog {
    public partial class ReplaceImageDialog : ESDialog {
        public ReplaceImageDialog(IConnector Connector) : base(Connector) {
            InitializeComponent();
            yesButton.Click += Replace;
            cancelButton.Click += Cancel;
        }

        public void Replace(object sender, EventArgs e) {
            var array = seletImageRadio.Checked ? Connector.CheckedImages : Connector.ImageArray;
            var indexes = new int[array.Length];
            for (var i = 0; i < array.Length; i++) indexes[i] = array[i].Index;
            var type = ColorBits.Unknown;
            if (_1555_Radio.Checked) {
                type = ColorBits.Argb1555;
            } else if (_4444_Radio.Checked) {
                type = ColorBits.Argb4444;
            } else if (_8888_Radio.Checked) type = ColorBits.Argb8888;
            var path = string.Empty;
            var mode = 0;
            if (array.Length == 1) {
                var dialog = new OpenFileDialog();
                dialog.Filter = $"{Language["ImageResource"]}|*.jpg;*.png;*.bmp";
                if (dialog.ShowDialog() == DialogResult.OK) path = dialog.FileName;
            } else if (fromGifBox.Checked) {
                var dialog = new OpenFileDialog();
                dialog.Filter = "GIF|*.gif";
                if (dialog.ShowDialog() == DialogResult.OK) path = dialog.FileName;
                mode = 1;
            } else {
                var dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK) path = dialog.SelectedPath;
                mode = 2;
            }

            if (!string.IsNullOrEmpty(path)) {
                Connector.Do("replaceImage", type, adjustPositionBox.Checked, mode, path, Connector.SelectedFile,
                    indexes);
                DialogResult = DialogResult.OK;
            }
        }

        public void Cancel(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
        }
    }
}