using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using ExtractorSharp.Component;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Handle;

namespace ExtractorSharp.Composition {
    /// <summary>
    /// </summary>
    public class Plugin {
        [ImportMany(typeof(ICommand))] private IEnumerable<Lazy<ICommand, IGuid>> _commands;

        [ImportMany(typeof(ESDialog))] private IEnumerable<Lazy<ESDialog, IGuid>> _dialogs;

        [ImportMany(typeof(Handler))] private IEnumerable<Lazy<Handler, IGuid>> _handlers;

        [ImportMany(typeof(IMenuItem))] private IEnumerable<Lazy<IMenuItem, IGuid>> _items;


        [Import] private Lazy<IPlugin, IMetadata> _lazyMetadata;

        private IMetadata _metadata;

        [ImportMany(typeof(IFileSupport))] private IEnumerable<Lazy<IFileSupport, IGuid>> _supports;

        /// <summary>
        ///     是否启用
        /// </summary>
        public bool Enable { set; get; } = true;

        public string Directory { set; get; }

        public Guid Guid => Guid.Parse(_metadata.Guid);

        public string Author => _metadata.Author;

        public string Description => _metadata.Description;

        public string Version => _metadata.Version;

        public string Since => _metadata.Since;

        public string Name => _metadata.Name;


        private static Controller Controller => Program.Controller;

        private static IConnector Connector => Program.Connector;

        private static MainForm MainForm => Program.Form;

        private static Viewer Viewer => Program.Viewer;

        public void Initialize() {
            _metadata = _lazyMetadata.Metadata;
            InstallCommand();
            InstallItem();
            InstallDialog();
            InstallSupport();
        }

        private void InstallCommand() {
            foreach (var lazy in _commands) {
                if (!Guid.TryParse(lazy.Metadata.Guid, out var guid)) continue;
                if (guid != Guid) continue;
                var cmd = lazy.Value;
                Controller.Registry(cmd.Name, cmd.GetType());
            }
        }

        private void InstallSupport() {
            foreach (var lazy in _supports) {
                if (!Guid.TryParse(lazy.Metadata.Guid, out var guid)) continue;
                if (guid != Guid) continue;
                var support = lazy.Value;
                Connector.FileSupports.Add(support);
            }
        }

        private void InstallItem() {
            foreach (var lazy in _items) {
                if (!Guid.TryParse(lazy.Metadata.Guid, out var guid)) continue;
                if (guid != Guid) continue;
                MainForm.AddMenuItem(lazy.Value);
            }
        }

        private void InstallDialog() {
            foreach (var lazy in _dialogs) {
                if (!Guid.TryParse(lazy.Metadata.Guid, out var guid)) continue;
                if (guid != Guid) continue;
                var dialog = lazy.Value;
                Viewer.Regisity(dialog.Name, dialog);
            }
        }

        private void InstallHandler() {
            foreach (var lazy in _handlers) {
                if (!Guid.TryParse(lazy.Metadata.Guid, out var guid)) continue;
                if (guid != Guid) continue;
                var handler = lazy.Value;
                Handler.Regisity(handler.Version, handler.GetType());
            }
        }
    }
}