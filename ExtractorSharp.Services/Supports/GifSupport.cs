using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Text;
using ExtractorSharp.Composition;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Supports {

    [Export(typeof(IFileSupport))]
    public class GifSupport : IFileSupport {
        public string Extension => ".gif";

        public void Encode(string file, List<Album> album) { }

        public List<Album> Decode(string filename) {
            var fs = File.Open(filename, FileMode.Open);
            var array = Bitmaps.ReadGif(fs);
            fs.Close();
            var album = new Album(array) {
                Path = filename.GetSuffix()
            };
            return new List<Album> { album };
        }
    }
}