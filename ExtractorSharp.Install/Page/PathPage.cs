using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using ExtractorSharp.Core.Config;

namespace ExtractorSharp.Install {
    public partial class PathPage : PagePanel {
        public PathPage(Dictionary<string,ConfigValue> Conifg) : base(Conifg) {
            InitializeComponent();
            browseButton.Click += Browse;
            textBox1.Click += Browse;
        }

        private void Browse(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                var path = dialog.SelectedPath;
                if (!File.Exists($"{path}\\{Config["ProjectName"]}.exe")) {
                    if (!path.EndsWith(Config["ProjectName"].Value)) {
                        path = string.Concat(path, $"\\{Config["ProjectName"]}");
                    }
                }
                textBox1.Text = path;
                textBox1.SelectionStart = path.Length;
                Config["InstallPath"] = new ConfigValue(path);
                Config["ApplicationPath"] = new ConfigValue($"{path}\\{Config["ProjectName"]}.exe".Resolve());
            }
        }



    }
}
