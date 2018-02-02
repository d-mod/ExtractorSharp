using ExtractorSharp.Composition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Command;
using ExtractorSharp.Component;
using ExtractorSharp.Composition;
using ExtractorSharp.Config;
using ExtractorSharp.Core.Control;
using ExtractorSharp.Data;
using ExtractorSharp.Loose;
using System.Diagnostics;
using System.Net;

namespace ExtractorSharp.Core {
    /// <summary>
    /// 插件宿主
    /// </summary>
    public class Hoster {

        public Dictionary<Guid, Plugin> List { get; }

        public List<Metadata> NetList { set; get; } = new List<Metadata>();

        private IConfig Config => Program.Config;

        private ComposablePartCatalog Catalog { get; }

        private const string MARKET_URL = "http://localhost:8080/api/plugin/list";


        private const string DOWNLOAD_URL = "http://localhost:8080/api/plugin/download";


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

        public void Download(Guid guid) {
            var name = $"{Config["RootPath"]}/{Config["UpdateExeName"]}";
            Process pro = Process.Start(name, $" -p {guid}");
            pro.Exited += (sender, e) => Install($"{Config["RootPath"]}/plugin/{guid}");
        }

        public bool Install(string dir) {
            try {
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
                plugin.Initialize();
                //记录插件
                List.Add(plugin.Guid, plugin);
            } catch (Exception e) {
                return false;
            }
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
            } catch (Exception) {

            }
        }
    }
}
