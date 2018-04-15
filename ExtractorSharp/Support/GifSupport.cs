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
            var album = new Album();
            album.Path = filename.GetSuffix();
            var sprites = new Sprite[array.Length];
            for (var i = 0; i < array.Length; i++) {
                sprites[i] = new Sprite(album);
                sprites[i].Picture = array[i];
            }
            album.List.AddRange(sprites);
            return new List<Album>() { album};
        }
        
    }
}
