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
using ExtractorSharp.Command;

namespace ExtractorSharp.Core{
    /// <summary>
    /// 插件宿主
    /// </summary>
    internal class Hoster {
        private CompositionContainer Container;

        /// <summary>
        /// 插件
        /// </summary>
        [ImportMany(RequiredCreationPolicy =CreationPolicy.Shared)]

        private IEnumerable<Lazy<IPlugin,IPluginMetadata>> plugins;

        [ImportMany(RequiredCreationPolicy =CreationPolicy.Shared)]
        private IEnumerable<Lazy<ICommand, IPluginMetadata>> commands;

        private List<Plugin> List;
        public Hoster() {
            Initialize();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize() {
            var Path = $"{Program.Config["RootPath"]}/plugin";
            if (Directory.Exists(Path)) {
                var catalog = new AggregateCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
                catalog.Catalogs.Add(new DirectoryCatalog(Path));
                Container = new CompositionContainer(catalog);
                Container.ComposeParts(this);
                List = Install(plugins).ToList();
            } else {
                Directory.CreateDirectory(Path);
            }
        }


        /// <summary>
        /// 安装插件
        /// </summary>
        /// <param name="lazys"></param>
        /// <returns></returns>
        public IEnumerable<Plugin> Install(IEnumerable<Lazy<IPlugin, IPluginMetadata>> lazys) {
            var guids = new HashSet<Guid>();
            foreach (var lazy in lazys) {
                if (!Guid.TryParse(lazy.Metadata.Guid, out Guid guid)) {
                    continue;
                }
                if (!guids.Add(guid)) {
                    continue;
                }
                var sucess = false;
                var plugin = new Plugin(lazy.Metadata);
                try {
                    lazy.Value.Install();
                } catch (Exception) {
                    sucess = false;
                }
                if (sucess)
                    yield return plugin;
             }
        }
    }
    
}
