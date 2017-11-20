using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Lib {
    /// <summary>
    /// FreeImage图片处理库封装
    /// </summary>
    public static class FreeImage {
        /// <summary>
        /// zlib压缩
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data) {
            var size = (long)(data.LongLength * 1.001 + 12);//缓冲长度 
            var target = new byte[(int)size];
            size = Compress(target, ref size, data, data.LongLength);
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
        public static byte[] Uncompress(byte[] data, long size) {
            var target = new byte[size];
            Uncompress(target,ref size, data, data.Length);
            return target;
        }


        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "_FreeImage_Initialise@4")]
        private static extern void Init();

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "_FreeImage_DeInitialise@0")]
        private static extern void Close();

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "_FreeImage_OpenMemory@8")]
        private static extern IntPtr OpenMemory(byte[] data, int size);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "_FreeImage_LoadFromMemory@12")]
        private static extern IntPtr LoadMemory(int format, IntPtr memory, int dib);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "_FreeImage_GetBits@4")]
        private static extern IntPtr GetBits(IntPtr handle);

        [DllImport("FreeImage.dll", CallingConvention =CallingConvention.Cdecl,EntryPoint = "_FreeImage_ZLibCompress@16")]
        private static extern int Compress([In, Out]byte[] dest,ref long destLen, byte[] source, long sourceLen);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "_FreeImage_ZLibUncompress@16")]
        private static extern int Uncompress([In, Out]byte[] dest,ref long destLen, byte[] source, long sourceLen);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "_FreeImage_GetFileTypeFromMemory@8")]
        private static extern int GetFormat(IntPtr memory,int dib);

        public static Bitmap Load(byte[] data, Size size) {
            Init();
            var memory = OpenMemory(data, data.Length);
            var format = GetFormat(memory,0);
            var handle = LoadMemory(format, memory,0);
            var bits = GetBits(handle);
            var length = size.Width * size.Height * 4;
            data = new byte[length];
            Marshal.Copy(bits, data, 0, data.Length);
            Close();
            return Tools.FromArray(data, size);
        }
    }
}
 