using ExtractorSharp.Data;
using System;
using System.Drawing;
using System.IO;

namespace ExtractorSharp.Handle {
    class OggHandler : Handler {
        byte[] Data;
        public OggHandler(Album Album) : base(Album) {}

        public override byte[] AdjustIndex() {
            return Data;
        }

        public override byte[] AdjustSuffix() {
            return new byte[0];
        }

        public override Bitmap ConvertToBitmap(ImageEntity entity) {
            return null;
        }

        public override byte[] ConvertToByte(ImageEntity entity) {
            return null;
        }

        public override void CreateFromStream(Stream stream) {
            Data = new byte[Album.Info_Length];
            stream.Read(Data);
            Album.Data = Data;
        }
        
    }
}
