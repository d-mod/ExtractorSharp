using System;

namespace ExtractorSharp.Exceptions {
    internal abstract class ApplicationException : Exception {
        public ApplicationException() { }

        public ApplicationException(string msg) : base(msg) { }
    }
}