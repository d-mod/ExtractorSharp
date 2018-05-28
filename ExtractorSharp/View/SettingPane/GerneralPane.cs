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
using ExtractorSharp.Core;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.View.SettingPane {
    public partial class GerneralPane : AbstractSettingPane {

        public GerneralPane(IConnector Data):base(Data){
            InitializeComponent();
            gamePathBox.Click += Browse;
            browseButton.Click += Browse;
        }

        private void Browse(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                var rsPath = $"{dialog.SelectedPath}/{Npks.IMAGE_DIR}";
                if (Directory.Exists(rsPath)) {
                    gamePathBox.Text = dialog.SelectedPath;
                } else {
                    Connector.SendError("SelectPathIsInvalid");
                }
            }
        }


        public override void Initialize() {
            gamePathBox.Text = Config["GamePath"].Value;
            autoSaveCheck.Checked = Config["AutoSave"].Boolean;
            autoUpdateCheck.Checked = Config["AutoUpdate"].Boolean;
        }

        public override void Save() {
            Config["GamePath"] = new ConfigValue(gamePathBox.Text);
            Config["ResourcePath"] = new ConfigValue($"{gamePathBox.Text}/{Npks.IMAGE_DIR}");
            Config["AutoSave"] = new ConfigValue(autoSaveCheck.Checked);
            Config["AutoUpdate"] = new ConfigValue(autoUpdateCheck.Checked);
        }
    }
}
