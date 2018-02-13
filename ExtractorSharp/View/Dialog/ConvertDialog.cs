using System;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.Component;
using ExtractorSharp.Handle;
using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Core.Control;
using ExtractorSharp.Config;

namespace ExtractorSharp.View {
    public partial class ConvertDialog : ESDialog {
        private Album[] Array;
        public ConvertDialog(IConnector Data) : base(Data) {
            InitializeComponent();
            var list = Handler.Versions;
            object[] array = new object[list.Count];
            for(var i = 0; i < list.Count; i++) {
                array[i] = list[i];
            }
            combo.Items.AddRange(array);
            yesButton.Click += Convert;
        }
        
        public override DialogResult Show(params object[] array) {
            if (array.Length > 0) {
                Array = array as Album[];
                combo.SelectedItem = Array[0].Version;
                return ShowDialog();
            }
            return DialogResult.None;
        }

        public void Convert(object sender, EventArgs e) {
            var Version = (Img_Version)combo.SelectedItem;
            var count = 0;
            foreach (var al in Array)
                count += al.List.Count;
            progress.Maximum = count;
            progress.Value = 0;
            progress.Visible = true;
            foreach (var al in Array) {
                if (al.Version != Version) {
                    foreach (var entity in al.List) {
                        var image = entity.Picture;
                        progress.Value++;
                    }
                    al.ConvertTo(Version);
                }
            }
            Connector.IsSave = false;
            progress.Visible = false;
            DialogResult = DialogResult.OK;
        }
        
    }
}
