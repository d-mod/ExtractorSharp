using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;

namespace ExtractorSharp.View.Dialog {
    public partial class SaveImageDialog : ESDialog {
        public SaveImageDialog(IConnector Connector) : base(Connector) {
            InitializeComponent();
            pathBox.Click += LoadPath;
            loadButton.Click += LoadPath;
            yesButton.Click += Replace;
            CancelButton = cancelButton;
        }

        public override DialogResult Show(params object[] args) {
            pathBox.Text = Config["SaveImagePath"].Value;
            fullPathCheck.Checked = Config["SaveImageFullPath"].Boolean;
            allImagesCheck.Checked = Config["SaveAllImage"].Boolean;
            if (Config["SaveImageTip"].Boolean) {
                return ShowDialog();
            }
            return Save();
        }

        private DialogResult Save() {
            var file = Connector.SelectedFile;
            var indices = Connector.CheckedImageIndices;
            if (file == null || indices.Length == 0) return DialogResult.Cancel;
            if (allImagesCheck.Checked) {
                indices = new int[0];
            }
            var name = nameBox.Text;
            var match = Regex.Match(name, @"\d+$", RegexOptions.Compiled);
            var value = match.Value;
            var incre = -1;
            var prefix = name;
            var digit = 0;
            if (match.Success) {
                incre = int.Parse(value);
                prefix = prefix.Remove(match.Index, match.Length);
                digit = value.Length;
            }
            Connector.Do("saveImage", file, 1, indices, pathBox.Text, prefix, incre, digit, fullPathCheck.Checked, Connector.Effect, allImagesCheck.Checked);
            return DialogResult.OK;
        }

        private void Replace(object sender, EventArgs e) {
            Config["SaveImagePath"] = new ConfigValue(pathBox.Text);
            Config["SaveImageTip"] = new ConfigValue(!tipsCheck.Checked);
            Config["SaveImageFullPath"] = new ConfigValue(fullPathCheck.Checked);
            Config["SaveAllImage"] = new ConfigValue(allImagesCheck.Checked);
            Config.Save();
            DialogResult = Save();
        }

        private void LoadPath(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                pathBox.Text = dialog.SelectedPath;
            }
        }
    }
}