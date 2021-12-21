using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.Dialog {

    [ExportMetadata("Name", "convert")]
    [Export(typeof(IView))]
    public partial class ConvertDialog : BaseDialog, IPartImportsSatisfiedNotification {

        private Album[] Array;

        [Import]
        private Store Store;

        public ConvertDialog() {
        }

        public void OnImportsSatisfied() {
            InitializeComponent();
            var list = Handler.Versions;
            var array = new object[list.Count];
            for(var i = 0; i < list.Count; i++) {
                array[i] = list[i];
            }
            combo.Items.AddRange(array);
            yesButton.Click += Convert;
        }

        public override object ShowView(params object[] args) {
            if(args.Length > 0) {
                Array = args as Album[];
                combo.SelectedItem = Array[0].Version;
                return ShowDialog();
            }
            return DialogResult.None;
        }

        public void Convert(object sender, EventArgs e) {
            var Version = (ImgVersion)combo.SelectedItem;
            var count = 0;
            foreach(var al in Array) {
                count += al.List.Count;
            }
            progress.Maximum = count;
            progress.Value = 0;
            progress.Visible = true;
            foreach(var al in Array) {
                if(al.Version != Version) {
                    foreach(var entity in al.List) {
                        var image = entity.Image;
                        progress.Value++;
                    }

                    al.ConvertTo(Version);
                }
            }
            Store.Set("/data/is-save", false);
            progress.Visible = false;
            DialogResult = DialogResult.OK;
        }
    }
}