using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Composition.Menu;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Menu {

    [Export(typeof(IMenuItem))]
    internal class OpenRecentMenuItem : InjectService, IChildrenItem {

        public string Key { set; get; } = "File/_OPEN[0]/OpenRecents";

        public List<IMenuItem> Children { set; get; } = new List<IMenuItem>();

        public string ToolTip { set; get; }

        public int Order { set; get; } = 3;

        public string Group { set; get; }

        public bool IsTile { set; get; }

        public void OnRecentsChanged() {
            this.Store.Get(StoreKeys.RECENTS, out var recents, new List<string>());
            recents = recents.FindAll(e => e.ToLower().EndsWith(".npk"));
            recents = recents.GetRange(0, Math.Min(10, recents.Count));
            this.Children = recents.Select(r => {
                void OpenRecent(object sender, EventArgs e) {
                    this.Controller.Do("OpenFile", new[] { r});
                }
                var item = new DefaultRouteItem(r.GetSuffix(), OpenRecent) {
                    Command = $"Open-{r}",
                    ToolTip = r
                };
                return item as IMenuItem;
            }).ToList();
        }




        public override void OnImportsSatisfied() {
            base.OnImportsSatisfied();
            this.OnRecentsChanged();
        }

    }


}
