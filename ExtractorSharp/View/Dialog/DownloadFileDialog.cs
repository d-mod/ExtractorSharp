using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using ExtractorSharp.Component;
using ExtractorSharp.Config;
using ExtractorSharp.Core.Control;

namespace ExtractorSharp.View.Dialog {
    public partial class DownloadFileDialog : EaseDialog {
        private WebClient Client;
        private List<string> List;
        private bool isDownload;
        private string Temp;
        private Controller Controller;
        public DownloadFileDialog(IConnector Data) : base(Data) {
            Controller = Program.Controller;
            InitializeComponent();
            List = new List<string>();
            comboBox1.Items.Add(new SeverInfo("韩服正式服", "http://d-fighter.dn.nexoncdn.co.kr/samsungdnf/neople/dnf_hg/"));
            comboBox1.Items.Add(new SeverInfo("韩服体验服", "http://d-fighter.dn.nexoncdn.co.kr/samsungdnf/neople/dnf_open/"));
            comboBox1.SelectedIndex = 0;
            Client = new WebClient();
            Client.DownloadProgressChanged += DownloadProgress;
            Client.DownloadFileCompleted += DownloadFileCompleted;
            searchButton.Click += Search;
            pathBox.Click += SelectPath;
            loadButton.Click += SelectPath;
            downloadItem.Click += Download;
        }

        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e) {   
        }




        public void SelectPath(object sender,EventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) 
                pathBox.Text = dialog.SelectedPath;
        }

        public void Download(object sender,EventArgs e) {
            if (pathBox.Text.Equals(string.Empty)||!Directory.Exists(pathBox.Text)) 
                SelectPath(sender, e);
            else {
                var temp = fileList.SelectedItem as string;
                var info = comboBox1.SelectedItem as SeverInfo;
                var url = info.Host + "/ImagePacks2/"+"" + temp + ".spk";
                Temp = pathBox.Text + "/" + url.GetName();
                Client.DownloadFileAsync(new Uri(url),Temp );
            }
        }

        private void DownloadProgress(object sender,DownloadProgressChangedEventArgs e) {
            bar.Value = e.ProgressPercentage;
        }

       
        

       


        public void Search(object sender, EventArgs e) {
            List.Clear();
            var info = comboBox1.SelectedItem as SeverInfo;
            var rs = Client.DownloadData(info.Host + "package.lst");
            var ms = new MemoryStream(rs);
            ms.Seek(56);
            while (ms.Position < ms.Length) {
                var name = ms.ReadString();
                if (name.EndsWith(".NPK"))
                    List.Add(name);
                if (ms.Position + 48 < ms.Length)
                    ms.Seek(48);
                else
                    break;
            }
            ms.Close();
            fileList.Items.Clear();
            fileList.Items.AddRange(List.ToArray());    
        }


    }

    public class SeverInfo {
       public string Name;
       public string Host;
        public SeverInfo(string Name, string Host) {
            this.Name = Name;
            this.Host = Host;
        }

        public override string ToString() {
            return Name;
        }
    }
}

