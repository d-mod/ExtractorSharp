using ExtractorSharp.Data;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace ExtractorSharp.Core.Lib {
    /// <summary>
    /// FreeImage图片处理库封装
    /// </summary>
    public static class Zlib {
        /// <summary>
        /// zlib压缩
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data) {
            var size = (int)(data.LongLength * 1.001 + 12);//缓冲长度 
            var target = new byte[size];
            Compress(target, ref size, data, data.Length);
            var temp = new byte[size];
            Array.Copy(target, temp, size);
            return temp;
        }

        /// <summary>
        /// zlib解压缩
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data, int size) {
            var target = new byte[size];
            Decompress(target, ref size, data, data.Length);
            return target;
        }


        [DllImport("zlib1.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "compress")]
        private static extern int Compress([In, Out]byte[] dest, ref int destLen, byte[] source, int sourceLen);

        [DllImport("zlib1.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "uncompress")]
        private static extern int Decompress([In, Out]byte[] dest, ref int destLen, byte[] source, int sourceLen);

      



      

    
    }
}
 