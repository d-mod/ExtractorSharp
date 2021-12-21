using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Handle {
    /// <summary>
    ///     Ver4处理器
    /// </summary>
    public class FourthHandler : SecondHandler {

        public FourthHandler(Album album) : base(album) { }



        public override ImageData GetImageData(Sprite sprite) {
            var data = sprite.Data;
            var width = sprite.Width;
            var height = sprite.Height;
            var size = width * height;
            if(sprite.ColorFormat == ColorFormats.ARGB_1555 && sprite.CompressMode == CompressMode.ZLIB) {
                data = Zlib.Decompress(data, size);
                var palette = this.Album.CurrentPalette;
                if(palette.Count > 0) {
                    using(var os = new MemoryStream()) {
                        foreach(var b in data) {
                            var j = b % palette.Count;
                            os.WriteColor(palette[j], ColorFormats.ARGB_8888);
                        }
                        data = os.ToArray();
                    }
                    return new ImageData(data, width, height);
                }
            }
            return base.GetImageData(sprite);
        }


        public override byte[] ConvertToByte(Sprite sprite) {
            if(sprite.ColorFormat == ColorFormats.ARGB_1555 && sprite.CompressMode == CompressMode.ZLIB) {
                using(var ms = new MemoryStream()) {
                    var exceed = false;
                    var data = sprite.ImageData.Data;
                    var table = this.Album.CurrentPalette.ToList();
                    for(var i = 0; i < data.Length; i += 4) {
                        var color = Color.FromArgb(data[i + 3], data[i + 2], data[i + 1], data[i]);
                        if(!table.Contains(color)) {
                            table.Add(color);
                        }
                        // 当颜色总数超过256个时,不再使用调色板模式
                        if(table.Count > 256) {
                            exceed = true;
                            break;
                        }
                        ms.WriteByte((byte)table.IndexOf(color));
                    }
                    if(!exceed) {
                        this.Album.CurrentPalette = table;
                        data = ms.ToArray();
                        if(data.Length < 2) {
                            data = new byte[2];
                        }
                        return data;
                    }
                }
                sprite.ColorFormat = ColorFormats.ARGB_8888;
            }
            if(sprite.ColorFormat > ColorFormats.ARGB_1555) {
                sprite.CompressMode = CompressMode.NONE;
            }
            return base.ConvertToByte(sprite);
        }


        public override byte[] AdjustData() {
            var table = this.Album.CurrentPalette;
            using(var ms = new MemoryStream()) {
                ms.WriteInt(table.Count);
                Colors.WritePalette(ms, table);
                ms.Write(base.AdjustData());
                return ms.ToArray();
            }
        }

        public override void ConvertToVersion(ImgVersion version) {
            if(version <= ImgVersion.Ver2 || version == ImgVersion.Ver5) {
                foreach(var item in this.Album.List) {
                    if(item.ColorFormat != ColorFormats.LINK) {
                        item.ColorFormat = ColorFormats.ARGB_8888;
                    }
                }
            }
        }

        public override void CreateFromStream(Stream stream) {
            var size = stream.ReadInt();
            var Table = new List<Color>(Colors.ReadPalette(stream, size));
            this.Album.Palettes = new List<List<Color>> {
                Table
            };
            base.CreateFromStream(stream);
        }
    }
}