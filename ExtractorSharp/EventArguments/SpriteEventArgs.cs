using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.EventArguments {
    class SpriteEventArgs :EventArgs{
        public Sprite Entity { set; get; }
        public Album Album { set; get; }
    }
}
