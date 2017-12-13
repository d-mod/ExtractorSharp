using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Handle;
using ExtractorSharp.Lib;
using System;
using System.Windows.Forms;

namespace ExtractorSharp.View {
    partial class OggPlayer : UserControl {
        private Controller Controller;
        private int handle;
        private bool isRun;
        private Language Language = Language.Default;
        public OggPlayer(Controller Controller) {
            this.Controller = Controller;
            InitializeComponent();
            Bass.Init();
            playButton.Click += Play;
            pauseButton.Click += Pause;
        }

        /// <summary>
        /// 显示窗口时播放
        /// </summary>
        public void Play() {
            Visible = true;
            if (handle != 0) {
                Bass.Stop(handle);
                Bass.Close(handle);
            }
            var ogg = Controller.SelectAlbum;
            if (ogg != null && ogg.Version == Img_Version.OGG) {
                ogg.Adjust();
                groupBox1.Text = ogg.Name;
                handle = Bass.CreateFromMemory(ogg.Data);
                isRun = true;
                Play(null, null);
            }
        }

        /// <summary>
        /// 继续播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Play(object sender,EventArgs e) {
            Bass.Play(handle, isRun);
            isRun = true;
        }




        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Pause(object sender,EventArgs e) {
            Bass.Pause(handle);
            isRun = false;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Close() {
            Bass.Stop(handle);
            Bass.Close(handle);
            Bass.Stop();
            Bass.Close();
        }
        
    }
}
