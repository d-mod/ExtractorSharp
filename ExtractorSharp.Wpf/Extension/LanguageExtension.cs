using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ExtractorSharp.Wpf.Extension {
    [MarkupExtensionReturnType(typeof(string))]
    public class FileExtension: MarkupExtension {
        public FileExtension() {

        }

        public override object ProvideValue(IServiceProvider provider) {
            return "HelloWorld";
        }
    }
}
