using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Composition.Stores;

namespace ExtractorSharp.Services {
    internal class StoreConfig : JsonConfig, IStoreFilter {

        private Store Store;

        public StoreConfig(string ConfigDirectory, Store Store) : base(ConfigDirectory) {
            this.Store = Store;
            Store.Register("/config/save", this.Save)
                .Register("/config/reset", this.Reset)
                .Register("/config/reload", this.Reload)
                ;
            Store.AddFilter(this);
        }

        private string FormatKey(string key) {
            var arr = key.Replace("/config/data/","").Split("-");
            arr = arr.Select(e => {
                return string.Concat(e.Substring(0, 1).ToUpper(),e.Substring(1));
            }).ToArray();
            return string.Join("", arr);
        }

        public void Set(string key, object value) {
            key = this.FormatKey(key);
            this[key] = new ConfigValue(value);
        }

        public object Get(string key) {
            key = this.FormatKey(key);
            return this[key].Object;
        }

        public bool IsMatch(string key) {
            return key.StartsWith("/config/data/");
        }
    }
}
