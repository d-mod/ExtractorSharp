using System;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Composition;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Composition;

namespace ExtractorSharp.View.SettingPane {
    public partial class InstalledPluginPane : AbstractSettingPane {
        public InstalledPluginPane(IConnector Connector) : base(Connector) {
            InitializeComponent();
            Flush();
            browseButton.Click += BrowsePlugin;
        }

        private Hoster Hoster => Program.Hoster;

        public void Flush() {
            list.Items.Clear();
            foreach (var plugin in Hoster.List.Values) {
                list.Items.Add(new PluginListItem(plugin));
            }
        }

        private void BrowsePlugin(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                var dir = dialog.SelectedPath;
                Connector.SendSuccess("PluginInstalled");
                Flush();
            }
        }


        public override void Initialize() { }

        public override void Save() { }

        private class PluginListItem : ListViewItem {
            public PluginListItem(Plugin Plugin) {
                this.Plugin = Plugin;
                Text = Plugin.Name;
            }
            public Plugin Plugin { get; }
        }
    }
}