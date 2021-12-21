using System.Collections.Generic;
using ExtractorSharp.Json.Converter;

namespace ExtractorSharp.Json {
    internal class LSHoster {
        public static Dictionary<LSType, List<IConverter>> Dictionary { get; } =
            new Dictionary<LSType, List<IConverter>>();

        /// <summary>
        ///     添加转换器
        /// </summary>
        /// <param name="converter"></param>
        public static void Add(IConverter converter) {
            if(!Dictionary.ContainsKey(converter.Type)) {
                Dictionary.Add(converter.Type, new List<IConverter>());
            }

            Dictionary[converter.Type].Add(converter);
        }
    }
}