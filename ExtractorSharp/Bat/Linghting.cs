using System;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.Handle;
using ExtractorSharp.Data;
using ExtractorSharp.Core;

namespace ExtractorSharp.Bat {
    class Linghting : Batch {
        public bool Run(Controller Controller,ImageEntity[] array, ref bool running, ProgressBar bar) {
            foreach (var entity in array) {
                if (!running)
                    return false;
                if (entity.Type == ColorBits.LINK)
                    continue;
                if (entity.Index % 3 != 1)
                    continue;
                var data = entity.Picture.ToArray();
                for (var i = 0; i < data.Length; i += 4) {
                    data[i] = (byte)Complie(data[i]);
                    data[i + 1] = (byte)Complie(data[i + 1]);
                    data[i + 2] = (byte)Complie(data[i + 2]);
                    data[i + 3] = data[i + 3];
                }
                entity.Picture = Tools.FromArray(data, entity.Size);
                bar.Value++;
            }
            return true;
        }

        public static int Complie(int up) {
            return up/2;
        }

        public override string ToString() {
            return "发光效果";
        }
    }
}
