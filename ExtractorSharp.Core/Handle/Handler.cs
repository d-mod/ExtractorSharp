using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ExtractorSharp.Core.Coder;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Json.Attr;

namespace ExtractorSharp.Core.Handle {
    /// <summary>
    ///     IMG操作类
    /// </summary>
    public abstract class Handler {
        private static readonly Dictionary<ImgVersion, Type> Dic = new Dictionary<ImgVersion, Type>();

        [LSIgnore] public Album Album;


        public Handler(Album album) {
            Album = album;
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
        ///     将字节集转换为图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract Bitmap ConvertToBitmap(Sprite entity);

        /// <summary>
        ///     将图片转换为字节集
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract byte[] ConvertToByte(Sprite entity);

        /// <summary>
        ///     新建指定个数的贴图
        /// </summary>
        /// <param name="count"></param>
        /// <param name="type"></param>
        /// <param name="index"></param>
        public virtual void NewImage(int count, ColorBits type, int index) { }

        /// <summary>
        ///     校正数据
        /// </summary>
        public void Adjust() {
            foreach (var entity in Album.List) {
                entity.Adjust();
            }
            Album.Count = Album.List.Count;
            var ms = new MemoryStream();
            var data = AdjustData();
            if (Album.Version > ImgVersion.Ver1) {
                ms.WriteString(NpkCoder.IMG_FLAG);
                ms.WriteLong(Album.IndexLength);
                ms.WriteInt((int) Album.Version);
                ms.WriteInt(Album.Count);
            }
            ms.Write(data);
            ms.Close();
            Album.Data = ms.ToArray();
            Album.Length = Album.Data.Length;
        }

        /// <summary>
        ///     注册版本处理器
        /// </summary>
        /// <param name="version"></param>
        /// <param name="type"></param>
        public static void Regisity(ImgVersion version, Type type) {
            if (Dic.ContainsKey(version)) {
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