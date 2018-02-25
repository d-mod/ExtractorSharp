using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ExtractorSharp.Component;
using System.Linq;
using ExtractorSharp.Data;
using ExtractorSharp.Core;
using ExtractorSharp.Loose;
using System.ComponentModel.Composition;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Json;

namespace ExtractorSharp.Plugin.Searcher {
    [ExportMetadata("Guid", "D72DF478-FAFF-43DF-B904-9EB338A08B54")]
    [Export(typeof(ESDialog))]
    public partial class MainDialog : ESDialog {
        private bool running;
        private List<SearchResult> List;
        private Dictionary<string, string> Dic;
        private int Count { set; get; }
        private int Mode { set; get; }


        [ImportingConstructor]
        public MainDialog(IConnector Connector) : base(Connector) {
            InitializeComponent();
            List = new List<SearchResult>();
            pathBox.Text = Config["GamePath"].Value;
            searchButton.Click += Search;
            resultList.MouseDoubleClick += MouseAddList;
            resultList.Cleared += ClearResult;
            resultList.Deleted += DeleteResult;
            addItem.Click += AddList;
            patternBox.TextChanged += Filter;
            patternBox.KeyDown += KeyPressSearch;
            addNPKItem.Click += AddNPK;
            displayModeBox.SelectedIndexChanged += ChangeMode;
            useDicBox.CheckedChanged += UseDic;
        }

        /// <summary>
        /// 删除所选的结果
        /// </summary>
        private void DeleteResult() {
            var array = resultList.SelectItems;
            foreach (var item in array) {
                resultList.Items.Remove(item);
                List.Remove(item);
            }
        }

        /// <summary>
        /// 清理结果列表
        /// </summary>
        private void ClearResult() {
            List.Clear();
            resultList.Items.Clear();
        }




        /// <summary>
        /// 切换显示模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeMode(object sender, EventArgs e) {
            resultList.Items.Clear();
            var list = new HashSet<SearchResult>();
            foreach (var result in List) {
                result.Mode = displayModeBox.SelectedIndex;
                list.Add(result);
            }
            var array = new SearchResult[list.Count];
            list.CopyTo(array);
            resultList.Items.AddRange(array);
            resultList.Focus();
        }

        /// <summary>
        /// 添加NPK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNPK(object sender, EventArgs e) {
            var array = GetNPK();
            if (array.Length > 0) {
                Connector.Do("addImg", NpkReader.Load(array).ToArray(), false);
            }
        }

        /// <summary>
        /// 获得搜索结果中的NPK
        /// </summary>
        /// <returns></returns>
        private string[] GetNPK() {
            var array = resultList.SelectItems;
            var str = new string[0];
            if (array.Length > 0) {
                var list = new HashSet<string>();
                foreach (var result in array) {
                    list.Add(result.Path);
                }
                str = new string[list.Count];
                list.CopyTo(str);
            }
            return str;
        }

        private string[] GetPattern() {
            var pattern = patternBox.Text.Split(" ");
            var patternList = new List<string>();
            for (var i = 0; i < pattern.Length; i++) {
                pattern[i] = pattern[i].Replace(".NPK", "");
                if (Dic.ContainsKey(pattern[i]))
                    pattern[i] = Dic[pattern[i]];
                patternList.Add(pattern[i]);
            }
            pattern = patternList.ToArray();
            return pattern;
        }

        /// <summary>
        /// Enter快捷键搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyPressSearch(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                patternBox.SelectionLength = 0;
                Search(sender, e);
            }
        }

        /// <summary>
        /// 选中'使用字典'时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UseDic(object sender, EventArgs e) {
            patternBox.AutoCompleteCustomSource.Clear();
            if (useDicBox.Checked) {
                var keys = new string[Dic.Count];
                Dic.Keys.CopyTo(keys, 0);
                patternBox.AutoCompleteCustomSource.AddRange(keys);
            }
        }


        public override DialogResult Show(params object[] args) {
            InitDictionary();
            UseDic(null, null);
            return ShowDialog();
        }

        /// <summary>
        /// 初始化字典
        /// </summary>
        private void InitDictionary() {
            Dic = new Dictionary<string, string>();
            var file = $"{Config["RootPath"]}/dictionary.txt";
            if (File.Exists(file)) {
                var data = File.ReadAllText(file);
                var builder = new LSBuilder();
                var obj = builder.ReadProperties(data);
                obj.GetValue(ref Dic);
            }
        }



        /// <summary>
        /// 二次过滤搜索结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter(object sender, EventArgs e) {
            var pattern = GetPattern();
            if (pattern.Length < 1) {
                return;
            }
            resultList.Items.Clear();
            var list = List.FindAll(
                item => {
                    if (allNameBox.Checked && !pattern[0].Equals(item.Name)) {
                        return false;
                    }
                    return pattern.All(pat => item.imgPath.Contains(pat));
                });
            var array = list.Distinct().ToArray();
            resultList.Items.AddRange(array);
        }

        /// <summary>
        /// 按下搜索键时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search(object sender, EventArgs e) {
            running = !running;
            if (!running) {//当处于搜索时,暂停搜索
                displayModeBox.Enabled = true;
                patternBox.Enabled = true;
                searchButton.Text = Language["Search"];
                return;
            }
            searchButton.Text = Language["Pause"];
            resultList.Items.Clear();
            List.Clear();
            List = new List<SearchResult>();
            if (Directory.Exists(Config["ResourcePath"].Value)) {
                Mode = displayModeBox.SelectedIndex;
                var thread = new Thread(Start) {
                    IsBackground = true
                };//启动搜索
                thread.Start();
            }
        }

        /// <summary>
        /// 加入列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddList(object sender, EventArgs e) {
            var array = resultList.SelectItems;
            var list = new List<Album>();
            if (displayModeBox.SelectedIndex == 1)
                foreach (var result in array)
                    list.AddRange(NpkReader.Load(result.Path));
            else {
                var npks = GetNPK();
                list = NpkReader.Load(npks);//同一NPK的文件只需要读取一次
                list = new List<Album>(NpkReader.Find(list, allNameBox.Checked, GetPattern()));//使用find过滤出符合条件的
            }
            DialogResult = DialogResult.OK;
            Connector.Do("addImg", list.ToArray(), false);
        }

        private void MouseAddList(object sender, MouseEventArgs e) {
            var index = resultList.IndexFromPoint(e.Location);
            if (index > -1) {
                var result = resultList.Items[index] as SearchResult;
                var al = NpkReader.LoadWithName(result.Path, result.imgPath);
                if (al != null) {
                    Connector.Do("addImg", new Album[] { al }, false);
                }
                DialogResult = DialogResult.OK;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            running = false;
            base.OnFormClosing(e);
        }

        private void BeginStart() {
            if (bar.InvokeRequired) {
                bar.Invoke(new MethodInvoker(BeginStart));
                return;
            }
            if (displayModeBox.InvokeRequired) {
                displayModeBox.Invoke(new MethodInvoker(BeginStart));
                return;
            }
            if (patternBox.InvokeRequired) {
                patternBox.Invoke(new MethodInvoker(BeginStart));
                return;
            }
            bar.Visible = true;
            displayModeBox.Enabled = false;
            patternBox.ReadOnly = true;
            bar.Maximum = Count;
            bar.Value = 0;
        }

        private void EndStart() {
            if (bar.InvokeRequired) {
                bar.Invoke(new MethodInvoker(EndStart));
                return;
            }
            if (searchButton.InvokeRequired) {
                searchButton.Invoke(new MethodInvoker(EndStart));
                return;
            }
            if (displayModeBox.InvokeRequired) {
                displayModeBox.Invoke(new MethodInvoker(EndStart));
                return;
            }
            if (patternBox.InvokeRequired) {
                patternBox.Invoke(new MethodInvoker(EndStart));
                return;
            }
            searchButton.Text = Language["Search"];
            bar.Visible = false;
            running = false;
            displayModeBox.Enabled = true;
            patternBox.ReadOnly = false;
        }


        private void Start() {
            var files = Directory.GetFiles(Config["ResourcePath"].Value);
            Count = files.Length;
            BeginStart();
            var modelDic = new Dictionary<string, string>();
            if (ignoreModelBox.Checked) {
                modelDic = Tools.LoadFileLst($"{Config["GamePath"]}/auto.lst");
            }
            var pattern = GetPattern();
            label: foreach (var file in files) {
                if (ignoreModelBox.Checked && !modelDic.ContainsKey(file.GetSuffix()))
                    continue;
                var list = NpkReader.Load(true, file);
                list = NpkReader.Find(list, allNameBox.Checked, pattern);
                var tempList = new List<SearchResult>();
                foreach (var album in list) {
                    var result = new SearchResult(Mode, file, album.Path);
                    if (!running) {
                        goto label;
                    }
                    if (!List.Contains(result)) {
                        List.Add(result);
                        tempList.Add(result);
                    }
                }
                Add(tempList);
            }
            EndStart();
        }

        private delegate void ParamInvoke(List<SearchResult> list);

        private void Add(List<SearchResult> list) {
            if (bar.Value < bar.Maximum) {
                if (bar.InvokeRequired) {
                    bar.Invoke(new ParamInvoke(Add), list);
                    return;
                }
                if (resultList.InvokeRequired) {
                    resultList.Invoke(new ParamInvoke(Add), list);
                }
                bar.Value++;
                resultList.Items.AddRange(list.ToArray());
            }
        }
    }

    

    /// <summary>
    /// 搜索结果
    /// </summary>
    class SearchResult {
        public string Path;
        public string Name { get; }
        public string imgPath;
        public string imgName { get; }
        public int Mode;
        public SearchResult(int Mode, string Path, string imgPath) {
            this.Path = Path;
            this.Name = Path.GetSuffix();
            this.imgPath = imgPath;
            this.imgName = imgPath.GetSuffix();
            this.Mode = Mode;
        }

        public override string ToString() {
            switch (Mode) {
                case 1:
                    return Name;
                case 2:
                    return Name + "," + imgName;
                default:
                    return imgName;
            }
        }

        public override int GetHashCode() => ToString().GetHashCode();

        public override bool Equals(object obj) => ToString().Equals(obj.ToString());
    }


}
