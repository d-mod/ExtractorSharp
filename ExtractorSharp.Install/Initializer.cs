using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.Install {
    class Initializer {

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
