using ExtractorSharp.Composition;
using ExtractorSharp.Config;
using ExtractorSharp.Data;
using ExtractorSharp.Draw;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ExtractorSharp.Core {
    public interface IConnector {
        #region  about file
        int[] CheckedFileIndices { get; }

        Album[] CheckedFiles { get; }


        Album[] FileArray { get; }

        int FileCount { get; }

        Album SelectedFile { get; }

        int SelectedFileIndex { get; set; }

        List<Album> List { get; }

        #endregion

        #region about image

        int[] CheckedImageIndices { get; }

        Sprite[] CheckedImages { get; }

        Sprite[] ImageArray { get; }

        int ImageCount { get; }

        Sprite SelectedImage { get; }

        int SelectedImageIndex { get; set; }

        #endregion

        #region field
        IConfig Config { get; }

        Language Language { get; }

        List<Language> LanguageList { get; }

        string SavePath { set; get; }

        bool IsSave { set; get; }

        #endregion

        #region flush
        /// <summary>
        /// 刷新画布
        /// </summary>
        void CanvasFlush();
        /// <summary>
        /// 刷新贴图列表,同时刷新画布
        /// </summary>
        void ImageListFlush();
        /// <summary>
        /// 仅刷新文件列表
        /// </summary>
        void FileListFlush();

        #endregion


        #region file method
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="clear">是否清空列表</param>
        /// <param name="args">文件路径</param>
        void AddFile(bool clear, params string[] args);
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="clear">是否清空列表</param>
        /// <param name="array">文件对象</param>
        void AddFile(bool clear, params Album[] array);

        void RemoveFile(params Album[] array);

        void Save();

        void Save(string file);

        void SelectPath();

        void Do(string name, params object[] args);


        #endregion


        #region draw

        void Draw(IPaint paint,Point location,decimal scale);
        #endregion

        void OnSaveChanged();

        event EventHandler SaveChanged;

        List<ISpriteConverter> SpriteConverters { get; }

        SpriteConverter SpirteConverter { get; }
    }
}
