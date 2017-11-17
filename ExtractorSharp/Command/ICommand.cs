

namespace ExtractorSharp.Command {
    /// <summary>
    /// 命令
    /// </summary>
    public interface ICommand {
        /// <summary>
        /// 执行
        /// </summary>
        void Do(params object[] args);  
        /// <summary>
        /// 撤销
        /// </summary>        
        void Undo();
        /// <summary>
        /// 重做
        /// </summary>
        void Redo();

        /// <summary>
        /// 可否撤销
        /// </summary>
        /// <returns></returns>
        bool CanUndo { get; }
        /// <summary>
        /// 是否更改保存状态
        /// </summary>
        /// <returns></returns>
        bool Changed { get; }
        
    }
    
}
