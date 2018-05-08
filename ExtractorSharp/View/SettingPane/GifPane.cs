using ExtractorSharp.Component;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.View.SettingPane {
    public partial class GifPane : AbstractSettingPane { 
        private Color Color {
            set {
                backgroundPanel.Color = value;
                backgroundBox.Text = value.ToHexString();
            }
            get {
                return backgroundPanel.Color;
            }
        }

        public GifPane(IConnector Connector) : base(Connector) { 
            InitializeComponent();
            backgroundPanel.MouseClick += BackgroundChange;
        }

        private void BackgroundChange(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                Color = Color.FromArgb(0,0,0,0);
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
            Config["GifDelay"] = new Config.ConfigValue(delayBox.Value);
            //Config["GifTransparent"] = new Config.ConfigValue(Color);
        }
    }
}
