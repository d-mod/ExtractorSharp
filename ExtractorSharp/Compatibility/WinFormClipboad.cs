using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.Composition.Compatibility;

namespace ExtractorSharp.Compatibility {

    [Export(typeof(IClipboad))]
    class WinFormClipboad :IClipboad{

        public StringCollection GetFileDropList() {
            return Clipboard.GetFileDropList();
        }

        public void SetFileDropList(StringCollection list) {
            Clipboard.SetFileDropList(list);
        }

        public string GetText() {
            return Clipboard.GetText();
        }

        public void SetText(string text) {
            Clipboard.SetText(text);
        }

        public void Clear() {
            Clipboard.Clear();
        }
    }
}
