using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.View.SettingPane {
    public partial class GifPane : AbstractSettingPane {
        public GifPane(IConnector Connector) : base(Connector) {
            InitializeComponent();
            backgroundPanel.MouseClick += BackgroundChange;
        }

        private Color Color {
            set {
                backgroundPanel.Color = value;
                backgroundBox.Text = value.ToHexString();
            }
            get => backgroundPanel.Color;
        }

        private void BackgroundChange(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                Color = Color.FromArgb(0, 0, 0, 0);
                return;
            }
            var dialog = new ColorDialog();
            dialog.Color = backgroundPanel.Color;
            if (dialog.ShowDialog() == DialogResult.OK) {
                Color = Color.FromArgb(0, dialog.Color);
            }
        }

        public override void Initialize() {
            delayBox.Value = Config["GifDelay"].Integer;
            Color = Config["GifTransparent"].Color;
        }

        public override void Save() {
            Config["GifDelay"] = new ConfigValue(delayBox.Value);
            //Config["GifTransparent"] = new Config.ConfigValue(Color);
        }
    }
}