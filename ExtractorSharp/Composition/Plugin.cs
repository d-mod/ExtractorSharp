using ExtractorSharp.Command;
using ExtractorSharp.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Composition {
    /// <summary>
    /// 
    /// </summary>
    public class Plugin {

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { set; get; } = true;

        public string Name { set; get; }

        public Guid Guid { set; get; }

        public string Author { get; }

        public string Description { get; }

        public string Versinon { get; }

        public string Since { get; }

        public Plugin(IMetadata metadata) {
            this.Name = metadata.Name;
            this.Author = metadata.Author;
            this.Description = metadata.Description;
            this.Versinon = metadata.Version;
            this.Since = metadata.Since;
        }
    }
}
