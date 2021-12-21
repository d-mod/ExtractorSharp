using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Config;

namespace ExtractorSharp.View.SettingPane {
    public partial class MarketplacePane : Panel {

        [Import]
        public IConfig Config;
        public MarketplacePane() {
            InitializeComponent();
            refreshButton.Click += Refresh;
            installButton.Click += Install;
        }


        public void Refresh(object sender, EventArgs e) {
            //Hoster.Refresh();
            list.Clear();
            // foreach (var data in Hoster.NetList) {
            //    list.Items.Add(new MetadataListItem(data));
            //}
        }


        public void Install(object sender, EventArgs e) {
            var array = list.SelectedItems;
            /*            if (array.Count > 0) {
                            var item = array[0] as MetadataListItem;
                            Guid.TryParse(item.Metadata.Guid, out var guid);
                            // Hoster.Download(guid);
                        }*/
        }


        public void Initialize() { }
        public void Save() { }


    }
}