using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Pvf {
    static class Program {
        static void Main(string[] args) {
            var f = "D:/地下城与勇士/Script.pvf";
            var stream = new FileStream(f, FileMode.Open);
            stream.Read(new byte[48]);
            var os = new StreamWriter("d:/test.txt");
            var j = 0;
            List<int> list = new List<int>();
            while (stream.Position<stream.Length) {
                var i = stream.ReadInt();
                stream.Read(new byte[20]);
                if (i < 1) {
                    Console.WriteLine(1);
                }
            }
            os.Close();
            stream.Close();
        }

        public static int Read(this Stream stream,byte[] data) {
            return stream.Read(data, 0, data.Length);
        }

        public static int ReadInt(this Stream stream) {
            var data = new byte[4];
            stream.Read(data, 0, 4);
            return BitConverter.ToInt32(data,0);
        }
    }
}
