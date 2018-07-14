using System.Collections.Generic;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Composition {
    /// <summary>
    ///     文件转换器
    ///     将其他格式的文件转为IMG格式
    /// </summary>
    public interface IFileSupport {
        string Extension { get; }

        List<Album> Decode(string filename);

        void Encode(string file, List<Album> album);
    }
}