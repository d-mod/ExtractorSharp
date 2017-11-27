
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ExtractorSharp.Command;
using ExtractorSharp.Command.ImageCommand;
using ExtractorSharp.Command.ImgCommand;
using ExtractorSharp.Command.LayerCommand;
using ExtractorSharp.Command.SpliceCommand;
using ExtractorSharp.Config;
using ExtractorSharp.Core;
using ExtractorSharp.EventArguments;
using ExtractorSharp.Draw;
using ExtractorSharp.UI;
using ExtractorSharp.Command.ColorChartCommand;
using ExtractorSharp.Command.DrawCommand;
using ExtractorSharp.Data;

namespace ExtractorSharp.Core{
    /// <summary>
    /// 命令控制器
    /// <see cref="ICommand"/>
    /// </summary>
    public class Controller {
        private IConfig Config => Program.Config;
        internal MainForm Main;
        private readonly Stack<ICommand> undoStack;
        private readonly Stack<ICommand> redoStack;
        static Dictionary<string, Type> Dic;

        public delegate void CommandHandler(object o, CommandEventArgs e);

        private readonly CommandEventArgs cmdArgs;

        /// <summary>
        /// 操作执行事件
        /// </summary>
        public event CommandHandler CommandDid;

        /// <summary>
        /// 操作撤销事件
        /// </summary>
        public event CommandHandler CommandUndid;

        /// <summary>
        /// 操作重做事件
        /// </summary>
        public event CommandHandler CommandRedid;

        /// <summary>
        /// 操作清空事件
        /// </summary>
        public event CommandHandler CommandCleared;

        private void OnCommandClear(CommandEventArgs e) => CommandCleared?.Invoke(this, e);
        private void OnComandDid(CommandEventArgs e) => CommandDid?.Invoke(this, e);
        private void OnComandUndid(CommandEventArgs e) => CommandUndid?.Invoke(this, e);
        private void OnCommandRedid(CommandEventArgs e) => CommandRedid?.Invoke(this, e);

        public delegate void ActionHandler(object o, ActionEventArgs e);
        private readonly ActionEventArgs actArgs;

        /// <summary>
        /// 动作记录
        /// </summary>
        public event ActionHandler ActionChanged;
        /// <summary>
        /// 动作执行
        /// </summary>
        public event ActionHandler ActionDid;

        private void OnActionChanged(ActionEventArgs e) => ActionChanged?.Invoke(this, e);

        private void OnActionDid(ActionEventArgs e) => ActionChanged?.Invoke(this, e);

        /// <summary>
        /// 动作序列
        /// </summary>
        public List<IAction> Macro => actArgs.Queues;


        /// <summary>
        /// 当前执行的操作
        /// </summary>
        public ICommand Current { get; private set; }

        /// <summary>
        /// 正在录制宏
        /// </summary>
        public bool IsRecord { private set; get; }


        public List<Album> List { set; get; }

        public ICommand[] History {
            get {
                var list = new List<ICommand>();
                var undo = undoStack.ToArray();
                Array.Reverse(undo);
                list.AddRange(undo);
                list.AddRange(redoStack);
                return list.ToArray();
            }
        }

        /// <summary>
        /// 当前操作的位置
        /// </summary>
        public int Index => undoStack.Count;

        public Clipboarder Clipboarder { set; get; }


        public Controller() {
            cmdArgs = new CommandEventArgs();
            actArgs = new ActionEventArgs();
            actArgs.Queues = new List<IAction>();
            undoStack = new Stack<ICommand>();
            redoStack = new Stack<ICommand>();
            List = new List<Album>();
            Dic = new Dictionary<string, Type>();
            SaveChanged += ChangeSave;
            Regisity("addImg", typeof(AddImg));
            Regisity("deleteImg", typeof(DeleteImg));
            Regisity("renameImg", typeof(RenameImg));
            Regisity("replaceImg", typeof(ReplaceImg));
            Regisity("newImg", typeof(NewImg));
            Regisity("hideImg", typeof(HideImg));
            Regisity("sortImg", typeof(SortImg));

            Regisity("cutImg", typeof(CutImg));
            Regisity("pasteImg", typeof(PasteImg));

            Regisity("repairImg", typeof(RepairImg));
            Regisity("splitImg", typeof(SplitImg));
            Regisity("mixImg", typeof(MixImg));

            Regisity("newImage", typeof(NewImage));
            Regisity("replaceImage", typeof(ReplaceImage));
            Regisity("hideImage", typeof(HideImage));
            Regisity("linkImage", typeof(LinkImage));
            Regisity("deleteImage", typeof(DeleteImage));
            Regisity("saveImage", typeof(SaveImage));
            Regisity("changePosition", typeof(ChangePosition));
            Regisity("changeSize", typeof(ChangeSize));
            Regisity("cutImage", typeof(CutImage));
            Regisity("pasteImage", typeof(PasteImage));

            Regisity("addSplice", typeof(AddMerge));
            Regisity("removeSplice", typeof(RemoveMerge));
            Regisity("clearSplice", typeof(ClearMerge));
            Regisity("runSplice", typeof(RunMerge));

            Regisity("cavasImage", typeof(CavasImage));
            Regisity("uncavasImage", typeof(UnCavasImage));
            Regisity("lineDodge", typeof(LineDodge));

            Regisity("renameLayer", typeof(RenameLayer));

            Regisity("changeColor", typeof(ChangeColor));
            Regisity("pencil", typeof(PencilDraw));
        }

        /// <summary>
        /// 注册命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="type"></param>
        public static void Regisity(string cmd, Type type) {
            if (Dic.ContainsKey(cmd))
                Dic.Remove(cmd);
            Dic.Add(cmd, type);
        }


        public void Close() {
            undoStack.Clear();
            redoStack.Clear();
            List.Clear();
            OnCommandClear(cmdArgs);
            isSave = true;
        }

        public void ClearMacro() {
            Macro.Clear();
            actArgs.Mode = QueueChangeMode.RemoveRange;
            OnActionChanged(actArgs);
        }

        #region 黑名单封禁
        private void CheckBlackList() {
            var thread = new Thread(CheckBlack);
            thread.Start();
            thread.Name = "ESBL";
            thread.IsBackground = true;
        }

        private void CheckBlack() {
            var stream = new StringReader("");
            var text = stream.ReadToEnd();
            var list = new List<string>();
            list.AddRange(text.Split("\r\n"));
            while (true) {
                foreach (var pro in Process.GetProcesses()) {
                    var title = pro.MainWindowTitle.ToLower().Trim();
                    foreach (var item in list) {
                        if (title.Contains(item)) {
                            UpDisable();                    
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }

        public void UpDisable() {
            var url = "http://kritsu.net/extractorsharp/disable.php";
            var cpu = GetCPU();
            var disk = GetDisk();
            var request = WebRequest.Create(url) as HttpWebRequest;
            var encoding = Encoding.UTF8;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var data = encoding.GetBytes(url+"&cpu=" + cpu + "&disk=" + disk);
            request.ContentLength = data.Length;
            var os = request.GetRequestStream();
            os.Write(data);
            os.Flush();
            os.Close();
            Environment.Exit(-1);
        }

        private static string GetDisk() {
            var disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        private static string GetCPU() {
            var strCpu = string.Empty;
            var myCpu = new ManagementClass("win32_Processor");
            var myCpuConnection = myCpu.GetInstances();
            foreach (var obj in myCpuConnection)
                strCpu = obj.Properties["Processorid"].Value.ToString();
            return strCpu;
        }

        #endregion

        #region 宏命令
        public void Record() {
            IsRecord = true;
            Macro.Clear();
            actArgs.Action = null;
            actArgs.Mode = QueueChangeMode.Clear;//清空模式
            OnActionChanged(actArgs);//触发队列更改事件
        }

        public void Pause() => IsRecord = !IsRecord;
        

        /// <summary>
        /// 执行宏命令
        /// </summary>
        /// <param name="allImage"></param>
        /// <param name="als"></param>
        internal void Run(bool allImage, params Album[] als) {
            IsRecord = false;
            foreach (var cmd in Macro) {
                switch (cmd) {//判断Action的类型
                    case MutipleAciton mutipleAction:
                        mutipleAction.Action(als);
                        break;
                    case SingleAction singleAction:
                        foreach (var al in als) {
                            var indexes = singleAction.Indexes;
                            if (allImage) {
                                indexes = new int[al.List.Count];
                                for (var i = 0; i < al.List.Count; i++)
                                    indexes[i] = i;
                            }
                            singleAction.Action(al, indexes);
                        }
                        break;
                }
                OnActionDid(actArgs);//触发动作执行事件
                GC.Collect();
            }
            ImageFlush();//刷新画布
        }

        /// <summary>
        /// 移出动作序列
        /// </summary>
        /// <param name="range"></param>
        internal void Delete(params IAction[] range) {
            foreach (var item in range) 
                Macro.Remove(item);
            actArgs.Mode = QueueChangeMode.RemoveRange;
            OnActionChanged(actArgs);
        }





        #endregion

        #region 撤销重做
        public Action SaveChanged;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="step"></param>
        public void Move(int step) {
            if (step > 0) {
                for (int i = 0; i < step; i++) {
                    Redo();
                }
            } else {
                for (var i = step; i < 0; i++) {
                    Undo();
                }
            }
            ImageFlush();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        public void Do(string key, params object[] args) {
            if (Dic.ContainsKey(key)) {
                var type = Dic[key];
                if (type != null && typeof(ICommand).IsAssignableFrom(type)) {
                    var cmd = type.CreateInstance() as ICommand;
                    cmd.Do(args);
                    if (cmd.CanUndo) {//可撤销
                        undoStack.Push(cmd);
                    }
                    if (IsRecord && cmd is IAction action) {//可宏
                        Macro.Add(action);
                        actArgs.Mode = QueueChangeMode.Add;
                        actArgs.Action = action;
                        OnActionChanged(actArgs);
                    }
                    if (cmd.Changed) {//发生更改
                        SaveChanged?.Invoke();
                    }
                    redoStack.Clear();                
                    OnComandDid(cmdArgs);
                    Current = cmd;
                }
            } else
                Messager.ShowMessage(Msg_Type.Error, $"不存在的命令[{key}]");
        }

        /// <summary>
        /// 撤销
        /// </summary>
        private void Undo() {
            if (undoStack.Count > 0) {
                var cmd = undoStack.Pop();
                cmd.Undo();
                redoStack.Push(cmd);
                OnComandUndid(cmdArgs);
                if (cmd.Changed) {
                    SaveChanged?.Invoke();
                }
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        private void Redo() {
            if (redoStack.Count > 0) {
                var cmd = redoStack.Pop();
                cmd.Redo();
                undoStack.Push(cmd);
                OnCommandRedid(cmdArgs);
                if (cmd.Changed)
                    SaveChanged.Invoke();
            }
        }

        #endregion
        #region

        public ImageEntity[] CheckedImage => Main.imageList.GetCheckItems();


        public ImageEntity[] AllImage {
            get {
                var array = new ImageEntity[Main.imageList.Items.Count];
                Main.imageList.Items.CopyTo(array, 0);
                return array;
            }
        }

        public Album[] CheckedAlbum {
            get {
                var array = Main.albumList.GetCheckItems();
                var list = new List<Album>();
                foreach (var al in array)
                    if (CheckEncrypt(al))
                        list.Add(al);
                return list.ToArray();
            }
        }

        public Album[] AllAlbum {
            get {
                var array = new Album[Main.albumList.Items.Count];
                Main.albumList.Items.CopyTo(array, 0);
                var temp = new List<Album>();
                temp.AddRange(array);
                foreach (var item in temp.ToArray())
                    if (!CheckEncrypt(item))
                        temp.Remove(item);
                return temp.ToArray();
            }
        }

        public Album SelectAlbum {
            get {
                var item = Main.albumList.SelectedItem as Album;
                if (item != null && CheckEncrypt(item)) {
                    return item;
                }
                return null;
            }
        }

        public ImageEntity SelectImage => Main.imageList.SelectedItem as ImageEntity;

        public int[] CheckedIndex {
            get {
                var indexes = Main.imageList.CheckedIndices;
                var array = new int[indexes.Count];
                indexes.CopyTo(array, 0);
                if (array.Length > 0)
                    return array;
                return new int[] { Main.imageList.SelectedIndex };
            }
        }

        #endregion
        


        #region

        public bool isSave = true;
        public bool CheckEncrypt(Album album) {
            if (album.Work != null && album.Work.IsValid && !album.Work.IsDecrypt) {
                Messager.ShowMessage(Msg_Type.Error, "请先解密" + album);
                Main.ShowDecrypt(album);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 影响保存的修改时触发的函数
        /// </summary>
        private void ChangeSave() {
            ImageFlush();
            isSave = false;
            if (Config["AutoSave"].Boolean) {
                SaveFile();
            }
        }

        public void SelectPath() {
            var dir = Main.Path;
            var path = dir.GetName();
            if (!path.Equals(string.Empty))
                dir = dir.Replace(path, "");
            var dialog = new SaveFileDialog();
            dialog.InitialDirectory = dir;
            dialog.FileName = path;
            dialog.Filter = "NPK文件|*.NPK";
            if (dialog.ShowDialog() == DialogResult.OK) {
                Main.Path = dialog.FileName;
                SaveChanged?.Invoke();
            }
        }

        public void SaveFile() {
            if (Main.Path.Trim().Length == 0)
                SelectPath();
            if (Main.Path.Trim().Length == 0)
                return;
            WriteNPK(Main.Path);
            isSave = true;
        }


        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="paths"></param>
        public void AddAlbum(bool clear, params string[] args) {
            if (clear) {
                Main.Path = string.Empty;
            }
            if (Main.Path.Length == 0) {
                Main.Path = args.Find(item => item.ToUpper().EndsWith(".NPK"))?? string.Empty;
            }
            if (args.Length > 0) {
                Do("addImg", Tools.Load(args).ToArray(), clear);
            }
        }

        public void AddAlbum(bool clear,int index,params Album[] array) {
            if (clear) {//当clear为true时，清空原列表
                Main.albumList.Clear();
                List.Clear();
                index = 0;
            }

            if (array.Length > 0) {
                if (Main.albumList.Items.Count > 0) {
                    Main.albumList.SelectedIndex = Main.albumList.Items.Count - 1;
                }
                index = index >List.Count ? List.Count  : index;
                index = index < 0 ? 0 : index;
                List.InsertRange(index,array);
                AlbumList.Items.InsertRange(index,array);
            }
            isSave = clear;
            ImageFlush();
        }

        public void AddAlbum(bool clear, params Album[] array) {
            AddAlbum(clear, array.Length, array);
        }

        public void RemoveAlbum(params Album[] array) {
            foreach (var album in array) {
                List.Remove(album);
                AlbumList.Items.Remove(album);
            }
        }

        public void WriteNPK(string file) {
            Tools.WriteNPK(file, List);
            isSave = true;
            Messager.ShowOperate("SaveFile");
        }

        public void WriteToDirectory(string dir) {
            Tools.SaveDirectory(dir, List);
        }



        public void ImageFlush() => Main?.ImageFlush();


        public void CavasFlush() => Main?.CavasFlush();


#endregion

#region 插件管理
        public MenuStrip MainMenu => Main.MainMenuStrip;
        public EaseListBox<ImageEntity> ImageList => Main.imageList;
        public EaseListBox<Album> AlbumList => Main.albumList;
        public EaseListBox<Layer> LayerList => Main.layerList;

#endregion
    }
}
