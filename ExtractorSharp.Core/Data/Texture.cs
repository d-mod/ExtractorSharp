using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Data {

    public enum TextureFormat {
        RGB_S3TC_DXT1_Format = 33776,
        RGBA_S3TC_DXT3_Format = 33778,
        RGBA_S3TC_DXT5_Format = 33779,
    }

    public class Texture {
        public int Length { get;  set; }
        public int Flags { get;  set; }
        public int Width { get;  set; }
        public int Height { get;  set; }
        public int Count { set; get; } = 1;
        public TextureFormat Format { set; get; }
        public bool IsCubemap { get; internal set; }
        public Mipmap[] Mipmaps { get; internal set; }
    }

    public class Mipmap {
        public int Width { set; get; }
        public int Height { set; get; }
        public byte[] Data { set; get; }
    }
}
