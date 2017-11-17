using System;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.UI;
using ExtractorSharp.Handle;
using ExtractorSharp.Core;
using ExtractorSharp.Data;

namespace ExtractorSharp.View {
    public partial class ConvertDialog : EaseDialog {
        Album[] array;
        Controller Controller;
        public ConvertDialog(){
            Controller = Program.Controller;
            InitializeComponent();
            var array = new Img_Version[Handler.Dic.Count];
            Handler.Dic.Keys.CopyTo(array, 0);
            foreach(var ver in array) 
                combo.Items.Add(ver);
            yesButton.Click += Convert;
        }
        
        public override DialogResult Show(params object[] array) {
            if (array.Length > 0) {
                this.array = array as Album[];
                combo.SelectedItem = this.array[0].Version;
                return ShowDialog();
            }
            return DialogResult.None;
        }

        public void Convert(object sender, EventArgs e) {
            var Version = (Img_Version)combo.SelectedItem;
            var count = 0;
            foreach (var al in array)
                count += al.List.Count;
            progress.Maximum = count;
            progress.Value = 0;
            progress.Visible = true;
            foreach (var al in array) {
                if (al.Version != Version) {
                    foreach (var entity in al.List) {
                        var Image = entity.Picture;
                        progress.Value++;
                    }
                    al.ConvertTo(Version);
                }
            }
            Controller.isSave = false;
            progress.Visible = false;
            GC.Collect();
            Visible = false;
        }
        
    }
}
