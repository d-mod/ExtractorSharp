using System.Windows.Forms;

namespace ExtractorSharp.UI {
    public partial class EaseTabPanel : TabControl {
        public EaseTabPanel() {
            InitializeComponent();
        }

        public void AddPage(string text, System.Windows.Forms.Control control) {
            var page = new TabPage(text);
            page.Controls.Add(control);
            TabPages.Add(page);
        }
    }
}
