using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ExtractorSharp.Component;
using System.Linq;
using ExtractorSharp.Data;
using ExtractorSharp.Core.Control;
using ExtractorSharp.Config;

namespace ExtractorSharp {
    public partial class SearchDialog: EaseDialog {
        private bool running;
        private List<SearchResult> List;
        private Dictionary<string, string> Dic;
        private Controller Controller;
        public SearchDialog(ICommandData Data) : base(Data) {
            InitializeComponent();
            List = new List<SearchResult>();
            Controller = Program.Controller;
            pathBox.Text = Config["GamePath"].Value;
            searchButton.Click += Search;
            resultList.MouseDoubleClick += MouseAddList;
            resultList.Cleared += ClearResult;
            resultList.Deleted += DeleteResult;
            addItem.Click += AddList;
            browseButton.Click += SelectPath;
            patternBox.TextChanged += Filter;
            patternBox.KeyDown += KeyPressSearch;
            pathBox.Click += SelectPath;
            addNPKItem.Click += AddNPK;
            displayModeBox.SelectedIndexChanged += ChangeMode;
            useDicBox.CheckedChanged += UseDic;
        }

        /// <summary>
        /// 删除所选的结果
        /// </summary>
        private void DeleteResult() {
            var array = resultList.CheckedItems;
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
        /// 选择游戏路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectPath(object sender, EventArgs e) {
            Program.SelectPath();
            if (Directory.Exists(Config["GamePath"].Value))
                pathBox.Text = Config["GamePath"].Value;
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
                Controller.Do("addImg", Tools.Load(array).ToArray(), false);
            }
        }

        /// <summary>
        /// 获得搜索结果中的NPK
        /// </summary>
        /// <returns></returns>
        private string[] GetNPK() {
            var array = resultList.CheckedItems;
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
        private  void UseDic(object sender, EventArgs e) {
            patternBox.AutoCompleteCustomSource.Clear();
            if (useDicBox.Checked) {
                var keys = new string[Dic.Count];
                Dic.Keys.CopyTo(keys, 0);
                patternBox.AutoCompleteCustomSource.AddRange(keys);
            }
        }


        public override DialogResult Show(params object[] args) {
            Dic = Program.InitDictionary();
            UseDic(null, null);
            return ShowDialog();
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
                    return pattern.All( pat => item.imgPath.Contains(pat) );
                });
            var array = list.Distinct().ToArray() ;
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
            var thread = new Thread(Search);//启动搜索
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 加入列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddList(object sender, EventArgs e) {
            var array = resultList.CheckedItems;
            var list = new List<Album>();
            if (displayModeBox.SelectedIndex == 1)
                foreach (var result in array)
                    list.AddRange(Tools.Load(result.Path));
            else {
                var npks = GetNPK();
                list = Tools.Load(npks);//同一NPK的文件只需要读取一次
                list = new List<Album>(Tools.Find(list, allNameBox.Checked, GetPattern()));//使用find过滤出符合条件的
            }
            DialogResult = DialogResult.OK;
            Controller.Do("addImg", list.ToArray(), false);
        }

        private void MouseAddList(object sender, MouseEventArgs e) {
            var index = resultList.IndexFromPoint(e.Location);
            if (index > -1) {
                var result = resultList.Items[index] as SearchResult;
                var al = Tools.LoadAlbum(result.Path, result.imgPath);
                if (al != null) {
                    Controller.Do("addImg", new Album[] { al }, false);
                }
                DialogResult = DialogResult.OK;
            }
        }      

        protected override void OnFormClosing(FormClosingEventArgs e) {
            running = false;
            base.OnFormClosing(e);
        }


        private  void Search() {
            if (Directory.Exists(Config["ResourcePath"].Value)) {
                bar.Visible = true;
                var files = Directory.GetFiles(Config["ResourcePath"].Value);
                bar.Maximum = files.Length;
                bar.Value = 0;
                displayModeBox.Enabled = false;
                patternBox.ReadOnly = true;
                var modelDic = new Dictionary<string, string>();    
                if (ignoreModelBox.Checked)
                    modelDic = Program.LoadFileList();
                var pattern = GetPattern();
                foreach (var file in files) {
                    if (ignoreModelBox.Checked && !modelDic.ContainsKey(file.GetName()))
                        continue;
                    var list = Tools.Load(true, file);
                    list = Tools.Find(list,allNameBox.Checked ,pattern);                 
                    foreach (var album in list) {
                        var result = new SearchResult(displayModeBox.SelectedIndex, file, album.Path);
                        if (!running) return;
                        if (!List.Contains(result)) {
                            resultList.Items.Add(result);
                            List.Add(result);
                        }
                    }
                    resultList.Update();
                    if (bar.Value < bar.Maximum)
                        bar.Value++;
                }
                searchButton.Text = Language["Search"];
                bar.Visible = false;
                running = false;
                displayModeBox.Enabled = true;
                patternBox.ReadOnly = false;
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
            this.Name = Path.GetName();
            this.imgPath = imgPath;
            this.imgName = imgPath.GetName();
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
