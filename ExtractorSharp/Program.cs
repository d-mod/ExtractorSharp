using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.EventArguments;
using ExtractorSharp.Json;
using ExtractorSharp.Properties;
using ExtractorSharp.Services;

namespace ExtractorSharp {
    /// <summary>
    ///     程序全局控制
    /// </summary>
    public class Program {
        internal static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        private static string[] Arguments;


        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main(string[] args) {

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.SetCompatibleTextRenderingDefault(true);
            Application.EnableVisualStyles();

            Arguments = args;
            Application.ThreadException += (o, e) => Starter.ThrowExecpetion(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (o, e) => Starter.ThrowExecpetion(e.ExceptionObject as Exception);


            var mainForm = Starter.GetValue<MainForm>();
            Application.Run(mainForm);

            /*            if (Config["AutoCheckUpdate"].Boolean) {
                            CheckUpdate(false);
                        }*/
            //LoadRegistry();
        }




        private static void OnShown(object sender, EventArgs e) {
            /*            var last = SubVersion(Config["Version"].Value);
                        var current = SubVersion(Version);
                        if (!last.Equals(current)) {
                            Config["Version"] = new ConfigValue(Version);
                            Config.Save();
                            if (Config["ShowFeature"].Boolean) {
                                Process.Start($"http://es.kritsu.net/feature/{Config["Version"]}.html");
                            }
                        }
                        if (Arguments.Length > 0) {
                            Connector.Load(Arguments[0]);
                        }*/
        }


        private static string SubVersion(string version) {
            var index = version.LastIndexOf(".");
            if(index > -1) {
                version = version.Substring(0, index);
            }
            return version;
        }







        /// <summary>
        ///     判断程序是否需要更新
        /// </summary>
        /// <returns></returns>
        public static void CheckUpdate(bool Tips) {
            /*            var builder = new LSBuilder();
                        var obj = builder.Get($"{Config["ApiHost"]}{Config["UpdateUrl"]}");
                        if (obj == null) {
                            return;
                        }
                        var info = obj["tag"].GetValue(typeof(VersionInfo)) as VersionInfo;
                        if (info != null && !info.Name.Equals(Version)) {
                            //若当前版本低于最新版本时，触发更新
                            if (MessageBox.Show(Language.Default["NeedUpdateTips"], Language.Default["Tips"],
                                    MessageBoxButtons.OKCancel) != DialogResult.OK) {
                                return; //提示更新
                            }
                            StartUpdate(); //启动更新
                        } else if (Tips) {
                            MessageBox.Show(Language.Default["NeedNotUpdateTips"]); //提示不需要更新
                        }
                        Config.Save();*/
        }


        /// <summary>
        ///     启动更新
        /// </summary>
        /// <param name="updateUrl"></param>
        /// <param name="address"></param>
        /// <param name="productName"></param>
        private static void StartUpdate() {
            /*            var name = $"{Config["RootPath"]}\\{Config["UpdateExeName"]}";
                        try {
                            var client = new WebClient();
                            client.DownloadFile(Config["UpdateExeUrl"].Value, name);
                            client.Dispose();
                            Process.Start(name);
                        } finally {
                            Environment.Exit(-1);
                        }*/
        }
    }
}
