using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System.Drawing;
using System.IO;

namespace ExtractorSharp.Handle {
    /// <summary>
    /// 其他类型文件的处理
    /// </summary>
    public class OtherHandler : Handler {
        byte[] Data = new byte[0];
        public OtherHandler(Album Album) : base(Album) {}

        public override byte[] AdjustData() {
            return Data;
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
