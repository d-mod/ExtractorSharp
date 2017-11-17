using System;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.UI;
using ExtractorSharp.Core;
using ExtractorSharp.Data;

namespace ExtractorSharp.View {
    public partial class NewImageDialog : EaseDialog {
        private Album Album;
        private Controller Controller { get; }
        public NewImageDialog() {
            Controller = Program.Controller;
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
            } else
                Messager.ShowMessage(Msg_Type.Warning, Language["NotSelectImgTips"]);
            return DialogResult.None;
        }

        public void Run(object sender, EventArgs e) {
            var count = (int)count_box.Value;
            var type = ColorBits.LINK;
            if (_1555_radio.Checked)
                type = ColorBits.ARGB_1555;
            else if (_4444_radio.Checked)
                type = ColorBits.ARGB_4444;
            else if (_8888_radio.Checked)
                type = ColorBits.ARGB_8888;
            var index = (int)index_box.Value;
            Controller.Do("newImage", Album, count, type, index);
            Visible = false;
        }

    }
}
