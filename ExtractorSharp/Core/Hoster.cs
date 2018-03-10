using ExtractorSharp.Composition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using ExtractorSharp.Config;
using ExtractorSharp.Data;
using ExtractorSharp.Json;
using System.Diagnostics;
using System.Net;
using ExtractorSharp.Exceptions;

namespace ExtractorSharp.Core {
    /// <summary>
    /// 插件宿主
    /// </summary>
    public class Hoster {

        public Dictionary<Guid, Plugin> List { get; }

        public List<Metadata> NetList { set; get; } = new List<Metadata>();

        private IConfig Config => Program.Config;

        private ComposablePartCatalog Catalog { get; }

        private const string MARKET_URL = "http://extractorsharp.kritsu.net/api/plugin/list";


        private const string DOWNLOAD_URL = "http://extractorsharp.kritsu.net/api/plugin/download";


        public Hoster() {
            List = new Dictionary<Guid, Plugin>();
            Catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var rootPath = Config["RootPath"];
            var Path = $"{rootPath}/plugin";
            if (Directory.Exists(Path)) {
                foreach (var dir in Directory.GetDirectories(Path)) {
                    Install(dir);
                }
            } else {
                Directory.CreateDirectory(Path);
            }
        }

        public bool Download(Guid guid) {
            var name = $"{Config["RootPath"]}/{Config["UpdateExeName"]}";
            try {
                var client = new WebClient();
                client.DownloadFile(Config["UpdateExeUrl"].Value, name);
                client.Dispose();
                Process.Start(name);
                Process pro = Process.Start(name, $" -p {guid}");
                pro.Exited += (sender, e) => Install($"{Config["RootPath"]}/plugin/{guid}");
            } catch(Exception e) {
                return false;
            }
            return true;
        }

        public bool Install(string dir) {
            var plugin = new Plugin();
            plugin.Directory = dir;
            //加载主程序模块和插件模块
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(this.Catalog);
            catalog.Catalogs.Add(new DirectoryCatalog(dir));
            var container = new CompositionContainer(catalog);
            //注入插件信息
            container.ComposeParts(plugin);
            //载入设置
            var confDir = $"{dir}/conf/conf.json";
            if (File.Exists(confDir)) {
                Config.Load(confDir);
            }
            //载入多语言
            var lanDir = $"{dir}/lan";
            if (Directory.Exists(lanDir)) {
                Language.CreateFromDir(lanDir);
            }
            //初始化
            try {
                plugin.Initialize();
            } catch(Exception) {
                return false;
            }
            //记录插件
            List.Add(plugin.Guid, plugin);
            return true;
        }

        public void Refresh() {
            NetList.Clear();
            try {
                var builder = new LSBuilder();
                var obj = builder.Get(MARKET_URL);
                if (obj["status"].Value.Equals("success")) {
                    NetList = obj["tag"].GetValue(typeof(List<Metadata>)) as List<Metadata>;
                }
            } catch (Exception e) {
                throw new PluginExecption("PluginListDownloadError");
            }
        }
    }
}
