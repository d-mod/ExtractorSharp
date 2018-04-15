using ExtractorSharp.Data;
using System.Drawing;

namespace ExtractorSharp.Composition {
    public delegate void SpriteEffect(Sprite sprite,ref Bitmap bmp);

    public interface IEffect {
        string Name { get; }
        bool Enable { set; get; }
        int Index { set; get; }
        void Handle(Sprite sprite, ref Bitmap bmp);
    }
}
