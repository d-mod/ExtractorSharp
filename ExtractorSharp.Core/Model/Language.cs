using System.Collections.Generic;
using System.IO;
using ExtractorSharp.Json;

namespace ExtractorSharp.Core.Model {
    /// <summary>
    ///     语言
    /// </summary>
    public class Language {
        private static Language _default;


        private Language() { }

        public Dictionary<string, Dictionary<string, string>> Group { set; get; } =
            new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        ///     语言名
        /// </summary>
        public string Name { get; set; } = "English";

        /// <summary>
        ///     语言ID
        /// </summary>
        public int Lcid { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key] {
            get => this["Dictionary", key];
            set => this["Dictionary", key] = value;
        }

        public string this[string group, string key] {
            get {
                if (group == null || key == null) {
                    return "";
                }
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
        ///     默认类
        /// </summary>
        public static Language Default {
            set => _default = value;
            get {
                if (_default != null) {
                    return _default;
                }
                _default = List.Find(e => e.Lcid == LocalLcid);
                if (_default != null) {
                    return _default;
                }
                return new Language();
            }
        }

        public static int LocalLcid { set; get; }


        public static List<Language> List { set; get; } = new List<Language>();


        public static void CreateFromDir(string dir) {
            if (Directory.Exists(dir)) {
                foreach (var file in Directory.GetFiles(dir, "*.json")) {
                    var lan = CreateFromFile(file);
                    var cur = List.Find(e => e.Lcid == lan.Lcid);
                    if (cur != null) {
                        cur.CopyFrom(lan);
                    } else {
                        List.Add(lan);
                    }
                }
            }
        }

        public bool Equals(Language lan) {
            return Lcid == lan.Lcid;
        }

        public override string ToString() {
            return Name;
        }

        public void CopyFrom(Language lan) {
            foreach (var group in lan.Group.Keys) {
                Group[group] = Group.ContainsKey(group) ? Group[group] : new Dictionary<string, string>();
                foreach (var key in lan.Group[group].Keys) {
                    Group[group][key] = lan.Group[group][key];
                }
            }
        }

        public static Language CreateFromJson(string json) {
            var builder = new LSBuilder();
            var obj = builder.ReadJson(json);
            var lan = new Language();
            obj.GetValue(ref lan);
            return lan;
        }


        /// <summary>
        ///     从指定路径获得语言集
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