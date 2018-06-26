using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using ExtractorSharp.Install.Loose;

namespace ExtractorSharp.Install {
    public partial class InstallForm : Form {
        private const string UPDATE_URL = "http://extractorsharp.kritsu.net/api/program/update";
        private const string DOWNLOAD_URL = "http://static.kritsu.net/plugin";
        private const string DOWNLOAD_PLUGIN_API_URL = "http://extractorsharp.kritsu.net/api/plugin/download";
        private readonly WebClient Client;
        private Guid Guid;
        private bool IsPlugin;
        private Stack<FileInfo> Stack;

        public InstallForm(string[] args) {
            CheckArgs(args);
            Client = new WebClient();
            Client.DownloadProgressChanged += ProgressChanged;
            Client.DownloadFileCompleted += ProgressCompleted;
            InitializeComponent();
            progressBar1.Maximum = 100;
            if (IsPlugin) {
                ComparePlugin();
            } else {
                Compare();
            }
        }

        private void CheckArgs(string[] args) {
            for (var i = 0; i < args.Length; i++) {
                switch (args[i]) {
                    case "-p":
                        //插件
                        IsPlugin = i < args.Length - 1 && Guid.TryParse(args[i + 1], out Guid);
                        break;
                }
            }
        }

        private void ProgressCompleted(object sender, AsyncCompletedEventArgs e) {
            if (Stack.Count > 0) {
                Download(Stack.Pop());
            } else if (!IsPlugin) Start();
            Client.Dispose();
            Environment.Exit(0);
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Start() {
            var exename = $"{Application.StartupPath}/extractorsharp.exe";
            if (File.Exists(exename)) {
                Process.Start(exename);
                Environment.Exit(-1);
            } else {
                textBox1.Text = $"未能找到主程序！/n{exename}";
            }
        }


        private void Compare() {
            var builder = new LSBuilder();
            var obj = builder.Get(UPDATE_URL);
            var version = obj.GetValue(typeof(VersionInfo)) as VersionInfo;
            var files = version.Files;
            Stack = new Stack<FileInfo>();
            foreach (var info in files) {
                if (!Check(info)) {
                    Stack.Push(info);
                }
            }
            if (Stack.Count > 0) {
                Download(Stack.Pop());
            } else {
                Start();
            }
        }

        private void ComparePlugin() {
            var builder = new LSBuilder();
            var obj = builder.Get(DOWNLOAD_PLUGIN_API_URL, new Dictionary<string, object> {
                ["guid"] = Guid.ToString()
            });
            var files = obj.GetValue(typeof(List<FileInfo>)) as List<FileInfo>;
            Stack = new Stack<FileInfo>();
            foreach (var info in files) {
                info.Name = $"plugin/{Guid}/{info.Name}";
                if (!Check(info)) Stack.Push(info);
            }
            if (Stack.Count > 0) Download(Stack.Pop());
        }


        private void Download(FileInfo info) {
            var uri = new Uri($"{DOWNLOAD_URL}/{info.Hash}");
            var filename = $"{Application.StartupPath}/{info.Name}";
            var dir = Path.GetDirectoryName(filename);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            textBox1.Text = $"正在下载文件\n({filename})";
            Client.DownloadFileAsync(uri, filename);
        }

        //验证文件一致性
        private bool Check(FileInfo info) {
            var f = $"{Application.StartupPath}/{info.Name}";
            //验证本地文件是否存在
            if (!File.Exists(f)) return false;
            var data = File.ReadAllBytes(f);
            //验证本地文件大小和hash
            if (data.Length != info.Length) return false;
            var hash = Hash(data);
            if (!hash.Equals(info.Hash)) return false;
            return true;
        }

        private string Hash(byte[] data) {
            using (var md5 = MD5.Create()) {
                data = md5.ComputeHash(data);
            }
            return ToHexString(data);
        }

        public static string ToHexString(byte[] bytes) {
            var builder = new StringBuilder();
            // 把数组每一字节换成16进制连成md5字符串
            var digital = 0;
            for (var i = 0; i < bytes.Length; i++) {
                digital = bytes[i];
                digital = digital < 0 ? digital + 256 : digital;
                builder.Append(digital.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}