using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.Composition {
    interface ITools {
        string Name { get; }
        Control View { get; }
    }
}
