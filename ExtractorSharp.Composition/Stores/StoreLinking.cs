using System;

namespace ExtractorSharp.Composition.Stores {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StoreLinking : Attribute {

        public StoreLinking(params string[] Keys) {
            this.Keys = Keys;
        }

        public string[] Keys { set; get; }
    }
}
