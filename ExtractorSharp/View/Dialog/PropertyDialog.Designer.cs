namespace ExtractorSharp.View {
    partial class PropertyDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.flashGroup = new System.Windows.Forms.GroupBox();
            this.flashSpeedBar = new System.Windows.Forms.TrackBar();
            this.flashSpeedBox = new System.Windows.Forms.NumericUpDown();
            this.gamePathGroup = new System.Windows.Forms.GroupBox();
            this.autoSaveCheck = new System.Windows.Forms.CheckBox();
            this.gamePathLoadButton = new ExtractorSharp.UI.EaseButton();
            this.gamePathBox = new System.Windows.Forms.TextBox();
            this.yesButton = new ExtractorSharp.UI.EaseButton();
            this.resetButton = new UI.EaseButton();
            this.cancelButton = new ExtractorSharp.UI.EaseButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.saveImageLoadButton = new ExtractorSharp.UI.EaseButton();
            this.saveImagetipsCheck = new System.Windows.Forms.CheckBox();
            this.saveImageBox = new System.Windows.Forms.TextBox();
            this.viewGroup = new System.Windows.Forms.GroupBox();
            this.girdLabel = new System.Windows.Forms.Label();
            this.gridBox = new System.Windows.Forms.NumericUpDown();
            languageBox = new System.Windows.Forms.ComboBox();
            languageGroup = new System.Windows.Forms.GroupBox();
            saveImageAllPathCheck = new System.Windows.Forms.CheckBox();
            this.flashGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flashSpeedBar)).BeginInit();
            this.gamePathGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.viewGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBox)).BeginInit();
            this.SuspendLayout();
            // 
            // flashGroup
            // 
            this.flashGroup.Controls.Add(this.flashSpeedBar);
            this.flashGroup.Controls.Add(this.flashSpeedBox);
            this.flashGroup.Location = new System.Drawing.Point(30, 270);
            this.flashGroup.Name = "flashGroup";
            this.flashGroup.Size = new System.Drawing.Size(394, 120);
            this.flashGroup.TabIndex = 0;
            this.flashGroup.TabStop = false;
            this.flashGroup.Text = Language["FlashSpeed"];
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
            this.flashSpeedBox.Minimum = 1;
            this.flashSpeedBox.Maximum = 100;
            this.flashSpeedBox.Value = 20;
            this.flashSpeedBox.Location = new System.Drawing.Point(120,70);
            // 
            // gamePathGroup
            // 
            this.gamePathGroup.Controls.Add(this.autoSaveCheck);
            this.gamePathGroup.Controls.Add(this.gamePathLoadButton);
            this.gamePathGroup.Controls.Add(this.gamePathBox);
            this.gamePathGroup.Location = new System.Drawing.Point(30, 13);
            this.gamePathGroup.Name = "gamePathGroup";
            this.gamePathGroup.Size = new System.Drawing.Size(394, 75);
            this.gamePathGroup.TabIndex = 1;
            this.gamePathGroup.TabStop = false;
            this.gamePathGroup.Text = Language["GamePath"];
            // 
            // autoSaveCheck
            // 
            this.autoSaveCheck.AutoSize = true;
            this.autoSaveCheck.Location = new System.Drawing.Point(20, 53);
            this.autoSaveCheck.Name = "autoSaveCheck";
            this.autoSaveCheck.Size = new System.Drawing.Size(72, 16);
            this.autoSaveCheck.TabIndex = 2;
            this.autoSaveCheck.Text = Language["AutoSave"];
            this.autoSaveCheck.UseVisualStyleBackColor = true;
            // 
            // gamePathLoadButton
            // 
            this.gamePathLoadButton.Location = new System.Drawing.Point(300, 21);
            this.gamePathLoadButton.Name = "gamePathLoadButton";
            this.gamePathLoadButton.Size = new System.Drawing.Size(75, 23);
            this.gamePathLoadButton.TabIndex = 1;
            this.gamePathLoadButton.Text = Language["Browse"];
            this.gamePathLoadButton.UseVisualStyleBackColor = true;
            // 
            // gamePathBox
            // 
            this.gamePathBox.Location = new System.Drawing.Point(20, 21);
            this.gamePathBox.Name = "gamePathBox";
            this.gamePathBox.Size = new System.Drawing.Size(260, 21);
            this.gamePathBox.TabIndex = 0;
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(257, 480);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 2;
            this.yesButton.Text = Language["OK"];
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(349, 480);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = Language["Cancel"];
            this.cancelButton.UseVisualStyleBackColor = true;
            //
            //
            //
            this.resetButton.Location = new System.Drawing.Point(165, 480);
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.Text = Language["Reset"];
            this.resetButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.saveImageLoadButton);
            this.groupBox1.Controls.Add(this.saveImagetipsCheck);
            this.groupBox1.Controls.Add(this.saveImageBox);
            this.groupBox1.Controls.Add(this.saveImageAllPathCheck);
            this.groupBox1.Location = new System.Drawing.Point(30, 105);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(394, 75);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = Language["SaveImage"];
            // 
            // saveImageLoadButton
            // 
            this.saveImageLoadButton.Location = new System.Drawing.Point(300, 17);
            this.saveImageLoadButton.Name = "saveImageLoadButton";
            this.saveImageLoadButton.Size = new System.Drawing.Size(75, 23);
            this.saveImageLoadButton.TabIndex = 3;
            this.saveImageLoadButton.Text = Language["Browse"];
            this.saveImageLoadButton.UseVisualStyleBackColor = true;
            // 
            // saveImagetipsCheck
            // 
            this.saveImagetipsCheck.AutoSize = true;
            this.saveImagetipsCheck.Location = new System.Drawing.Point(20, 53);
            this.saveImagetipsCheck.Name = "saveImagetipsCheck";
            this.saveImagetipsCheck.TabIndex = 2;
            this.saveImagetipsCheck.Text = Language["SaveImageTips"];
            this.saveImagetipsCheck.UseVisualStyleBackColor = true;

            saveImageAllPathCheck.AutoSize = true;
            saveImageAllPathCheck.Location = new System.Drawing.Point(200, 53);
            saveImageAllPathCheck.Text = Language["SavePathTips"];
            // 
            // saveImageBox
            // 
            this.saveImageBox.Location = new System.Drawing.Point(20, 20);
            this.saveImageBox.Name = "saveImageBox";
            this.saveImageBox.Size = new System.Drawing.Size(260, 21);
            this.saveImageBox.TabIndex = 0;
            // 
            // viewGroup
            // 
            this.viewGroup.Controls.Add(this.girdLabel);
            this.viewGroup.Controls.Add(this.gridBox);
            this.viewGroup.Location = new System.Drawing.Point(30, 196);
            this.viewGroup.Name = "viewGroup";
            this.viewGroup.Size = new System.Drawing.Size(394, 57);
            this.viewGroup.TabIndex = 5;
            this.viewGroup.TabStop = false;
            this.viewGroup.Text = Language["View"];
            // 
            // girdLabel
            // 
            this.girdLabel.AutoSize = true;
            this.girdLabel.Location = new System.Drawing.Point(18, 19);
            this.girdLabel.Name = "girdLabel";
            this.girdLabel.Size = new System.Drawing.Size(53, 12);
            this.girdLabel.TabIndex = 1;
            this.girdLabel.Text = Language["GridGap"];
            // 
            // gridBox
            // 
            this.gridBox.Location = new System.Drawing.Point(100, 17);
            this.gridBox.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.gridBox.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.gridBox.Name = "gridBox";
            this.gridBox.Size = new System.Drawing.Size(120, 21);
            this.gridBox.TabIndex = 0;
            this.gridBox.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            //
            //
            //
            languageGroup.Location = new System.Drawing.Point(30,400);
            languageGroup.AutoSize = true;
            languageGroup.Text = Language["Language"];
            languageGroup.Controls.Add(languageBox);
            languageGroup.Size = new System.Drawing.Size(394, 69);
            languageBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            languageBox.Location = new System.Drawing.Point(30, 30);
            //
            //panel 
            //
            AutoScroll = true;
            Controls.Add(this.gamePathGroup);
            Controls.Add(this.groupBox1);
            Controls.Add(this.flashGroup);
            Controls.Add(this.viewGroup);
            Controls.Add(this.languageGroup);
            // 
            // PropertyDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(457, 520);
            Controls.Add(this.yesButton);
            Controls.Add(this.cancelButton);
            Controls.Add(resetButton);
            CancelButton = cancelButton;
            this.Name = "PropertyDialog";
            this.Text = Language["Setting"];
            this.flashGroup.ResumeLayout(false);
            this.flashGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flashSpeedBar)).EndInit();
            this.gamePathGroup.ResumeLayout(false);
            this.gamePathGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.viewGroup.ResumeLayout(false);
            this.viewGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox flashGroup;
        private System.Windows.Forms.TrackBar flashSpeedBar;
        private System.Windows.Forms.NumericUpDown flashSpeedBox;
        private System.Windows.Forms.GroupBox gamePathGroup;
        private System.Windows.Forms.TextBox gamePathBox;
        private UI.EaseButton gamePathLoadButton;
        private UI.EaseButton yesButton;
        private UI.EaseButton cancelButton;
        private UI.EaseButton resetButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox saveImageBox;
        private System.Windows.Forms.CheckBox saveImagetipsCheck;
        private System.Windows.Forms.CheckBox saveImageAllPathCheck;
        private UI.EaseButton saveImageLoadButton;
        private System.Windows.Forms.GroupBox viewGroup;
        private System.Windows.Forms.Label girdLabel;
        private System.Windows.Forms.NumericUpDown gridBox;
        private System.Windows.Forms.CheckBox autoSaveCheck;
        private System.Windows.Forms.ComboBox languageBox;
        private System.Windows.Forms.GroupBox languageGroup;
    }
}