using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Exceptions {
    class UpdateException: ProgramException {
        public UpdateException() {

        }
        public UpdateException(string msg) : base(msg) {

        }
    }
}
