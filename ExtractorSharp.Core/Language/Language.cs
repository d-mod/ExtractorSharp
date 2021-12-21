using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ExtractorSharp.Json;

namespace ExtractorSharp.Core {
    /// <summary>
    ///     语言
    /// </summary>

    public class Language {


        protected Language() { }

        private readonly Regex Interpolation = new Regex(@"<[\w\s\d_]+>");

        public static Language Current { private set; get; } = Empty;

        /// <summary>
        /// 空语言集
        /// </summary>
        public static readonly Language Empty = new Language();

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

        public string this[object obj] {
            get {
                if(obj is string s) {
                    return this[s];
                }
                return this.Parse(obj?.ToString());
            }
        }

        public string this[string group, string key] {
            get {
                if(string.IsNullOrEmpty(group) || string.IsNullOrEmpty(key)) {
                    return "";
                }
                if(this.Group.ContainsKey(group) && this.Group[group].ContainsKey(key)) {
                    return this.Group[group][key];
                }
                if(this.Interpolation.IsMatch(key)) {
                    return this.Parse(key);
                }
                return key;
            }
            set {
                if(!this.Group.ContainsKey(group)) {
                    this.Group.Add(group, new Dictionary<string, string>());
                }
                this.Group[group][key] = value;
            }
        }



        public bool Equals(Language lan) {
            return this.Lcid == lan.Lcid;
        }

        public override string ToString() {
            return this.Name;
        }

        public string Parse(string content) {
            if(content != null) {
                var matches = this.Interpolation.Matches(content);
                for(var i = 0; i < matches.Count; i++) {
                    var match = matches[i];
                    if(match.Success) {
                        var value = match.Value;
                        var key = value.Substring(1, value.Length - 2);
                        content = content.Replace(value, this[key]);
                    }
                }
            }
            return content;
        }

        public void CopyFrom(Language lan) {
            foreach(var group in lan.Group.Keys) {
                this.Group[group] = this.Group.ContainsKey(group) ? this.Group[group] : new Dictionary<string, string>();
                foreach(var key in lan.Group[group].Keys) {
                    this.Group[group][key] = lan.Group[group][key];
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

        public static Language CreateCurrent(string json) {
            Current = CreateFromJson(json);
            return Current;
        }
    }
}