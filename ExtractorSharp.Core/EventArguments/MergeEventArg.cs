using System;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.EventArguments {
    /// <summary>
    ///     拼合事件
    /// </summary>
    public class MergeEventArgs : EventArgs {
        /// <summary>
        ///     拼合总数
        /// </summary>
        public int Count { set; get; }

        /// <summary>
        ///     拼合指向的文件
        /// </summary>
        public Album Album { set; get; }

        /// <summary>
        ///     拼合进度
        /// </summary>
        public int Progress { set; get; }
    }
}