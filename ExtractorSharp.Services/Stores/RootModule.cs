using System.Collections.Generic;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Composition.Stores;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Stores {
    internal class RootModule : IPartImportsSatisfiedNotification {

        [Import]
        private Store Store { set; get; }

        [StoreBinding("/data/files")]
        public List<Album> Files { set; get; } = new List<Album>();


        [StoreBinding("/merge/queues")]
        public List<Album> Queues { set; get; } = new List<Album>();

        [StoreBinding("/data/is-save")]
        public bool isSave { set; get; } = true;

        public void OnImportsSatisfied() {
            this.Store.Subscribe(this);
        }
    }
}
