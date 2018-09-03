using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Net;
using ExtractorSharp.Install.Loose;
using System.Diagnostics;
using ExtractorSharp.Core.Config;

namespace ExtractorSharp.Install {
    public partial class InstallPage : PagePanel {

        private const string UPDATE_URL = "http://kritsu.net/api/project/release/latest/es";
        private const string DOWNLOAD_URL = "http://file.kritsu.net";
        private readonly WebClient Client;
        private Stack<FileInfo> Stack;

        public InstallPage(Dictionary<string,ConfigValue> Config):base(Config){
            this.Config = Config;
            Client = new WebClient();
            Client.DownloadProgressChanged += ProgressChanged;
            Client.DownloadFileCompleted += ProgressCompleted;
            InitializeComponent();
            ShowNext = false;
            ShowCancel = false;
        }

        

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            progressBar1.Value = e.ProgressPercentage;
        }


        private void ProgressCompleted(object sender, AsyncCompletedEventArgs e) {
            textBox1.Text = textBox1.Text.Replace("正在下载", "已下载");
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
            if (Stack.Count > 0) {
                Download(Stack.Pop());
            } else {
                Next();
            }
        }


        public override void Init() {
            var builder = new LSBuilder();
            var obj = builder.Get(UPDATE_URL);
            var version = obj["tag"].GetValue(typeof(VersionInfo)) as VersionInfo;
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
                Next();
            }
        }




        //验证文件一致性
        private bool Check(FileInfo info) {
            var f = $"{Application.StartupPath}\\{info.Name}".Resolve();
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
            return hash.Equals(info.Hash);
        }

        private void Download(FileInfo info) {
            var uri = new Uri($"{DOWNLOAD_URL}/{info.Hash}");
            var filename = $"{Config["InstallPath"]}\\{info.Name}".Resolve();
            var dir = Path.GetDirectoryName(filename);
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            textBox1.Text = string.Concat(textBox1.Text,$"正在下载...{info.Name}\r\n");
            textBox1.SelectionStart = textBox1.Text.Length; 
            textBox1.ScrollToCaret();
            Client.DownloadFileAsync(uri, filename);
        }


        private string Hash(byte[] data) {
            using (var md5 = SHA256.Create()) {
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
