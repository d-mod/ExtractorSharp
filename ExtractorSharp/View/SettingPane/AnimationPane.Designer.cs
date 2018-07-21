using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.View.SettingPane {
    partial class AnimationPane {
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
            this.animationGroup = new GroupBox();
            this.animationBar = new TrackBar();
            this.animationSpeedBox = new System.Windows.Forms.NumericUpDown();
            this.animationGroup.SuspendLayout();
            this.SuspendLayout();
            this.animationGroup.Location = new System.Drawing.Point(13, 15);
            this.animationGroup.Size = new System.Drawing.Size(380, 100);
            this.animationGroup.TabIndex = 2;
            this.animationGroup.TabStop = false;
            this.animationGroup.Text = Language["AnimationSpeed"];

            // 
            // flashSpeedBar
            // 
            this.animationBar.LargeChange = 10;
            this.animationBar.Location = new System.Drawing.Point(20, 21);
            this.animationBar.Maximum = 100;
            this.animationBar.Minimum = 1;
            this.animationBar.Name = "animationSpeedBar";
            this.animationBar.Size = new System.Drawing.Size(350, 45);
            this.animationBar.TabIndex = 0;
            this.animationBar.Value = 20;
            //
            //
            //
            this.animationBar.BackColor = Color.White;
            this.animationSpeedBox.Minimum = 1;
            this.animationSpeedBox.Maximum = 100;
            this.animationSpeedBox.Value = 20;
            this.animationSpeedBox.Location = new System.Drawing.Point(120, 70);

            this.animationGroup.Controls.Add(this.animationBar);
            this.animationGroup.Controls.Add(this.animationSpeedBox);
            // 
            // FilePane
            // 
            this.Controls.Add(this.animationGroup);
            this.Parent = "View";
            this.Name = "Animation";
            this.Size = new System.Drawing.Size(329, 244);
            this.animationGroup.ResumeLayout(false);
            this.animationGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox flashGroup;
        private TrackBar animationBar;
        private System.Windows.Forms.NumericUpDown animationSpeedBox;
        private System.Windows.Forms.GroupBox animationGroup;
    }
}
