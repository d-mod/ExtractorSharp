using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Lib {
    class Gif {
        private const string GIF_FLAG = "GIF89a";

        private short Width;
        private short Height;

        private void Save(Stream stream) {
            stream.WriteString(GIF_FLAG);
            stream.Seek(-1);
            stream.WriteShort(Width);
            stream.WriteShort(Height);
            stream.WriteByte(0);//色表长度
        }

    }
}
