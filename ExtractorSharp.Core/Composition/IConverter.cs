using ExtractorSharp.Data;
using System.Drawing;

namespace ExtractorSharp.Composition {
    public delegate void SpriteConverter(Sprite sprite,ref Bitmap bmp);

    public interface ISpriteConverter {
        string Name { get; }
        bool Enable { set; get; }
        int Index { set; get; }
        void Convert(Sprite sprite, ref Bitmap bmp);
    }
}
