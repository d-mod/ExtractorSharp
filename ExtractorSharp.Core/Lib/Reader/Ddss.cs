using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Core.Lib {
    public class Ddss {

        const int DXT_1 = 0x31545844;
        const int DXT_3 = 0x33545844;
        const int DXT_5 = 0x35545844;
        const int DDS_MAGIC = 0x20534444;
        const int DDS_MIPMAP_COUNT = 0x20000;

        public static Texture Parse(byte[] data) {
            using (var ms = new MemoryStream(data)) {
                return Parse(ms);
            }
        }


        public static Texture Parse(Stream stream) {
            Texture texture = new Texture();
            var magic = stream.ReadInt();
            if (magic != DDS_MAGIC) {
                throw new Exception("Invalid magic number in DDS header");
            }
            texture.Length = stream.ReadInt();
            texture.Flags = stream.ReadInt();
            texture.Width = stream.ReadInt();
            texture.Height = stream.ReadInt();
            stream.Seek(8);
            var count = stream.ReadInt();
            stream.Seek(48);
            var pfFlags = stream.ReadInt();
            if ((pfFlags & DDS_MIPMAP_COUNT)!=0) {
                texture.Count = Math.Max(1, count);
            }
            var format = stream.ReadInt();
            switch (format) {
                case DXT_1:
                    texture.Format = TextureFormat.RGB_S3TC_DXT1_Format;
                    break;
                case DXT_3:
                    texture.Format = TextureFormat.RGBA_S3TC_DXT3_Format;
                    break;
                case DXT_5:
                    texture.Format = TextureFormat.RGBA_S3TC_DXT5_Format;
                    break;
            }
            stream.Seek(8);
            var offset = texture.Length + 4;
            stream.Seek(offset, SeekOrigin.Begin);
            var width = texture.Width;
            var height = texture.Height;
            texture.Mipmaps = new Mipmap[texture.Count];
            for (var i = 0; i < texture.Count; i++) {
                var len = Math.Max(4, width) / 4 * Math.Min(4, height);
                stream.Read(len, out byte[] data);
                Mipmap mip = new Mipmap() {
                    Width = width,
                    Height = height,
                    Data = data
                };
                texture.Mipmaps[i] = mip;
                width = Math.Max(width >> 1, 1);
                height = Math.Max(height >> 1, 1);
            }

            return texture;
        }

        public static byte[] DeCompressDXT1(byte[] data) {
            var buf = new byte[data.Length * 4];
            var ms = new MemoryStream(data);
            for (var i = 0; i < data.Length; i += 4) {
                
                var color0 = ms.ReadShort();
                var color1 = ms.ReadShort();
                var indices = Read2Bits(ms, 4);
                for(var j = 0; j < indices.Length; j++) {
                }
            }
            return null;
        }

        public static byte[] Read2Bits(Stream stream,int byteLength) {
            byte[] data = new byte[byteLength * 4];
            for (int i = 0; i < byteLength; i++) {
                var b = stream.ReadByte();
                for (int j = 0; j < 4; j++) {
                    data[i * 4 + j] = (byte)((b >> (3 - j)) & 0x03);
                }
            }
            return data;
        }
    }
}
