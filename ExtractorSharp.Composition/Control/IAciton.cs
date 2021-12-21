using System.Collections.Generic;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Composition.Control {
    /// <summary>
    ///    批量操作
    /// </summary>
    public interface IMacro : ICommand { }

    /// <summary>
    ///     多IMG操作
    /// </summary>
    public interface IMutipleMacro : IMacro {
        void Action(IEnumerable<Album> files);
    }

    /// <summary>
    ///     单IMG操作
    /// </summary>
    public interface ISingleMacro : IMacro {

        int[] Indices { get; set; }

        void Action(Album file, int[] indices);

    }
}