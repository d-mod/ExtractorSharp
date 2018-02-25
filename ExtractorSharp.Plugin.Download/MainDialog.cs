using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using ExtractorSharp.Component;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using ExtractorSharp.Loose;

namespace ExtractorSharp.View.Dialog {
    [ExportMetadata("Guid", "87844cf0-a062-4f34-8c3b-d8e6bda28daa")]
    [Export(typeof(ESDialog))]
    public partial class MainDialog : ESDialog {
        private List<string> List;
        private List<string> Queues;
        private Dictionary<string,byte[]> Computed;
        private string[] typeArray = { NpkReader.IMAGE_DIR,NpkReader.SOUND_DIR};


        [ImportingConstructor]
        public MainDialog(IConnector Connector) : base(Connector) {
            InitializeComponent();
            List = new List<string>();
            Queues = new List<string>();
            Computed = new Dictionary<string,byte[]>();
            keywordBox.TextChanged += (o, e) => ListFlush();
            serverList.SelectedIndexChanged += Search;
            typeList.SelectedIndexChanged += Search;
            downloadItem.Click += Download;
            LoadServerList();
        }


        private void LoadServerList() {
            var file = $"{Config["RootPath"]}/conf/server.json";
            if (File.Exists(file)) {
                var builder = new LSBuilder();
                var obj=builder.Read(file);
                var list = obj.GetValue(typeof(SeverInfo[])) as SeverInfo[];
                serverList.Items.AddRange(list);
                serverList.SelectedIndex = 0;
            }
        }
       


        private void ListFlush() {
            var items = fileList.CheckedItems;
            var index = fileList.SelectedIndex;
            var itemArray = new string[items.Count];
            items.CopyTo(itemArray, 0);
            fileList.Items.Clear();
            var args = keywordBox.Text.Trim().Split(" ");
            var array = List.FindAll(e => args.Length == 0 || args.All(arg => e.ToLower().Contains(arg.ToLower())));
            fileList.Items.AddRange(array.ToArray());
            for (var i = 0; i < array.Count; i++) {
                if (itemArray.Contains(array[i])) {
                    fileList.SetItemChecked(i, true);
                }
            }
            if (fileList.Items.Count > 0) {
                if (index < 1 || index > fileList.Items.Count - 1) {
                    index = Math.Min(index, fileList.Items.Count - 1);
                    index = Math.Max(index, 0);
                }
                fileList.SelectedIndex = index;
            }
        }

        private void Download(object sender, EventArgs e) {
            var temp = fileList.SelectItems;
            var info = serverList.SelectedItem as SeverInfo;
            Queues.Clear();
            Computed.Clear();
            var dir = $"{Config["RootPath"]}/download";
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            for (var i = 0; i < temp.Length; i++) {
                using (var client = new WebClient()) {
                    var url = $"{info.Host}/{typeArray[typeList.SelectedIndex]}/{temp[i]}.spk";
                    var file = $"{dir}/{temp[i]}.spk";
                    Queues.Add(file);
                    client.DownloadDataAsync(new Uri(url));
                    client.DownloadProgressChanged += DownloadProgress;
                    client.DownloadDataCompleted += (o, ev) => {
                        Queues.Remove(file);
                        Computed.Add(file, ev.Result);
                        if (Queues.Count == 0) {
                            bar.Value = 0;
                            var list = new List<Album>();
                            foreach (var entry in Computed) {
                                using (var ms = new MemoryStream(entry.Value)) {
                                    list.AddRange(NpkReader.ReadNPK(ms, entry.Key));
                                }
                            }
                            Connector.Do("addImg", list.ToArray(), false);
                        }
                    };
                }
            }
        }


        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e) {
            bar.Value = e.ProgressPercentage;
        }


        private void Search(object sender, EventArgs e) {
            List.Clear();
            var info = serverList.SelectedItem as SeverInfo;
            using (var client = new WebClient()) {
                var uri = new Uri($"{info.Host}/package.lst");
                client.DownloadStringAsync(uri);
                client.DownloadProgressChanged += (o, ex) => bar.Value = ex.ProgressPercentage;
                client.DownloadStringCompleted += (o, ex) => {
                    bar.Value = 0;
                    var rs = ex.Result;
                    var array = rs.Split("\0");
                    var pattern = typeList.SelectedIndex == 0 ? ".NPK" : ".npk";
                    List = Array.FindAll(array, item => item.EndsWith(pattern)).ToList();              
                    ListFlush();
                };
                 
            }
        }

        private class SeverInfo {
            public string Name;
            public string Host;

            public override string ToString() {
                return Name;
            }
        }

    }

}

