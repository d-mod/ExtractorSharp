using ExtractorSharp.Data;
using ExtractorSharp.Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;

namespace ExtractorSharp.Handle {
    /// <summary>
    /// Ver5处理器
    /// </summary>
    class FifthHandler : Handler{
        private Dictionary<ImageEntity, DDS_Info> Map = new Dictionary<ImageEntity, DDS_Info>();
        private List<DDS> List = new List<DDS>();
        public FifthHandler(Album Album) : base(Album) {}

        /// <summary>
        /// 校正下标
        /// </summary>
        /// <returns></returns>
        public override byte[] AdjustIndex() {
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
                    ms.WriteInt(entity.Cavas_Size.Width);
                    ms.WriteInt(entity.Cavas_Size.Height);
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
                    ms.WriteInt(entity.Cavas_Width);
                    ms.WriteInt(entity.Cavas_Height);
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
            ms.WriteColorChart(table);
            var list = new List<DDS>();
            foreach (var index in Map.Values) {
                var dds = index.DDS;
                if (list.Contains(dds))
                    continue;
                ms.WriteInt((int)dds.Version);
                ms.WriteInt(dds.unknow);
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

        public override byte[] AdjustSuffix() {
            if (!Equals(Album.Handler))
                return Album.Handler.AdjustSuffix();
            var ms = new MemoryStream();
            var list = new List<DDS>();
            foreach (var index in Map.Values) {
                var dds = index.DDS;
                if (list.Contains(dds))
                    continue;
                ms.Write(dds.Data);
                list.Add(dds);
            }
            foreach (var entity in Album.List) 
                if (entity.Compress == Compress.ZLIB && entity.Type < ColorBits.LINK) 
                    ms.Write(entity.Data);                         
            ms.Close();
            return ms.ToArray();
        }

        public override Bitmap ConvertToBitmap(ImageEntity entity) {
            if (entity.Type < ColorBits.LINK && entity.Length > 0) {
                var data = entity.Data;
                var type = entity.Type;
                var size = entity.Width * entity.Height * (type == ColorBits.ARGB_8888 ? 4 : 2);
                if (entity.Compress == Compress.ZLIB)
                    data = FreeImage.Uncompress(data, size);
                var ms = new MemoryStream(data);
                data = new byte[entity.Size.Width * entity.Size.Height * 4];
                for (var i = 0; i < data.Length; i += 4) {
                    var temp = ms.ReadColor(type);
                    temp.CopyTo(data, i);
                }
                return Tools.FromArray(data, entity.Size);
            } else {
                if (Map.ContainsKey(entity)) {
                    var index = Map[entity];
                    var dds = index.DDS;
                        var data = dds.Data;
                        data = FreeImage.Uncompress(data, dds.DDS_Size);
                        var bmp = new Bitmap(entity.Width, entity.Height);
                        var src = new Rectangle(index.LeftUp, index.Size);
                        var dst = new Rectangle(Point.Empty, index.Size);
                        using (var g = Graphics.FromImage(bmp)) {
                            g.DrawImage(dds.Pictrue, dst, src, GraphicsUnit.Pixel);
                        }
                        return bmp;
                } else
                    return new Bitmap(1, 1);
            }
        }

        public override byte[] ConvertToByte(ImageEntity entity) {
            if (entity.Compress == Compress.ZLIB && entity.Type < ColorBits.LINK) {
                var stream = new MemoryStream();
                stream.WriteImage(entity);
                stream.Close();
                return stream.ToArray();
            }
            var dds = DDS.CreateFromBitmap(entity.Picture, entity.Compress);
            Map[entity] = new DDS_Info(dds, new Point(0, 0), new Point(entity.Width, entity.Height));
            return dds.Data;
        }


        public override void CreateFromStream(Stream stream) {
            int index_count = stream.ReadInt();
            Album.Length = stream.ReadInt();
            int count = stream.ReadInt();
            var table = new List<Color>(stream.ReadColorChart(count));
            Album.Tables = new List<List<Color>>();
            Album.Tables.Add(table);
            var list = new List<DDS>();
            for (int i = 0; i < index_count; i++) {
                var dds = new DDS();
                dds.Version = (DDS_Version)stream.ReadInt();
                dds.unknow = stream.ReadInt();
                dds.Index = stream.ReadInt();
                dds.Size = stream.ReadInt();
                dds.DDS_Size = stream.ReadInt();
                dds.Width = stream.ReadInt();
                dds.Height = stream.ReadInt();
                list.Add(dds);
            }
            var indexes = new int[Album.Count];
            var dic = new Dictionary<ImageEntity, int>();
            var ver2List = new List<ImageEntity>();
            for (var i = 0; i < Album.Count; i++) {
                var entity = new ImageEntity(Album);
                entity.Type = (ColorBits)stream.ReadInt();
                entity.Compress = (Compress)stream.ReadInt();
                if (entity.Type > ColorBits.LINK) {//ver5 a型
                    entity.Width = stream.ReadInt();
                    entity.Height = stream.ReadInt();
                    stream.Seek(4);                      //保留，固定为0
                    entity.X = stream.ReadInt();
                    entity.Y = stream.ReadInt();
                    entity.Cavas_Width = stream.ReadInt();
                    entity.Cavas_Height = stream.ReadInt();
                    stream.Seek(4);
                    var index = stream.ReadInt();
                    var dds = list[index];
                    var leftup = new Point(stream.ReadInt(), stream.ReadInt());
                    var rightdown = new Point(stream.ReadInt(), stream.ReadInt());
                    stream.Seek(4);
                    Map.Add(entity, new DDS_Info(dds, leftup, rightdown));
                } else if (entity.Type == ColorBits.LINK)
                    dic.Add(entity, stream.ReadInt());
                else {
                    entity.Width = stream.ReadInt();
                    entity.Height = stream.ReadInt();
                    entity.Length = stream.ReadInt();
                    entity.X = stream.ReadInt();
                    entity.Y = stream.ReadInt();
                    entity.Cavas_Width = stream.ReadInt();
                    entity.Cavas_Height = stream.ReadInt();
                    if (entity.Length == 0) {
                        var unknow7 = stream.ReadInt();
                        var unknow5 = stream.ReadInt();
                        var leftup = new Point(stream.ReadInt(), stream.ReadInt());
                        var rightdown= new Point(stream.ReadInt(), stream.ReadInt());
                        var unknow6 = stream.ReadInt();
                        Map.Add(entity, new DDS_Info(list[0], leftup,rightdown));
                    } else { 
                        ver2List.Add(entity);
                    }
                }
                entity.Index = Album.List.Count;
                Album.List.Add(entity);
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
                var image = entity.Picture;
                if (Version == Img_Version.Ver2) {
                    if (entity.Type == ColorBits.DXT_1)
                        entity.Type = ColorBits.ARGB_1555;
                    if (entity.Type == ColorBits.DXT_5)
                        entity.Type = ColorBits.ARGB_8888;
                } else if(Version==Img_Version.Ver4)
                    entity.Type = ColorBits.ARGB_1555;         
                if (entity.Compress > Compress.ZLIB)
                    entity.Compress = Compress.ZLIB;
            }
        }
    }
}
