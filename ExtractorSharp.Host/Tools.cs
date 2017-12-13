using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Host {
   static class Tools {
        public static string ReadProp(this StringReader reader) {
            var line = reader.ReadLine();
            var index = line.IndexOf(":")+1;
            line = line.Substring(index);
            return line.Trim();
        }
    }
}
