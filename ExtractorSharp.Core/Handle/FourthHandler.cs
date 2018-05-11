using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ExtractorSharp.Handle {
    /// <summary>
    /// Ver4处理器
    /// </summary>
    public class FourthHandler : SecondHandler {

        public FourthHandler(Album Album) : base(Album) {}

        public override Bitmap ConvertToBitmap(Sprite entity) {
            var data = entity.Data;
            var size = entity.Width * entity.Height;
            if (entity.Type==ColorBits.ARGB_1555&&entity.Compress == Compress.ZLIB) {
                data = Zlib.Decompress(data, size);
                var table = Album.CurrentTable;
                if (table.Count > 0) {
                    using (var os = new MemoryStream()) {
                        for (var i = 0; i < data.Length; i++) {
                            var j = data[i] % table.Count;
                            Colors.WriteColor(os, table[j], ColorBits.ARGB_8888);
                        }
                        data = os.ToArray();
                    }
                }
            }
            return base.ConvertToBitmap(entity);
        }

        public override byte[] ConvertToByte(Sprite entity) {
            if (entity.Compress == Compress.NONE) {
                return base.ConvertToByte(entity);
            }
            using (var ms = new MemoryStream()) {
                var data = entity.Picture.ToArray();
                var table = Album.CurrentTable;
                for (var i = 0; i < data.Length; i += 4) {
                    var color = Color.FromArgb(data[i + 3], data[i + 2], data[i + 1], data[i]);
                    if (!table.Contains(color))
                        table.Add(color);
                    ms.WriteByte((byte)table.IndexOf(color));
                }
                data = ms.ToArray();
                if (data.Length < 2) {
                    data = new byte[2];
                }
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

        public override void ConvertToVersion(Img_Version Version) {
            if (Version == Img_Version.Ver2) {
                Album.List.ForEach(item => item.Type = item.Type == ColorBits.ARGB_1555 && item.Compress == Compress.ZLIB ? ColorBits.ARGB_8888 : item.Type);
            }
        }

        public override void CreateFromStream(Stream stream) {
            var size = stream.ReadInt();
            var Table = new List<Color>(Colors.ReadPalette(stream,size));
            Album.Tables = new List<List<Color>>();
            Album.Tables.Add(Table);
            base.CreateFromStream(stream);
        }

    }
}
