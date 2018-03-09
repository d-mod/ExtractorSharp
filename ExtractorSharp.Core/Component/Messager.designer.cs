using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.Core{
    partial class Messager {
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
            iconLabel = new Label();
            button = new Button();
            SuspendLayout();

            iconLabel.Location = new Point(24, 24);
            iconLabel.Name = "box";
            iconLabel.Size = new Size(32, 32);
            iconLabel.TabIndex = 1;
            iconLabel.BackColor = Color.Transparent;
            //            
            // box
            // 
            label.Location = new Point(72, 32);
            label.Name = "box";
            label.Size = new Size(200, 50);
            label.TabIndex = 1;
            label.BackColor = Color.Transparent;
            // 
            // button
            // 
            button.Location = new Point(215, 25);
            button.Name = "button";
            button.Size = new Size(75, 25);
            button.TabIndex = 2;
            button.Text = Language["OK"];
            button.UseVisualStyleBackColor = true;
            // 
            // MessageForm
            // 
            ClientSize = new Size(250, 50);
            Controls.Add(button);
            Controls.Add(label);
            Controls.Add(iconLabel);
            BackColor = Color.FromArgb(250,250,250,250);
            Name = "MessageForm";
            ResumeLayout(false);
            ClientSize = new Size(300, 75);
            BorderStyle = BorderStyle.FixedSingle;
            Visible = false;
        }

        #endregion
        private Label iconLabel;
        private Label label;
        private Button button;
       
    }
}