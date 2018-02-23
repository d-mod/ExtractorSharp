using ICSharpCode.SharpZipLib.BZip2;
using System;
using System.IO;
using System.Text;

namespace ExtractorSharp.Core.Handle {
    public class SpkReader {
        private const string COMPRESS_HEADER = "BZh91AY&SY";
        public static byte[] Decompress(Stream stream) {
            stream.Seek(268);
            var size = stream.ReadInt();
            while (stream.Position < stream.Length) {
                var data = new byte[10];
                stream.Read(data);
                var header = Encoding.Default.GetString(data);
                stream.Seek(-9);
                if (header == COMPRESS_HEADER) {
                    stream.Seek(-1);
                    break;
                }
            }
            var compress_data = new byte[size];
            stream.Read(compress_data);
            stream.Seek(48);
            var ms = new MemoryStream();
            var decompress_data = Decompress(compress_data);
            ms.Write(decompress_data);
            while (stream.Position < stream.Length) {
                var len = Math.Min(stream.Length - stream.Position, 4);
                var data = new byte[len];
                stream.Read(data);
                if (len == 4 && data[0] == 0x01 && data[1] == 0 && data[2] == 0 && data[3] == 0) {
                    break;
                }
                ms.Write(data);
            }
            ms.Close();
            return ms.ToArray();
        }
        

        private static byte[] Decompress(byte[] data) {
            var ms = new MemoryStream(data);
            var os = new MemoryStream();
            BZip2.Decompress(ms, os);
            ms.Close();
            os.Close();
            return os.ToArray();
        }
        
    }
}
