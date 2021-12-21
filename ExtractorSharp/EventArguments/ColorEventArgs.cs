using System;
using System.Drawing;

namespace ExtractorSharp.EventArguments {
    public class ColorEventArgs : EventArgs {
        public Color OldColor { set; get; }
        public Color NewColor { set; get; }
    }
}