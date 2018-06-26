using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Handle {
    /// <summary>
    ///     Ver5处理器
    /// </summary>
    public class FifthHandler : SecondHandler {
        private readonly Dictionary<int, TextureInfo> _map = new Dictionary<int, TextureInfo>();
        public readonly List<Texture> List = new List<Texture>();
        public FifthHandler(Album album) : base(album) { }


        public override Bitmap ConvertToBitmap(Sprite entity) {
            if (entity.Type < ColorBits.Link && entity.Length > 0) return base.ConvertToBitmap(entity);

            if (!_map.ContainsKey(entity.Index)) return new Bitmap(1, 1);
            var index = _map[entity.Index];
            var dds = index.Texture;
            var bmp = dds.Pictrue.Clone(index.Rectangle, PixelFormat.DontCare);
            if (index.Top != 0) bmp.RotateFlip(RotateFlipType.Rotate90FlipXY);
            return bmp;
        }

        public override byte[] ConvertToByte(Sprite entity) {
            if (entity.Type < ColorBits.Link && entity.Length > 0) return base.ConvertToByte(entity);

            if (entity.Width * entity.Height == 1) {
                entity.CompressMode = CompressMode.None;
                return base.ConvertToByte(entity);
            }
            if (entity.CompressMode == CompressMode.Zlib) entity.CompressMode = CompressMode.DdsZlib;
            var dds = Texture.CreateFromBitmap(entity);
            _map[entity.Index] = new TextureInfo {
                Texture = dds,
                RightDown = new Point(dds.Width, dds.Height)
            };
            return new byte[0];
        }


        /// <summary>
        ///     校正下标
        /// </summary>
        /// <returns></returns>
        public override byte[] AdjustData() {
            List.Clear();
            foreach (var index in _map.Values) {
                var dds = index.Texture;
                if (List.Contains(dds)) continue;
                dds.Index = List.Count;
                List.Add(dds);
            }

            var ms = new MemoryStream();
            ms.WriteInt(Album.CurrentTable.Count);
            Colors.WritePalette(ms, Album.CurrentTable);

            foreach (var dds in List) {
                ms.WriteInt((int) dds.Version);
                ms.WriteInt((int) dds.Type);
                ms.WriteInt(dds.Index);
                ms.WriteInt(dds.Length);
                ms.WriteInt(dds.FullLength);
                ms.WriteInt(dds.Width);
                ms.WriteInt(dds.Height);
            }

            var ver2List = new List<Sprite>();
            var start = ms.Length;
            foreach (var entity in Album.List) {
                ms.WriteInt((int) entity.Type);
                if (entity.Type == ColorBits.Link) {
                    ms.WriteInt(entity.Target.Index);
                    continue;
                }
                ms.WriteInt((int) entity.CompressMode);
                ms.WriteInt(entity.Size.Width);
                ms.WriteInt(entity.Size.Height);
                ms.WriteInt(entity.Length);
                ms.WriteInt(entity.Location.X);
                ms.WriteInt(entity.Location.Y);
                ms.WriteInt(entity.CanvasSize.Width);
                ms.WriteInt(entity.CanvasSize.Height);
                if (entity.Type < ColorBits.Link && entity.Length != 0) {
                    ver2List.Add(entity);
                    continue;
                }
                var info = _map[entity.Index];
                ms.WriteInt(info.Unknown);
                ms.WriteInt(info.Texture.Index);
                ms.WriteInt(info.LeftUp.X);
                ms.WriteInt(info.LeftUp.Y);
                ms.WriteInt(info.RightDown.X);
                ms.WriteInt(info.RightDown.Y);
                ms.WriteInt(info.Top);
            }
            Album.IndexLength = ms.Length - start;
            foreach (var dds in List) ms.Write(dds.Data);

            foreach (var sprite in ver2List) ms.Write(sprite.Data);
            ms.Close();
            var data = ms.ToArray();
            Album.Length = data.Length + 40;
            ms = new MemoryStream();
            ms.WriteInt(List.Count);
            ms.WriteInt(Album.Length);
            ms.Write(data);
            ms.Close();
            return ms.ToArray();
        }

        public override void CreateFromStream(Stream stream) {
            var indexCount = stream.ReadInt();
            Album.Length = stream.ReadInt();
            var count = stream.ReadInt();
            var table = new List<Color>(Colors.ReadPalette(stream, count));
            Album.Tables = new List<List<Color>> {table};
            var list = new List<Texture>();
            for (var i = 0; i < indexCount; i++) {
                var dds = new Texture {
                    Version = (TextureVersion) stream.ReadInt(),
                    Type = (ColorBits) stream.ReadInt(),
                    Index = stream.ReadInt(),
                    Length = stream.ReadInt(),
                    FullLength = stream.ReadInt(),
                    Width = stream.ReadInt(),
                    Height = stream.ReadInt()
                };
                list.Add(dds);
            }
            var dic = new Dictionary<Sprite, int>();
            var ver2List = new List<Sprite>();
            for (var i = 0; i < Album.Count; i++) {
                var entity = new Sprite(Album);
                entity.Index = Album.List.Count;
                entity.Type = (ColorBits) stream.ReadInt();
                Album.List.Add(entity);
                if (entity.Type == ColorBits.Link) {
                    dic.Add(entity, stream.ReadInt());
                    continue;
                }
                entity.CompressMode = (CompressMode) stream.ReadInt();
                entity.Width = stream.ReadInt();
                entity.Height = stream.ReadInt();
                entity.Length = stream.ReadInt(); //保留，固定为0
                entity.X = stream.ReadInt();
                entity.Y = stream.ReadInt();
                entity.CanvasWidth = stream.ReadInt();
                entity.CanvasHeight = stream.ReadInt();
                if (entity.Type < ColorBits.Link && entity.Length != 0) {
                    ver2List.Add(entity);
                    continue;
                }
                var j = stream.ReadInt();
                var k = stream.ReadInt();
                var dds = list[k];
                var leftup = new Point(stream.ReadInt(), stream.ReadInt());
                var rightdown = new Point(stream.ReadInt(), stream.ReadInt());
                var top = stream.ReadInt();
                var info = new TextureInfo {
                    Unknown = j,
                    Texture = dds,
                    LeftUp = leftup,
                    RightDown = rightdown,
                    Top = top
                };
                _map.Add(entity.Index, info);
            }

            foreach (var entity in dic.Keys) entity.Target = Album.List[dic[entity]];
            foreach (var dds in list) {
                var data = new byte[dds.Length];
                stream.Read(data);
                dds.Data = data;
            }
            foreach (var entity in ver2List) {
                var data = new byte[entity.Length];
                stream.Read(data);
                entity.Data = data;
            }
        }

        public override void ConvertToVersion(ImgVersion version) {
            foreach (var entity in Album.List) {
                entity.Load();
                if (version <= ImgVersion.Ver2) {
                    if (entity.Type == ColorBits.Dxt1) entity.Type = ColorBits.Argb1555;
                    if (entity.Type == ColorBits.Dxt5) entity.Type = ColorBits.Argb8888;
                } else if (version == ImgVersion.Ver4) {
                    entity.Type = ColorBits.Argb1555;
                }
                if (entity.CompressMode > CompressMode.Zlib) entity.CompressMode = CompressMode.Zlib;
            }
        }
    }
}