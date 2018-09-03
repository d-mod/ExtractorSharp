namespace ExtractorSharp.Install.Page {
    partial class SettingPage {
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.gamePathGroup = new System.Windows.Forms.GroupBox();
            this.tipsLabel = new System.Windows.Forms.Label();
            this.gamePathGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(17, 45);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(255, 21);
            this.textBox1.TabIndex = 0;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(278, 43);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 4;
            this.browseButton.Text = "浏览";
            this.browseButton.UseVisualStyleBackColor = true;
            // 
            // gamePathGroup
            // 
            this.gamePathGroup.Controls.Add(this.tipsLabel);
            this.gamePathGroup.Controls.Add(this.browseButton);
            this.gamePathGroup.Controls.Add(this.textBox1);
            this.gamePathGroup.Location = new System.Drawing.Point(53, 24);
            this.gamePathGroup.Name = "gamePathGroup";
            this.gamePathGroup.Size = new System.Drawing.Size(368, 113);
            this.gamePathGroup.TabIndex = 1;
            this.gamePathGroup.TabStop = false;
            this.gamePathGroup.Text = "设置游戏路径";
            // 
            // tipsLabel
            // 
            this.tipsLabel.AutoSize = true;
            this.tipsLabel.Location = new System.Drawing.Point(15, 85);
            this.tipsLabel.Name = "tipsLabel";
            this.tipsLabel.Size = new System.Drawing.Size(245, 12);
            this.tipsLabel.TabIndex = 5;
            this.tipsLabel.Text = "设置不正确可能会导致部分功能无法正常使用";
            // 
            // SettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gamePathGroup);
            this.Name = "SettingPanel";
            this.Size = new System.Drawing.Size(450, 300);
            this.gamePathGroup.ResumeLayout(false);
            this.gamePathGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.GroupBox gamePathGroup;
        private System.Windows.Forms.Label tipsLabel;
    }
}
