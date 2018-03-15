using ExtractorSharp.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.EventArguments {
    public class LayerEventArgs {
        public IPaint Last { set; get; }
        public IPaint Current { set; get; }
        public int ChangedIndex { set; get; }
    }
}
