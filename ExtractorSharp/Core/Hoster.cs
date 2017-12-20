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

namespace ExtractorSharp.Core{
    /// <summary>
    /// 插件宿主
    /// </summary>
    public class Hoster {
        private CompositionContainer Container;

        /// <summary>
        /// 插件
        /// </summary>
        [ImportMany(RequiredCreationPolicy =CreationPolicy.Shared)]

        private IEnumerable<IPlugin> plugins;

        [ImportMany(RequiredCreationPolicy =CreationPolicy.Shared)]
        private IEnumerable<ICommand> commands;

        [ImportMany(RequiredCreationPolicy =CreationPolicy.Shared)]
        public IEnumerable<IMenuItem> ItemList { set; get; }

        [ImportMany(RequiredCreationPolicy =CreationPolicy.Shared)]
        public IEnumerable<EaseDialog> DialogList { set; get; }

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
        public IEnumerable<Plugin> Install(IEnumerable<IPlugin> list) {
            var guids = new HashSet<Guid>();
            foreach (var item in list) {
                var sucess = false;
                try {
                    item.Initialize();
                } catch (Exception) {
                    sucess = false;
                }
                if (sucess) {
                    yield return new Plugin();
                }
            }
        }
    }
    
}
