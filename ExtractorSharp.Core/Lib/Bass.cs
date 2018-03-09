using System.Runtime.InteropServices;
using System.Threading;

namespace ExtractorSharp.Core.Lib {
    /// <summary>
    /// bass音效处理库封装
    /// </summary>
    public static class Bass {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [DllImport("bass.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "BASS_Init")]
        public static extern bool Init(int device, int freq, int flags, int win, int clsid);

        /// <summary>
        /// 创建句柄
        /// </summary>
        /// <returns></returns>
        [DllImport("bass.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "BASS_StreamCreateFile")]
        public static extern int CreateFromMemory(bool mem, byte[] file, long offset, long length, int flags);

        /// <summary>
        /// 关闭句柄
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("bass.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "BASS_StreamFree")]
        public static extern bool Close(int handle);
        /// <summary>
        /// 关闭Bass
        /// </summary>
        /// <returns></returns>
        [DllImport("bass.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "BASS_Free")]
        public static extern bool Close();

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <returns></returns>
        [DllImport("bass.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "BASS_Stop")]
        public static extern bool Stop();

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="restart">是否重新播放</param>
        /// <returns></returns>
        [DllImport("bass.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "BASS_ChannelPlay")]
        public static extern bool Play(int handle, bool restart);

        /// <summary>
        /// 暂停播放
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("bass.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "BASS_ChannelPause")]
        public static extern bool Pause(int handle);

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("bass.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "BASS_ChannelStop")]
        public static extern bool Stop(int handle);

        /// <summary>
        /// 获得播放位置
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("bass.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "BASS_ChannelGetPosition")]
        public static extern int GetPosition(int handle);


        /// <summary>
        /// 设置播放位置
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("bass.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "BASS_ChannelSetPosition")]
        public static extern int SetPosition(int handle);


        /// <summary>
        /// 获得播放长度
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("bass.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "BASS_ChannelGetLength")]
        public static extern int GetLength(int handle,int mode);

        [DllImport("bass.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "BASS_ChannelBytes2Seconds")]
        public static extern int GetTime(int handle,int position);

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        private static bool Init() => Init(-1, 44100, 0, 0, 0);

        /// <summary>
        /// 创建句柄
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static int CreateFromMemory(byte[] data) =>CreateFromMemory(true, data, 0, data.Length, 0);


        public static int Play(byte[] data) {
            Init();
            int handle = CreateFromMemory(data);
            Play(handle, false);
            return handle;
        }
    }
}
