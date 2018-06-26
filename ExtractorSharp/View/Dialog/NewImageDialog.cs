using System;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.Dialog {
    public partial class NewImageDialog : ESDialog {
        private Album Album;

        public NewImageDialog(IConnector Connector) : base(Connector) {
            InitializeComponent();
            yesButton.Click += Run;
            CancelButton = cancelButton;
        }

        public override DialogResult Show(params object[] args) {
            var album = args[0] as Album;
            if (album != null) {
                Album = album;
                index_box.Maximum = album.List.Count;
                index_box.Value = album.List.Count;
                return ShowDialog();
            }

            Connector.SendWarning("NotSelectImgTips");
            return DialogResult.None;
        }

        public void Run(object sender, EventArgs e) {
            var count = (int) count_box.Value;
            var type = ColorBits.Link;
            if (_1555_radio.Checked) {
                type = ColorBits.Argb1555;
            } else if (_4444_radio.Checked) {
                type = ColorBits.Argb4444;
            } else if (_8888_radio.Checked) {
                type = ColorBits.Argb8888;
            }
            var index = (int) index_box.Value;
            Connector.Do("newImage", Album, count, type, index);
            DialogResult = DialogResult.OK;
        }
    }
}