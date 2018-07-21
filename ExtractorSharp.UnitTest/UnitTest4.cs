using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtractorSharp.UnitTest {
    [TestClass]
    public class UnitTest4 {
        private const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        private const int PROCESS_VM_READ = 0x0010;
        private const int PROCESS_VM_WRITE = 0x0020;

        [TestMethod]
        public void ReadMemory() {
            var process = GetProcess();
            if (process == null) return;
            var handle = process.Handle;
            var r = ReadProcessMemory(process.Handle.ToInt32(), 0x2f60, out var address, 4, out var ptr);
            Assert.IsTrue(r);
        }

        public Process GetProcess() {
            var arr = Process.GetProcesses();
            for (var i = 0; i < arr.Length; i++) {
                if (arr[i].ProcessName.Equals("DNF")) {
                    return arr[i];
                }
            }
            return null;
        }


        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);


        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll ")]
        private static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, out int lpBuffer, int nSize,
            out IntPtr lpNumberOfBytesRead);
    }
}