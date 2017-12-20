using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.Component;
using System.IO;
using ExtractorSharp.Config;

namespace ExtractorSharp.View.SettingPane {
    public partial class GerneralPane : AbstractSettingPane {

        public GerneralPane(ICommandData Data):base(Data){
            InitializeComponent();
        }

        private void Browse(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                var rsPath = $"{dialog.SelectedPath}/ImagePacks2";
                if (Directory.Exists(rsPath)) {
                    gamePathBox.Text = dialog.SelectedPath;
                    Config["GamePath"] = new ConfigValue(dialog.SelectedPath);
                    Config["ResourcePath"] = new ConfigValue($"{dialog.SelectedPath}/ImagePacks2");
                }
            }
        }


        public override void Initialize() {
            gamePathBox.Text = Config["GamePath"].Value;
        }

        public override void Save() {
            Config["GamePath"] = new ConfigValue(gamePathBox.Text);
        }
    }
}
