using System;
using ExtractorSharp.Core;

namespace ExtractorSharp.EventArguments {
    public class MergeQueueEventArgs : EventArgs {
        public QueueChangeMode Mode { set; get; }
        public object Tag { set; get; }
    }
}