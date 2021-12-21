using System;

namespace ExtractorSharp.Core.Providers {

    public class LanguageProvider : IFormatProvider, ICustomFormatter {

        private readonly Language Language;

        public LanguageProvider(Language Language) {
            this.Language = Language;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider) {
            return this.Language[arg];
        }

        public object GetFormat(Type formatType) {
            return this;
        }
    }
}
