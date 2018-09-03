using ExtractorSharp.Core.Config;
using ExtractorSharp.Install.Page;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ExtractorSharp.Install {
    public partial class InstallForm : Form {
        
        public Dictionary<string, ConfigValue> Config;

        public Stack<Type> Stack;
        
        public InstallForm(string[] args) {
            InitializeComponent();
            InitConfig();
            Init();
        }

        private void InitConfig() {
            var startupPath = Application.StartupPath;
            Config = new Dictionary<string, ConfigValue>();

            Config["ProjectName"] = new ConfigValue("ExtractorSharp");

            var driver = startupPath;
            var showPath = !File.Exists($"{startupPath}\\{Config["ProjectName"]}.exe");
            if (showPath) {
                driver = $"D:\\{Config["ProjectName"]}";
                if (!Directory.Exists(driver)) {
                    driver = $"C:\\Program Files\\{Config["ProjectName"]}";
                }
            }
            Config["ShowPath"] = new ConfigValue(showPath);
            Config["InstallPath"] = new ConfigValue($"{driver}");
            Config["ApplicationPath"] = new ConfigValue($"{driver}\\{Config["ProjectName"]}.exe".Resolve());
            Config["RunApplication"] = new ConfigValue(true);
        }

        private void Init() {
            Stack = new Stack<Type>();
            if (Config["ShowPath"].Boolean) {
                Stack.Push(typeof(EndPage));
                Stack.Push(typeof(InstallPage));
                Stack.Push(typeof(PathPage));
            } else {
                Stack.Push(typeof(InstallPage));
            }
            Next(this,null);
        }

        private void End() {
            if (Config["RunApplication"].Boolean) {
                Process.Start(Config["ApplicationPath"].Value);
                Cancel(this, null);
            }
        }


        private void Cancel(object sender,EventArgs e) {
            Close();
            Environment.Exit(0);
        }


        private void Next(object sender,EventArgs e) {
            if (Stack.Count == 0) {
                End();
                return;
            }
            var type = Stack.Pop();
            var panel = type.CreateInstance(Config) as PagePanel;
            panel.Nexting += Next;
            panel.Canceling += Cancel;
            this.Controls.Clear();
            this.Controls.Add(panel);
            panel.Init();
        }






   

    }
}