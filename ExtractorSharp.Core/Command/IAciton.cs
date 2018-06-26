using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Command {
    /// <summary>
    ///     多图层操作
    /// </summary>
    public interface IAction : ICommand { }

    /// <summary>
    ///     多IMG操作
    /// </summary>
    public interface IMutipleAciton : IAction {
        void Action(params Album[] array);
    }

    /// <summary>
    ///     单IMG操作
    /// </summary>
    public interface ISingleAction : IAction {
        int[] Indices { get; set; }
        void Action(Album album, int[] indexes);
    }
}