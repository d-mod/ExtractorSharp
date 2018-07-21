using System.Windows.Forms;
using ExtractorSharp.Component;

namespace ExtractorSharp.View.Dialog {
    partial class NewImageDialog {
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
            this.count_box = new NumericUpDown();
            this.count_Label = new Label();
            this.yesButton = new ESButton();
            this.cancelButton = new ESButton();
            this.index_box = new NumericUpDown();
            this.offset_Label = new Label();
            this.group = new GroupBox();
            this.link_radio = new RadioButton();
            this._1555_radio = new RadioButton();
            this._4444_radio = new RadioButton();
            this._8888_radio = new RadioButton();
            this.group.SuspendLayout();
            this.SuspendLayout();
            // 
            // count_box
            // 
            this.count_box.Location = new System.Drawing.Point(39, 78);
            this.count_box.Maximum = 9999;
            this.count_box.Minimum =1;
            this.count_box.Name = "count_box";
            this.count_box.Size = new System.Drawing.Size(200, 21);
            this.count_box.TabIndex = 0;
            this.count_box.Value =1;
            // 
            // count_Label
            // 
            this.count_Label.AutoSize = true;
            this.count_Label.Location = new System.Drawing.Point(37, 47);
            this.count_Label.Name = "count_Label";
            this.count_Label.Size = new System.Drawing.Size(53, 12);
            this.count_Label.TabIndex = 1;
            this.count_Label.Text = Language["ImageCount"];
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(39, 306);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 4;
            this.yesButton.Text = Language["OK"];
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(164, 306);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = Language["Cancel"];
            // 
            // index_box
            // 
            this.index_box.Location = new System.Drawing.Point(39, 144);
            this.index_box.Name = "index_box";
            this.index_box.Size = new System.Drawing.Size(200, 21);
            this.index_box.TabIndex = 6;
            this.index_box.Minimum = -1;
            this.index_box.Value = 0;
            // 
            // label2
            // 
            this.offset_Label.AutoSize = true;
            this.offset_Label.Location = new System.Drawing.Point(37, 117);
            this.offset_Label.Name = "label2";
            this.offset_Label.Size = new System.Drawing.Size(53, 12);
            this.offset_Label.TabIndex = 7;
            this.offset_Label.Text = Language["Offset"];
            // 
            // groupBox1
            // 
            this.group.Controls.Add(this._8888_radio);
            this.group.Controls.Add(this._4444_radio);
            this.group.Controls.Add(this._1555_radio);
            this.group.Controls.Add(this.link_radio);
            this.group.Location = new System.Drawing.Point(39, 171);
            this.group.Name = "groupBox1";
            this.group.Size = new System.Drawing.Size(200, 116);
            this.group.TabIndex = 8;
            this.group.TabStop = false;
            this.group.Text = Language["ColorBits"];
            // 
            // link_radio
            // 
            this.link_radio.AutoSize = true;
            this.link_radio.Checked = true;
            this.link_radio.Cursor = System.Windows.Forms.Cursors.Default;
            this.link_radio.Location = new System.Drawing.Point(7, 21);
            this.link_radio.Name = "link_radio";
            this.link_radio.Size = new System.Drawing.Size(83, 16);
            this.link_radio.TabIndex = 0;
            this.link_radio.TabStop = true;
            this.link_radio.Text = Language["Default"];
            // 
            // _1555_radio
            // 
            this._1555_radio.AutoSize = true;
            this._1555_radio.Location = new System.Drawing.Point(7, 44);
            this._1555_radio.Name = "_1555_radio";
            this._1555_radio.Size = new System.Drawing.Size(125, 16);
            this._1555_radio.TabIndex = 1;
            this._1555_radio.Text = "ARGB_1555";
            // 
            // _4444_radio
            // 
            this._4444_radio.AutoSize = true;
            this._4444_radio.Location = new System.Drawing.Point(7, 66);
            this._4444_radio.Name = "_4444_radio";
            this._4444_radio.Size = new System.Drawing.Size(125, 16);
            this._4444_radio.TabIndex = 2;
            this._4444_radio.Text = "ARGB_4444";
            // 
            // _8888_radio
            // 
            this._8888_radio.AutoSize = true;
            this._8888_radio.Location = new System.Drawing.Point(6, 88);
            this._8888_radio.Name = "_8888_radio";
            this._8888_radio.Size = new System.Drawing.Size(125, 16);
            this._8888_radio.TabIndex = 3;
            this._8888_radio.Text = "ARGB_8888";
            // 
            // NewImageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 346);
            this.Controls.Add(this.group);
            this.Controls.Add(this.offset_Label);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.count_Label);
            this.Controls.Add(this.count_box);
            this.Controls.Add(this.index_box);
            this.Name = "NewImageDialog";
            this.Text = Language["NewImage"];
            this.group.ResumeLayout(false);
            this.group.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NumericUpDown count_box;
        private NumericUpDown index_box;
        private System.Windows.Forms.Label count_Label;
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label offset_Label;
        private GroupBox group;
        private System.Windows.Forms.RadioButton link_radio;
        private System.Windows.Forms.RadioButton _1555_radio;
        private System.Windows.Forms.RadioButton _4444_radio;
        private System.Windows.Forms.RadioButton _8888_radio;
    }
}