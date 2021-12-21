using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using ExtractorSharp.Composition.View;

namespace ExtractorSharp.Composition.Core {

    [Export]
    public class Viewer {

        public delegate void ViewEventHandler(object sender, ViewEventArgs e);

        public event ViewEventHandler ViewCreated;

        [ImportMany(typeof(IView))]
        private IEnumerable<Lazy<IView, IViewMetadata>> lazies;

        public object Show(string name, params object[] args) {
            if(!string.IsNullOrEmpty(name)) {
                var lazy = this.lazies.FirstOrDefault(f => name.EqualsIgnoreCase(f.Metadata.Name));
                if(lazy != null) {
                    if(!lazy.IsValueCreated) {
                        ViewCreated?.Invoke(this, new ViewEventArgs {
                            Name = lazy.Metadata.Name,
                            Title = lazy.Metadata.Title ?? lazy.Metadata.Name,
                            View = lazy.Value
                        }); ;
                    }
                    return lazy.Value.ShowView(args);
                }
            }
            return null;
        }


    }

    public class ViewEventArgs : EventArgs {

        public string Name { set; get; }

        public string Title { set; get; }

        public IView View { set; get; }

    }
}
