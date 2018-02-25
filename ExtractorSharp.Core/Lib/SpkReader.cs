using ICSharpCode.SharpZipLib.BZip2;
using System.IO;
using System.Linq;

namespace ExtractorSharp.Core.Lib {
    public static class SpkReader {
        private static byte[] HEADER = { 0x42, 0x5a, 0x68, 0x39, 0x31, 0x41, 0x59, 0x26, 0x53, 0x59 };
        private static byte[] MARK = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x0e, 0x00, 0xff, 0xff, 0xff, 0xff, 0xff, 0xef, 0xf1, 0xff };
        private static byte[] TAIL = { 0x01, 0x00, 0x00, 0x00 };
       
        
        /// <summary>
        /// 解压spk
        /// <see href="https://musoucrow.github.io/2017/07/21/spk_analysis/"/>
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] Decompress(Stream stream) {
            stream.Seek(272);
            var content = new byte[stream.Length - stream.Position];
            stream.Read(content);
            var parts = content.Split(HEADER);
            var ms = new MemoryStream();
            for (var i = 1; i < parts.Length; i++) {
                var list = parts[i].Split(MARK);
                var comrpessData = Decompress(HEADER.Concat(list[0]).ToArray());
                ms.Write(comrpessData);
                if (list.Length > 1) {
                    for (var j = 1; j < list.Length - 1; j++) {
                        ms.Write(list[j].Sub(32));
                    }
                    var last = list.Last();
                    var pos = last.LastIndexOf(TAIL);
                    if (pos > -1) {
                        ms.Write(last.Sub(32, pos + 1));
                    }

                }
            }
            ms.Close();
            return ms.ToArray();
        }

        private static byte[] Decompress(byte[] data) {
            var ms = new MemoryStream(data);
            var os = new MemoryStream();
            BZip2.Decompress(ms, os, false);
            ms.Close();
            os.Close();
            return os.ToArray();
        }

    }
}
