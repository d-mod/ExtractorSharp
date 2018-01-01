using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Component;

namespace ExtractorSharp.View{
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
            this.checkbox = new CheckBox();
            this.cancelButton = new ESButton();
            this.group = new GroupBox();
            this.allImageRadio = new RadioButton();
            this.checkImageRadio = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.x_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.y_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.max_width_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.max_height_box)).BeginInit();
            this.group.SuspendLayout();
            this.SuspendLayout();
            // 
            // x_box
            // 
            this.x_box.Location = new System.Drawing.Point(127, 127);
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
            this.y_box.Location = new System.Drawing.Point(127, 163);
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
            this.max_width_box.Location = new System.Drawing.Point(127, 204);
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
            this.max_height_box.Location = new System.Drawing.Point(127, 246);
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
            this.x_radio.Location = new System.Drawing.Point(45, 132);
            this.x_radio.Name = "x_radio";
            this.x_radio.Size = new System.Drawing.Size(30, 16);
            this.x_radio.TabIndex = 4;
            this.x_radio.Text = "X";
            this.x_radio.UseVisualStyleBackColor = true;
            // 
            // y_radio
            // 
            this.y_radio.AutoSize = true;
            this.y_radio.Location = new System.Drawing.Point(44, 168);
            this.y_radio.Name = "y_radio";
            this.y_radio.Size = new System.Drawing.Size(30, 16);
            this.y_radio.TabIndex = 5;
            this.y_radio.Text = "Y";
            this.y_radio.UseVisualStyleBackColor = true;
            // 
            // max_width_radio
            // 
            this.max_width_radio.AutoSize = true;
            this.max_width_radio.Location = new System.Drawing.Point(44, 209);
            this.max_width_radio.Name = "max_width_radio";
            this.max_width_radio.Size = new System.Drawing.Size(60, 16);
            this.max_width_radio.TabIndex = 6;
            this.max_width_radio.Text = Language["CavasWidth"];
            this.max_width_radio.UseVisualStyleBackColor = true;
            // 
            // max_height_radio
            // 
            this.max_height_radio.AutoSize = true;
            this.max_height_radio.Location = new System.Drawing.Point(44, 251);
            this.max_height_radio.Name = "max_height_radio";
            this.max_height_radio.Size = new System.Drawing.Size(60, 16);
            this.max_height_radio.TabIndex = 7;
            this.max_height_radio.Text = Language["CavasHeight"];
            this.max_height_radio.UseVisualStyleBackColor = true;
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(41, 320);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 8;
            this.yesButton.Text = Language["OK"];
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // checkbox
            // 
            this.checkbox.AutoSize = true;
            this.checkbox.Location = new System.Drawing.Point(44, 298);
            this.checkbox.Name = "checkbox";
            this.checkbox.Size = new System.Drawing.Size(72, 16);
            this.checkbox.TabIndex = 9;
            this.checkbox.Text = Language["RealativePosition"];
            this.checkbox.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(174, 320);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = Language["Cancel"];
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.group.Controls.Add(this.allImageRadio);
            this.group.Controls.Add(this.checkImageRadio);
            this.group.Location = new System.Drawing.Point(45, 46);
            this.group.Name = "groupBox1";
            this.group.Size = new System.Drawing.Size(181, 75);
            this.group.TabIndex = 11;
            this.group.TabStop = false;
            this.group.Text = Language["HandleObject"];
            // 
            // allImageRadio
            // 
            this.allImageRadio.AutoSize = true;
            this.allImageRadio.Location = new System.Drawing.Point(6, 42);
            this.allImageRadio.Name = "allImageRadio";
            this.allImageRadio.Size = new System.Drawing.Size(71, 16);
            this.allImageRadio.TabIndex = 1;
            this.allImageRadio.Text = Language["AllImage"];
            this.allImageRadio.UseVisualStyleBackColor = true;
            // 
            // checkImageRadio
            // 
            this.checkImageRadio.AutoSize = true;
            this.checkImageRadio.Checked = true;
            this.checkImageRadio.Location = new System.Drawing.Point(6, 20);
            this.checkImageRadio.Name = "checkImageRadio";
            this.checkImageRadio.Size = new System.Drawing.Size(71, 16);
            this.checkImageRadio.TabIndex = 0;
            this.checkImageRadio.TabStop = true;
            this.checkImageRadio.Text = Language["CheckImage"];
            this.checkImageRadio.UseVisualStyleBackColor = true;
            // 
            // ChangePositonDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(284, 370);
            this.Controls.Add(this.group);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.checkbox);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.max_height_radio);
            this.Controls.Add(this.max_width_radio);
            this.Controls.Add(this.y_radio);
            this.Controls.Add(this.x_radio);
            this.Controls.Add(this.max_height_box);
            this.Controls.Add(this.max_width_box);
            this.Controls.Add(this.y_box);
            this.Controls.Add(this.x_box);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ChangePositonDialog";
            this.Text = Language["ChangeImagePosition"];
            ((System.ComponentModel.ISupportInitialize)(this.x_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.y_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.max_width_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.max_height_box)).EndInit();
            this.group.ResumeLayout(false);
            this.group.PerformLayout();
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
        private CheckBox checkbox;
        private Button cancelButton;
        private GroupBox group;
        private RadioButton checkImageRadio;
        private RadioButton allImageRadio;
    }
}