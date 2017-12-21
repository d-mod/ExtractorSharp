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

namespace ExtractorSharp.Core{
    /// <summary>
    /// 插件宿主
    /// </summary>
    public class Hoster {
        private CompositionContainer Container;

        /// <summary>
        /// 插件
        /// </summary>
        [ImportMany(typeof(IPlugin))]
        private IEnumerable<Lazy<IPlugin,IMetadata>> plugins;

        [ImportMany(typeof(ICommand))]
        private IEnumerable<Lazy<ICommand,IGuid>> commands;

        [ImportMany(typeof(IMenuItem))]
        private IEnumerable<Lazy<IMenuItem,IGuid>> items { set; get; }

        [ImportMany(typeof(EaseDialog))]
        private IEnumerable<Lazy<EaseDialog,IGuid>> dialogs { set; get; }

        public Dictionary<Guid,Plugin> List { set; get; }

        private IConfig Config => Program.Config;

        private Controller Controller => Program.Controller;

        private MainForm MainForm => Program.Form;

        private Viewer Viewer => Program.Viewer;

        public Hoster() {
            var Path = $"{Config["RootPath"]}/plugin";
            if (Directory.Exists(Path)) {
                var catalog = new AggregateCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
                catalog.Catalogs.Add(new DirectoryCatalog(Path));
                Container = new CompositionContainer(catalog);
                Container.ComposeParts(this);
                Install();
            } else {
                Directory.CreateDirectory(Path);
            }
        }

 
        public void Install() {
            InstallPlugin();
            InstallCommand();
            InstallItem();
            InstallDialog();
        }

        public void InstallPlugin() {
            List = new Dictionary<Guid, Plugin>();
            foreach (var lazy in plugins) {
                var meta = lazy.Metadata;
                var guidStr = meta.Guid;
                if (Guid.TryParse(guidStr, out Guid guid)) {
                    if (List.ContainsKey(guid)) {
                        continue;
                    }
                    var plugin = new Plugin(meta);
                    plugin.Enable = !Config[guidStr].Boolean;
                    List.Add(guid,plugin);
                }
            }
           
        }

        public void InstallCommand() {
            foreach (var lazy in commands) {
                var meta = lazy.Metadata;
                if (Guid.TryParse(meta.Guid, out Guid guid)) {
                    if (!List.ContainsKey(guid)) {
                        continue;
                    }
                    var plugin = List[guid];
                    var cmd = lazy.Value;
                    if (plugin.Enable) {
                        Controller.Regisity(cmd.Name,cmd.GetType());
                    }
                }
            }
        }

        public void InstallItem() {
            foreach (var lazy in items) {
                var meta = lazy.Metadata;
                if (Guid.TryParse(meta.Guid, out Guid guid)) {
                    if (!List.ContainsKey(guid)) {
                        continue;
                    }
                    var plugin = List[guid];
                    if (plugin.Enable) {
                        MainForm.AddMenuItem(lazy.Value);
                    }
                }
            }
        }

        public void InstallDialog() {
            foreach (var lazy in dialogs) {
                var meta = lazy.Metadata;
                if (Guid.TryParse(meta.Guid, out Guid guid)) {
                    if (!List.ContainsKey(guid)) {
                        continue;
                    }
                    var plugin = List[guid];
                    var dialog = lazy.Value;
                    if (plugin.Enable) {
                        Viewer.Regisity(dialog.Name, dialog);
                    }
                }
            }
        }
        
    }
    
}
