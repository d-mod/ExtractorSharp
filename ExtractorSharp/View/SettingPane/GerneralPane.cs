using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.SettingPane {

    [Export(typeof(ISetting))]
    [ExportMetadata("Index", 0)]
    [ExportMetadata("Parent", "Gerneral")]
    public partial class GerneralPane : Panel, ISetting, IPartImportsSatisfiedNotification {

        [Import]
        public IConfig Config;

        [Import]
        public Language Language;

        [Import]
        private Messager Messager;

        public object View => this;

        public void OnImportsSatisfied() {
            InitializeComponent();
            gamePathBox.Click += Browse;
            browseButton.Click += Browse;
        }

        private void Browse(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if(dialog.ShowDialog() == DialogResult.OK) {
                var rsPath = $"{dialog.SelectedPath}/{NpkCoder.IMAGE_DIR}";
                if(Directory.Exists(rsPath)) {
                    gamePathBox.Text = dialog.SelectedPath;
                } else {
                    Messager.Error("SelectPathIsInvalid");
                }
            }
        }


        public void Initialize() {
            gamePathBox.Text = Config["GamePath"].Value;
            autoSaveCheck.Checked = Config["AutoSave"].Boolean;
            autoUpdateCheck.Checked = Config["AutoCheckUpdate"].Boolean;
            updatedShowFeatureCheck.Checked = Config["ShowFeature"].Boolean;
        }

        public void Save() {
            Config["GamePath"] = new ConfigValue(gamePathBox.Text);
            Config["ResourcePath"] = new ConfigValue($"{gamePathBox.Text}\\{NpkCoder.IMAGE_DIR}");
            Config["AutoSave"] = new ConfigValue(autoSaveCheck.Checked);
            Config["AutoCheckUpdate"] = new ConfigValue(autoUpdateCheck.Checked);
            Config["ShowFeature"] = new ConfigValue(updatedShowFeatureCheck.Checked);
        }
    }
}