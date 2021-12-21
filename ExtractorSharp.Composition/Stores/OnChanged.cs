using System;

namespace ExtractorSharp.Composition.Stores {

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class OnChanged : Attribute {

        public OnChanged(string propertyName) {
            this.PropertyName = propertyName;
        }

        public string PropertyName { set; get; }

    }
}
