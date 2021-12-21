using System.Text;

namespace ExtractorSharp.Model {
    internal class VersionInfo {
        public string[] Info;
        public string Name;
        public string Time;

        public override string ToString() {
            var buf = new StringBuilder();
            buf.Append(Name);
            buf.AppendLine();
            if(Time != null) {
                buf.AppendLine(Time);
            }
            foreach(var inf in Info) {
                buf.AppendLine(inf);
            }
            return buf.ToString();
        }
    }
}