using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Handle {
    public class SixthHandler : SecondHandler {

        public SixthHandler(Album album) : base(album) { }

        public override ImageData GetImageData(Sprite sprite) {
            var data = sprite.Data;
            var width = sprite.Width;
            var height = sprite.Height;
            var size = width * height;
            if(sprite.ColorFormat == ColorFormats.ARGB_1555 && sprite.CompressMode == CompressMode.ZLIB) {
                data = Zlib.Decompress(data, size);
                var table = this.Album.CurrentPalette;
                if(table.Count > 0) {
                    using(var os = new MemoryStream()) {
                        foreach(var i in data) {
                            os.WriteColor(table[i % table.Count], ColorFormats.ARGB_8888);
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
                    var data = sprite.ImageData.Data;
                    var palette = this.Album.CurrentPalette.ToList();
                    var exceed = false;
                    for(var i = 0; i < data.Length && palette.Count <= 256; i += 4) {
                        var color = Color.FromArgb(data[i + 3], data[i + 2], data[i + 1], data[i]);
                        if(!palette.Contains(color)) {
                            palette.Add(color);
                        }
                        if(palette.Count > 256) {
                            exceed = true;
                            break;
                        }
                        ms.WriteByte((byte)palette.IndexOf(color));
                    }
                    if(!exceed) {
                        this.Album.CurrentPalette = palette;
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
            using(var ms = new MemoryStream()) {
                ms.WriteInt(this.Album.Palettes.Count);
                foreach(var table in this.Album.Palettes) {
                    ms.WriteInt(table.Count);
                    Colors.WritePalette(ms, table);
                }
                ms.Write(base.AdjustData());
                return ms.ToArray();
            }
        }

        public override void ConvertToVersion(ImgVersion version) {
            if(version > ImgVersion.Ver2 && version != ImgVersion.Ver5) {
                return;
            }

            foreach(var item in this.Album.List) {
                if(item.ColorFormat != ColorFormats.LINK) {
                    item.ColorFormat = ColorFormats.ARGB_8888;
                }
            }
        }

        public override void CreateFromStream(Stream stream) {
            var size = stream.ReadInt();
            this.Album.Palettes = new List<List<Color>>();
            for(var i = 0; i < size; i++) {
                var count = stream.ReadInt();
                var table = Colors.ReadPalette(stream, count);
                this.Album.Palettes.Add(table.ToList());
            }
            base.CreateFromStream(stream);
        }
    }
}