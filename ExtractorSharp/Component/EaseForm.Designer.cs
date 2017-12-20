using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Properties;
using ExtractorSharp.Config;
using System.IO;
using ExtractorSharp.Data;

namespace ExtractorSharp.Component {
    partial class EaseForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Config["FormColor"].Color;
            BackgroundImageLayout = (ImageLayout)Config["FormImageLayout"].Integer;
            if (File.Exists(Config["FormIcon"].Value)) {
                Icon = Icon.ExtractAssociatedIcon(Config["FormIcon"].Value);
            } else {
                Icon = Resources.aww;
            }
            StartPosition = FormStartPosition.CenterScreen;
        }
        #endregion
    }
}