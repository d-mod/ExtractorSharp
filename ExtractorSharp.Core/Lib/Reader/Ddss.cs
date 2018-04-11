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
            texture.Pitch = stream.ReadInt();
            texture.Depth = stream.ReadInt();
            var count = stream.ReadInt();
            texture.Reverse=stream.Read(11);
            stream.Seek(37);
            var pfFlags = stream.ReadInt();
            if ((pfFlags & DDS_MIPMAP_COUNT) != 0) {
                texture.Count = Math.Max(1, count);
            }
            var format = stream.ReadInt();
            var blockBytes = 0;
            switch (format) {
                case DXT_1:
                    blockBytes = 8;
                    texture.Format = TextureFormat.RGB_S3TC_DXT1_Format;
                    break;
                case DXT_3:
                    blockBytes = 16;
                    texture.Format = TextureFormat.RGBA_S3TC_DXT3_Format;
                    break;
                case DXT_5:
                    blockBytes = 16;
                    texture.Format = TextureFormat.RGBA_S3TC_DXT5_Format;
                    break;
            }
            stream.Seek(8);
            var offset = texture.Length + 4;
            stream.Seek(offset, SeekOrigin.Begin);
            var width = texture.Width;
            var height = texture.Height;
            width = width / 4 * 4;
            height = height / 4 * 4;
            var len = width * height / 16 * blockBytes;
            texture.Mipmaps = new Mipmap[texture.Count];
            for (var i = 0; i < texture.Count; i++) {
                stream.Read(len, out byte[] data);
                switch (format) {
                    case DXT_1:
                        data = DecodeDXT1(data, width, height);
                        break;
                    case DXT_3:
                        data = DecodeDXT3(data, width, height);
                        break;
                    case DXT_5:
                        data = DecodeDXT5(data, width, height);
                        break;
                }
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



        public static byte[] DecodeDXT1(byte[] data, int width, int height) {
            var ms = new MemoryStream(data);
            var buf = new byte[width * height * 4];
            for (var x = 0; x < width; x += 4) {
                for (var y = 0; y < height; y += 4) {
                    var co0 = ms.ReadUShort();
                    var co1 = ms.ReadUShort();
                    var color0 = DecodeRGB565(co0);
                    var color1 = DecodeRGB565(co1);
                    var color2 = new byte[4];
                    var color3 = new byte[4];
                    if (co0 > co1 ) {
                        color2[0] = (byte)((color0[0] * 2 + color1[0]) / 3);
                        color2[1] = (byte)((color0[1] * 2 + color1[1]) / 3);
                        color2[2] = (byte)((color0[2] * 2 + color1[2]) / 3);
                        color2[3] = 0xff;
                        color3[0] = (byte)((color0[0] + color1[0] * 2) / 3);
                        color3[1] = (byte)((color0[1] + color1[1] * 2) / 3);
                        color3[2] = (byte)((color0[2] + color1[2] * 2) / 3);
                        color3[3] = 0xff;
                    } else {
                        color2[0] = (byte)((color0[0] + color1[0]) / 2);
                        color2[1] = (byte)((color0[1] + color1[1]) / 2);
                        color2[2] = (byte)((color0[2] + color1[2]) / 2);
                        color2[3] = (byte)((color0[3] + color1[3]) / 2);
                    }
                    var colors = new byte[][] { color0, color1, color2, color3 };
                    var index = ms.ReadInt();
                    for (int i = 0; i < 16; i++) {
                        var code = ((index >> (i * 2)) & 0x3);
                        var pos = 4 * (height * (x + i / 4) + y + i % 4);
                        Array.Copy(colors[code], 0, buf, pos, 4);
                    }
                }
            }
            return buf;
        }


        public static byte[] DecodeDXT3(byte[] data, int width, int height) {
            var ms = new MemoryStream(data);
            var buf = new byte[width * height * 4];
            for (var x = 0; x < width; x += 4) {
                for (var y = 0; y < height; y += 4) {
                    var alphas = new ushort[4];
                    //rgb数据 rgb565 
                    var c0 = ms.ReadUShort();
                    var c1 = ms.ReadUShort();
                    //rgb索引
                    var index = ms.ReadInt();

                    alphas[0] = ms.ReadUShort();
                    alphas[1] = ms.ReadUShort();
                    alphas[2] = ms.ReadUShort();
                    alphas[3] = ms.ReadUShort();

                    var color0 = DecodeRGB565(c0);
                    var color1 = DecodeRGB565(c1);

                    var color2 = new byte[4];

                    var color3 = new byte[4];
                    color2[0] = (byte)((color0[0] * 2 + color1[0]) / 3);
                    color2[1] = (byte)((color0[1] * 2 + color1[1]) / 3);
                    color2[2] = (byte)((color0[2] * 2 + color1[2]) / 3);
                    color3[0] = (byte)((color0[0] + color1[0] * 2) / 3);
                    color3[1] = (byte)((color0[1] + color1[1] * 2) / 3);
                    color3[2] = (byte)((color0[2] + color1[2] * 2) / 3);

                    var colors = new byte[][] { color0, color1, color2, color3 };

                    for (var i = 0; i < 16; i++) {
                        var select = (index >> (i * 2)) & 0x3;
                        var pos = 4 * (height * (x + i / 4) + y + i % 4);
                        var a = (byte)(alphas[i / 4] & 0xf);
                        a |= (byte)(a << 4);
                        var col = colors[select];
                        buf[pos + 0] = col[0];
                        buf[pos + 1] = col[1];
                        buf[pos + 2] = col[2];
                        buf[pos + 3] = a;
                    }
                }
            }
            return buf;
        }


        public static byte[] DecodeDXT5(byte[] data, int width, int height) {
            var ms = new MemoryStream(data);
            var buf = new byte[width * height * 4];
            for (var x = 0; x < width; x += 4) {
                for (var y = 0; y < height; y += 4) {
                    var alphas = new byte[8];
                    alphas[0] = (byte)ms.ReadByte();
                    alphas[1] = (byte)ms.ReadByte();
                    if (alphas[0] > alphas[1]) {
                        // Bit code 000 = alpha_0, 001 = alpha_1, others are interpolated.
                        alphas[2] = (byte)((6 * alphas[0] + 1 * alphas[1]) / 7); // bit code 010
                        alphas[3] = (byte)((5 * alphas[0] + 2 * alphas[1]) / 7); // bit code 011
                        alphas[4] = (byte)((4 * alphas[0] + 3 * alphas[1]) / 7); // bit code 100
                        alphas[5] = (byte)((3 * alphas[0] + 4 * alphas[1]) / 7); // bit code 101
                        alphas[6] = (byte)((2 * alphas[0] + 5 * alphas[1]) / 7); // bit code 110
                        alphas[7] = (byte)((1 * alphas[0] + 6 * alphas[1]) / 7); // bit code 111
                    } else {
                        alphas[2] = (byte)((4 * alphas[0] + 1 * alphas[1]) / 5); // Bit code 010
                        alphas[3] = (byte)((3 * alphas[0] + 2 * alphas[1]) / 5); // Bit code 011
                        alphas[4] = (byte)((2 * alphas[0] + 3 * alphas[1]) / 5); // Bit code 100
                        alphas[5] = (byte)((1 * alphas[0] + 4 * alphas[1]) / 5); // Bit code 101
                        alphas[6] = 0x00; // Bit code 110
                        alphas[7] = 0xff; // Bit code 111
                    }
                    //alpha通道索引 每个3bit
                    var bit = Read6Byte(ms);
                    //rgb数据 rgb565 
                    var c0 = ms.ReadUShort();
                    var c1 = ms.ReadUShort();
                    //rgb索引
                    var index = ms.ReadInt();

                    var color0 = DecodeRGB565(c0);
                    var color1 = DecodeRGB565(c1);

                    var color2 = new byte[4];

                    var color3 = new byte[4];
                    if (c0 > c1) {
                        color2[0] = (byte)((color0[0] * 2 + color1[0]) / 3);
                        color2[1] = (byte)((color0[1] * 2 + color1[1]) / 3);
                        color2[2] = (byte)((color0[2] * 2 + color1[2]) / 3);
                        color3[0] = (byte)((color0[0] + color1[0] * 2) / 3);
                        color3[1] = (byte)((color0[1] + color1[1] * 2) / 3);
                        color3[2] = (byte)((color0[2] + color1[2] * 2) / 3);
                    } else {
                        color2[0] = (byte)((color0[0] + color1[0]) / 2);
                        color2[1] = (byte)((color0[1] + color1[1]) / 2);
                        color2[2] = (byte)((color0[2] + color1[2]) / 2);
                    }
                    var colors = new byte[][] { color0, color1, color2, color3 };

                    for (var i = 0; i < 16; i++) {
                        var select = (index >> (i * 2)) & 0x3;
                        var pos = 4 * (height * (x + i / 4) + y + i % 4);
                        var ai = (bit >> (i * 3)) & 0x7;
                        var col = colors[select];
                        buf[pos + 0] = col[0];
                        buf[pos + 1] = col[1];
                        buf[pos + 2] = col[2];
                        buf[pos + 3] = alphas[ai];
                    }
                }
            }
            return buf;
        }

        public static byte[] DecodeRGB565(ushort color) {
            byte[] rgb = new byte[4];
            rgb[0] = (byte)((color) & 0x1f);
            rgb[1] = (byte)((color >> 5) & 0x3f);
            rgb[2] = (byte)((color >> 11) & 0x1f);

            rgb[0] = (byte)((rgb[0] << 3) | (rgb[0] >> 2));
            rgb[1] = (byte)((rgb[1] << 2) | (rgb[1] >> 4));
            rgb[2] = (byte)((rgb[2] << 3) | (rgb[2] >> 2));
            rgb[3] = 0xff;
            return rgb;
        }


        public static ulong Read6Byte(Stream ms) {
            var buf = new byte[8];
            ms.Read(buf, 0, 6);
            return BitConverter.ToUInt64(buf, 0);
        }

    }
}
