using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            var size = (int)(data.LongLength * 1.001 + 12);//缓冲长度 
            var target = new byte[size];
            size = Compress(target, ref size, data, data.Length);
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
        public static byte[] Uncompress(byte[] data, int size) {
            var target = new byte[size];
            Uncompress(target, ref size, data, data.Length);
            return target;
        }


        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_Initialise@4")]
        private static extern void Init(int id);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_DeInitialise@0")]
        private static extern void Close();

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_Allocate@24")]
        private static extern IntPtr Allocate(int width,int height,int bpp,int r,int g,int b);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_Unload@4")]
        private static extern void Unload(IntPtr dib);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_OpenMemory@8")]
        private static extern IntPtr OpenMemory(byte[] data, int size);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_WriteMemory@16")]
        private static extern uint WriteMemory(byte[] data, int size, int offset, IntPtr memory);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_LoadFromMemory@12")]
        private static extern IntPtr LoadFromMemory(int format, IntPtr memory, int dib);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_SaveToMemory@16")]
        private static extern bool SaveToMemory(int fif, IntPtr dib, IntPtr memory, int flag);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_CloseMemory@4")]
        private static extern void CloseMemory(IntPtr memmory);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetBits@4")]
        private static extern IntPtr GetBits(IntPtr handle);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetWidth@4")]
        private static extern int GetWidth(IntPtr handle);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetHeight@4")]
        private static extern int GetHeight(IntPtr handle);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_ZLibCompress@16")]
        private static extern int Compress([In, Out]byte[] dest, ref int destLen, byte[] source, int sourceLen);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_ZLibUncompress@16")]
        private static extern int Uncompress([In, Out]byte[] dest, ref int destLen, byte[] source, int sourceLen);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetFileTypeFromMemory@8")]
        private static extern int GetFormat(IntPtr memory, int dib);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetPalette@4")]
        private static extern IntPtr GetPalette(IntPtr handle);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetPixelIndex@16")]
        private static extern void GetPixelIndex(IntPtr handle, int w, int h, ref int index);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetPixelColor@16")]
        private static extern void GetPixelColor(IntPtr handle, int w, int h, out int value);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_OpenMultiBitmap@24")]
        private static extern IntPtr OpenMultiBitmap(int format,string filename,int create_new,int read_only,int keep_cache_in_memory,int flags);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_LoadMultiBitmapFromMemory@12")]
        private static extern IntPtr LoadGifFromMemory(int format, IntPtr memory, int dib);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetPageCount@4")]
        private static extern int GetPageCount(IntPtr handle);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_LockPage@8")]
        private static extern IntPtr LockPage(IntPtr handle, int index);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_UnlockPage@12")]
        private static extern void UnlockPage(IntPtr handle, IntPtr page, int able);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_AppendPage@8")]
        private static extern void AppendPage(IntPtr bitmap, IntPtr page);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_Save@16")]
        private static extern bool Save(int fif, IntPtr dib, string filename, int flag);

        public static Bitmap Load(byte[] data, Size size) {
            var memory = OpenMemory(data, data.Length);
            var format = GetFormat(memory, 0);
            var handle = LoadFromMemory(format, memory, 0);
            var bits = GetBits(handle);
            var length = size.Width * size.Height * 4;
            data = new byte[length];
            Marshal.Copy(bits, data, 0, data.Length);
            return Tools.FromArray(data, size);
        }



        public static Bitmap[] ReadGif(string path) {
            var fs = new FileStream(path, FileMode.Open);
            var data = new byte[fs.Length];
            fs.Read(data);
            fs.Close();
            var memory = OpenMemory(data, data.Length);
            var format = GetFormat(memory, 0);
            var handle = LoadGifFromMemory(format, memory, 2);
            var count = GetPageCount(handle);
            var array = new Bitmap[count];
            for (var i = 0; i < count; i++) {
                var page = LockPage(handle, i);
                var width = GetWidth(page);
                var height = GetHeight(page);
                var bits = GetBits(page);
                var length = width * height * 4;
                data = new byte[length];
                Marshal.Copy(bits, data, 0, data.Length);
                UnlockPage(handle, page, 1);
                array[i] = Tools.FromArray(data, new Size(width, height));
                array[i].RotateFlip(RotateFlipType.Rotate180FlipX);
            }
            return array;
        }

        public static void WriteGif(string path, ImageEntity[] array) {
            var gif = OpenMultiBitmap(25, "", 1, 0, 0, 2);
            int w = 1;
            int h = 1;
            int x = 800;
            int y = 600;
            foreach (var entity in array) {
                if (entity.Width + entity.X > w)
                    w = entity.Width + entity.X;
                if (entity.Height + entity.X > h)
                    h = entity.Height + entity.Y;
                if (entity.X < x)
                    x = entity.X;
                if (entity.Y < y)
                    y = entity.Y;
            }

            for (var i = 0; i < array.Length; i++) {
                var dib = Allocate(array[i].Width, array[i].Height, 8,0,0,0);
                var memory = OpenMemory(new byte[0], 0);
                SaveToMemory(0, dib, memory, 0);
                var bmp = new Bitmap(w, h);
                using (var g = Graphics.FromImage(bmp)) {
                    g.DrawImage(array[i].Picture, array[i].X - x, array[i].Y - y);
                }
                var data = array[i].Picture.ToArray();
                array[i].Picture.GetHbitmap();
                WriteMemory(data, data.Length, 0, memory);
                CloseMemory(memory);
                AppendPage(gif, dib);
                Save(0, dib, "d:/test/" + i + ".png", 0);
            }
            var count=GetPageCount(gif);
            Save(25, gif, path, 0);
        }
    }
}
 