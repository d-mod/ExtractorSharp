using System;

namespace ExtractorSharp.Exceptions {
    internal class UpdateException : ApplicationException {
        public UpdateException() { }

        public UpdateException(string msg) : base(msg) { }
    }
}