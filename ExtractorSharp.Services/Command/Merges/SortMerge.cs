using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Commands {


    [ExportCommand("SortMerge")]
    internal class SortMerge : InjectService, ICommand {


        private Dictionary<string, int> Rules;

        public void Do(CommandContext context) {
            this.Store.Get("/merge/rules", out this.Rules)
                .Use<List<Album>>("/merge/queue", queues => {
                    queues.Sort(this.Comparer);
                    return queues;
                });
        }




        public int Comparer(Album a1, Album a2) {
            var index1 = this.IndexOf(a1);
            var index2 = this.IndexOf(a2);
            if(index1 == index2) {
                return 0;
            }
            if(index1 < index2) {
                return 1;
            }
            return -1;
        }


        /// <summary>
        ///     获得拼合序号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int IndexOf(Album album) {
            var name = album.Name;
            name = name.Substring(name.IndexOf("_") + 1);
            name = name.Replace(".img", ""); //去除.img后缀
            var regex = new Regex("\\d+");
            var matches = regex.Matches(name);
            var suf = 0;
            for(var i = 0; i < matches.Count; i++) {
                //移除数字序号
                name = name.Replace(matches[i].Value, "");
                if(i != 0) {
                    suf = int.Parse(matches[i].Value);
                }
            }
            if(this.Rules.ContainsKey(name)) {
                return this.Rules[name] + suf;
            }
            return -1;
        }

    }
}
