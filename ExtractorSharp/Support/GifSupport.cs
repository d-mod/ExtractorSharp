using ExtractorSharp.Composition;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System.Collections.Generic;
using System.IO;

namespace ExtractorSharp.Support {
    class GifSupport : IFileConverter {

        public List<Album> Load(string filename) {
            var fs = File.Open(filename, FileMode.Open);
            var array = Bitmaps.ReadGif(fs);
            fs.Close();
            var album = new Album(array);
            album.Path = filename.GetSuffix();
            return new List<Album>() { album};
        }
        
    }
}
