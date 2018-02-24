using ExtractorSharp.Loose;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ExtractorSharp.Config {
    public class JsonConfig : IConfig {
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

        public ConfigValue this[string group, string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private readonly LSBuilder reader;
        

        /// <summary>
        /// 根据对应的指定的key，获得对应的ConfigValue
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ConfigValue this[string key] {
            get {
                if (UserConfig.ContainsKey(key) && UserConfig[key].NotEmpty) {
                    return UserConfig[key];
                }
                if (OriginalConfig.ContainsKey(key)) {
                    return OriginalConfig[key];
                }
                return ConfigValue.NullValue;
            }
            set {
                if (OriginalConfig.ContainsKey(key) && OriginalConfig[key].Equals(value)) {//如果和默认设置相同则不操作
                    if (UserConfig.ContainsKey(key)) { //移除和默认设置相同的设置
                        UserConfig.Remove(key);
                    }
                    return;
                }
                UserConfig[key] = value;
            }
        }

        public JsonConfig() {
            reader = new LSBuilder();
        }


        private void Init(LSObject obj) {
            ConfigDir = Application.StartupPath + "/conf/";
            if (!Directory.Exists(ConfigDir)) {
                Directory.CreateDirectory(ConfigDir);
            }
            OriginalConfig = Read(obj);
            Reload();
        }

        private Dictionary<string, ConfigValue> Read(LSObject obj) {
            var config = new Dictionary<string, ConfigValue>();
            foreach (var node in obj) {
                if (!config.ContainsKey(node.Name)) {
                    config.Add(node.Name, new ConfigValue(node.Value));
                }
            }
            if (config.ContainsKey("UserConfig")) {
                UserPath = ConfigDir + config["UserConfig"].Value;
            }
            return config;
        }

        public void Save() {
            SaveAs(UserPath, UserConfig);
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void SaveAs(string Path, IEnumerable<KeyValuePair<string, ConfigValue>> config) {
            var root = new LSObject();
            foreach (var entry in config) {
                var item = new LSObject();
                item.Name = entry.Key;
                item.Value = entry.Value.Object;
                root.Add(item);
            }
            reader.Write(root,Path);
        }

        public void Load(string url) {
            var root = reader.Read(url);
            Init(root);
        }

        public void Load(Stream stream) {
            var root = reader.Read(stream);
            Init(root);
        }

        public void LoadConfig(string xml) {
            var root = reader.ReadJson(xml);
            Init(root);
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
                var root = reader.Read(UserPath);
                UserConfig = Read(root);
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
