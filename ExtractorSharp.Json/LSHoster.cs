using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Json.Converter;

namespace ExtractorSharp.Json {
    class LSHoster {
        public static Dictionary<LSType, List<IConverter>> Dictionary { get; } = new Dictionary<LSType, List<Json.Converter.IConverter>>();

        /// <summary>
        /// 添加转换器
        /// </summary>
        /// <param name="converter"></param>
        public static void Add(IConverter converter) {
            if (!Dictionary.ContainsKey(converter.Type)) {
                Dictionary.Add(converter.Type, new List<IConverter>());
            }
            Dictionary[converter.Type].Add(converter);
        }

    }
}
