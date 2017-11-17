using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.View;
using ExtractorSharp.Core;
using ExtractorSharp.Data;

namespace ExtractorSharp.Bat {
    class Split : Batch {
        public bool Run(Controller Controller, ImageEntity[] array, ref bool running, ProgressBar bar) {
            bar.Visible = true;
            bar.Maximum = array.Length;
            bar.Value = 0;
            foreach (var entity in array) {
                entity.UnCavasImage();
                bar.Value++;
            }
            bar.Visible = false;
            return true;        
        }
        public override string ToString() {
            return "去画布化";
        }
    }
}
