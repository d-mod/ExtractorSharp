using System;
using System.ComponentModel.Composition;

namespace ExtractorSharp.Composition.Control {
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class CommandParameter : Attribute {

        public CommandParameter() {

        }

        public CommandParameter(string Name) {
            this.Name = Name;
        }

        public string Name { set; get; }

        public bool IsRequired { set; get; } = true;

        public bool IsDefault { set; get; } = false;
    }
}
