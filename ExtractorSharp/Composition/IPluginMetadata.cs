using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Composition {
    public interface IPluginMetadata {
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
