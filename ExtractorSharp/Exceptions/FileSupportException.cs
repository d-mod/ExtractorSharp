using System;

namespace ExtractorSharp.Exceptions {
    class FileSupportException : ApplicationException {
        public FileSupportException() { }

        public FileSupportException(string msg) : base(msg) { }
    }
}
