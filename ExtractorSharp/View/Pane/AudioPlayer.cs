﻿using System;
using System.Windows.Forms;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.View.Pane {
    internal partial class AudioPlayer : UserControl {

        private int handle;

        private bool isRun;

        public Language Language { set; get; } = Language.Empty;


        public AudioPlayer() {
            InitializeComponent();
            playButton.Click += Play;
            pauseButton.Click += Pause;
        }

        /// <summary>
        ///     显示窗口时播放
        /// </summary>
        public void Play(Album ogg) {
            Visible = true;
            if(handle != 0) {
                Bass.Stop(handle);
                Bass.Close(handle);
            }

            if(ogg != null && ogg.Version == ImgVersion.Other) {
                ogg.Adjust();
                groupBox1.Text = ogg.Name;
                handle = Bass.Play(ogg.Data);
                isRun = true;
                Play(null, null);
            }
        }

        /// <summary>
        ///     继续播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Play(object sender, EventArgs e) {
            Bass.Play(handle, isRun);
            isRun = true;
        }


        /// <summary>
        ///     暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Pause(object sender, EventArgs e) {
            Bass.Pause(handle);
            isRun = false;
        }

        /// <summary>
        ///     释放资源
        /// </summary>
        public void Close() {
            Bass.Stop(handle);
            Bass.Close(handle);
            Bass.Stop();
            Bass.Close();
        }
    }
}