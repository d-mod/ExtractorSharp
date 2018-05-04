using ExtractorSharp.Composition;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Support {
    class SpkSupport : IFileConverter {
        public void Encode(string file, List<Album> album) {

        }

        public List<Album> Decode(string filename) {
            var fs = File.Open(filename, FileMode.Open);
            var data = Spks.Decompress(fs);
            fs.Close();
            using (var ms = new MemoryStream(data)) {
                return Npks.ReadNPK(ms, filename.RemoveSuffix(".spk"));
            }
        }
    }
}
