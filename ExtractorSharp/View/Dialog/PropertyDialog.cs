using System;
using System.Windows.Forms;
using ExtractorSharp.UI;
using ExtractorSharp.Properties;
using ExtractorSharp.Config;
using ExtractorSharp.Data;
using ExtractorSharp.Core;

namespace ExtractorSharp.View {
    public partial class PropertyDialog : EaseDialog {
        private Controller Controller { get; }
        public PropertyDialog() {
            Controller = Program.Controller;
            InitializeComponent();
            yesButton.Click += SaveProperty;
            gamePathBox.Click += GamePathLoad;
            gamePathLoadButton.Click += GamePathLoad;
            saveImageBox.Click += SaveImageLoad;
            saveImageLoadButton.Click += SaveImageLoad;
            resetButton.Click += Reset;
            flashSpeedBar.ValueChanged += FlashSpeedBarChanged;
            flashSpeedBox.ValueChanged += FlashSpeedBoxChanged;
        }

        private void Reset(object sender, EventArgs e) {
            Config.Reset();
            ViewRefresh();
        }

        private void FlashSpeedBarChanged(object sender, EventArgs e) {
            if (flashSpeedBox.Value != flashSpeedBar.Value)
                flashSpeedBox.Value = flashSpeedBar.Value;
        }

        private void FlashSpeedBoxChanged(object sender, EventArgs e) {
            if (flashSpeedBox.Value != flashSpeedBar.Value)
                flashSpeedBar.Value = (int)flashSpeedBox.Value;
        }

        /// <summary>
        /// 界面刷新
        /// </summary>
        public void ViewRefresh() {
            gamePathBox.Text = Config["GamePath"].Value;
            autoSaveCheck.Checked = Config["AutoSave"].Boolean;
            saveImageBox.Text = Config["SaveImagePath"].Value;
            flashSpeedBar.Value = Config["FlashSpeed"].Integer;
            saveImagetipsCheck.Checked = Config["SaveImageTip"].Boolean;
            gridBox.Value = Config["GridGap"].Integer;
            saveImageAllPathCheck.Checked = Config["SaveImageAllPath"].Boolean;
            languageBox.Items.Clear();
            foreach (var item in Program.LanguageList) {
                languageBox.Items.Add(item);
                if (item.LCID == Config["LCID"].Integer)
                    languageBox.SelectedItem = item;
            }
        }

        public override DialogResult Show(params object[] args) {
            Config.Reload();
            ViewRefresh();
            return ShowDialog();
        }

        private void SaveProperty(object sender, EventArgs e) {
            Config["GamePath"] = new ConfigValue(gamePathBox.Text);
            Config["AutoSave"] = new ConfigValue(autoSaveCheck.Checked);
            Config["SaveImagePath"] = new ConfigValue(saveImageBox.Text);
            Config["FlashSpeed"] = new ConfigValue(flashSpeedBar.Value);
            Config["SaveImageTip"] = new ConfigValue(saveImagetipsCheck.Checked);
            Config["GridGap"] = new ConfigValue(gridBox.Value);
            Config["LCID"] = new ConfigValue((languageBox.SelectedItem as Language)?.LCID);
            Config["SaveImageAllPath"] = new ConfigValue(saveImageAllPathCheck.Checked);
            Config.Save();
            Visible = false;
            Controller.CavasFlush();
        }

        private void GamePathLoad(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                gamePathBox.Text = dialog.SelectedPath;
        }

        private void SaveImageLoad(object sender, EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                saveImageBox.Text = dialog.SelectedPath;
        }
    }
}
