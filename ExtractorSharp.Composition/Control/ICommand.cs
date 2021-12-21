namespace ExtractorSharp.Composition.Control {
    /// <summary>
    ///     指令
    /// </summary>
    public interface ICommand {
        /// <summary>
        ///     执行
        /// </summary>
        void Do(CommandContext context);
    }

    /// <summary>
    /// 指令执行前的验证
    /// </summary>
    public interface IConfirm : ICommand {

    }

    /// <summary>
    /// 可回滚的指令
    /// </summary>
    public interface IRollback : ICommand {


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