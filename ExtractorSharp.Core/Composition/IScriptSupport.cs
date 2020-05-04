using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Core.Composition {

    /// <summary>
    /// 对脚本文件的支持
    /// </summary>
    public interface IScriptSupport {
        string Extension { get; }

        bool Execute(string file);

    }
}
