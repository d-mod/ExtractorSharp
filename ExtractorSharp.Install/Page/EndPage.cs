using ExtractorSharp.Core.Config;
using System.Collections.Generic;

namespace ExtractorSharp.Install.Page {
    public partial class EndPage : PagePanel {
        public EndPage(Dictionary<string,ConfigValue> Config) : base(Config) {
            InitializeComponent();
            check.CheckedChanged += (o, e) => Config["RunApplication"] = new ConfigValue(check.Checked);
        }
    }
}
