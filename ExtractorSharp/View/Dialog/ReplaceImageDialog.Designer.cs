using ExtractorSharp.Component;
using System.Drawing;

namespace ExtractorSharp.View {
    partial class ReplaceImageDialog {
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
            this.yesButton = new ESButton();
            this.cancelButton = new ESButton();
            this.channelGroup = new System.Windows.Forms.GroupBox();
            this._8888_Radio = new System.Windows.Forms.RadioButton();
            this._4444_Radio = new System.Windows.Forms.RadioButton();
            this._1555_Radio = new System.Windows.Forms.RadioButton();
            this._default_Radio = new System.Windows.Forms.RadioButton();
            this.group = new System.Windows.Forms.GroupBox();
            this.allImageRadio = new System.Windows.Forms.RadioButton();
            this.seletImageRadio = new System.Windows.Forms.RadioButton();
            this.fromGifBox = new System.Windows.Forms.CheckBox();
            this.adjustPositionBox = new System.Windows.Forms.CheckBox();
            this.channelGroup.SuspendLayout();
            this.group.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.yesButton.Location = new System.Drawing.Point(247, 275);
            this.yesButton.Name = "button1";
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 2;
            this.yesButton.Text = Language["OK"];
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.cancelButton.Location = new System.Drawing.Point(362, 275);
            this.cancelButton.Name = "button2";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = Language["Cancel"];
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // channelBox
            // 
            this.channelGroup.Controls.Add(this._8888_Radio);
            this.channelGroup.Controls.Add(this._4444_Radio);
            this.channelGroup.Controls.Add(this._1555_Radio);
            this.channelGroup.Controls.Add(this._default_Radio);
            this.channelGroup.Location = new System.Drawing.Point(297, 32);
            this.channelGroup.Name = "channelBox";
            this.channelGroup.Size = new System.Drawing.Size(184, 150);
            this.channelGroup.TabIndex = 4;
            this.channelGroup.TabStop = false;
            this.channelGroup.Text = Language["ColorBits"];
            // 
            // _8888_Radio
            // 
            this._8888_Radio.AutoSize = true;
            this._8888_Radio.Location = new System.Drawing.Point(40, 121);
            this._8888_Radio.Name = "_8888_Radio";
            this._8888_Radio.Size = new System.Drawing.Size(125, 16);
            this._8888_Radio.TabIndex = 2;
            this._8888_Radio.Text = "ARGB_8888";
            this._8888_Radio.UseVisualStyleBackColor = true;
            // 
            // _4444_Radio
            // 
            this._4444_Radio.AutoSize = true;
            this._4444_Radio.Location = new System.Drawing.Point(40, 88);
            this._4444_Radio.Name = "_4444_Radio";
            this._4444_Radio.Size = new System.Drawing.Size(125, 16);
            this._4444_Radio.TabIndex = 1;
            this._4444_Radio.Text = "ARGB_4444";
            this._4444_Radio.UseVisualStyleBackColor = true;
            // 
            // _1555_Radio
            // 
            this._1555_Radio.AutoSize = true;
            this._1555_Radio.Checked = true;
            this._1555_Radio.Location = new System.Drawing.Point(40,55);
            this._1555_Radio.Name = "_1555_Radio";
            this._1555_Radio.Size = new System.Drawing.Size(125, 16);
            this._1555_Radio.TabIndex = 0;
            this._1555_Radio.TabStop = true;
            this._1555_Radio.Text = "ARGB_1555";
            this._1555_Radio.UseVisualStyleBackColor = true;

            this._default_Radio.AutoSize = true;
            this._default_Radio.Checked = true;
            this._default_Radio.Location = new System.Drawing.Point(40, 22);
            this._default_Radio.Name = "_default_Radio";
            this._default_Radio.Size = new System.Drawing.Size(125, 16);
            this._default_Radio.TabIndex = 0;
            this._default_Radio.TabStop = true;
            this._default_Radio.Text = Language["Default"];
            this._default_Radio.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.group.Controls.Add(this.allImageRadio);
            this.group.Controls.Add(this.seletImageRadio);
            this.group.Location = new System.Drawing.Point(57, 32);
            this.group.Name = "groupBox1";
            this.group.Size = new System.Drawing.Size(184, 120);
            this.group.TabIndex = 5;
            this.group.TabStop = false;
            this.group.Text = Language["Target"];
            // 
            // allImageRadio
            // 
            this.allImageRadio.AutoSize = true;
            this.allImageRadio.Location = new System.Drawing.Point(40, 77);
            this.allImageRadio.Name = "allImageRadio";
            this.allImageRadio.Size = new System.Drawing.Size(119, 16);
            this.allImageRadio.TabIndex = 1;
            this.allImageRadio.TabStop = true;
            this.allImageRadio.Text = Language["AllImage"];
            this.allImageRadio.UseVisualStyleBackColor = true;
            // 
            // seletImageRadio
            // 
            this.seletImageRadio.AutoSize = true;
            this.seletImageRadio.Checked = true;
            this.seletImageRadio.Location = new System.Drawing.Point(40, 33);
            this.seletImageRadio.Name = "seletImageRadio";
            this.seletImageRadio.Size = new System.Drawing.Size(107, 16);
            this.seletImageRadio.TabIndex = 0;
            this.seletImageRadio.TabStop = true;
            this.seletImageRadio.Text = Language["CheckImage"];
            this.seletImageRadio.UseVisualStyleBackColor = true;
            // 
            // fromGifBox
            // 
            this.fromGifBox.AutoSize = true;
            this.fromGifBox.Location = new System.Drawing.Point(79, 174);
            this.fromGifBox.Name = "fromGifBox";
            this.fromGifBox.Size = new System.Drawing.Size(78, 16);
            this.fromGifBox.TabIndex = 6;
            this.fromGifBox.Text = Language["ReplaceFromGif"];
            this.fromGifBox.UseVisualStyleBackColor = true;
            // 
            // adjustPositionBox
            // 
            this.adjustPositionBox.AutoSize = true;
            this.adjustPositionBox.Location = new System.Drawing.Point(79, 224);
            this.adjustPositionBox.Name = "adjustPositionBox";
            this.adjustPositionBox.Size = new System.Drawing.Size(72, 16);
            this.adjustPositionBox.TabIndex = 7;
            this.adjustPositionBox.Text = Language["AdjustPosition"];
            this.adjustPositionBox.UseVisualStyleBackColor = true;
            // 
            // GifListDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 330);
            this.Controls.Add(this.adjustPositionBox);
            this.Controls.Add(this.fromGifBox);
            this.Controls.Add(this.group);
            this.Controls.Add(this.channelGroup);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.yesButton);
            this.Name = "ReplaceDialog";
            this.Text = Language["ReplaceImage"];
            this.channelGroup.ResumeLayout(false);
            this.channelGroup.PerformLayout();
            this.group.ResumeLayout(false);
            this.group.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox channelGroup;
        private System.Windows.Forms.RadioButton _default_Radio;
        private System.Windows.Forms.RadioButton _1555_Radio;
        private System.Windows.Forms.RadioButton _4444_Radio;
        private System.Windows.Forms.RadioButton _8888_Radio;
        private System.Windows.Forms.GroupBox group;
        private System.Windows.Forms.RadioButton seletImageRadio;
        private System.Windows.Forms.RadioButton allImageRadio;
        private System.Windows.Forms.CheckBox fromGifBox;
        private System.Windows.Forms.CheckBox adjustPositionBox;
    }
}