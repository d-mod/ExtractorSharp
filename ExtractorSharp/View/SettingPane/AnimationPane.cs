using System;
using ExtractorSharp.Component;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;

namespace ExtractorSharp.View.SettingPane {
    public partial class AnimationPane : AbstractSettingPane {
        public AnimationPane(IConnector Data) : base(Data) {
            InitializeComponent();
            animationBar.ValueChanged += FlashSpeedBarChanged;
            animationSpeedBox.ValueChanged += FlashSpeedBoxChanged;
        }

        private void FlashSpeedBarChanged(object sender, EventArgs e) {
            if (animationSpeedBox.Value != animationBar.Value) {
                animationSpeedBox.Value = animationBar.Value;
            }
        }

        private void FlashSpeedBoxChanged(object sender, EventArgs e) {
            if (animationSpeedBox.Value != animationBar.Value) {
                animationBar.Value = (int) animationSpeedBox.Value;
            }
        }

        public override void Initialize() {
            animationBar.Value = Config["FlashSpeed"].Integer;
        }

        public override void Save() {
            Config["FlashSpeed"] = new ConfigValue(animationBar.Value);
        }
    }
}