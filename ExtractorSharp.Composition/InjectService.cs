using System;
using System.ComponentModel.Composition;
using ExtractorSharp.Composition.Compatibility;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core;

namespace ExtractorSharp.Composition {
    public class InjectService : IPartImportsSatisfiedNotification, IDisposable {

        [Import]
        protected Store Store { set; get; }

        [Import]
        protected Controller Controller { set; get; }

        [Import]
        protected Language Language { set; get; }

        [Import]
        protected IConfig Config { set; get; }

        [Import]
        protected Messager Messager { set; get; }

        [Import]
        protected Viewer Viewer { set; get; }

        [Import]
        protected ICommonMessageBox MessageBox { set; get; }

        public virtual void OnImportsSatisfied() {
            this.Store.Subscribe(this);
        }

        public void Emit(string propertyName) {

        }

        public void Dispose() {
            this.Store.UnSubscribe(this);
        }
    }
}
