using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Handle {
    /// <summary>
    ///     Ver4处理器
    /// </summary>
    public class FourthHandler : SecondHandler {
        public FourthHandler(Album album) : base(album) { }

        public override Bitmap ConvertToBitmap(Sprite entity) {
            var data = entity.Data;
            var size = entity.Width * entity.Height;
            if (entity.Type == ColorBits.ARGB_1555 && entity.CompressMode == CompressMode.ZLIB) {
                data = Zlib.Decompress(data, size);
                var table = Album.CurrentTable;
                if (table.Count > 0) {
                    using (var os = new MemoryStream()) {
                        foreach (var b in data) {
                            var j = b % table.Count;
                            os.WriteColor(table[j], ColorBits.ARGB_8888);
                        }
                        data = os.ToArray();
                    }
                    return Bitmaps.FromArray(data, entity.Size);
                }
            }
            return base.ConvertToBitmap(entity);
        }

        public override byte[] ConvertToByte(Sprite entity) {
            if (entity.CompressMode == CompressMode.NONE) return base.ConvertToByte(entity);
            using (var ms = new MemoryStream()) {
                var data = entity.Picture.ToArray();
                var table = Album.CurrentTable;
                for (var i = 0; i < data.Length; i += 4) {
                    var color = Color.FromArgb(data[i + 3], data[i + 2], data[i + 1], data[i]);
                    if (!table.Contains(color)) {
                        table.Add(color);
                    }
                    ms.WriteByte((byte) table.IndexOf(color));
                }
                data = ms.ToArray();
                if (data.Length < 2) data = new byte[2];
                return data;
            }
        }


        public override byte[] AdjustData() {
            var table = Album.CurrentTable;
            using (var ms = new MemoryStream()) {
                ms.WriteInt(table.Count);
                Colors.WritePalette(ms, table);
                ms.Write(base.AdjustData());
                return ms.ToArray();
            }
        }

        public override void ConvertToVersion(ImgVersion version) {
            if (version <= ImgVersion.Ver2 || version == ImgVersion.Ver5) {
                foreach (var item in Album.List) {
                    if (item.Type != ColorBits.LINK) {
                        item.Type = ColorBits.ARGB_8888;
                    }
                }
            }
        }

        public override void CreateFromStream(Stream stream) {
            var size = stream.ReadInt();
            var Table = new List<Color>(Colors.ReadPalette(stream, size));
            Album.Tables = new List<List<Color>>();
            Album.Tables.Add(Table);
            base.CreateFromStream(stream);
        }
    }
}