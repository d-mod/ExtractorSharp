using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Properties;
using ExtractorSharp.Config;
using System.IO;
using ExtractorSharp.Data;

namespace ExtractorSharp.UI {
    partial class EaseForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;
        protected Language Language = Language.Default;

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
            BackColor = ViewConfig["FormColor"].Color;
            BackgroundImageLayout = (ImageLayout)ViewConfig["FormImageLayout"].Integer;
            if (File.Exists(ViewConfig["FormIcon"].Value))
                Icon = Icon.ExtractAssociatedIcon(ViewConfig["FormIcon"].Value);
            else
                Icon = Resources.aww;
            StartPosition = FormStartPosition.CenterScreen;
        }
        #endregion
    }
}