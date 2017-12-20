namespace ExtractorSharp.View.Dialog{
    partial class VersionDialog {
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
            this.box = new System.Windows.Forms.TextBox();
            this.button = new ExtractorSharp.Component.EaseButton();
            this.SuspendLayout();
            // 
            // box
            // 
            this.box.Location = new System.Drawing.Point(26, 22);
            this.box.Multiline = true;
            this.box.Name = "box";
            this.box.ReadOnly = true;
            this.box.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.box.Size = new System.Drawing.Size(235, 217);
            this.box.TabIndex = 0;
            // 
            // button
            // 
            this.button.Location = new System.Drawing.Point(100, 245);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(75, 23);
            this.button.TabIndex = 1;
            this.button.Text = Language["OK"];
            this.button.UseVisualStyleBackColor = true;
            // 
            // VersionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 277);
            this.Controls.Add(this.button);
            this.Controls.Add(this.box);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "VersionDialog";
            this.Text = Language["Features"];
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox box;
        private Component.EaseButton button;
    }
}