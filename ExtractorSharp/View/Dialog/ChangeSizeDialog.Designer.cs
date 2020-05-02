namespace ExtractorSharp.View.Dialog {
    partial class ChangeSizeDialog {
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
            this.customRadio = new System.Windows.Forms.RadioButton();
            this.widthLabel = new System.Windows.Forms.Label();
            this.heightLabel = new System.Windows.Forms.Label();
            this.widthBox = new System.Windows.Forms.NumericUpDown();
            this.heightBox = new System.Windows.Forms.NumericUpDown();
            this.trimRadio = new System.Windows.Forms.RadioButton();
            this.group = new System.Windows.Forms.GroupBox();
            this.scaleRadio = new System.Windows.Forms.RadioButton();
            this.scaleBox = new System.Windows.Forms.NumericUpDown();
            this.useDefaultBox = new System.Windows.Forms.CheckBox();
            this.yesButton = new ExtractorSharp.Component.ESButton();
            this.cancelButton = new ExtractorSharp.Component.ESButton();
            ((System.ComponentModel.ISupportInitialize)(this.widthBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightBox)).BeginInit();
            this.group.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleBox)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButton1
            // 
            this.customRadio.AutoSize = true;
            this.customRadio.Location = new System.Drawing.Point(36, 32);
            this.customRadio.Name = "radioButton1";
            this.customRadio.Size = new System.Drawing.Size(59, 16);
            this.customRadio.TabIndex = 0;
            this.customRadio.TabStop = true;
            this.customRadio.Text = Language["CustomFrameSize"];
            this.customRadio.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(56, 68);
            this.widthLabel.Name = "label1";
            this.widthLabel.Size = new System.Drawing.Size(17, 12);
            this.widthLabel.TabIndex = 1;
            this.widthLabel.Text = Language["Width"];
            // 
            // label2
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(219, 68);
            this.heightLabel.Name = "label2";
            this.heightLabel.Size = new System.Drawing.Size(17, 12);
            this.heightLabel.TabIndex = 3;
            this.heightLabel.Text = Language["Height"];
            // 
            // numericUpDown1
            // 
            this.widthBox.Location = new System.Drawing.Point(79, 66);
            this.widthBox.Name = "numericUpDown1";
            this.widthBox.Size = new System.Drawing.Size(120, 21);
            this.widthBox.TabIndex = 5;
            this.widthBox.Minimum = 1;
            this.widthBox.Maximum = 2000;
            // 
            // numericUpDown2
            // 
            this.heightBox.Location = new System.Drawing.Point(242, 66);
            this.heightBox.Name = "numericUpDown2";
            this.heightBox.Size = new System.Drawing.Size(120, 21);
            this.heightBox.TabIndex = 6;
            this.heightBox.Minimum = 1;
            this.heightBox.Maximum = 2000;
            // 
            // radioButton2
            // 
            this.trimRadio.AutoSize = true;
            this.trimRadio.Location = new System.Drawing.Point(36, 171);
            this.trimRadio.Name = "radioButton2";
            this.trimRadio.Size = new System.Drawing.Size(71, 16);
            this.trimRadio.TabIndex = 7;
            this.trimRadio.TabStop = true;
            this.trimRadio.Text = Language["TrimImage"];
            this.trimRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.group.Controls.Add(this.useDefaultBox);
            this.group.Controls.Add(this.scaleBox);
            this.group.Controls.Add(this.scaleRadio);
            this.group.Controls.Add(this.trimRadio);
            this.group.Controls.Add(this.heightBox);
            this.group.Controls.Add(this.widthBox);
            this.group.Controls.Add(this.heightLabel);
            this.group.Controls.Add(this.widthLabel);
            this.group.Controls.Add(this.customRadio);
            this.group.Location = new System.Drawing.Point(67, 37);
            this.group.Name = "groupBox1";
            this.group.Size = new System.Drawing.Size(396, 211);
            this.group.TabIndex = 0;
            this.group.TabStop = false;
            
            // 
            // radioButton3
            // 
            this.scaleRadio.AutoSize = true;
            this.scaleRadio.Location = new System.Drawing.Point(36, 102);
            this.scaleRadio.Name = "radioButton3";
            this.scaleRadio.Size = new System.Drawing.Size(83, 16);
            this.scaleRadio.TabIndex = 8;
            this.scaleRadio.TabStop = true;
            this.scaleRadio.Text = $"{Language["CanvasScale"]}(%)";
            this.scaleRadio.UseVisualStyleBackColor = true;
            // 
            // numericUpDown3
            // 
            this.scaleBox.Location = new System.Drawing.Point(79, 127);
            this.scaleBox.Name = "numericUpDown3";
            this.scaleBox.Size = new System.Drawing.Size(120, 21);
            this.scaleBox.TabIndex = 10;
            this.scaleBox.Minimum = 20;
            this.scaleBox.Maximum = 10000;
            // 
            // checkBox1
            // 
            this.useDefaultBox.AutoSize = true;
            this.useDefaultBox.Location = new System.Drawing.Point(227, 129);
            this.useDefaultBox.Name = "checkBox1";
            this.useDefaultBox.Size = new System.Drawing.Size(96, 16);
            this.useDefaultBox.TabIndex = 11;
            this.useDefaultBox.Text = Language["UseDefaultScale"];
            this.useDefaultBox.UseVisualStyleBackColor = true;
            // 
            // esButton1
            // 
            this.yesButton.Location = new System.Drawing.Point(247, 275);
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 1;
            this.yesButton.Text = Language["OK"];
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // esButton2
            // 
            this.cancelButton.Location = new System.Drawing.Point(362, 275);
            this.cancelButton.Name = "esButton2";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = Language["Cancel"];
            this.cancelButton.UseVisualStyleBackColor = true;
            this.CancelButton = cancelButton;
            // 
            // ChangeSizeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 330);
            this.Controls.Add(this.group);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.yesButton);
            this.Text = Language["ChangeImageSize"];
            ((System.ComponentModel.ISupportInitialize)(this.widthBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightBox)).EndInit();
            this.group.ResumeLayout(false);
            this.group.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton customRadio;
        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.NumericUpDown widthBox;
        private System.Windows.Forms.NumericUpDown heightBox;
        private System.Windows.Forms.NumericUpDown xBox;
        private System.Windows.Forms.NumericUpDown yBox;
        private System.Windows.Forms.RadioButton trimRadio;
        private System.Windows.Forms.GroupBox group;
        private System.Windows.Forms.RadioButton scaleRadio;
        private System.Windows.Forms.NumericUpDown scaleBox;
        private System.Windows.Forms.CheckBox useDefaultBox;
        private Component.ESButton yesButton;
        private Component.ESButton cancelButton;
    }
}