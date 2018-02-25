using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using ExtractorSharp.Loose.Attr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ExtractorSharp.Handle {
    /// <summary>
    /// IMG操作类
    /// </summary>
    public abstract class Handler{
        private static Dictionary<Img_Version, Type> Dic = new Dictionary<Img_Version, Type>();

        public static List<Img_Version> Versions => Dic.Keys.ToList();

        public static Handler CreateHandler(Img_Version version,Album album) {
            var type = Dic[version];
            return type.CreateInstance(album) as Handler;
        }

        [LSIgnore]
        public Album Album;

        public Img_Version Version { get; }
        /// <summary>
        /// 从流初始化(默认读取)
        /// </summary>
        /// <param name="stream"></param>
        public abstract void CreateFromStream(Stream stream);
        /// <summary>
        /// 将字节集转换为图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract Bitmap ConvertToBitmap(ImageEntity entity);
        /// <summary>
        /// 将图片转换为字节集
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract byte[] ConvertToByte(ImageEntity entity);
        /// <summary>
        /// 新建指定个数的贴图
        /// </summary>
        /// <param name="count"></param>
        public virtual void NewImage(int count, ColorBits type, int index) { }
        /// <summary>
        /// 校正数据
        /// </summary>
        public void Adjust() {
            foreach (var entity in Album.List) {
                entity.Adjust();
            }
            Album.Count = Album.List.Count;
            var ms = new MemoryStream();
            if (Album.Version == Img_Version.OGG) {
                ms.Write(AdjustIndex());
                ms.Close();
            } else {
                var index_data = AdjustIndex();
                var suffix_data = AdjustSuffix();
                ms.WriteString(NpkReader.IMG_FLAG);
                ms.WriteLong(Album.Info_Length);
                ms.WriteInt((int)Album.Version);
                ms.WriteInt(Album.Count);
                ms.Write(index_data);
                ms.Write(suffix_data);
                ms.Close();
            }
            Album.Data = ms.ToArray();
            Album.Length = Album.Data.Length;
        }

        /// <summary>
        /// 注册版本处理器
        /// </summary>
        /// <param name="Version"></param>
        /// <param name="type"></param>
        public static void Regisity(Img_Version Version, Type type) {
            if (Dic.ContainsKey(Version)) {
                Dic.Remove(Version);
            }
            Dic.Add(Version, type);
        }


        public abstract byte[] AdjustIndex();

        public abstract byte[] AdjustSuffix();

        public virtual void ConvertToVersion(Img_Version Version) { }

        public Handler() {

        }

        public Handler(Album Album) {
            this.Album = Album;
        }

    }
}
