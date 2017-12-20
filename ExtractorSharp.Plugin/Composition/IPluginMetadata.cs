using System;

namespace ExtractorSharp.Composition {
    public class PluginMetadataAttribute :Attribute{
        public PluginMetadataAttribute() {

        }

        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 作者
        /// </summary>
         string Author { get; }
        /// <summary>
        /// 版本
        /// </summary>
         string Version { get; }
        /// <summary>
        /// 备注      
        /// </summary>
         string Description { get; }
        /// <summary>
        /// 序列号
        /// </summary>
         string Guid { get; }
    }
    
}
