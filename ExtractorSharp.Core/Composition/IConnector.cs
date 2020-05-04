using System;
using System.Collections.Generic;
using System.Drawing;
using ExtractorSharp.Core.Config;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Core.Composition {
    public interface IConnector {
        List<IEffect> Effects { get; }

        SpriteEffect Effect { get; }

        List<IFileSupport> FileSupports { get; }


        #region draw

        void Draw(IPaint paint, Point location, decimal scale);

        #endregion

        void OnSaveChanged();

        event FileChangeEventHandler SaveChanged;

        event FileChangeEventHandler RecentChanged;

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

        List<string> Recent { set; get; }

        string SavePath { set; get; }

        bool IsSave { set; get; }

        #endregion

        #region flush

        /// <summary>
        ///     刷新画布
        /// </summary>
        void CanvasFlush();

        /// <summary>
        ///     刷新贴图列表,同时刷新画布
        /// </summary>
        void ImageListFlush();

        /// <summary>
        ///     仅刷新文件列表
        /// </summary>
        void FileListFlush();

        #endregion


        #region file method

        /// <summary>
        ///     添加文件
        /// </summary>
        /// <param name="clear">是否清空列表</param>
        /// <param name="args">文件路径</param>
        void AddFile(bool clear, params string[] args);

        /// <summary>
        ///     添加文件
        /// </summary>
        /// <param name="clear">是否清空列表</param>
        /// <param name="array">文件对象</param>
        void AddFile(bool clear, params Album[] array);

        /// <summary>
        /// 载入文件
        /// </summary>
        /// <param name="file"></param>
        void Load(string file);

        List<Album> LoadFile(params string[] args);

        void RemoveFile(params Album[] array);

        void Save();

        void Save(string file);

        void SelectPath();

        void Do(string name, params object[] args);

        object Dispatch(string name, params object[] args);

        void SendMessage(MessageType type, string msg);

        void SendSuccess(string name);

        void SendError(string name);

        void SendWarning(string name);

        #endregion
    }


    public delegate void FileChangeEventHandler(object sender, EventArgs e);
}