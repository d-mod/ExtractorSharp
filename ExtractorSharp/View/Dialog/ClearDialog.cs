using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.UI;
using ExtractorSharp.Properties;
using ExtractorSharp.Config;
using ExtractorSharp.Core;

namespace ExtractorSharp.View {
    internal partial class ClearDialog : EaseDialog {
        private string Path => Config["ResourcePath"].Value;
        private Dictionary<string, string> Dic;
        public ClearDialog(){
            InitializeComponent();
            modeBox.SelectedIndex = 0;
            pathBox.Text = Config["GamePath"].Value;
            searhButton.Click += Search;
            clearButton.Click += Clear;
            pathBox.Click += SelectPath;
            loadButton.Click += SelectPath;
        }

        public void SelectPath(object sender, EventArgs e) {
            Program.SelectPath();
            if (Directory.Exists(Config["GamePath"].Value))
                pathBox.Text = Config["GamePath"].Value;
        }

        public void Clear(object sender, EventArgs e) {
            if (!Directory.Exists(Path)) {
                Messager.ShowError("SelectPathIsInvalid");
                return;
            }
            var array = list.GetCheckItems(false);
            bar.Maximum = array.Length;
            bar.Value = 0;
            bar.Visible = true;
            var backup = $"{Config["RootPath"]}/backup/";
            if (!Directory.Exists(backup))
                Directory.CreateDirectory(backup);
            foreach (var f in array) {
                var file = Path + f;
                if (File.Exists(file)) {
                    try {
                        if (backupCheck.Checked) 
                            File.Move(file, backup + f);
                         else
                            File.Delete(file);
                    } catch (Exception) {
                       Messager.ShowMessage(Msg_Type.Error, "文件" + file + "被占用,清理失败");
                    }
                }
                list.Items.Remove(f);
                bar.Value++;
            }
            bar.Visible = false;
            string msg = "";
            if (backupCheck.Checked)
                msg = "成功备份" + array.Length + "个文件到" + backup;
            else
                msg = "成功清理" + array.Length + "个模型";
            Messager.ShowMessage(Msg_Type.Operate, msg);
            GC.Collect();
        }

        public void Search(object sender, EventArgs e) {
            if (!Directory.Exists(Path)) {
                Messager.ShowError("SelectPathIsInvalid");
                return;
            }
            list.Items.Clear();
            if (modeBox.SelectedIndex == 0) 
                SimpleClear();
            else if (modeBox.SelectedIndex == 1)
                DepthClear();
            GC.Collect();
        }

        public void SimpleClear() {
            var files = Directory.GetFiles(Path,"*.NPK");
            bar.Maximum = files.Length;
            bar.Value = 0;
            bar.Visible = true;
            foreach (var f in files) {
                if (Regex.IsMatch(f.GetName().ToUpper(), @".*\(.*\)\.NPK"))
                    list.Items.Add(f.GetName(),true);
                bar.Value++;
            }
            bar.Visible = false;
        }

        public void DepthClear() {
            Dic = Dic ?? Program.LoadFileList();
            var files = Directory.GetFiles(Path);
            bar.Maximum = files.Length;
            bar.Value = 0;
            bar.Visible = true;
            foreach (var f in files) {
                var name = f.GetName();
                if (!Dic.ContainsKey(name)) 
                    list.Items.Add(name,true);
                bar.Value++;            
            }
            bar.Visible = false;
        }
    }


}
