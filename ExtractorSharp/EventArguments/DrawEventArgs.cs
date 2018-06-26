using ExtractorSharp.Core.Draw;

namespace ExtractorSharp.EventArguments {
    /// <summary>
    ///     绘制事件参数
    /// </summary>
    internal class DrawEventArgs {
        /// <summary>
        ///     当前选择的画笔
        /// </summary>
        public IBrush Brush { set; get; }
    }
}