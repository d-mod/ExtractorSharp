using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.EventArguments {
    public class DialogDataEventArgs :EventArgs{
        /// <summary>
        /// 
        /// </summary>
        public string Key;
        public object Value;
    }
}
