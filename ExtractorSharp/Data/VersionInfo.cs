using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Data {
    class VersionInfo {
        public string Name;
        public string Time;
        public string[] Info;

        public override string ToString() {
            var buf = new StringBuilder();
            buf.Append(Name);
            buf.AppendLine();
            if (Time != null) {
                buf.AppendLine(Time);
            }
            foreach (var inf in Info) {
                buf.AppendLine(inf);
            }
            return buf.ToString();
        }

    }
}
