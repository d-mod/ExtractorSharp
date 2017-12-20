using ExtractorSharp.Composition;
using System;
using System.Collections.Generic;

namespace ExtractorSharp.Core.Composition {
    public class PluginItemAttribute :Attribute{
        public string Name { get; }

        public PluginItemType Parent { get; } = PluginItemType.MAIN;

        public string Command { get; }

        public Dictionary<string, string> Children { get; }

        public PluginItemAttribute(string Name) {
            this.Name = Name;
        }

        public PluginItemAttribute(string Name,PluginItemType Parent): this(Name){
            this.Parent = Parent;
        }

        public PluginItemAttribute(string Name,PluginItemType Parent,string Command) : this(Name, Parent) {
            this.Command = Command;
        }

        public PluginItemAttribute(string Name,PluginItemType Parent,Dictionary<string,string> Children) : this(Name,Parent) {
            this.Children = Children;
        }

    }
}
