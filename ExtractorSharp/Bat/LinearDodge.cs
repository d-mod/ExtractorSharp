using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Handle;
using ExtractorSharp.Command;
using ExtractorSharp.Core;
using ExtractorSharp.Data;

namespace ExtractorSharp.Bat {
    /// <summary>
    /// 线性减淡
    /// </summary>
    class LinearDodge : Batch {
        public bool Run(Controller Controller,ImageEntity[] array, ref bool running, ProgressBar bar) {
            foreach (var entity in array) {
                if (!running)
                    return false;
                if (entity.Type == ColorBits.LINK)
                    continue;
                var data = entity.Picture.ToArray();
                for (var i = 0; i < data.Length; i += 4) {
                    data[i] = (byte)Complie(data[i]);
                    data[i + 1] = (byte)Complie(data[i + 1]);
                    data[i + 2] = (byte)Complie(data[i + 2]);
                    data[i + 3] = (byte)(data[i + 3]*0.75);
                }
                entity.Picture = Tools.FromArray(data, entity.Size);
                bar.Value++;
            }
            return true;
        }

        public static int Complie(int up) {
            var result = (int)(up * 2);
            result = result > 255 ? 255 : result;
            return result;
        }

        public override string ToString() {
            return "线性减淡";
        }
    }
}
