namespace ExtractorSharp.View.SettingPane {
    partial class SaveImagePane {
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
            this.savePathBox = new System.Windows.Forms.TextBox();
            this.browseButton = new ExtractorSharp.Component.ESButton();
            this.emptyButton = new Component.ESButton();
            this.savePathGroup = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.promptCheck = new System.Windows.Forms.CheckBox();
            this.fullPathCheck = new System.Windows.Forms.CheckBox();
            this.savePathGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.savePathBox.Location = new System.Drawing.Point(15, 37);
            this.savePathBox.Size = new System.Drawing.Size(158, 21);
            this.savePathBox.TabIndex = 0;
            // 
            // easeButton1
            // 
            this.browseButton.Location = new System.Drawing.Point(171, 35);
            this.browseButton.Size = new System.Drawing.Size(75, 24);
            this.browseButton.TabIndex = 1;
            this.browseButton.Text = Language["Browse"];
            this.browseButton.UseVisualStyleBackColor = true;
            //
            //
            //
            this.emptyButton.Location = new System.Drawing.Point(245, 35);
            this.emptyButton.Size = new System.Drawing.Size(75, 24);
            this.emptyButton.TabIndex = 1;
            this.emptyButton.Text = Language["Empty"];
            this.emptyButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.savePathGroup.Controls.Add(this.savePathBox);
            this.savePathGroup.Controls.Add(this.browseButton);
            this.savePathGroup.Controls.Add(this.emptyButton);
            this.savePathGroup.Location = new System.Drawing.Point(13, 15);
            this.savePathGroup.Size = new System.Drawing.Size(350, 100);
            this.savePathGroup.TabIndex = 2;
            this.savePathGroup.TabStop = false;
            this.savePathGroup.Text = Language["SaveImagePath"];
            // 
            // checkBox1
            // 
            this.promptCheck.AutoSize = true;
            this.promptCheck.Location = new System.Drawing.Point(13, 139);
            this.promptCheck.Size = new System.Drawing.Size(78, 16);
            this.promptCheck.TabIndex = 3;
            this.promptCheck.Text = Language["SaveImageTips"];
            this.promptCheck.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.fullPathCheck.AutoSize = true;
            this.fullPathCheck.Location = new System.Drawing.Point(13, 186);
            this.fullPathCheck.Size = new System.Drawing.Size(78, 16);
            this.fullPathCheck.Text = Language["SavePathTips"];
            this.fullPathCheck.UseVisualStyleBackColor = true;
            // 
            // FilePane
            // 
            this.Controls.Add(this.fullPathCheck);
            this.Controls.Add(this.promptCheck);
            this.Controls.Add(this.savePathGroup);
            this.Parent = "File";
            this.Name = "SaveImage";
            this.Size = new System.Drawing.Size(329, 244);
            this.savePathGroup.ResumeLayout(false);
            this.savePathGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox savePathBox;
        private Component.ESButton emptyButton;
        private Component.ESButton browseButton;
        private System.Windows.Forms.GroupBox savePathGroup;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox promptCheck;
        private System.Windows.Forms.CheckBox fullPathCheck;
    }
}
