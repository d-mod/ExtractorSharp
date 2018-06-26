using System;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Composition;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Composition;

namespace ExtractorSharp.View.SettingPane {
    public partial class MarketplacePane : AbstractSettingPane {
        public MarketplacePane(IConnector Connector) : base(Connector) {
            InitializeComponent();
            refreshButton.Click += Refresh;
            installButton.Click += Install;
        }

        private Hoster Hoster => Program.Hoster;

        public void Refresh(object sender, EventArgs e) {
            Hoster.Refresh();
            list.Clear();
            foreach (var data in Hoster.NetList) list.Items.Add(new MetadataListItem(data));
        }


        public void Install(object sender, EventArgs e) {
            var array = list.SelectedItems;
            if (array.Count > 0) {
                var item = array[0] as MetadataListItem;
                Guid.TryParse(item.Metadata.Guid, out var guid);
                Hoster.Download(guid);
            }
        }


        public override void Initialize() { }
        public override void Save() { }

        private class MetadataListItem : ListViewItem {
            public MetadataListItem(Metadata Metadata) {
                this.Metadata = Metadata;
                Text = Metadata.Name;
            }

            public Metadata Metadata { get; }
        }
    }
}