using System.IO;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Handle {
    /// <summary>
    ///     其他类型文件的处理
    /// </summary>
    public class OtherHandler : Handler {
        private byte[] _data = new byte[0];
        public OtherHandler(Album album) : base(album) { }

        public override byte[] AdjustData() {
            return this._data;
        }



        public override byte[] ConvertToByte(Sprite entity) {
            return new byte[0];
        }

        public override void CreateFromStream(Stream stream) {
            stream.Read((int)this.Album.IndexLength, out this._data);
            this.Album.Data = this._data;
        }

        public override ImageData GetImageData(Sprite sprite) {
            return null;
        }
    }
}