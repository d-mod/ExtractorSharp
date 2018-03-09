using System.Windows.Forms;

namespace ExtractorSharp.View{
    partial class CanvasDialog {
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

        private void InitializeComponent() {
            this.width_box = new System.Windows.Forms.NumericUpDown();
            this.height_box = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.yesButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.width_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.height_box)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDown1
            //        
            this.width_box.Location = new System.Drawing.Point(90, 17);
            this.width_box.Size = new System.Drawing.Size(88, 21);
            this.width_box.TabIndex = 0;
            this.width_box.Maximum = 9999;
            this.width_box.Minimum = 1;
            // 
            // numericUpDown2
            // 
            this.height_box.Location = new System.Drawing.Point(184, 17);
            this.height_box.Size = new System.Drawing.Size(88, 21);
            this.height_box.TabIndex = 1;
            this.height_box.Maximum = 9999;
            this.height_box.Minimum = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = Language["Size"];
            // 
            // button1
            // 
            this.yesButton.Location = new System.Drawing.Point(23, 61);
            this.yesButton.Name = "button1";
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 3;
            this.yesButton.Text = Language["OK"];
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.cancelButton.Location = new System.Drawing.Point(184, 61);
            this.cancelButton.Name = "button2";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = Language["Cancel"];
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // BatCavas
            // 
            this.ClientSize = new System.Drawing.Size(284, 96);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.height_box);
            this.Controls.Add(this.width_box);
            this.Text = Language["CanvasImage"];
            ((System.ComponentModel.ISupportInitialize)(this.width_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.height_box)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private NumericUpDown height_box;
        private Label label1;
        private Button yesButton;
        private Button cancelButton;
        private NumericUpDown width_box;

    }
}