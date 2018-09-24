using System;
using System.IO;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;

namespace ExtractorSharp.View.SettingPane {
    public partial class GerneralPane : AbstractSettingPane {
        public GerneralPane(IConnector Data) : base(Data) {
            InitializeComponent();
            gamePathBox.Click += Browse;
            browseButton.Click += Browse;
        }

        private void Browse(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                var rsPath = $"{dialog.SelectedPath}/{NpkCoder.IMAGE_DIR}";
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
            autoUpdateCheck.Checked = Config["AutoCheckUpdate"].Boolean;
            updatedShowFeatureCheck.Checked = Config["ShowFeature"].Boolean;
        }

        public override void Save() {
            Config["GamePath"] = new ConfigValue(gamePathBox.Text);
            Config["ResourcePath"] = new ConfigValue($"{gamePathBox.Text}\\{NpkCoder.IMAGE_DIR}");
            Config["AutoSave"] = new ConfigValue(autoSaveCheck.Checked);
            Config["AutoCheckUpdate"] = new ConfigValue(autoUpdateCheck.Checked);
            Config["ShowFeature"] = new ConfigValue(updatedShowFeatureCheck.Checked);
        }
    }
}