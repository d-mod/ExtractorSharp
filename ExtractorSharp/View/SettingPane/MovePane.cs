using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using ExtractorSharp.Components;
using ExtractorSharp.Composition.Config;

namespace ExtractorSharp.View.SettingPane {
    public partial class MovePane : Panel {

        [Import]
        public IConfig Config;
        public MovePane() {
            InitializeComponent();
            moveStepBar.ValueChanged += MoveStepBarChanged;
            moveStepBox.ValueChanged += MoveStepBoxChanged;
        }

        private void MoveStepBarChanged(object sender, EventArgs e) {
            if(moveStepBox.Value != moveStepBar.Value) {
                moveStepBox.Value = moveStepBar.Value;
            }
        }

        private void MoveStepBoxChanged(object sender, EventArgs e) {
            if(moveStepBox.Value != moveStepBar.Value) {
                moveStepBar.Value = (int)moveStepBox.Value;
            }
        }

        public void Initialize() {
            moveStepBox.Value = Config["MoveStep"].Integer;
            autoChangePositionCheck.Checked = Config["AutoChangePosition"].Boolean;
        }

        public void Save() {
            Config["MoveStep"] = new ConfigValue(moveStepBox.Value);
            Config["AutoChangePosition"] = new ConfigValue(autoChangePositionCheck.Checked);
        }

    }
}
