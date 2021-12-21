using System.Collections;
using System.Collections.Generic;
using System.IO;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Json;

namespace ExtractorSharp.Services {

    public class JsonConfig : IConfig {
        private readonly LSBuilder builder;

        public JsonConfig(string ConfigDirectory) {
            this.builder = new LSBuilder();
            this.ConfigDirectory = ConfigDirectory;
        }


        /// <summary>
        ///     系统设置
        /// </summary>
        private Dictionary<string, ConfigValue> OriginalConfig { get; } = new Dictionary<string, ConfigValue>();

        /// <summary>
        ///     用户设置
        /// </summary>
        private Dictionary<string, ConfigValue> UserConfig { get; } = new Dictionary<string, ConfigValue>();

        public string ConfigDirectory { get; }
        /// <summary>
        /// 用户配置文件路径
        /// </summary>
        public string UserPath { set; get; }

        /// <summary>
        ///     根据对应的指定的key，获得对应的ConfigValue
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ConfigValue this[string key] {
            get {
                if(this.UserConfig.ContainsKey(key) && this.UserConfig[key].NotEmpty) {
                    return this.UserConfig[key];
                }
                if(this.OriginalConfig.ContainsKey(key)) {
                    return this.OriginalConfig[key];
                }
                return ConfigValue.NullValue;
            }
            set {
                if(this.OriginalConfig.ContainsKey(key) && this.OriginalConfig[key].Equals(value)) {
                    //如果和默认设置相同则不操作
                    if(this.UserConfig.ContainsKey(key)) {
                        this.UserConfig.Remove(key);
                    }
                    return;
                }

                this.UserConfig[key] = value;
            }
        }

        public void Save() {
            if(string.IsNullOrEmpty(this.UserPath) && this.OriginalConfig.ContainsKey("UserConfig")) {
                this.UserPath = this.ConfigDirectory + this.OriginalConfig["UserConfig"].Value;
            }
            this.SaveAs(this.UserPath, this.UserConfig);
        }

        public void Load(string url) {
            var root = this.builder.Read(url);
            this.Init(root);
        }

        public void Load(Stream stream) {
            var root = this.builder.Read(stream);
            this.Init(root);
        }

        public void LoadConfig(string xml) {
            var root = this.builder.ReadJson(xml);
            this.Init(root);
        }

        /// <summary>
        ///     重置用户设置
        /// </summary>
        public void Reset() {
            this.UserConfig.Clear();
        }


        /// <summary>
        ///     重载用户设置
        /// </summary>
        public void Reload() {
            this.Reset();
            if(File.Exists(this.UserPath)) {
                var root = this.builder.Read(this.UserPath);
                this.Read(root, this.UserConfig);
            }
        }

        /// <summary>
        ///     导入
        ///     不会修改已存在的系统默认设置
        /// </summary>
        public void Import(IConfig config) {
            foreach(var entry in config) {
                if(!this.OriginalConfig.ContainsKey(entry.Key)) {
                    this.OriginalConfig.Add(entry.Key, entry.Value);
                }
                if(!this.UserConfig.ContainsKey(entry.Key)) {
                    this.UserConfig.Add(entry.Key, entry.Value);
                }
                this.UserConfig[entry.Key] = entry.Value;
            }
        }

        /// <summary>
        ///     导出
        /// </summary>
        public void Export(string fileName) {
            this.SaveAs(fileName, this);
        }


        public IEnumerator<KeyValuePair<string, ConfigValue>> GetEnumerator() {
            foreach(var key in this.OriginalConfig.Keys) {
                yield return new KeyValuePair<string, ConfigValue>(key, this[key]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }


        private void Init(LSObject obj) {
            if(!Directory.Exists(this.ConfigDirectory)) {
                Directory.CreateDirectory(this.ConfigDirectory);
            }
            this.Read(obj, this.OriginalConfig);
            this.Reload();
        }

        private Dictionary<string, ConfigValue> Read(LSObject obj, Dictionary<string, ConfigValue> config) {
            foreach(var node in obj) {
                if(!config.ContainsKey(node.Name)) {
                    config.Add(node.Name, new ConfigValue(node.Value));
                }
            }
            return config;
        }

        /// <summary>
        ///     保存配置
        /// </summary>
        private void SaveAs(string Path, IEnumerable<KeyValuePair<string, ConfigValue>> config) {
            var root = new LSObject();
            foreach(var entry in config) {
                if(!entry.Value.Saveable) {
                    continue;
                }
                var item = new LSObject {
                    Name = entry.Key,
                    Value = entry.Value.Object
                };
                root.Add(item);
            }
            this.builder.Write(root, Path);
        }
    }
}