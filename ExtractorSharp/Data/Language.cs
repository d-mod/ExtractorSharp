using ExtractorSharp.Loose;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ExtractorSharp.Data { 
    /// <summary>
    /// 语言
    /// </summary>
    public class Language {

        private Dictionary<string, string> Dictionary;
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
            get {
             if (Dictionary.ContainsKey(key))
                    return Dictionary[key];
                return key;
            }
            private set {
                if (Dictionary.ContainsKey(key))
                    Dictionary.Remove(key);
                Dictionary.Add(key, value);
            }
        }

        /// <summary>
        /// 默认类
        /// </summary>
        public static Language Default { set; get; } = new Language();

        private Language() { }

        public bool Equals(Language Lan) => LCID == Lan.LCID;//根据LCID判断唯一

        public override string ToString() => Name;

        /// <summary>
        /// 从xml对象获得语言集
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static Language CreateFromXml(XmlDocument doc) {
            var lan = new Language();
            var root = doc.DocumentElement;
            var list = root.ChildNodes;
            lan.Name = root.GetAttribute("Name");
            int.TryParse(root.GetAttribute("LCID"), out int lcid);//LCID,表示地区标识符
            lan.LCID = lcid;
            foreach (XmlNode item in list)
                if (item.NodeType != XmlNodeType.Comment)//过滤注释
                    lan[item.Name] = item.InnerText;//根据标签名和标签内容得到key-value对
            return lan;
        }

        public static Language CreateFromJson(string json) {
            var builder = new LSBuilder();
            var obj = builder.ReadJson(json);
            var lan = new Language();
            obj.GetValue(ref lan);
            return lan;
        }
         
        /// <summary>
        /// 从字符串获得语言集
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Language CreateFromXml(string str) {
            var doc = new XmlDocument();
            doc.LoadXml(str);
            return CreateFromXml(doc);
        }

        /// <summary>
        /// 从流获得语言集
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Language CreateFromXml(Stream stream) {
            var doc = new XmlDocument();
            doc.Load(stream);
            return CreateFromXml(doc);
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
