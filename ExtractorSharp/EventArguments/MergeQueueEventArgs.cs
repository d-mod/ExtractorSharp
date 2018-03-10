using ExtractorSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp{
    public class MergeQueueEventArgs :EventArgs{
        public QueueChangeMode Mode { set; get; }
        public object Tag { set; get; }
    }
    
}
