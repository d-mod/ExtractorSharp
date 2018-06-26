using ExtractorSharp.Core.Draw;

namespace ExtractorSharp.EventArguments {
    public class LayerEventArgs {
        public IPaint Last { set; get; }
        public IPaint Current { set; get; }
        public int ChangedIndex { set; get; }
    }
}