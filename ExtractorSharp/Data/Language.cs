using ExtractorSharp.Loose;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ExtractorSharp.Data { 
    /// <summary>
    /// 语言
    /// </summary>
    public class Language {

        public Dictionary<string, Dictionary<string, string>> Group { set; get; } = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 语言名
        /// </summary>
        public string Name { get; set; } = "English";
        /// <summary>
        /// 语言ID
        /// </summary>
        public int LCID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key] {
            get => this["Dictionary", key];
            set => this["Dictionary", key] = value;
        }

        public string this[string group, string key] {
            get {
                if (Group.ContainsKey(group) && Group[group].ContainsKey(key)) {
                    return Group[group][key];
                }
                return key;
            }
            set {
                if (!Group.ContainsKey(group)) {
                    Group.Add(group, new Dictionary<string, string>());
                }
                Group[group][key] = value;
            }
        }

        /// <summary>
        /// 默认类
        /// </summary>
        public static Language Default { set; get; } = new Language();

        private Language() { }

        public bool Equals(Language Lan) => LCID == Lan.LCID;//根据LCID判断唯一

        public override string ToString() => Name;

        public static Language CreateFromJson(string json) {
            var builder = new LSBuilder();
            var obj = builder.ReadJson(json);
            var lan = new Language();
            obj.GetValue(ref lan);
            return lan;
        }


        /// <summary>
        /// 从指定路径获得语言集
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Language CreateFromFile(string path) {
            var reader = new LSBuilder();
            var obj = reader.Read(path);
            var lan = new Language();
            obj.GetValue(ref lan);
            return lan;
        }
    }
}
