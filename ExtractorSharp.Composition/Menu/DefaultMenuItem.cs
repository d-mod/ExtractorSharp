using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtractorSharp.Composition.Menu {

    public class DefaultMenuItem : IChildrenItem {

        public DefaultMenuItem() {
        }

        public DefaultMenuItem(string key) {
            this.Key = key;
        }

        public DefaultMenuItem(IMenuItem item) {
            this.Order = item.Order;
            this.ToolTip = item.ToolTip;
            this.Key = item.Key.GetSuffix();
            if(item is IChildrenItem child) {
                this.Children = child.Children?.Select(e => e.Clone())?.ToList() ?? new List<IMenuItem>();
                this.IsTile = child.IsTile || this.IsTile;
            }
        }

        private string ParseKey(string key) {
            if(key.StartsWith("_")) {
                this.IsTile = true;
                key = key.Substring(1);
            }
            var match = Regex.Match(key, @"\[\-?\d+\]$");
            if(match.Success) {
                var value = match.Value;
                var order = int.Parse(value.Substring(1, value.Length - 2));
                this.Order = order;
                key = key.Replace(match.Value, "");
            }
            return key;
        }

        private string _key;

        public string Key {
            set => this._key = this.ParseKey(value);
            get => this._key;
        }

        public List<IMenuItem> Children { set; get; } = new List<IMenuItem>();

        public string ToolTip { set; get; }

        public int Order { set; get; } = -1;

        public bool IsTile { set; get; }
    }


}
