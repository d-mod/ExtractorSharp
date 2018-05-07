using ExtractorSharp.Component;
using ExtractorSharp.Composition;
using ExtractorSharp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.View.SettingPane {

    public partial class MarketplacePane : AbstractSettingPane {

        private Hoster Hoster => Program.Hoster;

        public MarketplacePane(IConnector Connector):base(Connector){
            InitializeComponent();
            this.refreshButton.Click += Refresh;
            this.installButton.Click += Install;
        }

        public void Refresh(object sender,EventArgs e) {
            Hoster.Refresh();
            list.Clear();
            foreach(var data in Hoster.NetList) {
                list.Items.Add(new MetadataListItem(data));
            }
        }


        public void Install(object sender, EventArgs e) {
            var array = list.SelectedItems;
            if (array.Count > 0) {
                var item = array[0] as MetadataListItem;
                Guid.TryParse(item.Metadata.Guid,out Guid guid);
                Hoster.Download(guid);
            }
        }

        private class MetadataListItem : ListViewItem {
            public Metadata Metadata { get; }
            public MetadataListItem(Metadata Metadata) {
                this.Metadata = Metadata;
                this.Text = Metadata.Name;
            }
        }


        public override void Initialize() { }
        public override void Save() { }
    }
}
