using ExtractorSharp.Command;
using ExtractorSharp.Command.DrawCommand;
using ExtractorSharp.Command.FileCommand;
using ExtractorSharp.Command.ImageCommand;
using ExtractorSharp.Command.ImgCommand;
using ExtractorSharp.Command.LayerCommand;
using ExtractorSharp.Command.MergeCommand;
using ExtractorSharp.Command.PaletteCommand;
using ExtractorSharp.Component;
using ExtractorSharp.Config;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using ExtractorSharp.Exceptions;
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
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += ShowDebug;
            AppDomain.CurrentDomain.UnhandledException += CatchException;
            Application.SetCompatibleTextRenderingDefault(true);
            Application.EnableVisualStyles();
            LoadRegistry();
            Controller = new Controller();
            RegistyCommand();
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

        private static void CatchException(object sender, UnhandledExceptionEventArgs e) {
            ShowDebug(sender, new ThreadExceptionEventArgs(e.ExceptionObject as Exception));
        }

        private static void RegistyCommand() {
            Controller.Registry("addImg", typeof(AddFile));
            Controller.Registry("deleteImg", typeof(DeleteFile));
            Controller.Registry("renameImg", typeof(RenameFile));
            Controller.Registry("replaceImg", typeof(ReplaceFile));
            Controller.Registry("replaceGif", typeof(ReplaceGif));
            Controller.Registry("newImg", typeof(NewFile));
            Controller.Registry("hideImg", typeof(HideFile));
            Controller.Registry("sortImg", typeof(SortFile));
            Controller.Registry("saveImg", typeof(SaveFile));

            Controller.Registry("cutImg", typeof(CutFile));
            Controller.Registry("pasteImg", typeof(PasteFile));

            Controller.Registry("repairFile", typeof(RepairFile));
            Controller.Registry("splitFile", typeof(SplitFile));
            Controller.Registry("mixFile", typeof(MixFile));
            Controller.Registry("moveFile", typeof(MoveFile));

            Controller.Registry("newImage", typeof(NewImage));
            Controller.Registry("replaceImage", typeof(ReplaceImage));
            Controller.Registry("hideImage", typeof(HideImage));
            Controller.Registry("linkImage", typeof(LinkImage));
            Controller.Registry("deleteImage", typeof(DeleteImage));
            Controller.Registry("saveImage", typeof(SaveImage));
            Controller.Registry("saveGif", typeof(SaveGif));
            Controller.Registry("changePosition", typeof(ChangePosition));
            Controller.Registry("changeSize", typeof(ChangeSize));

            Controller.Registry("cutImage", typeof(CutImage));
            Controller.Registry("pasteImage", typeof(PasteImage));
            Controller.Registry("pasteSingleImage", typeof(PasteSingleImage));
            Controller.Registry("moveImage", typeof(MoveImage));

            Controller.Registry("addMerge", typeof(AddMerge));
            Controller.Registry("removeMerge", typeof(RemoveMerge));
            Controller.Registry("clearMerge", typeof(ClearMerge));
            Controller.Registry("runMerge", typeof(RunMerge));
            Controller.Registry("moveMerge", typeof(MoveMerge));

            Controller.Registry("canvasImage", typeof(CanvasImage));
            Controller.Registry("uncanvasImage", typeof(UnCanvasImage));
            Controller.Registry("lineDodge", typeof(LineDodge));

            Controller.Registry("renameLayer", typeof(RenameLayer));

            Controller.Registry("changeColor", typeof(ChangeColor));
            Controller.Registry("pencil", typeof(PencilDraw));
            Controller.Registry("eraser", typeof(EraserDraw));
            Controller.Registry("moveTools", typeof(MoveToolsDraw));
        }

        private static void ShowDebug(object sender, ThreadExceptionEventArgs e) {
            try {
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
                if ((e.Exception is ProgramException && Connector != null)) {
                    Connector.SendError(e.Exception.Message);
                } else if (Config["Profile"].Value.Equals("release")) {
                    Viewer.Show("debug", "debug", log);
                }
            } catch (Exception ex) {

            }
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
            Viewer.Regisity("setting", typeof(SettingDialog));
            Viewer.Regisity("saveImage", typeof(SaveImageDialog));
            Viewer.Regisity("version", typeof(VersionDialog));
            Viewer.Regisity("changeSize", typeof(ChangeSizeDialog));
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
            Config.LoadConfig(Resources.View);
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
            var builder = new LSBuilder();
            var obj = builder.Get(Config["UpdateUrl"].Value);
            if (obj == null) {
                return;
            }
            var info = obj.GetValue(typeof(VersionInfo)) as VersionInfo;
            if (info != null && !info.Name.Equals(Version)) {//若当前版本低于最新版本时，触发更新
                if (MessageBox.Show(Language.Default["NeedUpdateTips"], Language.Default["Tips"], MessageBoxButtons.OKCancel) != DialogResult.OK) {
                    return;                 //提示更新
                }
                StartUpdate();//启动更新
            } else if (Tips) {
                MessageBox.Show(Language.Default["NeedNotUpdateTips"]);//提示不需要更新
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
                    Connector.SendError("SelectPathIsInvalid");
                }
            }
            return false;
        }

    }
}
