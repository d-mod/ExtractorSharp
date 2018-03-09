using System;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Config;
using ExtractorSharp.Data;
using ExtractorSharp.Core;
using System.Text.RegularExpressions;
using System.Drawing;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Composition;

namespace ExtractorSharp.View {
    public partial class SaveImageDialog : ESDialog {
        private int[] Indexes;
        private Album Album;
        public SaveImageDialog(IConnector Connector) : base(Connector) {
            InitializeComponent();
            pathBox.Click += LoadPath;
            loadButton.Click += LoadPath;
            yesButton.Click += Replace;
            CancelButton = cancelButton;
        }

        public override DialogResult Show(params object[] args) {
            pathBox.Text = Config["SaveImagePath"].Value;
            allPathCheck.Checked = Config["SaveImageAllPath"].Boolean;
            Album = args[0] as Album;
            Indexes = args[1] as int[];
            if (Config["SaveImageTip"].Boolean) {
                return ShowDialog();
            }
            Save();
            return DialogResult.None;
        }

        private void Save() {
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
            prefix = (allPathCheck.Checked ? Album.Path : Album.Name) + "/" + prefix;
            Connector.Do("saveImage", Album, 1, Indexes, pathBox.Text, prefix, incre, digit,Connector.SpirteConverter);
        }

        private void Replace(object sender, EventArgs e) {
            Config["SaveImagePath"] = new ConfigValue(pathBox.Text);
            Config["SaveImageTip"] = new ConfigValue(!tipsCheck.Checked);
            Config["SaveImageAllPath"] = new ConfigValue(allPathCheck.Checked);
            Config.Save();
            Save();
            DialogResult = DialogResult.OK;
        }

        public void LoadPath(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                pathBox.Text = dialog.SelectedPath;
            }   
        }


    }
}
