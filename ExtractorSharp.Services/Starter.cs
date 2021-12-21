using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Json;
using ExtractorSharp.Services.Constants;
using ExtractorSharp.Services.Properties;
using Microsoft.Win32;

namespace ExtractorSharp.Services {
    public class Starter {

        private Starter() {

            var catalog = new AggregateCatalog();

            foreach(var ass in AppDomain.CurrentDomain.GetAssemblies()) {
                catalog.Catalogs.Add(new AssemblyCatalog(ass));
            }

            var pluginPath = $"{AppPath}/plugins";
            if(Directory.Exists(pluginPath)) {
                foreach(var dir in Directory.GetDirectories(pluginPath)) {
                    catalog.Catalogs.Add(new DirectoryCatalog(dir));
                }
            }

            this.InitLanguage();
            this.InitStore();
            this.LoadRecent();

            this.Container = new CompositionContainer(catalog);

            this.Container.ComposeExportedValue(this.Language);
            this.Container.ComposeExportedValue(this.Store);
            this.Container.ComposeExportedValue(this.Config);
        }

        private static readonly string AppPath = AppDomain.CurrentDomain.BaseDirectory;

        private CompositionContainer Container { set; get; }

        private Store Store;

        private IConfig Config;

        private Language Language;


        private void InitLanguage() {
            this.Language = Language.CreateCurrent(Resources.Chinese);
        }

        private void InitStore() {

            var rules = new Dictionary<string, int>();
            var builder = new LSBuilder();
            builder.ReadJson(Resources.Rules).GetValue(ref rules);


            var sessionId = Guid.NewGuid().ToString("N");

            Console.WriteLine($"APP STARTUP...SESSION-ID:{sessionId}");


            this.Store = new Store()
                .Create(StoreKeys.FILES, new List<Album>())
                .Create(StoreKeys.SAVE_PATH, string.Empty)
                .Create(StoreKeys.IS_SAVED, true)
                .Create(StoreKeys.MERGE_QUEUES, new List<Album>())
                .Create("/merge/version", 2)
                .Create(StoreKeys.MERGE_RULES, rules)
                .ReadOnly(StoreKeys.APP_SESSION_ID,sessionId)
                .ReadOnly(StoreKeys.APP_DIR,AppPath)
                ;
            this.Store.Subscribe(this);

            this.Config = new StoreConfig($"{AppPath}/conf/", this.Store);
            this.Config.LoadConfig(Resources.Config);
            this.Config.LoadConfig(Resources.View);
        }




        private void LoadRecent() {
            var builder = new LSBuilder();
            var recentConfigPath = $@"{AppPath}/conf/recent.json";
            var recent = new List<string>();
            if(File.Exists(recentConfigPath)) {
                recent = builder.Read(recentConfigPath).GetValue(typeof(List<string>)) as List<string>;
            }
            this.Store.Set(StoreKeys.RECENTS, recent, e => builder.WriteObject(e, recentConfigPath));
        }

        /// <summary>
        ///     加载注册表
        /// </summary>
        private void LoadRegistry() {
            try {
                var path = this.Config["GamePath"].Value;
                if(string.IsNullOrEmpty(path)|| !Directory.Exists(path)) {
                    path = Registry.CurrentUser
                        .OpenSubKey("software\\tencent\\dnf", RegistryKeyPermissionCheck.Default,
                            RegistryRights.ReadKey).GetValue("InstallPath").ToString();
                    this.Config["GamePath"] = new ConfigValue(path);
                }             
                this.Config["ResourcePath"] = new ConfigValue($"{this.Config["GamePath"]}\\ImagePacks2");
                this.Config.Save();
            } catch(Exception e) {
                Console.Write(e);
            }
        }

        private static Starter _instance;

        private static Starter Instance {
            get {
                if(_instance == null) {
                    _instance = new Starter();
                }
                return _instance;
            }
        }

        public static void ThrowExecpetion(Exception e) {
            var Config = GetValue<IConfig>();
            var Messager = GetValue<Messager>();
            var Viewer = GetValue<Viewer>();
            var profile = Config["Profile"].Value;
            if(profile == "debug") {
                return;
            }
            try {
                var log = $"{e.Message};\r\n{e.StackTrace}";
                var data = Encoding.UTF8.GetBytes(log);
                log = Convert.ToBase64String(data);
                var dir = $"{AppPath}/log";
                if(!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }
                var current = $"{dir}/error_{DateTime.Now:yyyyMMddHHmmss}.log";
                File.WriteAllBytes(current, data);
                switch(e) {
                    case IOException _:
                        Messager.Error("FileHandleError");
                        break;
                    case ApplicationException _:
                        Messager.Error(e.Message);
                        break;
                    default:
                        if(profile=="release") {
                            Viewer.Show("debug", "debug", log);
                        }
                        break;
                }
            } catch(Exception) { }
        }

        public static T GetValue<T>() {
            return Instance.Container.GetExportedValue<T>();
        }

        public static void Inject<T>(T t) {
            Instance.Container.ComposeExportedValue(t);
        }

    }
}
