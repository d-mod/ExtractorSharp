using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Exceptions {
    class FileSupportException :ApplicationException{
        public FileSupportException() { }

        public FileSupportException(string msg) : base(msg) { }
    }
}
