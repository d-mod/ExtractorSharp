using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.Dialog {


    [ExportMetadata("Name", "newImage")]
    [Export(typeof(IView))]
    public partial class NewImageDialog : BaseDialog, IPartImportsSatisfiedNotification {
        private Album Album;


        [Import]
        private Controller Controller;

        [Import]
        private Messager Messager;

        public NewImageDialog() {
        }

        public void OnImportsSatisfied() {
            InitializeComponent();
            yesButton.Click += Run;
            CancelButton = cancelButton;
        }

        public override object ShowView(params object[] args) {
            var album = args[0] as Album;
            if(album != null) {
                Album = album;
                index_box.Maximum = album.List.Count;
                index_box.Value = album.List.Count;
                return ShowDialog();
            }
            Messager.Warning("NotSelectImgTips");
            return DialogResult.None;
        }

        public void Run(object sender, EventArgs e) {
            var count = (int)count_box.Value;
            var type = ColorFormats.LINK;
            if(_1555_radio.Checked) {
                type = ColorFormats.ARGB_1555;
            } else if(_4444_radio.Checked) {
                type = ColorFormats.ARGB_4444;
            } else if(_8888_radio.Checked) {
                type = ColorFormats.ARGB_8888;
            }
            var index = (int)index_box.Value;
            Controller.Do("NewImage", new CommandContext(Album){
                { "Type" , type },
                { "Count" , count },
                { "Index" , index }
            });
            DialogResult = DialogResult.OK;
        }
    }
}