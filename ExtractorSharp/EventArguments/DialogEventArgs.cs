using System;
using ExtractorSharp.Composition;

namespace ExtractorSharp.EventArguments {
    /// <summary>
    ///     窗口相关事件
    /// </summary>
    public class DialogEventArgs : EventArgs {
        public IView Dialog { set; get; }

        public Type DialogType { set; get; }
    }
}