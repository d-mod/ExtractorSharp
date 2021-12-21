using System.Collections.Generic;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Supports {


    [Export(typeof(IFileSupport))]
    public class ImgSupport : IFileSupport {
        public string Extension => ".img";

        public List<Album> Decode(string filename) {
            return NpkCoder.Load(filename);
        }

        public void Encode(string file, List<Album> album) {

        }
    }
}
