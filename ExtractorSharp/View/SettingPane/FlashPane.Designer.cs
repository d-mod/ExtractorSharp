using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.View.SettingPane {
    partial class FlashPane {
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
            this.flashSpeedGroup = new GroupBox();
            this.flashSpeedBar = new TrackBar();
            this.flashSpeedBox = new System.Windows.Forms.NumericUpDown();
            this.flashSpeedGroup.SuspendLayout();
            this.SuspendLayout();
            this.flashSpeedGroup.Location = new System.Drawing.Point(13, 15);
            this.flashSpeedGroup.Size = new System.Drawing.Size(380, 100);
            this.flashSpeedGroup.TabIndex = 2;
            this.flashSpeedGroup.TabStop = false;
            this.flashSpeedGroup.Text = Language["FlashSpeed"];

            // 
            // flashSpeedBar
            // 
            this.flashSpeedBar.LargeChange = 10;
            this.flashSpeedBar.Location = new System.Drawing.Point(20, 21);
            this.flashSpeedBar.Maximum = 100;
            this.flashSpeedBar.Minimum = 1;
            this.flashSpeedBar.Name = "flashSpeedBar";
            this.flashSpeedBar.Size = new System.Drawing.Size(350, 45);
            this.flashSpeedBar.TabIndex = 0;
            this.flashSpeedBar.Value = 20;
            //
            //
            //
            this.flashSpeedBar.BackColor = Color.White;
            this.flashSpeedBox.Minimum = 1;
            this.flashSpeedBox.Maximum = 100;
            this.flashSpeedBox.Value = 20;
            this.flashSpeedBox.Location = new System.Drawing.Point(120, 70);

            this.flashSpeedGroup.Controls.Add(this.flashSpeedBar);
            this.flashSpeedGroup.Controls.Add(this.flashSpeedBox);
            // 
            // FilePane
            // 
            this.Controls.Add(this.flashSpeedGroup);
            this.Parent = "View";
            this.Name = "Animation";
            this.Size = new System.Drawing.Size(329, 244);
            this.flashSpeedGroup.ResumeLayout(false);
            this.flashSpeedGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox flashGroup;
        private TrackBar flashSpeedBar;
        private System.Windows.Forms.NumericUpDown flashSpeedBox;
        private System.Windows.Forms.GroupBox flashSpeedGroup;
    }
}
