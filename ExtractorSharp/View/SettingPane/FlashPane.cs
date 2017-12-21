using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Config;
using ExtractorSharp.Core;

namespace ExtractorSharp.View.SettingPane {
    public partial class FlashPane : AbstractSettingPane {
        public FlashPane(IConnector Data):base(Data) {
            InitializeComponent();
            flashSpeedBar.ValueChanged += FlashSpeedBarChanged;
            flashSpeedBox.ValueChanged += FlashSpeedBoxChanged;
        }

        private void FlashSpeedBarChanged(object sender, EventArgs e) {
            if (flashSpeedBox.Value != flashSpeedBar.Value)
                flashSpeedBox.Value = flashSpeedBar.Value;
        }

        private void FlashSpeedBoxChanged(object sender, EventArgs e) {
            if (flashSpeedBox.Value != flashSpeedBar.Value)
                flashSpeedBar.Value = (int)flashSpeedBox.Value;
        }

        public override void Initialize() {
            flashSpeedBar.Value = Config["FlashSpeed"].Integer;
        }

        public override void Save() {
            Config["FlashSpeed"] = new ConfigValue(flashSpeedBar.Value);
        }
    }
}
