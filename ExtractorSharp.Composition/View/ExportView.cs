using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Composition.View {
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportView : ExportAttribute, IName {

        public ExportView(string Name) : base(typeof(IView)) {
            this.Name = Name;
        }

        public string Name { set; get; }

    }
}
