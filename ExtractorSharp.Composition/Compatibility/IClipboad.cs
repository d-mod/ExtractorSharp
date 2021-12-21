using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Composition.Compatibility {

    public enum ClipMode {
        CUT,
        COPY
    }

    public class Clipboader {


    }

    public interface IClipboad {

        StringCollection GetFileDropList();

        void SetFileDropList(StringCollection list);

        string GetText();

        void SetText(string text);

        void Clear();

    }
}
