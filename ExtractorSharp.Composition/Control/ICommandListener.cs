namespace ExtractorSharp.Composition.Control {

    /// <summary>
    /// 命令监听器
    /// </summary>
    public interface ICommandListener {
        /// <summary>
        /// 命令执行前
        /// </summary>
        /// <param name="e"></param>
        void Before(CommandEventArgs e);

        /// <summary>
        /// 命令执行后
        /// </summary>
        /// <param name="e"></param>
        void After(CommandEventArgs e);

    }
}
