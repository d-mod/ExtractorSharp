using System;
using System.Windows;
using System.Windows.Markup;
using ExtractorSharp.Core;
using ExtractorSharp.Services;

namespace ExtractorSharp {

    [MarkupExtensionReturnType(typeof(string))]
    public class LanguageExtension : MarkupExtension {

        public object Key { set; get; }

        public string Group { set; get; }

        private Language Language { get; } = Language.Empty;

        public LanguageExtension() {
            this.Language = Starter.GetValue<Language>();
        }

        public LanguageExtension(object o) : this() {
            this.Key = o?.ToString();
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            var key = this.Key;
            if(key == null) {
                if(!(serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget service)) {
                    return null;
                }
                if(service.TargetObject.GetType().Name.EndsWith("SharedDp")) {
                    return this;
                }
                if(service.TargetObject is FrameworkElement element) {
                    key = element.DataContext;
                }
            }
            if(this.Group != null) {
                return this.Language[this.Group, this.Key.ToString()];
            }

            return this.Language[key] ?? key;
        }
    }
}
