using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ExtractorSharp.Composition.Compatibility;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Compatibility {

    [Export(typeof(IClipboad))]
    class WPFClipboad : IClipboad {

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
