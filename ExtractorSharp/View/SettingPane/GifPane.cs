using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.SettingPane {


    [Export(typeof(ISetting))]
    [ExportMetadata("Name", "Gif")]
    [ExportMetadata("Parent", "File")]
    public partial class GifPane : Panel, ISetting, IPartImportsSatisfiedNotification {

        [Import]
        private IConfig Config;

        [Import]
        private Language Language;

        public object View => this;

        public void OnImportsSatisfied() {
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
            if(e.Button == MouseButtons.Right) {
                Color = Color.FromArgb(0, 0, 0, 0);
                return;
            }
            var dialog = new ColorDialog {
                Color = backgroundPanel.Color
            };
            if(dialog.ShowDialog() == DialogResult.OK) {
                Color = Color.FromArgb(0, dialog.Color);
            }
        }

        public void Initialize() {
            delayBox.Value = Config["GifDelay"].Integer;
            Color = Config["GifTransparent"].Color;
        }

        public void Save() {
            Config["GifDelay"] = new ConfigValue(delayBox.Value);
            //Config["GifTransparent"] = new Config.ConfigValue(Color);
        }
    }
}