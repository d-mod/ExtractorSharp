using ExtractorSharp.Loose;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

namespace ExtractorSharp.Install {
    public partial class UpdateForm : Form {

        private const string UPDATE_URL ="http://extractorsharp.kritsu.net/api/program/update?type=release";
        private const string DOWNLOAD_URL = "http://static.kritsu.net/file";
        private Stack<FileInfo> Stack;
        private WebClient Client;
        public UpdateForm(string[] args) {
            Client = new WebClient();
            Client.DownloadProgressChanged += ProgressChanged;
            Client.DownloadFileCompleted += ProgressCompleted;
            InitializeComponent();
            progressBar1.Maximum = 100;
            Compare();
        }

        private void ProgressCompleted(object sender, AsyncCompletedEventArgs e) {
            if (Stack.Count > 0) {
                Download(Stack.Pop());
            } else {
                Client.Dispose();
                Start();
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Start() {
            Process.Start($"{Application.StartupPath}/extractorsharp.exe");
            Environment.Exit(-1);
        }



        private void Compare() {
            var builder = new LSBuilder();
            var obj = builder.Get(UPDATE_URL);
            var version = obj.GetValue(typeof(VersionInfo)) as VersionInfo;
            var file = version.File;
            Stack = new Stack<FileInfo>();
            foreach (var info in file) {
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

        private void Download(FileInfo info) {
            var uri = new Uri($"{DOWNLOAD_URL}/{info.Hash}");
            var filename = $"{Application.StartupPath}/{info.Name}";
            Client.DownloadFileAsync(uri, filename);
        }

        //验证文件一致性
        private bool Check(FileInfo info) {
            var f = $"{Application.StartupPath}/{info.Name}";
            //验证本地文件是否存在
            if (!File.Exists(f)) {
                return false;
            }
            var data = File.ReadAllBytes(f);
            //验证本地文件大小和hash
            if (data.Length != info.Length) {
                return false;
            }
            var hash = Hash(data);
            if (!hash.Equals(info.Hash)) {
                return false;
            }
            return true;
        }

        private string Hash(byte[] data) {
            using (var md5 = MD5.Create()) {
                data = md5.ComputeHash(data);
            }
            return ToHexString(data);
        }

        public static String ToHexString(byte[] bytes) {
            var builder = new StringBuilder();
            // 把数组每一字节换成16进制连成md5字符串
            var digital = 0;
            for (int i = 0; i < bytes.Length; i++) {
                digital = bytes[i];
                digital = digital < 0 ? digital + 256 : digital;
                builder.Append(digital.ToString("x2"));
            }
            return builder.ToString();
        }





    }
}
