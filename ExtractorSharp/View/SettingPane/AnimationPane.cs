using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.SettingPane {

    [Export(typeof(ISetting))]
    [ExportMetadata("Name", "Animation")]
    [ExportMetadata("Parent", "View")]
    public partial class AnimationPane : Panel, ISetting, IPartImportsSatisfiedNotification {

        [Import]
        private IConfig Config;

        [Import]
        private Language Language;

        public object View => this;


        private void FlashSpeedBarChanged(object sender, EventArgs e) {
            if(animationSpeedBox.Value != animationBar.Value) {
                animationSpeedBox.Value = animationBar.Value;
            }
        }

        private void FlashSpeedBoxChanged(object sender, EventArgs e) {
            if(animationSpeedBox.Value != animationBar.Value) {
                animationBar.Value = (int)animationSpeedBox.Value;
            }
        }

        public void Initialize() {
            animationBar.Value = Config["FlashSpeed"].Integer;
        }

        public void Save() {
            Config["FlashSpeed"] = new ConfigValue(animationBar.Value);
        }

        public void OnImportsSatisfied() {
            InitializeComponent();
            animationBar.ValueChanged += FlashSpeedBarChanged;
            animationSpeedBox.ValueChanged += FlashSpeedBoxChanged;
        }
    }
}