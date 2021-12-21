using System;

namespace ExtractorSharp.EventArguments {
    public class ItemEventArgs : EventArgs {
        public int Index { set; get; }
        public int[] Indices { set; get; }
        public int Target { set; get; }
    }
}