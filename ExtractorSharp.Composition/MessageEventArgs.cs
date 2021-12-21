using System;

namespace ExtractorSharp.Composition {
    public class MessageEventArgs : EventArgs {

        public MessageType Type { set; get; }

        public string Message { set; get; }

    }

    public enum MessageType {
        /// <summary>
        ///     错误！
        /// </summary>
        Error,

        /// <summary>
        ///     警告！
        /// </summary>
        Warning,

        /// <summary>
        ///     完成！
        /// </summary>
        Success,

        None
    }
}
