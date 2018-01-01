using ExtractorSharp.Command;
using ExtractorSharp.Component;
using ExtractorSharp.Core.Control;
using ExtractorSharp.Handle;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Composition {
    /// <summary>
    /// 
    /// </summary>
    public class Plugin {

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { set; get; } = true;



        [Import]
        private Lazy<IPlugin, IMetadata> lazy;

        private IMetadata Metadata;

        [ImportMany(typeof(ICommand))]
        private IEnumerable<Lazy<ICommand, IGuid>> commands;

        [ImportMany(typeof(IMenuItem))]
        private IEnumerable<Lazy<IMenuItem, IGuid>> items;

        [ImportMany(typeof(ESDialog))]
        private IEnumerable<Lazy<ESDialog, IGuid>> dialogs;

        [ImportMany(typeof(ESDialog))]
        private IEnumerable<Lazy<Handler, IGuid>> handlers;

        public string Directory { set; get; }

        public Guid Guid => Guid.Parse(Metadata.Guid);

        public string Author => Metadata.Author;

        public string Description => Metadata.Description;

        public string Versinon => Metadata.Version;

        public string Since => Metadata.Since;

        public string Name => Metadata.Name;


        private Controller Controller=>Program.Controller;

        private MainForm MainForm => Program.Form;

        private Viewer Viewer => Program.Viewer;

        public Plugin() {}

        public void Initialize() {
            Metadata = lazy.Metadata;
            InstallCommand();
            InstallItem();
            InstallDialog();
        }

        private void InstallCommand() {
            foreach (var lazy in commands) {
                if (Guid.TryParse(lazy.Metadata.Guid, out Guid guid)) {
                    if (guid == this.Guid) {
                        var cmd = lazy.Value;
                        Controller.Regisity(cmd.Name,cmd.GetType());
                    }
                }
            }
        }

        private void InstallItem() {
            foreach (var lazy in items) {
                if (Guid.TryParse(lazy.Metadata.Guid, out Guid guid)) {
                    if (guid == this.Guid) {
                        MainForm.AddMenuItem(lazy.Value);
                    }
                }
            }
        }

        private void InstallDialog() {
            foreach (var lazy in dialogs) {
                if (Guid.TryParse(lazy.Metadata.Guid, out Guid guid)) {
                    if (guid == this.Guid) {
                        var dialog = lazy.Value;
                        Viewer.Regisity(dialog.Name,dialog);
                    }
                }
            }
        }

        private void InstallHandler() {
            foreach (var lazy in handlers) {
                if (Guid.TryParse(lazy.Metadata.Guid, out Guid guid)) {
                    if (guid == this.Guid) {
                        var handler = lazy.Value;
                        Handler.Regisity(handler.Version,handler.GetType());
                    }
                }
            }
        }
    }
}
