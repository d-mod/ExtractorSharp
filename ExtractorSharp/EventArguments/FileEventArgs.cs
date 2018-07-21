using System;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.EventArguments {
    internal class FileEventArgs : EventArgs {
        public Sprite Entity { set; get; }
        public Album Album { set; get; }
    }
}