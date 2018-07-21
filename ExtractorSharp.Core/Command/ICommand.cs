namespace ExtractorSharp.Core.Command {
    /// <summary>
    ///     命令
    /// </summary>
    public interface ICommand {
        string Name { get; }

        /// <summary>
        ///     可否撤销
        /// </summary>
        /// <returns></returns>
        bool CanUndo { get; }

        /// <summary>
        ///     是否对文件有实质影响
        /// </summary>
        /// <returns></returns>
        bool IsChanged { get; }

        /// <summary>
        ///     执行
        /// </summary>
        void Do(params object[] args);

        /// <summary>
        ///     撤销
        /// </summary>
        void Undo();

        /// <summary>
        ///     重做
        /// </summary>
        void Redo();
    }
}