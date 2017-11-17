using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.Handle;
using ExtractorSharp.Core;
using ExtractorSharp.Data;

namespace ExtractorSharp.Bat {
    /// <summary>
    /// 批量染色
    /// </summary>
    class BatDye : Batch {

        public bool Run(Controller Controller,ImageEntity[] array, ref bool running, ProgressBar bar) {
            var dialog = new ColorDialog();
            var Color = new Color();
            if (dialog.ShowDialog() == DialogResult.OK) {
                Color = dialog.Color;
                foreach (var entity in array) {
                    if (!running)
                        return false;
                    if (entity.Type == ColorBits.LINK)
                        continue;
                    var data = entity.Picture.ToArray();
                    for (var i = 0; i < data.Length; i += 4) {
                        data[i + 0] = (byte)Complie(data[i + 0], Color.B);
                        data[i + 1] = (byte)Complie(data[i + 1], Color.G);
                        data[i + 2] = (byte)Complie(data[i + 2], Color.R);
                        data[i + 3] = (byte)Complie(data[i + 3], Color.A);
                    }
                    entity.Picture = Tools.FromArray(data, entity.Size);
                    bar.Value++;
                }
            }
            return true;
        }

        public static int Complie(int up, int down) {
            var result = up + down -255 ;
            result = result < 0 ? 0 : result;
            return result;
        }
            
        public override string ToString() {
            return "染色";
        }
        
    }
}
