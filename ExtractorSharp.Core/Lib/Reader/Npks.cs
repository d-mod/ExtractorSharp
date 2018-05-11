using ExtractorSharp.Data;
using ExtractorSharp.Handle;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtractorSharp.Core.Lib {
    public static class Npks {
        public const string NPK_FlAG = "NeoplePack_Bill";
        public const string IMG_FLAG = "Neople Img File";
        public const string IMAGE_FLAG = "Neople Image File";

        public const string IMAGE_DIR = "ImagePacks2";

        public const string SOUND_DIR = "SoundPacks";

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

        /// <summary>
        /// 读取img路径
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static string ReadPath(this Stream stream) {
            var data = new byte[256];
            var i = 0;
            while (i < 256) {
                data[i] = (byte)(stream.ReadByte() ^ Key[i]);
                if (data[i] == 0) {
                    break;
                }
                i++;
            }
            stream.Seek(255 - i);//防止因加密导致的文件名读取错误
            return Encoding.Default.GetString(data).Replace("\0", "");
        }


        /// <summary>
        /// 写入img路径
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="str"></param>
        private static void WritePath(this Stream stream, string str) {
            var data = new byte[256];
            var temp = Encoding.Default.GetBytes(str);
            temp.CopyTo(data, 0);
            for (var i = 0; i < data.Length; i++) {
                data[i] = (byte)(data[i] ^ Key[i]);
            }
            stream.Write(data);
        }

        /// <summary>
        /// 读取一个贴图
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Bitmap ReadImage(Stream stream, Sprite entity) {
            var data = new byte[entity.Width * entity.Height * 4];
            for (var i = 0; i < data.Length; i += 4) {
                var bits = entity.Type;
                if (entity.Version == Img_Version.Ver4 && bits == ColorBits.ARGB_1555) {
                    bits = ColorBits.ARGB_8888;
                }
                var temp = Colors.ReadColor(stream,bits);
                temp.CopyTo(data, i);
            }
            return Bitmaps.FromArray(data, entity.Size);
        }






        /// <summary>
        /// 计算NPK的校验码
        /// </summary>
        /// <param name="count"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private static byte[] CompileHash(byte[] source) {
            if (source.Length < 1) {
                return new byte[0];
            }
            var count = source.Length / 17 * 17;
            var data = new byte[count];
            Array.Copy(source, 0, data, 0, count);
            using (var sha = new SHA256Managed()) {
                data = sha.ComputeHash(data);
            }
            return data;
        }


        private static List<Album> ReadInfo(Stream stream) {
            var flag = stream.ReadString();
            var List = new List<Album>();
            if (flag != NPK_FlAG) {
                return List;
            }
            var count = stream.ReadInt();
            for (var i = 0; i < count; i++) {
                List.Add(new Album {
                    Offset = stream.ReadInt(),
                    Length = stream.ReadInt(),
                    Path = stream.ReadPath()
                });
            }
            return List;
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
                List.AddRange(ReadInfo(stream));
                if (List.Count > 0) {
                    stream.Seek(32);
                }
            } else if (flag == IMG_FLAG || flag == IMAGE_FLAG || file.EndsWith(".ogg")) {
                var album = new Album();
                album.Path = file.GetSuffix();
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
                    album.Info_Length = stream.ReadInt();
                    stream.Seek(2);
                    album.Version = (Img_Version)stream.ReadInt();
                    album.Count = stream.ReadInt();
                    album.InitHandle(stream);
                } else {
                    stream.Seek(album.Offset, SeekOrigin.Begin);
                    var name = album.Name.ToLower();
                    if (name.EndsWith(".ogg")) {
                        album.Version = Img_Version.Other;
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

    


        /// <summary>
        /// 保存为NPK
        /// </summary>
        /// <param name="fileName"></param>
        public static void WriteNpk(Stream stream, List<Album> List) {
            var position = 52 + List.Count * 264;
            for (var i = 0; i < List.Count; i++) {
                List[i].Adjust();
                if (i > 0) {
                    position += List[i - 1].Length;
                }
                List[i].Offset = position;
            }
            var ms = new MemoryStream();
            ms.WriteString(NPK_FlAG);
            ms.WriteInt(List.Count);
            foreach (var album in List) {
                ms.WriteInt(album.Offset);
                ms.WriteInt(album.Length);
                ms.WritePath(album.Path);
            }
            ms.Close();
            var data = ms.ToArray();
            stream.Write(data);
            stream.Write(CompileHash(data));
            foreach (var album in List) {
                stream.Write(album.Data);
            }
        }

        #region 加载保存
        public static List<Album> Load(string file) => Load(false, file);

        public static List<Album> Load(bool onlyPath, string file) {
            var List = new List<Album>();
            if (Directory.Exists(file)) {
                return Load(onlyPath, Directory.GetFiles(file));
            }
            if (!File.Exists(file)) {
                return List;
            }
            using (var stream = File.OpenRead(file)) {
                if (onlyPath) {
                    return ReadInfo(stream);
                }
                List<Album> enums = stream.ReadNPK(file);
                return enums;
            }
        }

        public static List<Album> Load(bool onlyPath, params string[] files) {
            var List = new List<Album>();
            foreach (string file in files) {
                List.AddRange(Load(onlyPath, file));
            }
            return List;
        }

        public static List<Album> Load(params string[] files) => Load(false, files);


        public static Album LoadWithName(string file, string name) {
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

        public static void Save(string file, List<Album> list) {
            using (var fs = File.OpenWrite(file)) {
                WriteNpk(fs, list);
            }
        }
        public static void SaveToDirectory(string dir,IEnumerable<Album> array) {
            foreach(var img in array) {
                img.Save($"{dir}/{img.Name}");
            }
        }


        #endregion

        #region 比较
        public static void Compare(string gamePath, Action<Album, Album> restore, params Album[] Array) => Compare(gamePath, IMAGE_DIR, restore, Array);

        /// <summary>
        /// 与游戏原文件进行对比
        /// </summary>
        /// <param name="gamePath"></param>
        /// <param name="restore"></param>
        /// <param name="Array"></param>
        public static void Compare(string gamePath, string dir, Action<Album, Album> restore, params Album[] Array) {
            var dic = new Dictionary<string, List<string>>();//将img按NPK分类
            foreach (var item in Array) {
                var path = GetFilePath(item);
                path = $"{gamePath}/{dir}/{path}";//得到游戏原文件路径
                if (!dic.ContainsKey(path))
                    dic.Add(path, new List<string>());
                dic[path].Add(item.Name);
            }
            var list = new List<Album>();
            foreach (var item in dic.Keys) {
                list.AddRange(FindAll(item, dic[item].ToArray()));//读取游戏原文件

            }
            for (var i = 0; i < Array.Length; i++) { //模型文件
                foreach (var item2 in list) { //游戏原文件
                    if (Array[i].Path.Equals(item2.Path)) {
                        restore.Invoke(item2, Array[i]);
                    }
                }
            }
        }

        #endregion
        

        /// <summary>
        /// 根据已有的文件名获得img集合
        /// </summary>
        /// <param name="file"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public static List<Album> FindAll(string file, params string[] args) {
            var list = Load(file);
            return Find(list, args);
        }


        public static List<Album> Find(IEnumerable<Album> Items, params string[] args) => Find(Items, false, args);

        public static List<Album> Find(IEnumerable<Album> Items, bool allCheck, params string[] args) {

            var list = new List<Album>(Items.Where(item => {
                if (!allCheck && args.Length == 0) {
                    return true;
                }
                if (allCheck && !args[0].Equals(item.Name)) {
                    return false;
                }
                return args.All(arg => item.Path.Contains(arg));
            }));
            if (list.Count == 0) {//当搜索结果为空时,启用V6规则搜索
                list.AddRange(Items.Where(item => MatchCode(item.Name, args[0])));
            }
            return list;
        }

        /// <summary>
        /// v6 匹配规则
        /// </summary>
        /// <param name="name1"></param>
        /// <param name="name2"></param>
        /// <returns></returns>
        public static bool MatchCode(string name1, string name2) {
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

        public static string CompleteCode(int code) {
            var str = code.ToString();
            while (str.Length < 4) {
                str = string.Concat(0, str);
            }
            return str;
        }   

        /// <summary>
        /// 根据文件路径得到NPK名
        /// </summary>
        /// <param name="album"></param>
        /// <returns></returns>
        public static string GetFilePath(Album file) {
            var path = file.Path;
            var index = path.LastIndexOf("/");
            if (index > -1) {
                path = path.Substring(0, index);
            }
            path = path.Replace("/", "_");
            path += ".NPK";
            return path;
        }

        public static Album[] SplitFile(Album file) {
            var arr = new Album[Math.Max(1,file.Tables.Count)];
            var regex = new Regex("\\d+");
            var path = file.Name;
            var match = regex.Match(path);
            if (!match.Success) {
                return arr;
            }
            var prefix = path.Substring(0, match.Index);
            var suffix = path.Substring(match.Index + match.Length);
            var code = int.Parse(match.Value);
            file.Adjust();
            var data = file.Data;
            var ms = new MemoryStream(data);
            for (var i = 0; i < arr.Length; i++) {
                var name = prefix + CompleteCode(code + i) + suffix;
                arr[i] = ReadNPK(ms, file.Name)[0];
                arr[i].Path = file.Path.Replace(file.Name, name);
                arr[i].Tables.Clear();
                if (file.Tables.Count > 0) {
                    arr[i].Tables.Add(file.Tables[i]);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            ms.Close();
            return arr;
        }

        public static Bitmap Preview(Album[]Array, int index) {
            var bmp = new Bitmap(130, 180);
            var g = Graphics.FromImage(bmp);
            var x = 800;
            var y = 600;
            foreach (var al in Array) {
                if (index < al.List.Count) {
                    var source = al.List[index];
                    if (source.Type == ColorBits.LINK) {//如果为链接贴图。则引用指向贴图的属性
                        source = source.Target;
                    }
                    if (source.Compress == Compress.NONE && source.Width * source.Height == 1) {//将透明图层过滤
                        continue;
                    }
                    if (source.X < x) {//获得最左点坐标
                        x = source.X;
                    }
                    if (source.Y < y) {//获得最上点坐标
                        y = source.Y;
                    }
                }
            }
            for (var i = 0; i < Array.Length; i++) {
                var img = Array[i];
                if (index > img.List.Count - 1) {
                    continue;
                }
                var source = img[index];
                if (source.Type == ColorBits.LINK) {//如果为链接贴图。则引用指向贴图的属性
                    source = source.Target;
                }
                if (source.Compress == Compress.NONE && source.Width * source.Height == 1) {//将透明图层过滤
                    continue;
                }
                g.DrawImage(source.Picture, source.X - x, source.Y - y);
            }
            g.Dispose();
            return bmp;
        }

    }
}
