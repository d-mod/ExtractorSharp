using System.Collections.Generic;
using System.IO;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Handle {
    public class FirstHandler : SecondHandler {
        public FirstHandler(Album album) : base(album) { }


        public override byte[] AdjustData() {
            using (var ms = new MemoryStream()) {
                ms.WriteString(NpkCoder.IMAGE_FLAG);
                ms.Write(new byte[6]);
                ms.WriteInt((int) Album.Version);
                ms.WriteInt(Album.Count);
                foreach (var entity in Album.List) {
                    ms.WriteInt((int) entity.Type);
                    if (entity.Type == ColorBits.LINK) {
                        ms.WriteInt(entity.Target.Index);
                        continue;
                    }
                    ms.WriteInt((int) entity.CompressMode);
                    ms.WriteInt(entity.Size.Width);
                    ms.WriteInt(entity.Size.Height);
                    ms.WriteInt(entity.Length);
                    ms.WriteInt(entity.Location.X);
                    ms.WriteInt(entity.Location.Y);
                    ms.WriteInt(entity.FrameSize.Width);
                    ms.WriteInt(entity.FrameSize.Height);
                    ms.Write(entity.Data);
                }
                Album.IndexLength = ms.Length;
                return ms.ToArray();
            }
        }

        public override void CreateFromStream(Stream stream) {
            Album.IndexLength = stream.ReadInt();
            stream.Seek(2);
            Album.Version = (ImgVersion) stream.ReadInt();
            Album.Count = stream.ReadInt();
            var dic = new Dictionary<Sprite, int>();
            for (var i = 0; i < Album.Count; i++) {
                var image = new Sprite(Album) {
                    Index = Album.List.Count,
                    Type = (ColorBits) stream.ReadInt()
                };
                Album.List.Add(image);
                if (image.Type == ColorBits.LINK) {
                    dic.Add(image, stream.ReadInt());
                    continue;
                }
                image.CompressMode = (CompressMode) stream.ReadInt();
                image.Width = stream.ReadInt();
                image.Height = stream.ReadInt();
                image.Length = stream.ReadInt();
                image.X = stream.ReadInt();
                image.Y = stream.ReadInt();
                image.FrameWidth = stream.ReadInt();
                image.FrameHeight = stream.ReadInt();
                if (image.CompressMode == CompressMode.NONE) {
                    image.Length = image.Size.Width * image.Size.Height * (image.Type == ColorBits.ARGB_8888 ? 4 : 2);
                }
                var data = new byte[image.Length];
                stream.Read(data);
                image.Data = data;
            }
            foreach (var image in Album.List) {
                if (image.Type == ColorBits.LINK) {
                    if (dic.ContainsKey(image) && dic[image] > -1 && dic[image] < Album.List.Count &&
                        dic[image] != image.Index) {
                        image.Target = Album.List[dic[image]];
                        image.Size = image.Target.Size;
                        image.FrameSize = image.Target.FrameSize;
                        image.Location = image.Target.Location;
                    } else {
                        Album.List.Clear();
                        return;
                    }
                }
            }
        }
    }
}