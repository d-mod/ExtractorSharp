using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ExtractorSharp.Handle {
    /// <summary>
    /// Ver5处理器
    /// </summary>
    public class FifthHandler : SecondHandler{
        private readonly Dictionary<Sprite, DDS_Info> Map = new Dictionary<Sprite, DDS_Info>();
        private readonly List<DDS> List = new List<DDS>();
        public FifthHandler(Album Album) : base(Album) {}

        /// <summary>
        /// 校正下标
        /// </summary>
        /// <returns></returns>
        public override byte[] AdjustData() {
            var ms = new MemoryStream();
            foreach (var entity in Album.List) {
                ms.WriteInt((int)entity.Type);
                ms.WriteInt((int)entity.Compress);
                if (entity.Type > ColorBits.LINK) {
                    ms.WriteInt(entity.Size.Width);
                    ms.WriteInt(entity.Size.Height);
                    ms.WriteInt(0x00);
                    ms.WriteInt(entity.Location.X);
                    ms.WriteInt(entity.Location.Y);
                    ms.WriteInt(entity.Canvas_Size.Width);
                    ms.WriteInt(entity.Canvas_Size.Height);
                    ms.WriteInt(0x00);
                    var info = Map[entity];
                    var dds_index = List.IndexOf(info.DDS);
                    ms.WriteInt(dds_index);
                    ms.WriteInt(info.LeftUp.X);
                    ms.WriteInt(info.LeftUp.Y);
                    ms.WriteInt(info.RightDown.X);
                    ms.WriteInt(info.RightDown.Y); 
                    ms.WriteInt(0x00);
                }else if (entity.Type == ColorBits.LINK) 
                    ms.WriteInt(Album.List.IndexOf(entity.Target));
                else {
                    ms.WriteInt(entity.Width);
                    ms.WriteInt(entity.Height);
                    ms.WriteInt(entity.Length);
                    ms.WriteInt(entity.X);
                    ms.WriteInt(entity.Y);
                    ms.WriteInt(entity.Canvas_Width);
                    ms.WriteInt(entity.Canvas_Height);
                }
            }
            ms.Close();
            var Index_Data = ms.ToArray();
            Album.Info_Length = Index_Data.Length;
            Album.Length = 44;
            ms = new MemoryStream();
            foreach (var entity in Album.List) {
                var index = Map[entity];
                var dds = index.DDS;
                if (entity.IsOpen) { 
                    dds = DDS.CreateFromBitmap(entity.Picture, entity.Compress);
                }
                if (!Map.ContainsValue(index)) {
                    dds.Index = Map.Count;
                    Map[entity] = index;
                    Album.Length += dds.Size;
                } 
            }
            ms.WriteInt(Map.Count);
            Album.Length += 28 * Map.Count + 64 * Album.List.Count;
            ms.WriteInt(Album.Length);
            var table = Album.CurrentTable.ToArray();
            ms.WriteInt(table.Length);
            Colors.WritePalette(ms, table);
            var list = new List<DDS>();
            foreach (var index in Map.Values) {
                var dds = index.DDS;
                if (list.Contains(dds)) {
                    continue;
                }
                ms.WriteInt((int)dds.Version);
                ms.WriteInt((int)dds.Type);
                ms.WriteInt(dds.Index);
                ms.WriteInt(dds.Size);
                ms.WriteInt(dds.DDS_Size);
                ms.WriteInt(dds.Width);
                ms.WriteInt(dds.Height);
                list.Add(dds);
            }
            ms.Write(Index_Data);
            ms.Close();
            Index_Data = ms.ToArray();
            return Index_Data;
        }

        public override Bitmap ConvertToBitmap(Sprite entity) {
            if (entity.Type < ColorBits.LINK && entity.Length > 0) {
                return base.ConvertToBitmap(entity);
            }
            if (Map.ContainsKey(entity)) {
                var index = Map[entity];
                var dds = index.DDS;
                var bmp = dds.Pictrue.Clone(index.Rectangle, PixelFormat.DontCare);
                if (index.Top != 0) {
                    bmp.RotateFlip(RotateFlipType.Rotate90FlipXY);
                }
                return bmp;
            }
            return new Bitmap(1, 1);

        }

        public override byte[] ConvertToByte(Sprite entity) {
            if (entity.Compress == Compress.ZLIB && entity.Type < ColorBits.LINK) {
                using (var ms = new MemoryStream()){
                    Npks.WriteImage(ms, entity);
                    return ms.ToArray();
                }
            }
            var dds = DDS.CreateFromBitmap(entity.Picture, entity.Compress);
            Map[entity] = new DDS_Info() {
                DDS = dds,
                RightDown = new Point(entity.Width, entity.Height)
            };
            return dds.Data;
        }


        public override void CreateFromStream(Stream stream) {
            int index_count = stream.ReadInt();
            Album.Length = stream.ReadInt();
            int count = stream.ReadInt();
            var table = new List<Color>(Colors.ReadPalette(stream, count));
            Album.Tables = new List<List<Color>>();
            Album.Tables.Add(table);
            var list = new List<DDS>();
            for (int i = 0; i < index_count; i++) {
                var dds = new DDS();
                dds.Version = (DDS_Version)stream.ReadInt();
                dds.Type = (ColorBits)stream.ReadInt();
                dds.Index = stream.ReadInt();
                dds.Size = stream.ReadInt();
                dds.DDS_Size = stream.ReadInt();
                dds.Width = stream.ReadInt();
                dds.Height = stream.ReadInt();
                list.Add(dds);
            }
            var dic = new Dictionary<Sprite, int>();
            var ver2List = new List<Sprite>();
            for (var i = 0; i < Album.Count; i++) {
                var entity = new Sprite(Album);
                entity.Index = Album.List.Count;
                entity.Type = (ColorBits)stream.ReadInt();
                Album.List.Add(entity);
                if (entity.Type == ColorBits.LINK) {
                    dic.Add(entity, stream.ReadInt());
                    continue;
                }
                entity.Compress = (Compress)stream.ReadInt();
                entity.Width = stream.ReadInt();
                entity.Height = stream.ReadInt();
                entity.Length = stream.ReadInt();                    //保留，固定为0
                entity.X = stream.ReadInt();
                entity.Y = stream.ReadInt();
                entity.Canvas_Width = stream.ReadInt();
                entity.Canvas_Height = stream.ReadInt();
                if (entity.Type < ColorBits.LINK && entity.Length != 0) {
                    ver2List.Add(entity);
                    continue;
                }
                int j = stream.ReadInt();
                var dds = list[stream.ReadInt()];
                var leftup = new Point(stream.ReadInt(), stream.ReadInt());
                var rightdown = new Point(stream.ReadInt(), stream.ReadInt());
                var top = stream.ReadInt();
                var info = new DDS_Info() {
                    DDS = dds,
                    LeftUp = leftup,
                    RightDown = rightdown,
                    Top = top
                };
                Map.Add(entity, info);
            }
            foreach (var entity in dic.Keys) {
                entity.Target = Album.List[dic[entity]];
            }
            foreach (var dds in list) {
                var data = new byte[dds.Size];
                stream.Read(data);
                dds.Data = data;
            }
            foreach (var entity in ver2List) {
                var data = new byte[entity.Length];
                stream.Read(data);
                entity.Data = data;
            }
        }

        public override void ConvertToVersion(Img_Version Version) {
            foreach (var entity in Album.List) {
                entity.Load();
                if (Version == Img_Version.Ver2) {
                    if (entity.Type == ColorBits.DXT_1) {
                        entity.Type = ColorBits.ARGB_1555;
                    }
                    if (entity.Type == ColorBits.DXT_5) {
                        entity.Type = ColorBits.ARGB_8888;
                    }
                } else if (Version == Img_Version.Ver4) {
                    entity.Type = ColorBits.ARGB_1555;
                }
                if (entity.Compress > Compress.ZLIB) {
                    entity.Compress = Compress.ZLIB;
                }
            }
        }
    }
}
