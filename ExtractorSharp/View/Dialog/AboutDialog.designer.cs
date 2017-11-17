using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.View{
    partial class AboutDialog {
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
            nameLabel = new Label();
            otherBox = new RichTextBox();
            SuspendLayout();
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new Point(90, 50);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new Size(185, 12);
            nameLabel.TabIndex = 0;
            nameLabel.Text = ProductName+" ver" + Program.Version;


            otherBox.Location = new Point(20, 250);
            otherBox.Size = new Size(300,200);
            otherBox.AutoSize = true;
            otherBox.BackColor = BackColor;
            otherBox.ForeColor = Color.Green;
            otherBox.ReadOnly = true;
            otherBox.BorderStyle = BorderStyle.None;
            // 
            // AboutForm
            // 
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            Size = new Size(400, 500);
            Controls.Add(otherBox);
            Controls.Add(nameLabel);
            Name = "AboutForm";
            Text = Language["About"];
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Label nameLabel;
        private RichTextBox otherBox;
    }
}