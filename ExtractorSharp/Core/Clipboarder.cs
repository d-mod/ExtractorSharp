using System;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core {
    /// <summary>
    ///     剪切板
    /// </summary>
    public class Clipboarder {
        private Clipboarder() { }
        public Album Album { private set; get; }
        public Album[] Albums { private set; get; }
        public int[] Indexes { private set; get; }
        public ClipMode Mode { private set; get; }
        public DateTime Time { private set; get; }
        public static Clipboarder Default { set; get; }

        public static Clipboarder CreateClipboarder(Album album, int[] indexes, ClipMode mode) {
            return new Clipboarder {
                Album = album,
                Indexes = indexes,
                Mode = mode
            };
        }

        public static Clipboarder CreateClipboarder(Album[] array, int[] indexes, ClipMode mode) {
            return new Clipboarder {
                Albums = array,
                Indexes = indexes,
                Mode = mode
            };
        }

        public static void Clear() {
            Default = null;
        }
    }

    public enum ClipMode {
        Cut,
        Copy
    }
}