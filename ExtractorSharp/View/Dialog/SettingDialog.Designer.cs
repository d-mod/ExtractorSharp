using System.Drawing;
using System.Windows.Forms;

namespace ExtractorSharp.View.Dialog {
    partial class SettingDialog {
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
            this.tree = new TreeView();
            this.panel = new Panel();
            this.applyButton = new Component.ESButton();
            this.yesButton = new ExtractorSharp.Component.ESButton();
            this.resetButton = new Component.ESButton();
            this.cancelButton = new ExtractorSharp.Component.ESButton();
            this.SuspendLayout();
            // 
            // easeTabPanel1
            // 
            this.tree.Location = new Point(20, 40);
            this.tree.Size = new System.Drawing.Size(150, 380);
            this.tree.TabIndex = 0;
            //
            //
            //
            this.panel.Size = new System.Drawing.Size(400, 340);
            this.panel.BackColor = this.BackColor;
            this.panel.BorderStyle = BorderStyle.FixedSingle;
            this.panel.Location = new System.Drawing.Point(200,40);
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(410, 390);
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 2;
            this.yesButton.Text = Language["OK"];
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(515, 390);
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = Language["Cancel"];
            //
            //
            //
            this.resetButton.Location = new System.Drawing.Point(305, 390);
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.Text = Language["Reset"];
            //
            //
            //
            this.applyButton.Location = new System.Drawing.Point(200, 390);
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.Text = Language["Apply"];
            // 
            // SettingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 440);
            this.Controls.Add(this.tree);
            this.Controls.Add(this.panel);
            this.Controls.Add(resetButton);
            this.Controls.Add(applyButton);
            this.Controls.Add(yesButton);
            this.Controls.Add(cancelButton);
            this.Text = Language["Setting"];
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private TreeView tree;
        private Component.ESButton applyButton;
        private Component.ESButton yesButton;
        private Component.ESButton cancelButton;
        private Component.ESButton resetButton;
        private Panel panel;
    }
}