using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ExtractorSharp.Handle {
    class SixthHandler :SecondHandler{
        public SixthHandler(Album Album) : base(Album) { }

        public override Bitmap ConvertToBitmap(Sprite entity) {
            var data = entity.Data;
            var size = entity.Width * entity.Height;
            if (entity.Compress != Compress.ZLIB) {
                return base.ConvertToBitmap(entity);
            }
            data = FreeImage.Decompress(data, size);
            var table = Album.CurrentTable;
            if (table.Count > 0) {
                using (var os = new MemoryStream()) {
                    foreach (var i in data) {
                        Colors.WriteColor(os,table[i % table.Count],ColorBits.ARGB_8888);
                    }
                    data = os.ToArray();
                }
            }
            return Bitmaps.FromArray(data, entity.Size);
        }



        public override byte[] ConvertToByte(Sprite entity) {
            if (entity.Compress == Compress.NONE)
                return base.ConvertToByte(entity);
            var data = entity.Picture.ToArray();
            var ms = new MemoryStream();
            for (var i = 0; i < data.Length; i += 4) {
                var color = Color.FromArgb(data[i + 3], data[i + 2], data[i + 1], data[i]);
                var table = Album.CurrentTable;
                if (!table.Contains(color)) {
                    table.Add(color);
                }
                ms.WriteByte((byte)table.IndexOf(color));
            }
            ms.Close();
            data = ms.ToArray();
            if (data.Length < 2) {
                data = new byte[2];
            }
            return data;
        }

        public override void NewImage(int count, ColorBits type, int index) {
            var array = new Sprite[count];
            if (count < 1) {
                return;
            }
            array[0] = new Sprite(Album);
            array[0].Index = index;
            array[0].Data = new byte[4];
            if (type != ColorBits.LINK) {
                array[0].Type = type;
            }
            for (var i = 1; i < count; i++) {
                array[i] = new Sprite(Album);
                array[i].Type = type;
                if (type == ColorBits.LINK) {
                    array[i].Target = array[0];
                }
                array[i].Index = array[0].Index + i;
            }
            Album.List.InsertAt(index, array);
        }

        public override byte[] AdjustIndex() {
            var ms = new MemoryStream();
            foreach (var entity in Album.List) {
                ms.WriteInt((int)entity.Type);
                if (entity.Type == ColorBits.LINK) {
                    ms.WriteInt(entity.Target.Index);
                    continue;
                }
                ms.WriteInt((int)entity.Compress);
                ms.WriteInt(entity.Size.Width);
                ms.WriteInt(entity.Size.Height);
                ms.WriteInt(entity.Length);
                ms.WriteInt(entity.Location.X);
                ms.WriteInt(entity.Location.Y);
                ms.WriteInt(entity.Canvas_Size.Width);
                ms.WriteInt(entity.Canvas_Size.Height);
            }
            ms.Close();
            var data = ms.ToArray();
            Album.Info_Length = data.Length;
            ms = new MemoryStream();
            ms.WriteInt(Album.Tables.Count);
            foreach(var table in Album.Tables) {
                ms.WriteInt(table.Count);
                Colors.WritePalette(ms,table.ToArray());
            }
            ms.Write(data);
            ms.Close();
            return ms.ToArray();
        }

        public override void ConvertToVersion(Img_Version Version) {
            if (Album.Version == Img_Version.Ver2 || Album.Version == Img_Version.Ver5) {
                foreach (var item in Album.List) {
                    if (item.Type != ColorBits.LINK) {
                        item.Type = ColorBits.ARGB_8888;
                    }
                }
            }
        }

        public override void CreateFromStream(Stream stream) {
            var size= stream.ReadInt();
            Album.Tables = new List<List<Color>>();
            for (int i = 0; i < size; i++) {
                var count = stream.ReadInt();
                var table = Colors.ReadPalette(stream,count);
                Album.Tables.Add(table.ToList());
            }
            base.CreateFromStream(stream);
        }
        
    }
}
