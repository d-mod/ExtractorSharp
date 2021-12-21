using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Composition.Compatibility {
    public sealed class SpriteFile :IFileObject{

        private Sprite Source { get; }

        public SpriteFile(string name, Sprite source) {
            this.Name = name;
            this.Source = source;
        }
        public string Name { set; get; }

        public void Load(Stream stream) {
            SpriteCoder.LoadFromStream(stream, this.Source);
        }

        public void Save(Stream stream) {
            SpriteCoder.SaveToStream(stream, this.Source);
        }
    }
}
