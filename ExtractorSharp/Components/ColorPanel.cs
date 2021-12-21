using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.Components {
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