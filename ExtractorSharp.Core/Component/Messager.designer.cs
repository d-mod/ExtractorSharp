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
            button = new Button();
            SuspendLayout();
            //            
            // box
            // 
            label.Location = new Point(0, 0);
            label.Name = "box";
            label.Size = new Size(200, 50);
            label.TabIndex = 1;
            label.Text = "";
            label.BackColor = Color.Transparent;
            // 
            // button
            // 
            button.Location = new Point(200, 15);
            button.Name = "button";
            button.Size = new Size(50, 20);
            button.TabIndex = 2;
            button.Text = Language["OK"];
            button.UseVisualStyleBackColor = true;
            // 
            // MessageForm
            // 
            ClientSize = new Size(250, 50);
            Controls.Add(button);
            Controls.Add(label);
            Name = "MessageForm";
            ResumeLayout(false);
            Visible = false;
        }

        #endregion
        private Label label;
        private Button button;
    }
}