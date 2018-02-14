using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text.RegularExpressions;
using ExtractorSharp.Component;
using ExtractorSharp.Core;

namespace ExtractorSharp.View {
    [ExportMetadata("Guid", "A4BB046F-ACAB-44B5-A0F0-2DD84278B43D")]
    [Export(typeof(ESDialog))]
    public partial class MainDialog : ESDialog {
        private string Path => Config["ResourcePath"].Value;
        private Dictionary<string, string> Dic;
        [ImportingConstructor]
        public MainDialog(IConnector Data) : base(Data) {
            InitializeComponent();
            modeBox.SelectedIndex = 0;
            pathBox.Text = Config["GamePath"].Value;
            searhButton.Click += Search;
            cleanButton.Click += Clear;
            pathBox.Click += SelectPath;
            loadButton.Click += SelectPath;
        }

        public void SelectPath(object sender, EventArgs e) {
            Connector.SelectPath();
            if (Directory.Exists(Config["GamePath"].Value))
                pathBox.Text = Config["GamePath"].Value;
        }

        public void Clear(object sender, EventArgs e) {
            if (!Directory.Exists(Path)) {
                Messager.ShowError("SelectPathIsInvalid");
                return;
            }
            var array = list.SelectItems;
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
            if (backupCheck.Checked) {
                msg = $"成功备份{array.Length}个文件到{backup}";
            } else {
                msg = $"成功清理{array.Length}个模型";
            }
            Messager.ShowMessage(Msg_Type.Operate, msg);
        }

        public void Search(object sender, EventArgs e) {
            if (!Directory.Exists(Path)) {
                Messager.ShowError("SelectPathIsInvalid");
                return;
            }
            list.Items.Clear();
            if (modeBox.SelectedIndex == 0) {
                SimpleClean();
            } else if (modeBox.SelectedIndex == 1) {
                DepthClean();
            }
        }

        public void SimpleClean() {
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

        public void DepthClean() {
            Dic = Dic ?? Tools.LoadFileLst($"{Config["GamePath"]}/auto.lst");
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
