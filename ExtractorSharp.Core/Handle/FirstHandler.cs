using System.IO;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Handle {
    public class FirstHandler : SecondHandler {
        public FirstHandler(Album album) : base(album) { }


        public override byte[] AdjustData() {
            using(var ms = new MemoryStream()) {
                ms.WriteString(NpkCoder.IMAGE_FLAG);
                ms.Write(new byte[6]);
                ms.WriteInt((int)this.Album.Version);
                ms.WriteInt(this.Album.Count);
                foreach(var entity in this.Album.List) {
                    ms.WriteInt((int)entity.ColorFormat);
                    if(entity.ColorFormat == ColorFormats.LINK) {
                        ms.WriteInt(entity.TargetIndex);
                        continue;
                    }
                    ms.WriteInt((int)entity.CompressMode);
                    ms.WriteInt(entity.Size.Width);
                    ms.WriteInt(entity.Size.Height);
                    ms.WriteInt(entity.Length);
                    ms.WriteInt(entity.Location.X);
                    ms.WriteInt(entity.Location.Y);
                    ms.WriteInt(entity.FrameSize.Width);
                    ms.WriteInt(entity.FrameSize.Height);
                    ms.Write(entity.Data);
                }
                this.Album.IndexLength = ms.Length;
                return ms.ToArray();
            }
        }

        public override void CreateFromStream(Stream stream) {
            this.Album.IndexLength = stream.ReadInt();
            stream.Seek(2);
            this.Album.Version = (ImgVersion)stream.ReadInt();
            this.Album.Count = stream.ReadInt();
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
                sprite.Length = stream.ReadInt();
                sprite.X = stream.ReadInt();
                sprite.Y = stream.ReadInt();
                sprite.FrameWidth = stream.ReadInt();
                sprite.FrameHeight = stream.ReadInt();
                if(sprite.CompressMode == CompressMode.NONE) {
                    sprite.Length = sprite.Size.Width * sprite.Size.Height * (sprite.ColorFormat == ColorFormats.ARGB_8888 ? 4 : 2);
                }
                var data = new byte[sprite.Length];
                stream.Read(data);
                sprite.Data = data;
            }

        }
    }
}