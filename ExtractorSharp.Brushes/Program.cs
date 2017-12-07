using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Brushes {
    public class Program {

        public static void Main(string[] args) {
            Process[] array = Process.GetProcessesByName("notepad");
            var pro = array[0];
            var handle = pro.Handle;
            var data = new byte[1024];
            ReadProcessMemory(handle, 0x0c, data, data.Length, out int n);
        }

        [DllImport("kernel32.dll ")]
        static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] data, int nSize, out int lpNumberOfBytesRead);
    }

}
