using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Support {
    internal class NpkSupport : IFileSupport {
        public string Extension => ".npk";

        public void Encode(string file, List<Album> album) { }

        public List<Album> Decode(string filename) {
            return NpkCoder.Load(filename);
        }
    }
}
