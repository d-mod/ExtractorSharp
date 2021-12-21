using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Components;

namespace ExtractorSharp.View.Dialog{
    partial class ChangePositonDialog {
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
            this.x_box = new System.Windows.Forms.NumericUpDown();
            this.y_box = new System.Windows.Forms.NumericUpDown();
            this.max_width_box = new System.Windows.Forms.NumericUpDown();
            this.max_height_box = new System.Windows.Forms.NumericUpDown();
            this.x_radio = new CheckBox();
            this.y_radio = new CheckBox();
            this.max_width_radio = new CheckBox();
            this.max_height_radio = new CheckBox();
            this.yesButton = new ESButton();
            this.realativePositionCheck = new CheckBox();
            this.cancelButton = new ESButton();
            this.propertiesGroup = new GroupBox();
            this.allImageCheck = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.x_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.y_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.max_width_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.max_height_box)).BeginInit();
            this.SuspendLayout();
            // 
            // x_box
            // 
            this.x_box.Location = new System.Drawing.Point(90, 30);
            this.x_box.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.x_box.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.x_box.Name = "x_box";
            this.x_box.Size = new System.Drawing.Size(122, 21);
            this.x_box.TabIndex = 0;
            // 
            // y_box
            // 
            this.y_box.Location = new System.Drawing.Point(90, 85);
            this.y_box.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.y_box.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.y_box.Name = "y_box";
            this.y_box.Size = new System.Drawing.Size(122, 21);
            this.y_box.TabIndex = 1;
            // 
            // max_width_box
            // 
            this.max_width_box.Location = new System.Drawing.Point(340, 30);
            this.max_width_box.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.max_width_box.Name = "max_width_box";
            this.max_width_box.Size = new System.Drawing.Size(122, 21);
            this.max_width_box.TabIndex = 2;
            // 
            // max_height_box
            // 
            this.max_height_box.Location = new System.Drawing.Point(340, 85);
            this.max_height_box.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.max_height_box.Name = "max_height_box";
            this.max_height_box.Size = new System.Drawing.Size(122, 21);
            this.max_height_box.TabIndex = 3;
            // 
            // x_radio
            // 
            this.x_radio.AutoSize = true;
            this.x_radio.Location = new System.Drawing.Point(30, 32);
            this.x_radio.Name = "x_radio";
            this.x_radio.Size = new System.Drawing.Size(30, 16);
            this.x_radio.TabIndex = 4;
            this.x_radio.Text = "X";
            this.x_radio.UseVisualStyleBackColor = true;
            // 
            // y_radio
            // 
            this.y_radio.AutoSize = true;
            this.y_radio.Location = new System.Drawing.Point(30, 87);
            this.y_radio.Name = "y_radio";
            this.y_radio.Size = new System.Drawing.Size(30, 16);
            this.y_radio.TabIndex = 5;
            this.y_radio.Text = "Y";
            this.y_radio.UseVisualStyleBackColor = true;
            // 
            // max_width_radio
            // 
            this.max_width_radio.AutoSize = true;
            this.max_width_radio.Location = new System.Drawing.Point(250, 32);
            this.max_width_radio.Name = "max_width_radio";
            this.max_width_radio.Size = new System.Drawing.Size(60, 16);
            this.max_width_radio.TabIndex = 6;
            this.max_width_radio.Text = Language["FrameWidth"];
            this.max_width_radio.UseVisualStyleBackColor = true;
            // 
            // max_height_radio
            // 
            this.max_height_radio.AutoSize = true;
            this.max_height_radio.Location = new System.Drawing.Point(250, 87);
            this.max_height_radio.Name = "max_height_radio";
            this.max_height_radio.Size = new System.Drawing.Size(60, 16);
            this.max_height_radio.TabIndex = 7;
            this.max_height_radio.Text = Language["FrameHeight"];
            this.max_height_radio.UseVisualStyleBackColor = true;

            propertiesGroup.Location = new Point(25, 46);
            propertiesGroup.Size = new Size(500, 125);
            propertiesGroup.Controls.Add(this.max_height_radio);
            propertiesGroup.Controls.Add(this.max_width_radio);
            propertiesGroup.Controls.Add(this.y_radio);
            propertiesGroup.Controls.Add(this.x_radio);
            propertiesGroup.Controls.Add(this.max_height_box);
            propertiesGroup.Controls.Add(this.max_width_box);
            propertiesGroup.Controls.Add(this.y_box);
            propertiesGroup.Controls.Add(this.x_box);
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(247, 275);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 8;
            this.yesButton.Text = Language["OK"];
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // checkbox
            // 
            this.realativePositionCheck.AutoSize = true;
            this.realativePositionCheck.Location = new System.Drawing.Point(140, 200);
            this.realativePositionCheck.Name = "checkbox";
            this.realativePositionCheck.Size = new System.Drawing.Size(72, 16);
            this.realativePositionCheck.TabIndex = 9;
            this.realativePositionCheck.Text = Language["RealativePosition"];
            this.realativePositionCheck.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(362, 275);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = Language["Cancel"];
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // allImageRadio
            // 
            this.allImageCheck.AutoSize = true;
            this.allImageCheck.Size = new System.Drawing.Size(72, 16);
            this.allImageCheck.Location = new System.Drawing.Point(35, 200);
            this.allImageCheck.TabIndex = 1;
            this.allImageCheck.Text = Language["AllImage"];
            this.allImageCheck.UseVisualStyleBackColor = true;

            // 
            // ChangePositonDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(550, 330);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.allImageCheck);
            this.Controls.Add(this.realativePositionCheck);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.propertiesGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ChangePositonDialog";
            this.Text = Language["ChangeImagePosition"];
            ((System.ComponentModel.ISupportInitialize)(this.x_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.y_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.max_width_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.max_height_box)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private NumericUpDown x_box;
        private NumericUpDown y_box;
        private NumericUpDown max_width_box;
        private NumericUpDown max_height_box;
        private CheckBox x_radio;
        private CheckBox y_radio;
        private CheckBox max_width_radio;
        private CheckBox max_height_radio;
        private Button yesButton;
        private CheckBox realativePositionCheck;
        private Button cancelButton;
        private GroupBox propertiesGroup;
        private CheckBox allImageCheck;
    }
}