using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System.Drawing;
using System.IO;

namespace ExtractorSharp.Handle {
    public class OggHandler : Handler {
        byte[] Data = new byte[0];
        public OggHandler(Album Album) : base(Album) {}

        public override byte[] AdjustIndex() {
            return Data;
        }

        public override byte[] AdjustSuffix() {
            return new byte[0];
        }

        public override Bitmap ConvertToBitmap(Sprite entity) {
            return null;
        }

        public override byte[] ConvertToByte(Sprite entity) => new byte[0];

        public override void CreateFromStream(Stream stream) {
            stream.Read((int)Album.Info_Length,out Data);
            Album.Data = Data;
        }
        
    }
}
