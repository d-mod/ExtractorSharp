using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Json.Converter {
    /// <summary>
    /// 转换器
    /// </summary>
    interface IConverter {
        LSType Type { get; }
        string Pattern { get; }
        bool Convert<T>(object Value, out T t);
    }
}
