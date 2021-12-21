using System;

namespace ExtractorSharp.EventArguments {
    public class ItemHoverEventArgs : EventArgs {
        /// <summary>
        ///     悬停选中的元素
        /// </summary>
        public object Item { set; get; }

        /// <summary>
        ///     悬停选中的下标
        /// </summary>
        public int Index { set; get; }

        /// <summary>
        ///     上次悬停选中的元素
        /// </summary>
        public object LastItem { set; get; }

        /// <summary>
        ///     上次悬停选中的下标
        /// </summary>
        public int LastIndex { set; get; }
    }
}