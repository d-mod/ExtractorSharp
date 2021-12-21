using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;

namespace ExtractorSharp.View.SettingPane {
    public partial class InstalledPluginPane : Panel {

        [Import]
        private Messager Messager;

        public InstalledPluginPane() {
            InitializeComponent();
            Flush();
            browseButton.Click += BrowsePlugin;
        }


        public void Flush() {
            list.Items.Clear();
            //  foreach (var plugin in Hoster.List.Values) {
            //     list.Items.Add(new PluginListItem(plugin));
            //  }
        }

        private void BrowsePlugin(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if(dialog.ShowDialog() == DialogResult.OK) {
                var dir = dialog.SelectedPath;
                Messager.Success("PluginInstalled");
                Flush();
            }
        }


        public void Initialize() { }

        public void Save() { }


    }
}