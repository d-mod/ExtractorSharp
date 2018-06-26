using System.Drawing;
using System.IO;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Handle {
    /// <summary>
    ///     其他类型文件的处理
    /// </summary>
    public class OtherHandler : Handler {
        private byte[] _data = new byte[0];
        public OtherHandler(Album album) : base(album) { }

        public override byte[] AdjustData() {
            return _data;
        }


        public override Bitmap ConvertToBitmap(Sprite entity) {
            return null;
        }

        public override byte[] ConvertToByte(Sprite entity) {
            return new byte[0];
        }

        public override void CreateFromStream(Stream stream) {
            stream.Read((int) Album.IndexLength, out _data);
            Album.Data = _data;
        }
    }
}