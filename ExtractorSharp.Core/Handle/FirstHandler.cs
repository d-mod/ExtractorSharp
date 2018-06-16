using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ExtractorSharp.Handle {
    public class FirstHandler :SecondHandler{
        public FirstHandler(Album Album) : base(Album) { }


        public override byte[] AdjustData() {
            using (var ms = new MemoryStream()) {
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
                    ms.Write(entity.Data);
                }
                Album.Info_Length = ms.Length;
                return ms.ToArray();
            }
        }
        
        public override void CreateFromStream(Stream stream) {
            var dic = new Dictionary<Sprite, int>();
            for (var i = 0; i < Album.Count; i++) {
                var image = new Sprite(Album) {
                    Index = Album.List.Count,
                    Type = (ColorBits)stream.ReadInt()
                };
                Album.List.Add(image);
                if (image.Type == ColorBits.LINK) {
                    dic.Add(image, stream.ReadInt());
                    continue;
                }
                image.Compress = (Compress)stream.ReadInt();
                image.Width = stream.ReadInt();
                image.Height = stream.ReadInt();
                image.Length = stream.ReadInt();
                image.X = stream.ReadInt();
                image.Y = stream.ReadInt();
                image.Canvas_Width = stream.ReadInt();
                image.Canvas_Height = stream.ReadInt();
                if (image.Compress == Compress.NONE) {
                    image.Length = image.Size.Width * image.Size.Height * (image.Type == ColorBits.ARGB_8888 ? 4 : 2);
                }
                var data = new byte[image.Length];
                stream.Read(data);
                image.Data = data;
            }        
            foreach (var image in Album.List) {
                if (image.Type == ColorBits.LINK) {
                    if (dic.ContainsKey(image) && dic[image] > -1 && dic[image] < Album.List.Count && dic[image] != image.Index) {
                        image.Target = Album.List[dic[image]];
                        image.Size = image.Target.Size;
                        image.Canvas_Size = image.Target.Canvas_Size;
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
