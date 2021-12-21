using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.View.SettingPane {
    partial class MovePane {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
            this.moveStepGroup = new GroupBox();
            this.moveStepBar = new TrackBar();
            this.autoChangePositionCheck = new CheckBox();
            this.moveStepBox = new System.Windows.Forms.NumericUpDown();
            this.moveStepGroup.SuspendLayout();
            this.SuspendLayout();
            this.moveStepGroup.Location = new System.Drawing.Point(13, 15);
            this.moveStepGroup.Size = new System.Drawing.Size(380, 100);
            this.moveStepGroup.TabIndex = 2;
            this.moveStepGroup.TabStop = false;
         //   this.moveStepGroup.Text = Language["MoveStep"];

            // 
            // flashSpeedBar
            // 
            this.moveStepBar.LargeChange = 10;
            this.moveStepBar.Location = new System.Drawing.Point(20, 21);
            this.moveStepBar.Maximum = 100;
            this.moveStepBar.Minimum = 1;
            this.moveStepBar.Name = "moveStepBar";
            this.moveStepBar.Size = new System.Drawing.Size(350, 45);
            this.moveStepBar.TabIndex = 0;
            this.moveStepBar.Value = 10;
            //
            //
            //
            this.moveStepBar.BackColor = Color.White;
            this.moveStepBox.Minimum = 1;
            this.moveStepBox.Maximum = 100;
            this.moveStepBox.Value = 20;
            this.moveStepBox.Location = new System.Drawing.Point(120, 70);

            this.moveStepGroup.Controls.Add(this.moveStepBar);
            this.moveStepGroup.Controls.Add(this.moveStepBox);

            autoChangePositionCheck.UseVisualStyleBackColor = true;
            autoChangePositionCheck.AutoSize = true;
            autoChangePositionCheck.Size = new Size(200, 30);
          //  autoChangePositionCheck.Text = Language["AutoChangePosition"];
            autoChangePositionCheck.Location = new Point(13, 130);

            this.Controls.Add(moveStepGroup);
            this.Controls.Add(autoChangePositionCheck);

//this.Name = "Move";
          //  this.Parent = "View";

        }

        
        private GroupBox moveStepGroup;
        private TrackBar moveStepBar;
        private NumericUpDown moveStepBox;
        private CheckBox autoChangePositionCheck;

        #endregion
    }
}
