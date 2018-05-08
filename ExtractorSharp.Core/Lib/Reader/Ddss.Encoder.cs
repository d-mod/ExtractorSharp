using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Core.Lib {
    public partial class Ddss {

		public static byte[] Encode(byte[] data,Size size) {
            var mipmap = new Mipmap() {
                Data = data,
                Width = size.Width,
                Height = size.Height
            };
            var texture = new Texture();
            texture.Mipmaps = new Mipmap[1] { mipmap };
            var ms = new MemoryStream();


            return new byte[0];
        }
    }
}
