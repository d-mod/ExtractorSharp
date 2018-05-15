using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Composition {
   /// <summary>
   /// 文件转换器
   /// 将其他格式的文件转为IMG格式
   /// </summary>
    public interface IFileSupport{

        string Pattern { get; }

        List<Album> Decode(string filename);

        void Encode(string file,List<Album> album);
    }
}
