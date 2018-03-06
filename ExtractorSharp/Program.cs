using ExtractorSharp.Component;
using ExtractorSharp.Config;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using ExtractorSharp.Handle;
using ExtractorSharp.Json;
using ExtractorSharp.Properties;
using ExtractorSharp.View;
using ExtractorSharp.View.Dialog;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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

        [Export(typeof(IConnector))]
        internal static IConnector Connector { set; get; }

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
            RegistyHandler();
            if (Config["AutoUpdate"].Boolean) {
                CheckUpdate(false);
            }
            if (Config["Profile"].Value.Equals("release")) {
                Application.ThreadException += ShowDebug; 
            }
            Application.SetCompatibleTextRenderingDefault(true);           
            Application.EnableVisualStyles();
            LoadRegistry();
            Controller = new Controller();
            Viewer = new Viewer();
            Drawer = new Drawer();
            Drawer.Select(Config["Brush"].Value);
            Drawer.Color = Config["BrushColor"].Color;
            Form = new MainForm();
            Form.Shown += OnShown;
            Connector = Form.Connector;
            Connector.AddFile(true, args);
            RegistyDialog();
            Viewer.DialogShown += ViewerDialogShown;
            Merger = new Merger();
            Hoster = new Hoster();
            Application.Run(Form);
        }

        private static void ShowDebug(object sender, ThreadExceptionEventArgs e) {
            var log = $"{e.Exception.Message};\r\n{e.Exception.StackTrace}";
            var data = Encoding.Default.GetBytes(log);
            log = Convert.ToBase64String(data);
            var dir = $"{Config["RootPath"]}/log";
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }      
            var current = $"{dir}/{DateTime.Now.ToString("yyyyMMddHHmmss")}.log";
            using (var fs = new FileStream(current, FileMode.Create)) {
                fs.Write(data);
            }
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
                    Connector.AddFile(true, new string[] { command });
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
            Viewer.Regisity("Merge", typeof(MergeDialog));
            Viewer.Regisity("newImg", typeof(NewImgDialog));
            Viewer.Regisity("convert", typeof(ConvertDialog));
            Viewer.Regisity("changePosition", typeof(ChangePositonDialog));
            Viewer.Regisity("about", typeof(AboutDialog));
            Viewer.Regisity("debug", typeof(BugDialog));
            Viewer.Regisity("newImage", typeof(NewImageDialog));
            Viewer.Regisity("canvas", typeof(CanvasDialog));
            Viewer.Regisity("setting", typeof(SettingDialog));
            Viewer.Regisity("saveImage", typeof(SaveImageDialog));
            Viewer.Regisity("version", typeof(VersionDialog));
         }


        public static void RegistyHandler() {
            Handler.Regisity(Img_Version.OGG, typeof(OggHandler));
            Handler.Regisity(Img_Version.Ver1, typeof(FirstHandler));
            Handler.Regisity(Img_Version.Ver2, typeof(SecondHandler));
            Handler.Regisity(Img_Version.Ver4, typeof(FourthHandler));
            Handler.Regisity(Img_Version.Ver5, typeof(FifthHandler));
            Handler.Regisity(Img_Version.Ver6, typeof(SixthHandler));
        }

        private static void ViewerDialogShown(object sender, DialogEventArgs e) {
            e.Dialog = e.DialogType.CreateInstance(Connector) as ESDialog;
            e.Dialog.Owner = Form;
        }


        /// <summary>
        /// 加载语言
        /// </summary>
        private static void LoadLanguage() {
            var chinese = Language.CreateFromJson(Resources.Chinese);
            Language.List = new List<Language>();
            Language.List.Add(chinese);
            var path =  $"{Config["RootPath"]}/lan/";
            Language.CreateFromDir(path);
            if (Config["LCID"].Integer == -1) {
                Config["LCID"] = new ConfigValue(Application.CurrentCulture.LCID);
                Config.Save();
            }
            Language.Local_LCID = Config["LCID"].Integer;
        }

        private static void LoadConfig() {
            Config = new JsonConfig();
            Config.LoadConfig(Resources.Config);
            Config["RootPath"] = new ConfigValue(Application.StartupPath);
            Config["ResourcePath"] = new ConfigValue($"{Config["GamePath"]}/ImagePacks2");
        }

        /// <summary>
        /// 加载注册表
        /// </summary>
        private static void LoadRegistry() {
            try {
                if (Config["GamePath"].Value.Equals(string.Empty) || !Directory.Exists(Config["GamePath"].Value)) {
                    var path = Registry.CurrentUser.OpenSubKey("software\\tencent\\dnf", RegistryKeyPermissionCheck.Default, RegistryRights.ReadKey).GetValue("InstallPath").ToString();
                    Config["GamePath"] = new ConfigValue(path);
                 }
                Config.Save();
            } catch (Exception e) {
                Console.Write(e);
            }
        }

       

   


        /// <summary>
        /// 判断程序是否需要更新
        /// </summary>
        /// <returns></returns>
        public static void CheckUpdate(bool Tips) {
            try {
                var builder = new LSBuilder();
                var obj = builder.Get(Config["UpdateUrl"].Value).GetValue(typeof(VersionInfo)) as VersionInfo;
                if (!obj.Name.Equals(Version)) {//若当前版本低于最新版本时，触发更新
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
                    Config["ResourcePath"] = new ConfigValue($"{dialog.SelectedPath}/ImagePacks2");
                    Config.Save();
                    return true;
                } else {
                    Messager.ShowWarnning("SelectPathIsInvalid");
                }
            }
            return false;
        }

    }
}
