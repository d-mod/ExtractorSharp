using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ExtractorSharp.Config {
    /// <summary>
    /// <para>设置集合</para>
    /// <para>设置分为系统设置和用户设置两种</para>
    /// <para>系统设置设定默认值，不可删改，可添加</para>
    /// <para>用户设置可修改</para>
    /// </summary>
    public class XmlConfig : IConfig {

        /// <summary>
        /// 系统设置
        /// </summary>
        private Dictionary<string, ConfigValue> OriginalConfig { get; set; }
        /// <summary>
        /// 用户设置
        /// </summary>
        private Dictionary<string, ConfigValue> UserConfig { get; set; }

        public string ConfigDir { set; get; }
        public string UserPath { set; get; }   //用户配置文件路径
        /// <summary>
        /// 根据对应的指定的key，获得对应的ConfigValue
        /// 当key前带有@时，则返回系统设置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ConfigValue this[string key] {
            get {
                if (key.StartsWith("@")) {
                    key = key.Substring(1);
                    if (OriginalConfig.ContainsKey(key))
                        return OriginalConfig[key];
                }
                if (UserConfig.ContainsKey(key)&&UserConfig[key].NotEmpty)
                    return UserConfig[key];
                if (OriginalConfig.ContainsKey(key))
                    return OriginalConfig[key];
                return ConfigValue.NullValue;
            }
            set {
                if (OriginalConfig.ContainsKey(key) && OriginalConfig[key].Equals(value)) {//如果和默认设置相同则不操作
                    if (UserConfig.ContainsKey(key)) //移除和默认设置相同的设置
                        UserConfig.Remove(key);
                    return;
                }
                if (UserConfig.ContainsKey(key)) //当用户设置已存在该选项时修改
                    UserConfig[key] = value;
                else                             //不存在时添加
                    UserConfig.Add(key, value);
            }
        }


        private void Init(XmlDocument doc) {
            ConfigDir = Application.StartupPath + "/conf/";
            if (!Directory.Exists(ConfigDir)) {
                Directory.CreateDirectory(ConfigDir);
            }
            OriginalConfig = Read(doc);
            Reload();
        }

        private Dictionary<string,ConfigValue> Read(XmlDocument doc) {
            var root = doc.DocumentElement;
            var list = root.ChildNodes;
            var config = new Dictionary<string, ConfigValue>();
            foreach (XmlNode node in list) {
                if (node.NodeType == XmlNodeType.Comment)
                    continue;
                config.Add(node.Name, new ConfigValue(node.InnerText));   
            }
            if (config.ContainsKey("UserConfig"))
                UserPath = ConfigDir + config["UserConfig"].Value;
            return config;
        }
        

        public void Save() => SaveAs(UserPath, UserConfig);


        /// <summary>
        /// 保存配置
        /// </summary>
        public void SaveAs(string Path, IEnumerable<KeyValuePair<string, ConfigValue>> config) {
            var doc = new XmlDocument();
            var root = doc.CreateElement("Config");
            doc.AppendChild(root);
            foreach (var entry in config) {
                var item = doc.CreateElement(entry.Key);
                item.InnerText = entry.Value.Value;
                root.AppendChild(item);
            }
            var fs = new FileStream(Path, FileMode.Create);
            doc.Save(fs);
            fs.Close();
        }

        public void Load(string url) {
            var doc = new XmlDocument();
            doc.Load(url);
            Init(doc);
        }

        public void Load(Stream stream) {
            var doc = new XmlDocument();
            doc.Load(stream);
            Init(doc);
        }

        public void LoadConfig(string xml) {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            Init(doc);
        }

        /// <summary>
        /// 重置用户设置
        /// </summary>
        public void Reset() => UserConfig.Clear();


        /// <summary>
        /// 重载用户设置
        /// </summary>
        public void Reload() {
            if (File.Exists(UserPath)) {
                var doc = new XmlDocument();
                doc.Load(UserPath);
                UserConfig = Read(doc);
            } else
                UserConfig = new Dictionary<string, ConfigValue>();
        }

        /// <summary>
        /// 导入
        /// 不会修改已存在的系统默认设置
        /// </summary>
        public void Import(IConfig config) {
            foreach (var entry in config) {
                if (!OriginalConfig.ContainsKey(entry.Key)) 
                    OriginalConfig.Add(entry.Key, entry.Value);             
                if (UserConfig.ContainsKey(entry.Key))
                    UserConfig[entry.Key] = entry.Value;
                else
                    UserConfig.Add(entry.Key, entry.Value);
            }

        }

        /// <summary>
        /// 导出
        /// </summary>
        public void Export(string fileName) => SaveAs(fileName, this);


        public IEnumerator<KeyValuePair<string, ConfigValue>> GetEnumerator() {
            foreach (var key in OriginalConfig.Keys)
                yield return new KeyValuePair<string, ConfigValue>(key, this[key]);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
     }

}
