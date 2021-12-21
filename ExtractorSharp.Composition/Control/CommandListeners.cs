using System;
using System.ComponentModel.Composition;

namespace ExtractorSharp.Composition.Control {
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandListeners : Attribute {

        public string[] Listeners { get; }

        public CommandListeners(params string[] Listeners) {
            this.Listeners = Listeners;
        }

    }
}
