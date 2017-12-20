using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.EventArguments {
    public class ItemDragEventArgs<T> :EventArgs{
        public int Index { set; get; }
        public int Target { set; get; }
    }
}
