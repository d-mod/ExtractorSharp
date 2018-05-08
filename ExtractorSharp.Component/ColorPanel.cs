using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.Component {
    public partial class ColorPanel : Panel {

        public Color Color {
            set {
                this.BackColor = value;
            }
            get {
                return this.BackColor;
            }
        }

        public ColorPanel() {
            InitializeComponent();
        }

    }
}
