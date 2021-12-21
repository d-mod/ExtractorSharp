namespace ExtractorSharp.View.SettingPane {
    partial class GridPane {
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
            this.gridGapBox = new System.Windows.Forms.NumericUpDown();
            this.gridGapGroup = new System.Windows.Forms.GroupBox();
            this.gridGapGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.gridGapBox.Location = new System.Drawing.Point(15, 37);
            this.gridGapBox.AutoSize = true;
            this.gridGapBox.Maximum = 500;
            this.gridGapBox.Minimum = 1;
            this.gridGapBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.gridGapGroup.Controls.Add(this.gridGapBox);
            this.gridGapGroup.Location = new System.Drawing.Point(13, 15);
            this.gridGapGroup.Size = new System.Drawing.Size(297, 100);
            this.gridGapGroup.TabIndex = 2;
            this.gridGapGroup.TabStop = false;
         //   this.gridGapGroup.Text = Language["GridGap"];
            // 
            // FilePane
            // 
            this.Controls.Add(this.gridGapGroup);
          //  this.Parent = "View";
        //    this.Name = "Grid";
            this.Size = new System.Drawing.Size(329, 244);
            this.gridGapGroup.ResumeLayout(false);
            this.gridGapGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown gridGapBox;
        private System.Windows.Forms.GroupBox gridGapGroup;
    }
}
