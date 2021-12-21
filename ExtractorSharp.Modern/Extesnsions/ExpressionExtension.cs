using System;
using System.Windows.Markup;

namespace ExtractorSharp {
    [MarkupExtensionReturnType(typeof(object))]
    public class ExpressionExtension : MarkupExtension {

        private string Expression { get; }

        public ExpressionExtension(string expression) {
            this.Expression = expression;
        }


        public override object ProvideValue(IServiceProvider serviceProvider) {



            return null;
        }
    }
}
