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
    public static class FreeImage {
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

        [DllImport("FreeImage",CallingConvention =CallingConvention.StdCall,EntryPoint = "_FreeImage_ConvertFromRawBits@36")]
        private static extern IntPtr FromArray(byte[] data, int width, int height, int pitch, int bpp, int red_mask, int green_mask, int blue_mask, int topdown);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetBPP@4")]
        private static extern int GetBPP(IntPtr dib);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetWidth@4")]
        private static extern int GetWidth(IntPtr handle);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetHeight@4")]
        private static extern int GetHeight(IntPtr handle);

        [DllImport("zlib1.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "compress")]
        private static extern int Compress([In, Out]byte[] dest, ref int destLen, byte[] source, int sourceLen);

        [DllImport("zlib1.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "uncompress")]
        private static extern int Decompress([In, Out]byte[] dest, ref int destLen, byte[] source, int sourceLen);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetFileTypeFromMemory@8")]
        private static extern int GetFormat(IntPtr memory, int dib);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetPalette@4")]
        private static extern IntPtr GetPalette(IntPtr handle);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetPixelIndex@16")]
        private static extern void GetPixelIndex(IntPtr handle, int w, int h, ref int index);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetPixelColor@16")]
        private static extern void GetPixelColor(IntPtr handle, int w, int h, out int value);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_OpenMultiBitmap@24")]
        private static extern IntPtr OpenMultiBitmap(int format,string filename,bool create_new, bool read_only, bool keep_cache_in_memory,int flags);

        [DllImport("FreeImage.dll",CallingConvention =CallingConvention.StdCall,EntryPoint ="_FreeImage_CloseMultiBitmap@8")]
        private static extern bool CloseMultiBitmap(IntPtr dib, int flag);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_LoadMultiBitmapFromMemory@12")]
        private static extern IntPtr LoadGifFromMemory(int format, IntPtr memory, int dib);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_GetPageCount@4")]
        private static extern int GetPageCount(IntPtr handle);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_LockPage@8")]
        private static extern IntPtr LockPage(IntPtr handle, int index);

        [DllImport("FreeImage.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "_FreeImage_UnlockPage@12")]
        private static extern void UnlockPage(IntPtr handle, IntPtr page, int able);




        public static Bitmap[] ReadGif(string path) {
            var fs = new FileStream(path, FileMode.Open);
            var data=fs.ReadToEnd();
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
                var bpp = GetBPP(page) / 8;
                var length = width * height * bpp;
                data = new byte[length];
                Marshal.Copy(bits, data, 0, data.Length);
                UnlockPage(handle, page, 1);
                array[i] = Bitmaps.FromArray(data, new Size(width, height));
                array[i].RotateFlip(RotateFlipType.Rotate180FlipX);
            }
            return array;
        }

        public static void WriteGif(string path, Image[] array) {
            var gif = AnimatedGif.AnimatedGif.Create(path, 100);
            for (var i = 0; i < array.Length; i++) {
                gif.AddFrame(array[i],AnimatedGif.GifQuality.Bit8);
            }
            gif.Dispose();
        }

    
    }
}
 