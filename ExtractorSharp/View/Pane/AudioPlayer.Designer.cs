namespace ExtractorSharp.View {
    partial class AudioPlayer {
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
            this.playButton = new System.Windows.Forms.Button();
            this.pauseButton = new ExtractorSharp.Component.ESButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.timeLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(68, 142);
            this.playButton.Margin = new System.Windows.Forms.Padding(0);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(76, 28);
            this.playButton.TabIndex = 1;
            this.playButton.Text = Language["Play"];
            this.playButton.UseVisualStyleBackColor = true;
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(234, 142);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(76, 28);
            this.pauseButton.TabIndex = 2;
            this.pauseButton.Text = Language["Pause"];
            this.pauseButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightBlue;
            this.groupBox1.Controls.Add(this.timeLabel);
            this.groupBox1.Controls.Add(this.playButton);
            this.groupBox1.Controls.Add(this.pauseButton);
            this.groupBox1.Location = new System.Drawing.Point(211, 165);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(365, 199);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(248, 114);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(35, 12);
            this.timeLabel.TabIndex = 3;
            this.timeLabel.Text = "00:00";
            // 
            // OggPlayer
            // 
            this.Visible = false;
            this.BackColor = System.Drawing.Color.Gray;
            this.Controls.Add(this.groupBox1);
            this.Location = new System.Drawing.Point(230, 90);
            this.Name = "OggPlayer";
            this.Size = new System.Drawing.Size(800, 600);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button playButton;
        private Component.ESButton pauseButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label timeLabel;
    }
}
