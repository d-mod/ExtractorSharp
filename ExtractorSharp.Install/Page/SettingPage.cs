using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.Core.Config;
using System.IO;

namespace ExtractorSharp.Install.Page {
    public partial class SettingPage : PagePanel {
        public SettingPage(Dictionary<string, ConfigValue> Dictionary) : base(Dictionary) {
            InitializeComponent();
            browseButton.Click += Browse;
            textBox1.Click += Browse;
        }

        private void Browse(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                var path = dialog.SelectedPath;
                if (!Directory.Exists($"{path}\\ImagePacks2")) {
                    tipsLabel.Text = "设置失败,无效的游戏目录";
                    tipsLabel.ForeColor = Color.Red;
                    return;
                }
                tipsLabel.Text = "设置成功";
                tipsLabel.ForeColor = Color.Green;
                textBox1.Text = path;
                textBox1.SelectionStart = path.Length;
                Config["GamePath"] = new ConfigValue(path);
            }
        }

        public override void Next() {
            base.Next();
        }

    }
}
