using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.EventArguments {
    class ColorEventArgs {
        public Color OldColor { set; get; }
        public Color NewColor { set; get; }
    }
}
