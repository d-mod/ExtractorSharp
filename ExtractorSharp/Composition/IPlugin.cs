using ExtractorSharp.Properties;
using System;
using ExtractorSharp.Users;

namespace ExtractorSharp.Composition {
    public interface IPlugin {
        /// <summary>
        /// 安装插件
        /// </summary>
        void Install();
        /// <summary>
        /// 卸载插件
        /// </summary>
        void UnInstall();

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
