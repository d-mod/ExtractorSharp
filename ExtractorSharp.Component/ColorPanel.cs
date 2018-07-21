using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.Component {
    public partial class ColorPanel : Panel {
        public ColorPanel() {
            InitializeComponent();
        }

        public Color Color {
            set => BackColor = value;
            get => BackColor;
        }
    }
}