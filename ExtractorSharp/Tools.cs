using ExtractorSharp.Core;
using ExtractorSharp.Draw;
using ExtractorSharp.UI;
using ExtractorSharp.Handle;
using ExtractorSharp.Lib;
using ExtractorSharp.Users;
using ExtractorSharp.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using ExtractorSharp.Data;

namespace ExtractorSharp {
    /// <summary>
    /// 工具函数集
    /// </summary>
    public static class Tools {

        private const string NPK_FlAG = "NeoplePack_Bill";
        public const string IMG_FLAG = "Neople Img File";
        private const string IMAGE_FLAG = "Neople Image File";
        private const string Model_FLAG = "ExtractorSharp Model File";
        private const string ENTITY_FLAG = "ExtractorSharp Image File";
        private static char[] key;
        private static char[] Key {
            get {
                if (key != null)
                    return key;
                var cs = new char[256];
                var temp = "puchikon@neople dungeon and fighter ".ToArray();
                temp.CopyTo(cs, 0);
                var ds = new char[] { 'D', 'N', 'F' };
                for (var i = temp.Length; i < 255; i++) {
                    cs[i] = ds[i % 3];
                }
                cs[255] = '\0';
                return key = cs;
            }
        }

        internal static byte[] Decrpt_Key => Decrpt_key ?? (Decrpt_key = Encoding.Unicode.GetBytes("这都破解了,那你很棒棒哦"));

        private static byte[] Decrpt_key;


        public static List<Album> Load(bool onlyPath, params string[] files) {
            var List = new List<Album>();
            foreach (string file in files)
                List.AddRange(Load(onlyPath, file));
            return List;
        }

        public static List<Album> Load(params string[] files) => Load(false, files);

        /// <summary>
        /// 根据已有img名，NPK名获得img对象
        /// </summary>
        /// <param name="file"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Album LoadAlbum(string file, string name) {
            var list = Load(file);
            Album al = null;
            foreach (var img in list) {
                if (img.Path.Equals(name)) {
                    al = img;
                    break;
                }
            }
            return al;
        }

        /// <summary>
        /// 根据已有的img名数组，NPK名获得img集合
        /// </summary>
        /// <param name="file"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public static IEnumerable<Album> LoadAlbumRange(string file,IEnumerable<string> nameList) {
            foreach (var img in Load(file)) {
                if (nameList.Contains(img.Name))
                    yield return img;
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static IEnumerable<Album> Load(bool onlyPath, string file) {
            var List = new List<Album>();
            if (Directory.Exists(file)) {
                foreach (var f in Directory.GetFiles(file))
                    List.AddRange(Load(onlyPath, f));
                return List;
            }
            if (!File.Exists(file)) {
                return List;
            }
           ;
            try {
                using (var stream = new FileStream(file, FileMode.Open)) {
                    if (onlyPath)
                        return stream.ReadInfo();
                    IEnumerable<Album> enums = stream.ReadNPK(file);
                    return enums;
                }
            } catch (IOException e) {
                Messager.ShowMessage(Msg_Type.Error, "文件" + file + "占用，打开失败");
                return List;
            }
        }

        public static void GetOriginal(Action<Album,Album> restore,params Album[] Array) {
            var dic = new Dictionary<string, List<string>>();//将img按NPK分类
            foreach (var item in Array) {
                var path = item.Path;
                var index = path.LastIndexOf("/"); //判断是否有前缀
                if (index > 0)
                    path = path.Substring(0, index);//获得img所在的文件夹
                path = path.Replace("/", "_");//通常情况下，游戏原文件名和img文件夹对应
                path = Program.ResourcePath + "/" + path + ".NPK";//得到游戏原文件路径
                if (!dic.ContainsKey(path))
                    dic.Add(path, new List<string>());
                dic[path].Add(item.Name);
            }
            var list = new List<Album>();
            foreach (var item in dic.Keys)
                list.AddRange(Tools.LoadAlbumRange(item, dic[item].ToArray()));//读取游戏原文件
            for (var i = 0; i < Array.Length; i++) { //模型文件
                foreach (var item2 in list) { //游戏原文件
                    if (Array[i].Path.Equals(item2.Path)) {
                        restore.Invoke(item2, Array[i]);               
                    }
                }
            }
            
        }


        /// <summary>
        /// 保存单个img
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="al"></param>     
        public static void SaveFile(string fileName, Album al) {
            if (al.Work != null && (!al.Work.IsDecrypt || !al.Work.CanExtract))
                Messager.ShowMessage(Msg_Type.Warning, "该img已禁止提取");
            using (var stream = new FileStream(fileName, FileMode.Create)) {
                al.Adjust();
                stream.Write(al.Data);
            }
        }

        /// <summary>
        /// 保存多个img到文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <param name="List"></param>
        public static void SaveDirectory(string path, IEnumerable<Album> List) {
            foreach (var album in List)
                SaveFile(path + "/" + album.Name, album);
        }


        /// <summary>
        /// 保存为NPK
        /// </summary>
        /// <param name="fileName"></param>
        public static void WriteNPK(string fileName, List<Album> List) {
            var position = 52 + List.Count * 264;
            for (var i = 0; i < List.Count; i++) {
                List[i].Adjust();
                if (i > 0)
                    position += List[i - 1].Length;
                List[i].Offset = position;
            }
            var ms = new MemoryStream();
            ms.WriteString(NPK_FlAG);
            ms.WriteInt(List.Count);
            foreach (var album in List) {
                ms.WriteInt(album.Offset);
                ms.WriteInt(album.Length);
                ms.WritePath(album.Path, album?.Work?.Key);
            }
            ms.Close();
            var data = ms.ToArray();
            var stream = new FileStream(fileName, FileMode.Create);
            stream.Write(data);
            stream.Write(CompileCode(data));
            foreach (var album in List) {
                stream.Write(album.Data);
            }
            stream.Close();
        }

        internal static Bitmap[] FromGif(string path) {
            var decoder = new ScreenToGif.Encoding.GifDecoder();
            decoder.Read(path);
            var array = new Bitmap[decoder.GetFrameCount()];
            for (var i = 0; i < array.Length; i++)
                array[i] = decoder.GetFrame(i) as Bitmap;
            return array;
        }

        internal static void SaveGif(string path,ImageEntity[] array) {
            using (var fs = new FileStream(path, FileMode.Create)) {
                using (var encoder = new ScreenToGif.Encoding.GifEncoder(fs)) {
                    int w = 1;
                    int h = 1;
                    int x = 800;
                    int y = 600;
                    foreach (var entity in array) {
                        if (entity.Width + entity.X > w)
                            w = entity.Width + entity.X;
                        if (entity.Height + entity.X > h)
                            h = entity.Height + entity.Y;
                        if (entity.X < x)
                            x = entity.X;
                        if (entity.Y < y)
                            y = entity.Y;
                    }
                    foreach (var entity in array) {
                        var bmp = new Bitmap(w, h);
                        using (var g = Graphics.FromImage(bmp))
                            g.DrawImage(entity.Picture, entity.X - x, entity.Y - y);
                        encoder.AddFrame(bmp);
                    }
                }
            }
        }

        /// <summary>
        /// 移除指定开头的后缀名
        /// </summary>
        /// <param name="c"></param>
        public static string RemoveSuffix(this string s ,string c) {
            var i = s.LastIndexOf(c);
            if (i > 0)
                return s.Substring(0, i);
            return s;
        }

        /// <summary>
        /// 移除指定结尾的前缀
        /// </summary>
        /// <param name="c"></param>
        public static string RemovePrefix(this string s, string c) {
            var i = s.IndexOf(c);
            if (i > 0)
                return s.Substring(i);
            return s;
        }

        public static string Completed(this int i) {
            var str = i.ToString();
            for (var j = str.Length; j < 4; j++) {
                str = string.Concat(0, str);
            }
            return str;
        }

        /// <summary>
        /// 搜索指定img列表中符合条件的img
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="Items"></param>
        /// <returns></returns>
        [Obsolete]
        public static IEnumerable<Album> Find2(IEnumerable<Album> Items, params string[] condition) {
            var isEmpty = true;
            foreach (var item in condition) {
                if (!item.Equals(string.Empty)) {
                    isEmpty = false;
                    break;
                }
            }
            if (isEmpty) {
                foreach (var item in Items)
                    yield return item;
                yield break;
            }
            var temp = new List<Album>();
            foreach (var album in Items) {
                var isMatch = true;
                foreach (var item in condition) {
                    if (item != string.Empty && !album.Path.Contains(item)) {
                        isMatch = false;
                        break;
                    }
                }
                if (isMatch)
                    yield return album;
            }
        }

        public static List<Album> Find(IEnumerable<Album> Items, params string[] args) => Find(Items, false, args);

        public static List<Album> Find(IEnumerable<Album> Items, bool allCheck, params string[] args) {
            var list = new List<Album>(Items.FindAll(item => {
                if (!allCheck && args.Length == 0)
                    return true;
                if (allCheck && !args[0].Equals(item.Name))
                    return false;
                return args.All(arg => item.Path.Contains(arg));
            }));
            if (list.Count == 0) {//当搜索结果为空时,启用V6规则搜索
                list.AddRange(Items.FindAll(item => Find(item.Name, args[0])));
            }
            return list;
        }

        public static bool Find(string name1, string name2) {
            var regex = new Regex("\\d+");
            var match0 = regex.Match(name1);
            var match1 = regex.Match(name2);
            if (match0.Success && match1.Success) {
                var code0 = int.Parse(match0.Value);
                var code1 = int.Parse(match1.Value);
                if (code0 == code1 || code0 == (code1 / 100 * 100)) {
                    return true;
                }
            }
            return false;
        }
        


        public static IEnumerable<T> FindAll<T>(this IEnumerable<T> IEnums,Predicate<T> match) {
            var list = new List<T>(IEnums);
            return list.FindAll(match);
        }



        /// <summary>
        /// 去除字符串中"/"和"\"前面的内容
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetName(this string str) => str.LastSubstring('\\', '/');

        /// <summary>
        /// 补全字符串,会去除相同的部分
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static string Complete(this string s1, string s2) {
            var cs1 = s1.ToCharArray();
            var cs2 = s2.ToCharArray();
            var r1 = string.Empty;
            var r2= string.Empty;
            for (int i = cs1.Length - 1, j = 0; i > 0 && j < cs2.Length; i--, j++) {
                r1 = cs1[i] + r1;
                r2 = r2 + cs2[j];
                if (r1.Equals(r2)) {
                    s2 = s2.Substring(j + 1);
                    break;
                }
            }
            return s1 + s2;
        }

        public static string LastSubstring(this string str, params char[] split) {
            var index = -1;
            foreach (char c in split) {
                var index2 = str.LastIndexOf(c);
                index = index > index2 || index2 == -1 ? index : index2;
            }
            return str.Substring(index + 1); ;
        }


        /// <summary>
        /// 读取一个int
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static int ReadInt(this Stream stream) {
            var buf = new byte[4];
            stream.Read(buf);
            return BitConverter.ToInt32(buf, 0);
        }

        public static short ReadShort(this Stream stream) {
            var buf = new byte[2];
            stream.Read(buf);
            return BitConverter.ToInt16(buf, 0);
        }


        /// <summary>
        /// 写入一个int
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="data"></param>
        public static void WriteInt(this Stream stream, int data) => stream.Write(BitConverter.GetBytes(data));


        /// <summary>
        /// 读取一个long
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static long ReadLong(this Stream stream) {
            var buf = new byte[8];
            stream.Read(buf);
            return BitConverter.ToInt64(buf, 0);
        }

        /// <summary>
        /// 写入一个long
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="l"></param>
        public static void WriteLong(this Stream stream, long l) {
            stream.Write(BitConverter.GetBytes(l));
        }

        /// <summary>
        /// 读取img路径
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ReadPath(this Stream stream) {
            var data = new byte[256];
            var i = 0;
            for (; i < 256; i++) {
                data[i] = (byte)(stream.ReadByte() ^ Key[i]);
                if (data[i] == 0) {
                    break;
                }
            }
            stream.Seek(255 - i);//防止因加密导致的文件名读取错误
            return Encoding.Default.GetString(data).Replace("\0", "");
        }

        /// <summary>
        /// 写入img路径
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="str"></param>
        public static void WritePath(this Stream stream, string str, byte[] keys) {
            var data = new byte[224];
            var temp = Encoding.Default.GetBytes(str);
            temp.CopyTo(data, 0);
            for (var i = 0; i < data.Length; i++) {
                data[i] = (byte)(data[i] ^ Key[i]);
            }
            stream.Write(data);
            if (keys != null) {
                keys = CompileCode(keys);
            } else {
                keys = new byte[32];
                for (var i = 0; i < 32; i++) {
                    keys[i] = (byte)(0 ^ Key[224 + i]);
                }
            }
            stream.Write(keys);             
        }

        /// <summary>
        /// 写入一个字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="str"></param>
        public static void WriteString(this Stream stream, string str) {
            stream.Write(Encoding.Default.GetBytes(str));
            stream.WriteByte(0);
        }

        public static void WriteShort(this Stream stream,short s) => stream.Write(BitConverter.GetBytes(s));
        

        /// <summary>
        /// 读出一个字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ReadString(this Stream stream) {
            var ms = new MemoryStream();
            var j = 0;
            while ((j = stream.ReadByte()) != 0 && stream.Position < stream.Length)
                ms.WriteByte((byte)j);
            ms.Close();
            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// 读取一个指定数量的色表
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<Color> ReadColorChart(this Stream stream, int count) {
            var Table = new List<Color>();
            for (var i = 0; i < count; i++) {
                var data = new byte[4];
                stream.Read(data);
                yield return Color.FromArgb(data[3], data[0], data[1], data[2]);
            }
        }

        public static List<T> ToList<T>(this IEnumerable<T> enums)=>new List<T>(enums);

        /// <summary>
        /// 安全插入 当插入的位置不在于集合的区间时，改为添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="t"></param>
        public static void InsertAt<T>(this List<T> list, int index, IEnumerable<T> t) {
            if (index > list.Count) {
                list.AddRange(t);
            } else if (index < 0) {
                list.InsertRange(0, t);
            } else {
                list.InsertRange(index, t);
            }
        }


        public static void WriteColorChart(this Stream stream,Color[] table) {
            for (var i = 0; i < table.Length; i++) {
                var color = table[i];
                var data = new byte[] { color.R, color.G, color.B, color.A };
                stream.Write(data);
            }
        }

        public static int Read(this Stream stream, byte[] buf) => stream.Read(buf, 0, buf.Length);

        public static void Write(this Stream stream, byte[] buf) => stream.Write(buf, 0, buf.Length);



        /// <summary>
        /// 将rgb数组转换为Bitmap
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap FromArray(byte[] data, Size size) {
            var bmp = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            var bmpData = bmp.LockBits(new Rectangle(Point.Empty, size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        /// <summary>
        /// 将Bitmap转换为rgb数组
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static byte[] ToArray(this Bitmap bmp) {
            var bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var data = new byte[bmp.Width * bmp.Height * 4];
            Marshal.Copy(bmpData.Scan0, data, 0, data.Length);
            bmp.UnlockBits(bmpData);
            return data;
        }

        public static Bitmap Star(this Bitmap bmp, decimal scale) {
            var size = bmp.Size.Star(scale);
            var image = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(image)) {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(bmp, new Rectangle(Point.Empty,size));
            }
            return image;
        }
        

        /// <summary>
        /// 将所有ARGB类型的数据转换为ARGB_8888的字节数组
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static byte[] ReadColor(this Stream stream, ColorBits type) {
            byte[] bs;
            if (type == ColorBits.ARGB_8888) {
                bs = new byte[4];
                stream.Read(bs);
                return bs;
            }
            byte a = 0;
            byte r = 0;
            byte g = 0;
            byte b = 0;
            bs = new byte[2];
            stream.Read(bs);
            switch (type) {
                case ColorBits.ARGB_1555:
                    a = (byte)(bs[1] >> 7);
                    r = (byte)((bs[1] >> 2) & 0x1f);
                    g = (byte)((bs[0] >> 5) | (bs[1] & 3) << 3);
                    b = (byte)(bs[0] & 0x1f);
                    a = (byte)(a * 0xff);
                    r = (byte)(r << 3 | r >> 2);
                    g = (byte)(g << 3 | g >> 2);
                    b = (byte)(b << 3 | b >> 2);
                    break;
                case ColorBits.ARGB_4444:
                    a = (byte)(bs[1] & 0xf0);
                    r = (byte)((bs[1] & 0xf) << 4);
                    g = (byte)(bs[0] & 0xf0);
                    b = (byte)((bs[0] & 0xf) << 4);
                    break;
                case ColorBits.UNKOWN:
                    break;
            }
            return new byte[] { b, g, r, a };
        }

        /// <summary>
        /// 将ARGB1555和ARGB4444转换为ARGB8888
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public static void WriteColor(this Stream stream, byte[] data, ColorBits type) {
            if (type == ColorBits.ARGB_8888) {
                stream.Write(data);
                return;
            }
            var a = data[3];
            var r = data[2];
            var g = data[1];
            var b = data[0];
            var left = 0;
            var right = 0;
            switch (type) {
                case ColorBits.ARGB_1555:
                    a = (byte)(a >> 7);
                    r = (byte)(r >> 3);
                    g = (byte)(g >> 3);
                    b = (byte)(b >> 3);
                    left = (byte)((g & 7) << 5 | b);
                    right = (byte)((a << 7) | (r << 2) | (g >> 3));
                    break;
                case ColorBits.ARGB_4444:
                    left = g | b >> 4;
                    right = a | r >> 4;
                    break;
            }
            stream.WriteByte((byte)left);
            stream.WriteByte((byte)right);
        }

        /// <summary>
        /// 写入一个颜色
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="color"></param>
        /// <param name="type"></param>
        public static void WriteColor(this Stream stream, Color color, ColorBits type) {
            var data = new byte[] { color.B, color.G, color.R, color.A };
            stream.WriteColor(data, type);
        }

        /// <summary>
        /// 读取一个贴图
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Bitmap ReadImage(this Stream stream, ImageEntity entity) {
            var data = new byte[entity.Width * entity.Height * 4];
            for (var i = 0; i < data.Length; i += 4) {
                var type = entity.Type;
                if (entity.Version == Img_Version.Ver4 && type == ColorBits.ARGB_1555)
                    type = ColorBits.ARGB_8888;
                var temp = stream.ReadColor(type);
                temp.CopyTo(data, i);
            }
            return FromArray(data, entity.Size);
        }



        /// <summary>
        /// 计算NPK的校验码
        /// </summary>
        /// <param name="count"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] CompileCode(byte[] source) {
            if (source.Length < 1)
                return new byte[0];
            var ms = new MemoryStream();
            var count = source.Length / 17 * 17;
            var data = new byte[count];
            Array.Copy(source, 0, data, 0, count);
            using (var sha = new SHA256Managed()) 
                data = sha.ComputeHash(data);      
            return data;
        }


        public static void WriteImage(this Stream stream, ImageEntity entity) {
            var type = entity.Type;
            if (entity.Type > ColorBits.ARGB_8888)//不支持dds的写入
                return;
            var data = entity.Picture.ToArray();
            if (entity.Version == Img_Version.Ver4 && entity.Compress == Compress.ZLIB && type == ColorBits.ARGB_1555)
                type = ColorBits.ARGB_8888;
            for (var i = 0; i < data.Length; i += 4) {
                var temp = new byte[4];
                Array.Copy(data, i, temp, 0, 4);
                stream.WriteColor(temp, type);
            }
        }

        /// <summary>
        /// 从NPK中获得img列表
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<Album> ReadNPK(this Stream stream, string file) {
            List<Album> List = new List<Album>();
            var flag = stream.ReadString();
            if (flag == NPK_FlAG) {//当文件是NPK时
                stream.Seek(0, SeekOrigin.Begin);
                List.AddRange(stream.ReadInfo());
                if (List.Count > 0)
                    stream.Seek(32);
            } else if (flag == IMG_FLAG || flag == IMAGE_FLAG || file.EndsWith(".ogg")) {
                var album = new Album();
                album.Path = file.GetName();
                List.Add(album);
            }
            for (var i = 0; i < List.Count; i++) {
                var album = List[i];
                stream.Seek(album.Offset, SeekOrigin.Begin);
                var album_flag = stream.ReadString();
                if (album_flag == IMG_FLAG) {
                    album.Info_Length = stream.ReadLong();
                    album.Version = (Img_Version)stream.ReadInt();
                    album.Count = stream.ReadInt();
                    album.InitHandle(stream);
                } else if (album_flag == IMAGE_FLAG) {
                    stream.Seek(10);
                    album.Version = Img_Version.Ver1;
                    album.Count = stream.ReadInt();
                    album.InitHandle(stream);
                } else {
                    stream.Seek(album.Offset, SeekOrigin.Begin);
                    if (album.Path.ToLower().EndsWith(".ogg")) {
                        album.Version = Img_Version.OGG;
                        if (i < List.Count - 1) {
                            album.Info_Length = List[i + 1].Offset - stream.Position;
                        } else {
                            album.Info_Length = stream.Length - stream.Position;
                        }
                        album.InitHandle(stream);
                    }
                }
            }
            return List;
        }

        public static string toCodeString(this int code) {
            var code_str = code + "";
            while (code_str.Length < 4) {
                code_str = "0" + code_str;
            }
            return code_str;
        }

        #region 图像处理

        /// <summary>
        /// 图片扫描,获得最小画布的矩形
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        [Obsolete]
        public static Rectangle Scan1(this Bitmap bmp) {
            var data = bmp.ToArray();
            var left = 0;//计算左侧空白
            var right = bmp.Width - 1;//计算右侧空白
            var up = 0;//计算上侧空白
            var down = bmp.Height - 1;//计算下侧空白
            for (; left < bmp.Width; left++) {
                var j = 0;
                for (var k = 0; k < bmp.Height && j == 0; k++)
                    for (var i = 0; i < 4 && j == 0; i++)
                        j = data[(k * bmp.Width + left) * 4 + i];
                if (j != 0)
                    break;
            }
            for (; right > 0; right--) {
                var j = 0;
                for (var k = 0; k < bmp.Height && j == 0; k++)
                    for (var i = 0; i < 4 && j == 0; i++)
                        j = data[(k * bmp.Width + right) * 4 + i];
                if (j != 0)
                    break;
            }
            for (; up < bmp.Height; up++) {
                var j = 0;
                for (var k = 0; k < bmp.Width && j == 0; k++)
                    for (var i = 0; i < 4 && j == 0; i++)
                        j = data[(up * bmp.Width + k) * 4 + i];
                if (j != 0)
                    break;
            }
            for (; down > 0; down--) {
                var j = 0;
                for (var k = 0; k < bmp.Width && j == 0; k++)
                    for (var i = 0; i < 4 && j == 0; i++)
                        j = data[(down * bmp.Width + k) * 4 + i];
                if (j != 0)
                    break;
            }
            var width = Math.Abs(right - left + 1);
            var height = Math.Abs(down - up + 1);
            return new Rectangle(left, up, width, height);
        }

        /// <summary>
        /// 图片扫描
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static Rectangle Scan(this Bitmap bmp) {
            var up = bmp.Height;
            var down = 0;
            var left = bmp.Width;
            var right = 0;
            var data = bmp.ToArray();
            for (var i = 0; i < bmp.Width; i++) {
                for (var j = 0; j < bmp.Height; j++)
                    for (var k = 0; k < 4; k++) {
                        var tp = data[(j * bmp.Width + i) * 4 + k];
                        if (tp != 0) {
                            up = up > j ? j : up;
                            left = left > i ? i : left;
                            down = down < j ? j : down;
                            right = right < i ? i : right;
                            break;
                        }
                    }
            }
            var width = Math.Abs(right - left + 1);
            var height = Math.Abs(down - up + 1);
            return new Rectangle(left, up, width, height);
        }


        [Obsolete]
        public static Bitmap Cavas1(this Bitmap bmp, Rectangle rect) {
            if (rect.Width > bmp.Width && rect.Height > bmp.Height) {
                var data = bmp.ToArray();
                var newData = new byte[rect.Width * rect.Height * 4];
                for (int i = 0; i < bmp.Height; i++) {
                    var pos = (rect.Width * (rect.Y + i) + rect.X) * 4;
                    var length = bmp.Width * 4;
                    Array.Copy(data, bmp.Width * i * 4, newData, pos, length);
                }
                bmp = FromArray(newData, rect.Size);
            }
            return bmp;
        }

        public static Bitmap Cavas(this Bitmap bmp, Rectangle rect) {
            var image = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(image)) 
                g.DrawImage(bmp, rect.X, rect.Y);
            return image;
        }



        public static Bitmap LinearDodge(this Bitmap bmp) {
            var data = bmp.ToArray();
            var _r = 0;
            var _g = 0;
            var _b = 0;
            var _a = 0;
            for(var i = 0; i < data.Length; i+=4) {
                var r = data[i];
                var g = data[i + 1];
                var b = data[i + 2];
                var a = data[i + 3];
                if (r + g + b + a < 300) {
                    a = (byte)(a >> 6);
                }
                data[i] = r;
                data[i + 1] = g;
                data[i + 2] = b;
                data[i + 3] = a;
            }
            bmp = FromArray(data, bmp.Size);
            return bmp;
        }

        /// <summary>
        /// 去除图片黑底
        /// </summary>
        /// <returns></returns>
        public static Bitmap RemoveBlack(this Bitmap bmp) {
            var ia = new ImageAttributes();
            var c = bmp.GetPixel(0, 0);
            ia.SetColorKey(Color.Empty, Color.Black);
            var matrice = new ColorMatrix();
            float[][] colorMatrixElements = {
   new float[] {1, 1,  1,  1, 1},        // red scaling factor of 2
   new float[] {1,  1,  1,  1, 1},        // green scaling factor of 1
   new float[] {1,  1,  1,  1, 1},        // blue scaling factor of 1
   new float[] {1,  1,  1,  1, 1},        // alpha scaling factor of 1
   new float[]{ 1,1,1,1,1 },
            };
            ia.SetColorMatrix(new ColorMatrix(colorMatrixElements));
            var rect = new Rectangle(Point.Empty, bmp.Size);
            var image = new Bitmap(bmp.Width, bmp.Height);
            using (var g = Graphics.FromImage(image))
                g.DrawImage(bmp, rect, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, ia);
            return image;
        }

        #endregion
        public static List<Album> ReadInfo(this Stream stream) {
            var flag = stream.ReadString();
            var List = new List<Album>();
            if (flag != NPK_FlAG)
                return List;
            var count = stream.ReadInt();
            for (var i = 0; i < count; i++) {
                var album = new Album();
                album.Offset = stream.ReadInt(); ;
                album.Length = stream.ReadInt();
                var path = stream.ReadPath();
                album.Path = path;
                List.Add(album);
            }
            return List;
        }

        #region 模板相关
        public static void ReplaceModel(Album Album, List<Layer> Layers) {
            foreach (var entity in Album.List) {
                for (int i = 0; i < Layers.Count; i++) {
                    if (entity.Index == Layers[i].Index) {
                        Layers[i].entity = entity;
                    }
                }
            }
        }

        public static List<Layer> ReadModel(string Path) {
            if (File.Exists(Path)) {
                var ms = new FileStream(Path, FileMode.Open);
                return ms.ReadModel();
            }
            return null;
        }


        /// <summary>
        /// 读入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static List<Layer> ReadModel(this Stream stream) {
            var List = new List<Layer>();
            var flag = stream.ReadString();
            if (flag != Model_FLAG)
                return List;
            var version = (Model_Version)stream.ReadInt();
            var count = stream.ReadInt();
            var Album = new Album();
            for (var i = 0; i < count; i++) {
                var layer = new Layer();
                var entity = new ImageEntity(Album);
                layer.Name = stream.ReadString();
                layer.Visible = stream.ReadInt() == 1;
                var Dev_X = stream.ReadInt();
                var Dev_Y = stream.ReadInt();
                entity.Index = stream.ReadInt();
                entity.Type = (ColorBits)stream.ReadInt();
                entity.Compress = (Compress)stream.ReadInt();
                entity.X = stream.ReadInt();
                entity.Y = stream.ReadInt();
                entity.Width = stream.ReadInt();
                entity.Height = stream.ReadInt();
                var length = stream.ReadInt();
                var data = new byte[length];
                stream.Read(data);
                entity.Data = data;
                layer.entity = entity;
                layer.AbsoluteLocation = new Point(Dev_X, Dev_Y);
                layer.Location = entity.Location.Add(layer.AbsoluteLocation);
                List.Add(layer);
            }
            stream.Close();
            return List;
        }

        internal static void SaveModel(string file, List<Layer> List) {
            var fs = new FileStream(file, FileMode.Create);
            fs.WriteString(Model_FLAG);
            fs.WriteInt((int)Model_Version.Ver1);
            fs.WriteInt(List.Count);
            var al = new Album();
            foreach (var layer in List) {
                var entity = new ImageEntity(al);
                entity.ReplaceImage(layer.entity.Type, false, layer.entity.Picture);
                entity.Location = layer.entity.Location;
                entity.Compress = layer.entity.Compress;
                entity.Adjust();
                fs.WriteString(layer.Name);
                fs.WriteInt(layer.Visible ? 1 : 0);
                fs.WriteInt(layer.AbsoluteLocation.X);
                fs.WriteInt(layer.AbsoluteLocation.Y);
                fs.WriteInt(layer.Index);
                fs.WriteInt((int)entity.Type);
                fs.WriteInt((int)entity.Compress);
                fs.WriteInt(entity.X);
                fs.WriteInt(entity.Y);
                fs.WriteInt(entity.Width);
                fs.WriteInt(entity.Height);
                fs.WriteInt(entity.Length);
                fs.Write(entity.Data);
            }
            fs.Close();
        }

        #endregion
        #region 加密
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal static Work Decrpt(this Stream stream) {
            var count_data = new byte[4];
            stream.Read(count_data);
            Array.Reverse(count_data);
            var count = BitConverter.ToInt32(count_data, 0);
            var bs = new byte[2];
            stream.Read(bs);
            if (bs.Check(new byte[] { 0x78, 0x9c })) {
                if (count > 0) {
                    var source = new byte[count];
                    stream.Read(source);
                    var aes = new AesCryptoServiceProvider();
                    aes.Mode = CipherMode.ECB;
                    aes.Key = Decrpt_Key;
                    aes.Padding = PaddingMode.Zeros;
                    aes.IV = new byte[16];
                    var diff = source.Length + aes.BlockSize - source.Length % aes.BlockSize;
                    var data = new byte[diff];
                    Array.Copy(source, data, source.Length);
                    data = aes.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
                    var ms = new MemoryStream(data);
                    var work = new Work();
                    work.Id = ms.ReadLong();
                    ms.Seek(4);
                    work.Version = Encrypt_Version.Ver2;
                    work.CanExtract = ms.ReadByte() == 1;
                    work.CanRead = ms.ReadByte() == 1;
                    work.Name = ms.ReadString();
                    work.Author = ms.ReadString();
                    work.Remark = ms.ReadString();
                    work.Update = DateTime.Parse(ms.ReadString());
                    work.Expire = DateTime.Parse(ms.ReadString());
                    var key = new byte[256];
                    ms.Read(key);
                    work.Key = key;
                    return work;
                } else {
                    var version = (Encrypt_Version)stream.ReadByte();
                    var size = ~stream.ReadInt();
                    count = 0 - count;
                    var source = new byte[count];                   
                    stream.Read(source);
                    return DecryptManager.Decrypt(version, source);
                }
            }
            return Work.CreateDefaultWork();
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="work"></param>
        internal static void Encrypt(this Stream stream, Work work) {
            if (work.Version == Encrypt_Version.Ver1) //强制升级
                work.Version = Encrypt_Version.Ver2;
            using (var ms = new MemoryStream()) {
                DecryptManager.Encrypt(ms, work);
                var data=ms.ToArray();
                var size = ~data.Length;
                var count = -data.Length;
                var count_data = BitConverter.GetBytes(count);
                Array.Reverse(count_data);
                stream.Write(count_data);
                stream.Write(new byte[] { 0x78, 0x9c });
                stream.WriteByte((byte)work.Version);
                stream.WriteInt(~data.Length);
                stream.Write(data);
            }
        }


        /// <summary>
        /// 密码加密
        /// </summary>
        /// 
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string pwd) {
            var source = Encoding.Unicode.GetBytes(pwd);
            var data = new byte[256];
            Array.Copy(source, data, source.Length);
            var aes = new AesCryptoServiceProvider();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.Zeros;
            aes.Key = Decrpt_Key;
            aes.IV = new byte[16];
            Array.Reverse(data);
            return aes.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
        }
        

        #endregion

        public static bool Check(this byte[] bs1, byte[] bs2) {
            var hex1 = bs1.ToHexString();
            var hex2 = bs2.ToHexString();
            return hex1.Equals(hex2);
        }



        public static T[] GetCheckItems<T>(this EaseListBox<T> list) => list.GetCheckItems<T>(true);


        public static T[] GetCheckItems<T>(this EaseListBox<T> list, bool selected) {
            var array = new T[list.CheckedItems.Count];
            list.CheckedItems.CopyTo(array, 0);
            if (selected && array.Length < 2) {
                var item = (T)list.SelectedItem;
                if (item != null)
                    array = new T[] { item };
            }
            return array;
        }

        public static void InsertRange(this CheckedListBox.ObjectCollection collection, int index, object[] array) {
            var i = 0;
            while (i < array.Length) {
                collection.Insert(index++, array[i++]);
            } 
        }

        public static void AddSeparator(this ToolStripItemCollection items) {
            items.Add(new ToolStripSeparator());
        }

        public static T Find<T>(this T[] array, Predicate<T> match) => Array.Find(array, match);


        public static string ToHexString(this byte[] data) {
            var sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sb.Append(data[i].ToString("x2"));
            return sb.ToString();
        }


        public static string GetString(this Size size) => size.Width + "," + size.Height;

        public static void Seek(this Stream stream, long offset) => stream.Seek(offset, SeekOrigin.Current);


        #region Point拓展        
        public static string GetString(this Point point) {
            return point.X + "," + point.Y;
        }

        public static Point Star(this Point Point, decimal step) {
            var x = (int)(Point.X * step);
            var y = (int)(Point.Y * step);
            return new Point(x, y);
        }

        public static Point Add(this Point p1, Point p2) {
            var x = p1.X + p2.X;
            var y = p1.Y + p2.Y;
            return new Point(x, y);
        }

        public static Point Divide(this Point Point, decimal step) {
            var x = (int)(Point.X / step);
            var y = (int)(Point.Y / step);
            return new Point(x, y);
        }

        public static Point Minus(this Point p1, Point p2) {
            var x = p1.X - p2.X;
            var y = p1.Y - p2.Y;
            return new Point(x, y);
        }

        public static Point Reverse(this Point point) {
            return new Point(-point.X, -point.Y);
        }

        #endregion
        public static Size Star(this Size Size, decimal step) {
            var width = (int)(Size.Width * step);
            var height = (int)(Size.Height * step);
            return new Size(width, height);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] Split(this string str, string pattern) => str.Split(pattern.ToArray(), StringSplitOptions.RemoveEmptyEntries);


        #region set拓展
        public static T[] ToArray<T>(this ISet<T> set) {
            var array = new T[set.Count];
            set.CopyTo(array, 0);
            return array;
        }
        #endregion

        /// <summary>
        /// 根据type创建一个新的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">实例类型</param>
        /// <param name="args">构造参数</param>
        /// <returns></returns>
        public static object CreateInstance(this Type type, params object[] args) => type.Assembly.CreateInstance(type.FullName, true, BindingFlags.Default, null, args, null, null);

        /// <summary>
        /// 将版本号转换为整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToIntVersion(this string str) {
            var st = str.Split('.');
            var rs = "";
            for (var i = st.Length - 1; i > -1; i--) {
                var temp = st[st.Length - 1 - i];
                str = temp.Length < 2 ? "0" + temp : temp;
                rs += str;
            }
            return int.Parse(rs);
        }

                /// <summary>
        /// 将版本号转换为整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToByteVersion(this string str) {
            var st = str.Split('.');
            var buf = new byte[4];
            for (var i =0;i<buf.Length&&i<st.Length;i++) 
                buf[i] = byte.Parse(st[i]);
            return buf;
        }

        public static string ToStringVersion(this int i) {
            var str = i.ToString();
            var rs = string.Empty;
            if (str.Length < 8)
                return string.Empty;
            for (int j = 0;  j < 4;  j+=2) 
                rs += int.Parse(str.Substring(j, j + 3));
            return rs;
        }

        public static void WriteImageEntity(this Stream stream,ImageEntity entity) {
            var data = entity.Picture.ToArray();
            if (entity.Compress != Compress.NONE) {
                data = FreeImage.Compress(data);
            }
            stream.WriteString(ENTITY_FLAG);
            stream.WriteInt((int)entity.Type);
            stream.WriteInt((int)entity.Compress);
            stream.WriteInt(entity.Width);
            stream.WriteInt(entity.Height);
            stream.WriteInt(data.Length);
            stream.WriteInt(entity.X);
            stream.WriteInt(entity.Y);
            stream.WriteInt(entity.Cavas_Width);
            stream.WriteInt(entity.Cavas_Height);
            stream.Write(data);
        }

        public static ImageEntity ReadImageEntity(this Stream stream,ImageEntity entity) {
            if (entity == null) {
                entity = new ImageEntity(null);
            }
            var flag=stream.ReadString();
            if (flag == ENTITY_FLAG) {
                entity.Type=(ColorBits)stream.ReadInt();
                entity.Compress = (Compress)stream.ReadInt();
                entity.Width = stream.ReadInt();
                entity.Height = stream.ReadInt();
                entity.Length = stream.ReadInt();
                entity.X = stream.ReadInt();
                entity.Y = stream.ReadInt();
                entity.Cavas_Width = stream.ReadInt();
                entity.Cavas_Height = stream.ReadInt();
                var data = new byte[entity.Length];
                stream.Read(data);
                if (entity.Compress != Compress.NONE) {
                    var length = entity.Width * entity.Height * 4;
                    data = FreeImage.Uncompress(data, length);
                }
                entity.Picture = FromArray(data, entity.Size);
            }
            return entity;
        }
    }
}
