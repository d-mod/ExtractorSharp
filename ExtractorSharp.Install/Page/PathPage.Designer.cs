namespace ExtractorSharp.Install {
    partial class PathPage {
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
            this.tipsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(47, 128);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(277, 21);
            this.textBox1.Text = Config["InstallPath"].Value;
            this.textBox1.TabIndex = 0;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(349, 126);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 1;
            this.browseButton.Text = "浏览";
            this.browseButton.UseVisualStyleBackColor = true;
            // 
            // tipsLabel
            // 
            this.tipsLabel.AutoSize = true;
            this.tipsLabel.Location = new System.Drawing.Point(45, 103);
            this.tipsLabel.Name = "tipsLabel";
            this.tipsLabel.Size = new System.Drawing.Size(89, 12);
            this.tipsLabel.TabIndex = 4;
            this.tipsLabel.Text = "请选择安装路径";
            // 
            // PathPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tipsLabel);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.textBox1);
            this.Name = "PathPanel";
            this.Controls.SetChildIndex(this.textBox1, 0);
            this.Controls.SetChildIndex(this.browseButton, 0);
            this.Controls.SetChildIndex(this.tipsLabel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Label tipsLabel;
    }
}
