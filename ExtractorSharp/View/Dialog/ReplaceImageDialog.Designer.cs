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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.channelBox = new System.Windows.Forms.GroupBox();
            this._8888_Radio = new System.Windows.Forms.RadioButton();
            this._4444_Radio = new System.Windows.Forms.RadioButton();
            this._1555_Radio = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.allImageRadio = new System.Windows.Forms.RadioButton();
            this.seletImageRadio = new System.Windows.Forms.RadioButton();
            this.fromGifBox = new System.Windows.Forms.CheckBox();
            this.adjustPostionBox = new System.Windows.Forms.CheckBox();
            this.channelBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(42, 256);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = Language["OK"];
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(148, 256);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = Language["Cancel"];
            this.button2.UseVisualStyleBackColor = true;
            // 
            // channelBox
            // 
            this.channelBox.Controls.Add(this._8888_Radio);
            this.channelBox.Controls.Add(this._4444_Radio);
            this.channelBox.Controls.Add(this._1555_Radio);
            this.channelBox.Location = new System.Drawing.Point(42, 104);
            this.channelBox.Name = "channelBox";
            this.channelBox.Size = new System.Drawing.Size(184, 97);
            this.channelBox.TabIndex = 4;
            this.channelBox.TabStop = false;
            this.channelBox.Text = Language["ColorBits"];
            // 
            // _8888_Radio
            // 
            this._8888_Radio.AutoSize = true;
            this._8888_Radio.Location = new System.Drawing.Point(6, 66);
            this._8888_Radio.Name = "_8888_Radio";
            this._8888_Radio.Size = new System.Drawing.Size(125, 16);
            this._8888_Radio.TabIndex = 2;
            this._8888_Radio.Text = "ARGB_8888";
            this._8888_Radio.UseVisualStyleBackColor = true;
            // 
            // _4444_Radio
            // 
            this._4444_Radio.AutoSize = true;
            this._4444_Radio.Location = new System.Drawing.Point(7, 44);
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
            this._1555_Radio.Location = new System.Drawing.Point(7, 21);
            this._1555_Radio.Name = "_1555_Radio";
            this._1555_Radio.Size = new System.Drawing.Size(125, 16);
            this._1555_Radio.TabIndex = 0;
            this._1555_Radio.TabStop = true;
            this._1555_Radio.Text = "ARGB_1555";
            this._1555_Radio.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.allImageRadio);
            this.groupBox1.Controls.Add(this.seletImageRadio);
            this.groupBox1.Location = new System.Drawing.Point(42, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(184, 72);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = Language["HandleObject"];
            // 
            // allImageRadio
            // 
            this.allImageRadio.AutoSize = true;
            this.allImageRadio.Location = new System.Drawing.Point(23, 43);
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
            this.seletImageRadio.Location = new System.Drawing.Point(23, 20);
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
            this.fromGifBox.Location = new System.Drawing.Point(42, 224);
            this.fromGifBox.Name = "fromGifBox";
            this.fromGifBox.Size = new System.Drawing.Size(78, 16);
            this.fromGifBox.TabIndex = 6;
            this.fromGifBox.Text = Language["ReplaceFromGif"];
            this.fromGifBox.UseVisualStyleBackColor = true;
            // 
            // adjustPostionBox
            // 
            this.adjustPostionBox.AutoSize = true;
            this.adjustPostionBox.Checked = true;
            this.adjustPostionBox.Location = new System.Drawing.Point(151, 224);
            this.adjustPostionBox.Name = "adjustPostionBox";
            this.adjustPostionBox.Size = new System.Drawing.Size(72, 16);
            this.adjustPostionBox.TabIndex = 7;
            this.adjustPostionBox.Text = Language["AdjustPosition"];
            this.adjustPostionBox.UseVisualStyleBackColor = true;
            // 
            // GifListDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 305);
            this.Controls.Add(this.adjustPostionBox);
            this.Controls.Add(this.fromGifBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.channelBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "ReplaceDialog";
            this.Text = Language["ReplaceImage"];
            this.channelBox.ResumeLayout(false);
            this.channelBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox channelBox;
        private System.Windows.Forms.RadioButton _1555_Radio;
        private System.Windows.Forms.RadioButton _4444_Radio;
        private System.Windows.Forms.RadioButton _8888_Radio;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton seletImageRadio;
        private System.Windows.Forms.RadioButton allImageRadio;
        private System.Windows.Forms.CheckBox fromGifBox;
        private System.Windows.Forms.CheckBox adjustPostionBox;
    }
}