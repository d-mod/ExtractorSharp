using ExtractorSharp.Data;

namespace ExtractorSharp.Command {

    /// <summary>
    /// 多图层操作
    /// </summary>
    public interface IAction : ICommand {
    }

    /// <summary>
    /// 多IMG操作
    /// </summary>
    interface MutipleAciton : IAction {
        void Action(params Album[] array);
    }

    /// <summary>
    /// 单IMG操作
    /// </summary>
    interface SingleAction : IAction {
        int[] Indexes { get; set; }
        void  Action(Album Album, int[] indexes);
    }
}
