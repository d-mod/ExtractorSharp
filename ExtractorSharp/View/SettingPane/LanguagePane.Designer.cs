namespace ExtractorSharp.View.SettingPane {
    partial class LanguagePane {
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
            this.languageBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.languageBox.FormattingEnabled = true;
            this.languageBox.Location = new System.Drawing.Point(26, 24);
            this.languageBox.Name = "comboBox1";
            this.languageBox.Size = new System.Drawing.Size(159, 20);
            this.languageBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageBox.TabIndex = 0;
            // 
            // LanguagePane
            // 
            this.Controls.Add(this.languageBox);
            this.Parent = "Language";
            this.Size = new System.Drawing.Size(241, 151);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox languageBox;
    }
}
