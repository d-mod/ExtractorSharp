﻿namespace ExtractorSharp.View.SettingPane {
    partial class GerneralPane {
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
            this.gamePathBox = new System.Windows.Forms.TextBox();
            this.browseButton = new ExtractorSharp.Components.ESButton();
            this.gamePathGroup = new System.Windows.Forms.GroupBox();
            this.autoSaveCheck = new System.Windows.Forms.CheckBox();
            this.autoUpdateCheck = new System.Windows.Forms.CheckBox();
            this.updatedShowFeatureCheck = new System.Windows.Forms.CheckBox();
            this.gamePathGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.gamePathBox.Location = new System.Drawing.Point(15, 37);
            this.gamePathBox.Size = new System.Drawing.Size(158, 21);
            this.gamePathBox.TabIndex = 0;
            // 
            // easeButton1
            // 
            this.browseButton.Location = new System.Drawing.Point(171, 35);
            this.browseButton.Size = new System.Drawing.Size(75, 24);
            this.browseButton.TabIndex = 1;
            this.browseButton.Text = Language["Browse"];
            this.browseButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.gamePathGroup.Controls.Add(this.gamePathBox);
            this.gamePathGroup.Controls.Add(this.browseButton);
            this.gamePathGroup.Location = new System.Drawing.Point(13, 15);
            this.gamePathGroup.Size = new System.Drawing.Size(297, 100);
            this.gamePathGroup.TabIndex = 2;
            this.gamePathGroup.TabStop = false;
            this.gamePathGroup.Text = Language["GamePath"];
            // 
            // checkBox1
            // 
            this.autoSaveCheck.AutoSize = true;
            this.autoSaveCheck.Location = new System.Drawing.Point(13, 139);
            this.autoSaveCheck.Size = new System.Drawing.Size(78, 16);
            this.autoSaveCheck.TabIndex = 3;
            this.autoSaveCheck.Text = Language["AutoSave"];
            this.autoSaveCheck.UseVisualStyleBackColor = true;

            this.autoUpdateCheck.AutoSize = true;
            this.autoUpdateCheck.Location = new System.Drawing.Point(13, 169);
            this.autoUpdateCheck.Size = new System.Drawing.Size(78, 16);
            this.autoUpdateCheck.TabIndex = 3;
            this.autoUpdateCheck.Text = Language["AutoCheckUpdate"];
            this.autoUpdateCheck.UseVisualStyleBackColor = true;


            this.updatedShowFeatureCheck.AutoSize = true;
            this.updatedShowFeatureCheck.Location = new System.Drawing.Point(13, 199);
            this.updatedShowFeatureCheck.Size = new System.Drawing.Size(78, 16);
            this.updatedShowFeatureCheck.TabIndex = 3;
            this.updatedShowFeatureCheck.Text = Language["UpdatedShowFeature"];
            this.updatedShowFeatureCheck.UseVisualStyleBackColor = true;
            // 
            // FilePane
            // 
            this.Controls.Add(this.autoSaveCheck);
            this.Controls.Add(this.autoUpdateCheck);
            this.Controls.Add(this.updatedShowFeatureCheck);
            this.Controls.Add(this.gamePathGroup);
            this.Size = new System.Drawing.Size(329, 244);
            this.gamePathGroup.ResumeLayout(false);
            this.gamePathGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox gamePathBox;
        private Components.ESButton browseButton;
        private System.Windows.Forms.GroupBox gamePathGroup;
        private System.Windows.Forms.CheckBox autoSaveCheck;
        private System.Windows.Forms.CheckBox autoUpdateCheck;
        private System.Windows.Forms.CheckBox updatedShowFeatureCheck;
    }
}
