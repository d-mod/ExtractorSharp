using System;
using System.Reflection;
using System.Windows.Forms;

namespace ExtractorSharp.Install {
    public static class Program {
        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static void Main(params string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new InstallForm(args));
        }


        public static string Resolve(this string path) {
            path = path.Replace("/", "\\");
            path = path.Replace("\\\\", "\\");
            return path;
        }

        public static object CreateInstance(this Type type, params object[] args) {
            return type.Assembly.CreateInstance(
                type.FullName ?? throw new InvalidOperationException(), true, BindingFlags.Default, null, args, null,
                null);
        }
    }
}