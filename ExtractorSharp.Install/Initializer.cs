using System.Windows.Forms;
using Microsoft.Win32;

namespace ExtractorSharp.Install {
    internal class Initializer {
        public void SetOpenWith() {
            var path = $"{Application.StartupPath}/extractorsharp.exe";
            var cmd = $"\"{path}\" \"%1\"";
            var name = "esharp";
            var surekamKey = Registry.ClassesRoot.CreateSubKey(name);
            surekamKey.SetValue("URL Protocol", "");
            var commandKey = surekamKey.CreateSubKey(@"shell\open\command");
            commandKey.SetValue("", cmd);
        }
    }
}