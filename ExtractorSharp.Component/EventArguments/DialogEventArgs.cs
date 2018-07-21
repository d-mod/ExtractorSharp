using System;

namespace ExtractorSharp.Component.EventArguments {
    /// <summary>
    ///     窗口相关事件
    /// </summary>
    public class DialogEventArgs : EventArgs {
        public ESDialog Dialog { set; get; }

        public Type DialogType { set; get; }
    }
}