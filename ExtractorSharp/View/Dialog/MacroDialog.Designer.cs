namespace ExtractorSharp.View {
    partial class MacroDialog {
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
            this.cancelButton = new ExtractorSharp.UI.EaseButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.recordItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadScriptItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveScriptItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox = new System.Windows.Forms.RichTextBox();
            this.runButton = new ExtractorSharp.UI.EaseButton();
            this.imgGroup = new System.Windows.Forms.GroupBox();
            this.selectImgRadio = new System.Windows.Forms.RadioButton();
            this.allImgRadio = new System.Windows.Forms.RadioButton();
            this.imageGroup = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.allImageRadio = new System.Windows.Forms.RadioButton();
            this.menuStrip1.SuspendLayout();
            this.imgGroup.SuspendLayout();
            this.imageGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(437, 323);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = Language["Cancel"];
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recordItem,
            this.stopItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(524, 25);
            this.menuStrip1.TabIndex = 7;
            // 
            // recordItem
            // 
            this.recordItem.Name = "recordItem";
            this.recordItem.Size = new System.Drawing.Size(44, 21);
            this.recordItem.Text = Language["Record"];
            // 
            // stopItem
            // 
            this.stopItem.Name = "stopItem";
            this.stopItem.Size = new System.Drawing.Size(44, 21);
            this.stopItem.Text = Language["Pause"];
            this.stopItem.Enabled = false;
            // 
            // loadScriptItem
            // 
            this.loadScriptItem.Name = "loadScriptItem";
            this.loadScriptItem.Size = new System.Drawing.Size(68, 21);
            this.loadScriptItem.Text = Language["LoadScript"];
            // 
            // SaveScriptItem
            // 
            this.SaveScriptItem.Name = "SaveScriptItem";
            this.SaveScriptItem.Size = new System.Drawing.Size(68, 21);
            this.SaveScriptItem.Text = Language["SaveScript"];
            // 
            // textBox
            // 
            this.textBox.BackColor = System.Drawing.Color.Black;
            this.textBox.ForeColor = System.Drawing.Color.White;
            this.textBox.Location = new System.Drawing.Point(0, 28);
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.Size = new System.Drawing.Size(524, 246);
            this.textBox.TabIndex = 8;
            this.textBox.Text = "";
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(347, 323);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(75, 23);
            this.runButton.TabIndex = 5;
            this.runButton.Text = Language["Run"];
            this.runButton.UseVisualStyleBackColor = true;
            // 
            // imgGroup
            // 
            this.imgGroup.Controls.Add(this.allImgRadio);
            this.imgGroup.Controls.Add(this.selectImgRadio);
            this.imgGroup.Location = new System.Drawing.Point(12, 289);
            this.imgGroup.Name = "imgGroup";
            this.imgGroup.Size = new System.Drawing.Size(142, 76);
            this.imgGroup.TabIndex = 9;
            this.imgGroup.TabStop = false;
            this.imgGroup.Text = Language["HandleImg"];
            // 
            // selectImgRadio
            // 
            this.selectImgRadio.AutoSize = true;
            this.selectImgRadio.Checked = true;
            this.selectImgRadio.Location = new System.Drawing.Point(7, 21);
            this.selectImgRadio.Name = "selectImgRadio";
            this.selectImgRadio.Size = new System.Drawing.Size(65, 16);
            this.selectImgRadio.TabIndex = 0;
            this.selectImgRadio.TabStop = true;
            this.selectImgRadio.Text = Language["CheckImg"];
            this.selectImgRadio.UseVisualStyleBackColor = true;
            // 
            // allImgRadio
            // 
            this.allImgRadio.AutoSize = true;
            this.allImgRadio.Location = new System.Drawing.Point(7, 41);
            this.allImgRadio.Name = "allImgRadio";
            this.allImgRadio.Size = new System.Drawing.Size(65, 16);
            this.allImgRadio.TabIndex = 1;
            this.allImgRadio.TabStop = true;
            this.allImgRadio.Text = Language["AllImg"];
            this.allImgRadio.UseVisualStyleBackColor = true;
            // 
            // imageGroup
            // 
            this.imageGroup.Controls.Add(this.allImageRadio);
            this.imageGroup.Controls.Add(this.radioButton1);
            this.imageGroup.Location = new System.Drawing.Point(173, 290);
            this.imageGroup.Name = "imageGroup";
            this.imageGroup.Size = new System.Drawing.Size(158, 75);
            this.imageGroup.TabIndex = 10;
            this.imageGroup.TabStop = false;
            this.imageGroup.Text = Language["HandleImage"];
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(7, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(71, 16);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = Language["CheckImage"];
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // allImageRadio
            // 
            this.allImageRadio.AutoSize = true;
            this.allImageRadio.Location = new System.Drawing.Point(7, 42);
            this.allImageRadio.Name = "allImageRadio";
            this.allImageRadio.Size = new System.Drawing.Size(71, 16);
            this.allImageRadio.TabIndex = 1;
            this.allImageRadio.TabStop = true;
            this.allImageRadio.Text = Language["AllImage"];
            this.allImageRadio.UseVisualStyleBackColor = true;
            // 
            // MacroDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 388);
            this.Controls.Add(this.imageGroup);
            this.Controls.Add(this.imgGroup);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MacroDialog";
            this.Text = Language["Action"];
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.imgGroup.ResumeLayout(false);
            this.imgGroup.PerformLayout();
            this.imageGroup.ResumeLayout(false);
            this.imageGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private UI.EaseButton cancelButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.RichTextBox textBox;
        private System.Windows.Forms.ToolStripMenuItem recordItem;
        private System.Windows.Forms.ToolStripMenuItem stopItem;
        private System.Windows.Forms.ToolStripMenuItem loadScriptItem;
        private System.Windows.Forms.ToolStripMenuItem SaveScriptItem;
        private UI.EaseButton runButton;
        private System.Windows.Forms.GroupBox imgGroup;
        private System.Windows.Forms.RadioButton selectImgRadio;
        private System.Windows.Forms.RadioButton allImgRadio;
        private System.Windows.Forms.GroupBox imageGroup;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton allImageRadio;
    }
}