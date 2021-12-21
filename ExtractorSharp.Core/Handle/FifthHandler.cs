using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Handle {
    /// <summary>
    ///     Ver5处理器
    /// </summary>
    public class FifthHandler : SecondHandler {
        private readonly Dictionary<int, TextureInfo> _map = new Dictionary<int, TextureInfo>();
        public readonly List<Texture> List = new List<Texture>();
        public FifthHandler(Album album) : base(album) { }



        public override ImageData GetImageData(Sprite sprite) {
            if(sprite.ColorFormat < ColorFormats.LINK && sprite.Length > 0) {
                return base.GetImageData(sprite);
            }
            if(!this._map.ContainsKey(sprite.Index)) {
                return ImageData.Empty;
            }
            var info = this._map[sprite.Index];
            var dds = info.Texture;
            var data = dds.ImageData.Clone(info.Rectangle);
            if(info.Top != 0) {
                data = data.RotateFlip();
            }
            return data;
        }




        public override byte[] ConvertToByte(Sprite sprite) {
            if(sprite.ColorFormat < ColorFormats.LINK && sprite.Length > 0) {
                return base.ConvertToByte(sprite);
            }
            if(sprite.Width * sprite.Height == 1) {
                sprite.CompressMode = CompressMode.NONE;
                return base.ConvertToByte(sprite);
            }
            if(sprite.CompressMode == CompressMode.ZLIB) {
                sprite.CompressMode = CompressMode.DDS_ZLIB;
            }
            var dds = Texture.CreateFromSprite(sprite);
            this._map[sprite.Index] = new TextureInfo {
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
            this.List.Clear();
            foreach(var index in this._map.Values) {
                var dds = index.Texture;
                if(this.List.Contains(dds)) {
                    continue;
                }

                dds.Index = this.List.Count;
                this.List.Add(dds);
            }

            var ms = new MemoryStream();
            ms.WriteInt(this.Album.CurrentPalette.Count);
            Colors.WritePalette(ms, this.Album.CurrentPalette);

            foreach(var dds in this.List) {
                ms.WriteInt((int)dds.Version);
                ms.WriteInt((int)dds.Type);
                ms.WriteInt(dds.Index);
                ms.WriteInt(dds.Length);
                ms.WriteInt(dds.FullLength);
                ms.WriteInt(dds.Width);
                ms.WriteInt(dds.Height);
            }

            var ver2List = new List<Sprite>();
            var start = ms.Length;
            foreach(var sprite in this.Album.List) {
                ms.WriteInt((int)sprite.ColorFormat);
                if(sprite.ColorFormat == ColorFormats.LINK) {
                    ms.WriteInt(sprite.TargetIndex);
                    continue;
                }
                ms.WriteInt((int)sprite.CompressMode);
                ms.WriteInt(sprite.Size.Width);
                ms.WriteInt(sprite.Size.Height);
                ms.WriteInt(sprite.Length);
                ms.WriteInt(sprite.Location.X);
                ms.WriteInt(sprite.Location.Y);
                ms.WriteInt(sprite.FrameSize.Width);
                ms.WriteInt(sprite.FrameSize.Height);
                if(sprite.ColorFormat < ColorFormats.LINK && sprite.Length != 0) {
                    ver2List.Add(sprite);
                    continue;
                }
                var info = this._map[sprite.Index];
                ms.WriteInt(info.Unknown);
                ms.WriteInt(info.Texture.Index);
                ms.WriteInt(info.LeftUp.X);
                ms.WriteInt(info.LeftUp.Y);
                ms.WriteInt(info.RightDown.X);
                ms.WriteInt(info.RightDown.Y);
                ms.WriteInt(info.Top);
            }
            this.Album.IndexLength = ms.Length - start;
            foreach(var dds in this.List) {
                ms.Write(dds.Data);
            }
            foreach(var sprite in ver2List) {
                ms.Write(sprite.Data);
            }
            ms.Close();
            var data = ms.ToArray();
            this.Album.Length = data.Length + 40;
            ms = new MemoryStream();
            ms.WriteInt(this.List.Count);
            ms.WriteInt(this.Album.Length);
            ms.Write(data);
            ms.Close();
            return ms.ToArray();
        }

        public override void CreateFromStream(Stream stream) {
            var indexCount = stream.ReadInt();
            this.Album.Length = stream.ReadInt();
            var count = stream.ReadInt();
            var table = new List<Color>(Colors.ReadPalette(stream, count));
            this.Album.Palettes = new List<List<Color>> { table };
            var list = new List<Texture>();
            for(var i = 0; i < indexCount; i++) {
                var dds = new Texture {
                    Version = (TextureVersion)stream.ReadInt(),
                    Type = (ColorFormats)stream.ReadInt(),
                    Index = stream.ReadInt(),
                    Length = stream.ReadInt(),
                    FullLength = stream.ReadInt(),
                    Width = stream.ReadInt(),
                    Height = stream.ReadInt()
                };
                list.Add(dds);
            }
            var ver2List = new List<Sprite>();
            for(var i = 0; i < this.Album.Count; i++) {
                var sprite = new Sprite(this.Album) {
                    Index = this.Album.List.Count,
                    ColorFormat = (ColorFormats)stream.ReadInt()
                };
                this.Album.List.Add(sprite);
                if(sprite.ColorFormat == ColorFormats.LINK) {
                    sprite.TargetIndex = stream.ReadInt();
                    continue;
                }
                sprite.CompressMode = (CompressMode)stream.ReadInt();
                sprite.Width = stream.ReadInt();
                sprite.Height = stream.ReadInt();
                sprite.Length = stream.ReadInt(); //保留，固定为0
                sprite.X = stream.ReadInt();
                sprite.Y = stream.ReadInt();
                sprite.FrameWidth = stream.ReadInt();
                sprite.FrameHeight = stream.ReadInt();
                if(sprite.ColorFormat < ColorFormats.LINK && sprite.Length != 0) {
                    ver2List.Add(sprite);
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
                this._map.Add(sprite.Index, info);
            }

            foreach(var dds in list) {
                var data = new byte[dds.Length];
                stream.Read(data);
                dds.Data = data;
            }
            foreach(var sprite in ver2List) {
                var data = new byte[sprite.Length];
                stream.Read(data);
                sprite.Data = data;
            }
        }

        public override void ConvertToVersion(ImgVersion version) {
            foreach(var sprite in this.Album.List) {
                sprite.Load();
                if(version <= ImgVersion.Ver2) {
                    if(sprite.ColorFormat == ColorFormats.DDS_DXT1) {
                        sprite.ColorFormat = ColorFormats.ARGB_1555;
                    }
                    if(sprite.ColorFormat == ColorFormats.DDS_DXT5) {
                        sprite.ColorFormat = ColorFormats.ARGB_8888;
                    }
                } else if(version == ImgVersion.Ver4) {
                    sprite.ColorFormat = ColorFormats.ARGB_1555;
                }
                if(sprite.CompressMode > CompressMode.ZLIB) {
                    sprite.CompressMode = CompressMode.ZLIB;
                }
            }
        }
    }
}