using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.View.Dialog{
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
            label = new Label();
            SuspendLayout();
            // 
            // nameLabel
            // 
            label.AutoSize = true;
            label.Location = new Point(105, 75);
            label.Name = "nameLabel";
            label.Size = new Size(185, 12);
            label.TabIndex = 0;
            label.Text = $"{ProductName} ver{Program.Version}\r\n\t  Copyright by Kritsu";

            
            // 
            // AboutForm
            // 
            AutoScaleDimensions = new SizeF(6F, 12F);
            AutoScaleMode = AutoScaleMode.Font;
            Size = new Size(400, 200);
            Controls.Add(label);
            Name = "AboutForm";
            Text = Language["About"];
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Label label;
    }
}