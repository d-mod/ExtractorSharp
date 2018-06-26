using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using ExtractorSharp.Composition;
using ExtractorSharp.Core.Config;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Exceptions;
using ExtractorSharp.Json;

namespace ExtractorSharp.Core {
    /// <summary>
    ///     插件宿主
    /// </summary>
    public class Hoster {

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

        public Dictionary<Guid, Plugin> List { get; }

        public List<Metadata> NetList { set; get; } = new List<Metadata>();

        private IConfig Config => Program.Config;

        private ComposablePartCatalog Catalog { get; }

        public bool Download(Guid guid) {
            var name = $"{Config["RootPath"]}/{Config["UpdateExeName"]}";
            try {
                var client = new WebClient();
                client.DownloadFile(Config["UpdateExeUrl"].Value, name);
                client.Dispose();
                Process.Start(name);
                var pro = Process.Start(name, $" -p {guid}");
                if (pro != null) {
                    pro.Exited += (sender, e) => Install($"{Config["RootPath"]}/plugin/{guid}");
                }
            } catch (Exception e) {
                return false;
            }
            return true;
        }

        public bool Install(string dir) {
            var plugin = new Plugin {Directory = dir};
            //加载主程序模块和插件模块
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(Catalog);
            catalog.Catalogs.Add(new DirectoryCatalog(dir));
            var container = new CompositionContainer(catalog);
            //注入插件信息
            container.ComposeParts(plugin);
            //载入设置
            var confDir = $"{dir}/conf/conf.json";
            if (File.Exists(confDir)) Config.Load(confDir);
            //载入多语言
            var lanDir = $"{dir}/lan";
            if (Directory.Exists(lanDir)) {
                Language.CreateFromDir(lanDir);
            }
            //初始化
            try {
                plugin.Initialize();
            } catch (Exception) {
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
                var obj = builder.Get(Config["MarketUrl"].Value);
                if (obj["status"].Value.Equals("success")) {
                    NetList = obj["tag"].GetValue(typeof(List<Metadata>)) as List<Metadata>;
                }
            } catch (Exception e) {
                throw new PluginExecption("PluginListDownloadError");
            }
        }
    }
}