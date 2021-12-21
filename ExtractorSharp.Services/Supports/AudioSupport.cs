using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using ExtractorSharp.Composition;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Supports {

    [Export(typeof(IFileSupport))]
    public class AudioSupport : IFileSupport {
        public string Extension => ".ogg";

        public List<Album> Decode(string filename) {
            using(var stream = File.OpenRead(filename)) {
                var album = new Album {
                    Path = filename.GetSuffix(),
                    Version = ImgVersion.Other
                };
                album.InitHandle(stream);
                return new List<Album> { album };
            }
        }

        public void Encode(string file, List<Album> album) {

        }
    }
}
