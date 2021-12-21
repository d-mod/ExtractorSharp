using System;

namespace ExtractorSharp.Composition {

    public class ProgressEventArgs : EventArgs {
        /// <summary>
        ///     总数
        /// </summary>
        public int Maximum { set; get; }

        /// <summary>
        ///     绑定的数据
        /// </summary>
        public object Result { set; get; }

        /// <summary>
        ///     进度
        /// </summary>
        public int Value { set; get; }

        public bool IsCompleted => this.Value >= this.Maximum;
    }

}
