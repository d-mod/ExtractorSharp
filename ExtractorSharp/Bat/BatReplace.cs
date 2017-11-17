using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.Core;
using ExtractorSharp.Data;

namespace ExtractorSharp.Bat {
    class BatReplace : Batch {
        private string Path = "";
        public bool Run(Controller Controller,ImageEntity[] array, ref bool running, ProgressBar bar) {
            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = Path;
            if (dialog.ShowDialog() == DialogResult.OK) {
                Path = dialog.SelectedPath;
                foreach (var entity in array) {
                    if (!running)
                        return false;
                    string path = Path + "/" + entity.Parent + "/" + entity.Index + ".png";
                    if (File.Exists(path)) {
                        entity.ReplaceImage(ColorBits.ARGB_8888,true,(Bitmap)Image.FromFile(path));
                        bar.Value++;
                    }
                }
                return true;
            }
            return false;
        }

        public override string ToString() {
            return "替换贴图";
        }
    }
}
