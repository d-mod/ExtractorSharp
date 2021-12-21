using System.Runtime.InteropServices;

namespace ExtractorSharp.Core.Lib {
    public static class Windows {

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

    }
}
