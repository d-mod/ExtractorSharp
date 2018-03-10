using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Exceptions {
    abstract class ProgramException:Exception {
        public ProgramException():base(){

        }
        public ProgramException(string msg) : base(msg) {

        }
    }
}
