
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.Composition.Compatibility;

namespace ExtractorSharp.Compatibility {

    [Export(typeof(ICommonMessageBox))]
    class WinFormMessageBox : ICommonMessageBox {
        public CommonMessageBoxResult Show(string title, string message, CommonMessageBoxButton button, CommonMessageBoxIcon icon) {
            return (CommonMessageBoxResult)MessageBox.Show(message, title, (MessageBoxButtons)button, (MessageBoxIcon)icon);
        }
    }
}
