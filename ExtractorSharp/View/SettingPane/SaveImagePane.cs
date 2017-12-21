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
using ExtractorSharp.Config;
using ExtractorSharp.Core;

namespace ExtractorSharp.View.SettingPane {
    public partial class SaveImagePane : AbstractSettingPane {
        public SaveImagePane(IConnector Data) :base(Data){
            InitializeComponent();
            savePathBox.Click += Browse;
            browseButton.Click += Browse;
            emptyButton.Click += (o, e) => savePathBox.Clear();
        }

        private void Browse(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                savePathBox.Text = dialog.SelectedPath;
            }
        }

        public override void Initialize() {
            promptCheck.Checked = Config["SaveImageTip"].Boolean;
            fullPathCheck.Checked = Config["SaveImageAllPath"].Boolean;
            savePathBox.Text = Config["SaveImagePath"].Value;
        }

        public override void Save() {
            Config["SaveImageTip"] = new ConfigValue(promptCheck.Checked);
            Config["SaveImageAllPath"] = new ConfigValue(fullPathCheck.Checked);
            Config["SaveImagePath"] = new ConfigValue(savePathBox.Text);
        }
    }
}
