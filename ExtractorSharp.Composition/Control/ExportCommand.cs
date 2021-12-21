using System;
using System.ComponentModel.Composition;

namespace ExtractorSharp.Composition.Control {

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportCommand : ExportAttribute, IName {

        public ExportCommand(string Name) : base(typeof(ICommand)) {
            this.Name = Name;
        }

        public string Name { set; get; }

    }
}
