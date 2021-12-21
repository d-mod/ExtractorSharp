using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Coder {
    public static class SpriteCoder {

        public const string SPRITE_FLAG = "Neople Sprite File";


        public static void SaveToStream(Stream stream, Sprite sprite) {
            var data = sprite.ImageData.Data;
            var length = data.Length;
            stream.WriteString(SPRITE_FLAG);
            stream.WriteInt((int)sprite.ColorFormat);
            stream.WriteInt((int)sprite.CompressMode);
            stream.WriteInt(sprite.Width);
            stream.WriteInt(sprite.Height);
            stream.WriteInt(length);
            stream.WriteInt(sprite.X);
            stream.WriteInt(sprite.Y);
            stream.WriteInt(sprite.FrameWidth);
            stream.WriteInt(sprite.FrameHeight);
            stream.Write(data);
        }

        public static Sprite LoadFromStream(Stream stream, Sprite sprite) {
            if(sprite == null) {
                sprite = new Sprite();
            }
            var pos = stream.Position;
            var flag = stream.ReadString();
            if(flag != SPRITE_FLAG) {
                stream.Position = pos;
                return null;
            }
            sprite.ColorFormat = (ColorFormats)stream.ReadInt();
            sprite.CompressMode = (CompressMode)stream.ReadInt();
            sprite.Width = stream.ReadInt();
            sprite.Height = stream.ReadInt();
            sprite.Length = stream.ReadInt();
            sprite.X = stream.ReadInt();
            sprite.Y = stream.ReadInt();
            var data = new byte[sprite.Length];
            stream.Read(data);
            sprite.ImageData = new ImageData(data, sprite.Width, sprite.Height);
            return sprite;
        }

    }
}
