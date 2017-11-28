using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Update {
    class VersionInfo {
        public string Version { set; get; } 
        public string Time { set; get; }
        public string[] Info { set; get; }
        public FileInfo[] File { set; get; }

        public override string ToString() {
            var buf = new StringBuilder();
            buf.Append(Version);
            buf.AppendLine();
            if (Time != null)
                buf.AppendLine(Time);
            foreach (var inf in Info) {
                buf.AppendLine(inf);
            }
            return buf.ToString();
        }

    }
}
