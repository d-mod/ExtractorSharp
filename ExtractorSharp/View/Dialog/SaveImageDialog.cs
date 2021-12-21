using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.View.Dialog {

    [Export(typeof(IView))]
    [ExportMetadata("Name","SaveImage")]
    public partial class SaveImageDialog : BaseDialog, IPartImportsSatisfiedNotification {


        public SaveImageDialog() {
        }

        public override object ShowView(params object[] args) {
            pathBox.Text = Config["SaveImagePath"].Value;
            fullPathCheck.Checked = Config["SaveImageFullPath"].Boolean;
            allImagesCheck.Checked = Config["SaveAllImage"].Boolean;
            if(Config["SaveImageTip"].Boolean) {
                return ShowDialog();
            }
            return Save();
        }

        private DialogResult Save() {
            ///TODO
            var file = Store.Get<Album>(StoreKeys.SELECTED_FILE);//Connector.SelectedFile;
            var indices = Store.Get<int[]>(StoreKeys.SELECTED_IMAGE_INDICES);//Connector.CheckedImageIndices;
            if(file == null || indices.Length == 0) {
                return DialogResult.Cancel;
            }

            if(allImagesCheck.Checked) {
                indices = new int[0];
            }
            var name = nameBox.Text;
            var match = Regex.Match(name, @"\d+$", RegexOptions.Compiled);
            var value = match.Value;
            var incre = -1;
            var prefix = name;
            var digit = 0;
            if(match.Success) {
                incre = int.Parse(value);
                prefix = prefix.Remove(match.Index, match.Length);
                digit = value.Length;
            }
            Controller.Do("SaveImage", new CommandContext(file) {
                { "Mode", 1 },
                { "Indices",indices },
                { "Path" ,pathBox.Text},
                { "Prefix",prefix },
                { "Increment",incre },
                { "Digit", digit},
                { "FullPath",fullPathCheck.Checked },
                { "AllImage",allImagesCheck.Checked }
            });
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
            if(dialog.ShowDialog() == DialogResult.OK) {
                pathBox.Text = dialog.SelectedPath;
            }
        }

        public void OnImportsSatisfied() {
            InitializeComponent();
            pathBox.Click += LoadPath;
            loadButton.Click += LoadPath;
            yesButton.Click += Replace;
            CancelButton = cancelButton;
        }
    }
}