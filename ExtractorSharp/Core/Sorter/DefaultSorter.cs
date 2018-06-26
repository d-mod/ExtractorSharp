using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Sorter {
    internal class DefaultSorter : ISorter {
        public Dictionary<string, int> Dictionary => Data as Dictionary<string, int>;
        public string Name { set; get; } = "DefaultSorter";

        public int Comparer(Album a1, Album a2) {
            var index1 = IndexOf(a1.Name);
            var index2 = IndexOf(a2.Name);
            if (index1 == index2) return 0;
            if (index1 < index2) return 1;
            return -1;
        }

        public object Data { set; get; }

        public Type Type { get; } = typeof(Dictionary<string, int>);

        /// <summary>
        ///     获得拼合序号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int IndexOf(string name) {
            name = name.Substring(name.IndexOf("_") + 1);
            name = name.Replace(".img", ""); //去除.img后缀
            var regex = new Regex("\\d+");
            var matches = regex.Matches(name);
            var suf = 0;
            for (var i = 0; i < matches.Count; i++) {
                //移除数字序号
                name = name.Replace(matches[i].Value, "");
                if (i != 0) suf = int.Parse(matches[i].Value);
            }

            if (Dictionary.ContainsKey(name)) return Dictionary[name] + suf;
            return -1;
        }
    }
}