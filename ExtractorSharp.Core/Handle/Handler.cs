using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Json.Attr;

namespace ExtractorSharp.Core.Handle {
    /// <summary>
    ///     IMG操作类
    /// </summary>
    public abstract class Handler {
        private static readonly Dictionary<ImgVersion, Type> Dic = new Dictionary<ImgVersion, Type>();

        static Handler() {
            Regisity(ImgVersion.Other, typeof(OtherHandler));
            Regisity(ImgVersion.Ver1, typeof(FirstHandler));
            Regisity(ImgVersion.Ver2, typeof(SecondHandler));
            Regisity(ImgVersion.Ver4, typeof(FourthHandler));
            Regisity(ImgVersion.Ver5, typeof(FifthHandler));
            Regisity(ImgVersion.Ver6, typeof(SixthHandler));
        }

        [LSIgnore] public Album Album;


        public Handler(Album album) {
            this.Album = album;
        }

        public static List<ImgVersion> Versions => Dic.Keys.ToList();

        public ImgVersion Version { get; } = ImgVersion.Ver2;

        public static Handler CreateHandler(ImgVersion version, Album album) {
            var type = Dic[version];
            return type.CreateInstance(album) as Handler;
        }

        /// <summary>
        ///     从流初始化(默认读取)
        /// </summary>
        /// <param name="stream"></param>
        public abstract void CreateFromStream(Stream stream);

        /// <summary>
        ///     获取图片数据
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public abstract ImageData GetImageData(Sprite sprite);

        /// <summary>
        ///     将图片转换为字节集
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public abstract byte[] ConvertToByte(Sprite sprite);

        /// <summary>
        ///     校正数据
        /// </summary>
        public void Adjust() {
            foreach(var sprite in this.Album.List) {
                sprite.Adjust();
            }
            this.Album.Count = this.Album.List.Count;
            var ms = new MemoryStream();
            var data = this.AdjustData();
            if(this.Album.Version > ImgVersion.Ver1) {
                ms.WriteString(NpkCoder.IMG_FLAG);
                ms.WriteLong(this.Album.IndexLength);
                ms.WriteInt((int)this.Album.Version);
                ms.WriteInt(this.Album.Count);
            }
            ms.Write(data);
            ms.Close();
            this.Album.Data = ms.ToArray();
            this.Album.Length = this.Album.Data.Length;
        }

        /// <summary>
        ///     注册版本处理器
        /// </summary>
        /// <param name="version"></param>
        /// <param name="type"></param>
        public static void Regisity(ImgVersion version, Type type) {
            if(Dic.ContainsKey(version)) {
                Dic.Remove(version);
            }
            Dic.Add(version, type);
        }

        public virtual void ConvertToVersion(ImgVersion version) { }

        public virtual byte[] AdjustData() {
            return new byte[0];
        }
    }
}