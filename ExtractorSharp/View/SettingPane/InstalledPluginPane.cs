using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.Composition;
using ExtractorSharp.Component;
using ExtractorSharp.Core;
using System.IO;

namespace ExtractorSharp.View.SettingPane {
    public partial class InstalledPluginPane : AbstractSettingPane {
        private Hoster Hoster => Program.Hoster;
        public InstalledPluginPane(IConnector Connector) : base(Connector) {
            InitializeComponent();
            Flush();
            browseButton.Click += BrowsePlugin;
        }

        public void Flush() {
            list.Items.Clear();
            foreach (var plugin in Hoster.List.Values) {
                list.Items.Add(new PluginListItem(plugin));
            }
        }

        private void BrowsePlugin(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                var dir=dialog.SelectedPath;
                if (Hoster.Install(dir)) {
                    Connector.SendSuccess("PluginInstalled");
                    Flush();
                } else {
                    Connector.SendError("PluginInstallError");
                }
            }
        }

     


        public override void Initialize() {

        }
        public override void Save() {
        }

        private class PluginListItem : ListViewItem {
            public Plugin Plugin { get; }
            public PluginListItem(Plugin Plugin) {
                this.Plugin = Plugin;
                this.Text = Plugin.Name;
            }
        }

    }
}
