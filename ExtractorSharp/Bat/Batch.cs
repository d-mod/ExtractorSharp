using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.Handle;
using ExtractorSharp.Core;
using ExtractorSharp.Data;

namespace ExtractorSharp.Bat {
    internal interface Batch {
        bool Run(Controller Controller,ImageEntity[] array, ref bool running, ProgressBar bar);
    }
}
    