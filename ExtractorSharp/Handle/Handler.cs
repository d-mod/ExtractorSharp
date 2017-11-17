using ExtractorSharp.Data;
using ExtractorSharp.Handle;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ExtractorSharp.Handle{
    /// <summary>
    /// img版本
    /// </summary>
    public enum Img_Version { 
        OGG = 0x00,
        Ver1 = 0x01,
        Ver2 = 0x02,
        Ver4 = 0x04,
        Ver5 = 0x05,
        Ver6 = 0x06,
    }
    /// <summary>
    /// IMG操作类
    /// </summary>
    public abstract class Handler{
        public static Dictionary<Img_Version, Type> Dic = new Dictionary<Img_Version, Type>();
        public Album Album;


        static Handler() {
            Regisity(Img_Version.OGG, typeof(OggHandler));
            Regisity(Img_Version.Ver1, typeof(FirstHandler));
            Regisity(Img_Version.Ver2, typeof(SecondHandler));
            Regisity(Img_Version.Ver4, typeof(FourthHandler));
            Regisity(Img_Version.Ver5, typeof(FifthHandler));
            Regisity(Img_Version.Ver6, typeof(SixthHandler));
        }
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
            foreach (var entity in Album.List)
                entity.Adjust();
            Album.Count = Album.List.Count;
            var Index_Data = AdjustIndex();
            var Work_Data = GetWorkData();
            var Suffix_Data = AdjustSuffix();
            Album.Info_Length += Work_Data.Length;
            var ms = new MemoryStream();
            if (Album.Version == Img_Version.OGG) {
                ms.Write(Index_Data);
                ms.Close();
            } else {
                ms.WriteString(Tools.IMG_FLAG);
                ms.WriteLong(Album.Info_Length);
                ms.WriteInt((int)Album.Version);
                ms.WriteInt(Album.Count);
                ms.Write(Index_Data);
                ms.Write(Work_Data);
                ms.Write(Suffix_Data);
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
            if (Dic.ContainsKey(Version))
                Dic.Remove(Version);
            Dic.Add(Version, type);
        }


        private byte[] GetWorkData() {
            var ms = new MemoryStream();
            if (Album.Work != null) {//加密字段
                ms.WriteInt((int)ColorBits.LINK);
                ms.WriteInt(Album.Count);
                ms.Encrypt(Album.Work);
            }
            ms.Close();
            return ms.ToArray();
        }

        public abstract byte[] AdjustIndex();

        public abstract byte[] AdjustSuffix();

        public virtual void ConvertToVersion(Img_Version Version) { }

        public Handler(Album Album) {
            this.Album = Album;
        }

    }
}
