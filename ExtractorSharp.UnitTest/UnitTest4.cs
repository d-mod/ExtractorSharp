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
            var process=Process.GetProcessById(964);
            var handle = process.Handle;
            var error = Marshal.GetLastWin32Error();
            handle += 0x31f4;
            var ptr = Marshal.ReadInt32(handle);
            Assert.IsTrue(handle.ToInt32() > 0);
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
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll ")]
        private static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, out int lpBuffer, int nSize,
            out IntPtr lpNumberOfBytesRead);
    }
}