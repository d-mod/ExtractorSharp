using System;
using ExtractorSharp.Core.Control;

namespace ExtractorSharp.Composition {
    public interface IPlugin {

        /// <summary>
        /// 初始化插件
        /// </summary>
        void Initialize();

    }
    /// <summary>
    /// 
    /// </summary>
    public class Plugin {
        internal IPluginMetadata MetaData { get; }
        internal Guid Guid { get; }
        
        internal Plugin(IPluginMetadata metadata) {
            MetaData = metadata;
        }
    }


 
}
