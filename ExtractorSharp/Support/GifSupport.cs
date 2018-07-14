using System.Collections.Generic;
using System.IO;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Support {
    internal class GifSupport : IFileSupport {
        public string Extension => ".gif";

        public void Encode(string file, List<Album> album) { }

        public List<Album> Decode(string filename) {
            var fs = File.Open(filename, FileMode.Open);
            var array = Bitmaps.ReadGif(fs);
            fs.Close();
            var album = new Album(array);
            album.Path = filename.GetSuffix();
            return new List<Album> {album};
        }
    }
}