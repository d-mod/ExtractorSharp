using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;

namespace ExtractorSharp.View.Dialog {
    public partial class AboutDialog : ESDialog {
        public AboutDialog(IConnector Data) : base(Data) {
            InitializeComponent();
        }
    }
}