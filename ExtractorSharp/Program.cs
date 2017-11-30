using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.AccessControl;
using System.Windows.Forms;
using ExtractorSharp.Composition;
using ExtractorSharp.Config;
using ExtractorSharp.Core;
using ExtractorSharp.UI;
using ExtractorSharp.Properties;
using ExtractorSharp.View;
using Microsoft.Win32;
using ExtractorSharp.View.Dialog;
using ExtractorSharp.Data;
using ExtractorSharp.Loose;
using System.Threading;
using System.Text;

namespace ExtractorSharp {
    /// <summary>
    /// 程序全局控制
    /// </summary>
    public static class Program {

        public static IConfig Config { get; private set; }

        internal static MainForm Form { get; private set; }
        internal static Controller Controller { get; private set; }
        internal static Viewer Viewer { get; private set; }
        internal static Hoster Hoster { get; private set; }
        internal static Merger Merger { get; private set; }
        internal static Drawer Drawer { get; private set; }
        internal static List<Language> LanguageList { get; private set; }
    
        internal static IConfig ViewConfig { get; private set; }

        internal readonly static string Version = Assembly.GetAssembly(typeof(Program)).GetName().Version.ToString();

        private static string[] Arguments;
       /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args) { 
            Arguments = args;
            LoadConfig();
            LoadLanguage();
            if (Config["AutoUpdate"].Boolean) {
                CheckUpdate(false);
            }
            Application.ThreadException += ShowDebug;   
            Application.SetCompatibleTextRenderingDefault(true);           
            Application.EnableVisualStyles();
            LoadRegistry();
            Controller = new Controller();
            Viewer = new Viewer();
            Drawer = new Drawer();
            Form = new MainForm();
            Controller.Main = Form;
            Controller.AddAlbum(true, args);
            Form.Shown += OnShown;
            RegistyDialog();
            Viewer.DialogShown += ViewerDialogShown;
            Hoster = new Hoster();
            Merger = new Merger();
            Application.Run(Form);
        }

        private static void ShowDebug(object sender, ThreadExceptionEventArgs e) {
            var log = $"{e.Exception.Message};\r\n{e.Exception.StackTrace}";
            var data = Encoding.Default.GetBytes(log);
            log = Convert.ToBase64String(data);
            Viewer.Show("debug", "debug", log);
        }



        private static void OnShown(object sender, EventArgs e) {
            if (!Config["Version"].Value.Equals(Version)) {
                Config["Version"] = new ConfigValue(Version);
                Config.Save();
                Viewer.Show("version", true);
            }
            if (Arguments.Length == 1) {
                var command = Arguments[0];
                if (!command.StartsWith("esharp://")) {
                    Controller.AddAlbum(true, command);
                    return;
                }
                command = command.Replace("esharp://", "");
                Arguments = command.Split("/");
            }
            if (Arguments.Length > 1) {
                var args = new object[Arguments.Length - 2];
                Array.Copy(Arguments, 2, args, 0, args.Length);
                var name = Arguments[1];
                switch (Arguments[0]) {
                    case "s":
                        Viewer.Show(name, args);
                        break;
                    case "c":
                        Controller.Do(name, args);
                        break;
                }
            }
        }

        /// <summary>
        /// 注册窗口
        /// </summary>
        private static void RegistyDialog() {
            Viewer.Regisity("replace", typeof(ReplaceImageDialog));
            Viewer.Regisity("fit", typeof(FitRoomDialog));
            Viewer.Regisity("splice", typeof(MergeDialog));
            Viewer.Regisity("search", typeof(SearchDialog));
            Viewer.Regisity("newImg", typeof(NewImgDialog));
            Viewer.Regisity("convert", typeof(ConvertDialog));
            Viewer.Regisity("changePosition", typeof(ChangePositonDialog));
            Viewer.Regisity("batch", typeof(BatDialog));
            Viewer.Regisity("clear", typeof(ClearDialog));
            Viewer.Regisity("about", typeof(AboutDialog));
            Viewer.Regisity("encrypt", typeof(EncryptDialog));
            Viewer.Regisity("debug", typeof(BugDialog));
            Viewer.Regisity("newImage", typeof(NewImageDialog));
            Viewer.Regisity("macro", typeof(MacroDialog));
            Viewer.Regisity("cavas", typeof(CavasDialog));
            Viewer.Regisity("property", typeof(PropertyDialog));
            Viewer.Regisity("saveImage", typeof(SaveImageDialog));
            Viewer.Regisity("version", typeof(VersionDialog));
            Viewer.Regisity("download", typeof(DownloadFileDialog));
        }

        private static void ViewerDialogShown(object sender, DialogEventArgs e) {
            e.Dialog = e.DialogType.CreateInstance() as EaseDialog;
            e.Dialog.Owner = Form;
        }


        /// <summary>
        /// 加载语言
        /// </summary>
        private static void LoadLanguage() {
            var chinese = Language.CreateFromJson(Resources.Chinese);
            LanguageList = new List<Language>();
            LanguageList.Add(chinese);
            var path =  $"{Config["RootPath"]}/lan/";
            if (Directory.Exists(path)) {
                foreach (var file in Directory.GetFiles(path, "*.json")) {
                    var lan = Language.CreateFromFile(file);
                    LanguageList.Add(lan);
                }
            } else {
                Directory.CreateDirectory(path);
            }
            if (Config["LCID"].Integer == -1) {
                Config["LCID"] = new ConfigValue(Application.CurrentCulture.LCID);
                Config.Save();
            }
            Language.Default = LanguageList.Find(lan => lan.LCID == Config["LCID"].Integer);
        }

        private static void LoadConfig() {
            Config = new JsonConfig();
            Config.LoadConfig(Resources.Config);
            ViewConfig = new JsonConfig();
            ViewConfig.LoadConfig(Resources.View);
            Config["RootPath"] = new ConfigValue(Application.StartupPath);
            Config["ResourcePath"] = new ConfigValue($"{Config["GamePath"]}/ImagePacks2");
        }

        /// <summary>
        /// 加载注册表
        /// </summary>
        private static void LoadRegistry() {
            if (Config["GamePath"].Value.Equals(string.Empty) || !Directory.Exists(Config["GamePath"].Value)) {
                var path = Registry.CurrentUser.OpenSubKey("software\\tencent\\dnf", RegistryKeyPermissionCheck.Default, RegistryRights.ReadKey).GetValue("InstallPath").ToString();
                Config["GamePath"] = new ConfigValue(path);
            }
            Config.Save();
        }

        /// <summary>
        /// 加载文件检测列表
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<string, string> LoadFileList() {
            var file = $"{Config["GamePath"]}/auto.lst";
            var dic = new Dictionary<string, string>();
            if (File.Exists(file)) {
                var fs = new StreamReader(file);
                while (!fs.EndOfStream) {
                    var str = fs.ReadLine();
                    str = str.Replace("\"", "");
                    var dt = str.Split(" ");
                    if (dt.Length < 1)
                        continue;
                    if (dt[0].EndsWith(".NPK"))
                        dic.Add(dt[0].GetName(), dt[1]);
                }
                fs.Close();
            }
            return dic;
        }

        /// <summary>
        /// 初始化字典
        /// </summary>
        internal static Dictionary<string, string> InitDictionary() {
            var dic = new Dictionary<string, string>();
            var file = $"{Config["RootPath"]}/dictionary.txt";
            if (File.Exists(file)) {
                var data = File.ReadAllText(file);
                var builder = new LSBuilder();
                var obj=builder.ReadProperties(data);
                obj.GetValue(ref dic);
            }
            return dic;
        }




        /// <summary>
        /// 判断程序是否需要更新
        /// </summary>
        /// <returns></returns>
        public static void CheckUpdate(bool Tips) {
            try {
                var builder = new LSBuilder();
                var obj = builder.Get(Config["UpdateUrl"].Value).GetValue(typeof(VersionInfo)) as VersionInfo;
                if (obj.Version.ToIntVersion() > Version.ToIntVersion()) {//若当前版本低于最新版本时，触发更新
                    if (MessageBox.Show(Language.Default["NeedUpdateTips"], "", MessageBoxButtons.OKCancel) != DialogResult.OK) {
                        return;                 //提示更新
                    }
                    StartUpdate();//启动更新
                } else if (Tips) {
                    MessageBox.Show(Language.Default["NeedNotUpdateTips"]);//提示不需要更新
                }
            } catch (Exception e) {
                Console.Write(e.StackTrace);
            }
            Config.Save();
        }



        /// <summary>
        /// 启动更新
        /// </summary>
        /// <param name="updateUrl"></param>
        /// <param name="address"></param>
        /// <param name="productName"></param>
        internal static void StartUpdate() {
            var name = $"{Config["RootPath"]}/{Config["UpdateExeName"]}";
            try {
                var client = new WebClient();
                client.DownloadFile(Config["UpdateExeUrl"].Value, name);
                client.Dispose();
                Process.Start(name);
            } finally {
                Environment.Exit(-1);
            }
        }

        /// <summary>
        /// 选择游戏目录
        /// </summary>
        internal static bool SelectPath() {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                var rsPath = $"{dialog.SelectedPath}/ImagePacks2";
                if (Directory.Exists(rsPath)) {
                    Config["GamePath"] = new ConfigValue(dialog.SelectedPath);
                    return true;
                } else { 
                    Messager.ShowWarnning("SelectPathIsInvalid");
                }
            }
            return false;
        }

        /// <summary>
        /// 提交bug
        /// </summary>
        /// <param name="remark"></param>
        /// <param name="contact"></param>
        /// <param name="buglog"></param>
        /// <returns></returns>
        internal static bool UploadBug(string remark, string contact, string log,string type) {
            try {
                var data = new Dictionary<string, object>() {
                    { "remark",remark },
                    { "contact",contact},
                    { "log", log },
                    { "type",type},
                    { "version",Version}
                };
                var builder = new LSBuilder();
                var resultObj=builder.Post(Config["DebugUrl"].Value,data);
                var result = (bool)resultObj.GetValue(typeof(bool));
                return result;
            } catch (Exception e) {
                Console.Write(e.StackTrace);
            }
            return false;
        }

    }
}
