using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ExtractorSharp.Core.Handle;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Exceptions;

namespace ExtractorSharp.Core.Coder {
    public static class NpkCoder {
        public const string NPK_FlAG = "NeoplePack_Bill";
        public const string IMG_FLAG = "Neople Img File";
        public const string IMAGE_FLAG = "Neople Image File";

        public const string IMAGE_DIR = "ImagePacks2";

        public const string SOUND_DIR = "SoundPacks";

        private const string KEY_HEADER="puchikon@neople dungeon and fighter ";

        public static Encoding Encoding = Encoding.UTF8;

        private static byte[] key;

        private static byte[] Key {
            get {
                if (key != null) {
                    return key;
                }
                var cs = new byte[256];
                int length = Encoding.GetBytes(KEY_HEADER, 0, KEY_HEADER.Length, cs, 0);
                var ds = Encoding.GetBytes("DNF");
                for (var i = length; i < 255; i++) {
                    cs[i] = ds[i % 3];
                }
                cs[255] = 0;
                return key = cs;
            }
        }

        /// <summary>
        ///     读取img路径
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
            stream.Seek(255 - i); //防止因加密导致的文件名读取错误
            return Encoding.GetString(data, 0, i);
        }


        /// <summary>
        ///     写入img路径
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="str"></param>
        private static void WritePath(this Stream stream, string str) {
            var data = new byte[256];
            Encoding.GetBytes(str, 0, str.Length, data, 0);
            for (var i = 0; i < data.Length; i++) {
                data[i] = (byte)(data[i] ^ Key[i]);
            }
            stream.Write(data);
        }

        /// <summary>
        ///     读取一个贴图
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Bitmap ReadImage(Stream stream, Sprite entity) {
            var data = new byte[entity.Width * entity.Height * 4];
            for (var i = 0; i < data.Length; i += 4) {
                var bits = entity.Type;
                if (entity.Version == ImgVersion.Ver4 && bits == ColorBits.ARGB_1555) {
                    bits = ColorBits.ARGB_8888;
                }
                Colors.ReadColor(stream, bits, data, i);
            }
            return Bitmaps.FromArray(data, entity.Size);
        }


        /// <summary>
        ///     计算NPK的校验码
        /// </summary>
        /// <param name="count"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private static byte[] CompileHash(byte[] data) {
            if (data.Length == 0) {
                return new byte[0];
            }
            try {
                using (var sha = new SHA256Managed()) {
                    return sha.ComputeHash(data, 0, data.Length / 17 * 17);
                }
            } catch {
                throw new FipsException();
            }
        }


        public static List<Album> ReadInfo(Stream stream) {
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
        ///     读取IMG或NPK
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="file">当格式非NPK时，需要得到文件名</param>
        /// <returns></returns>
        public static List<Album> ReadNpk(Stream stream, string file) {
            var List = new List<Album>();
            var flag = stream.ReadString();
            if (flag == NPK_FlAG) {
                //当文件是NPK时
                stream.Seek(0, SeekOrigin.Begin);
                List.AddRange(ReadInfo(stream));
                if (List.Count > 0) {
                    stream.Seek(32);
                }
            } else {
                var album = new Album();
                if (file != null) {
                    album.Path = file.GetSuffix();
                }
                List.Add(album);
            }
            for (var i = 0; i < List.Count; i++) {
                var length = i < List.Count - 1 ? List[i + 1].Offset : stream.Length;
                ReadImg(stream,List[i], length);
            }
            return List;
        }

        public static List<Album> ReadNpk(Stream stream) {
            return ReadNpk(stream, null);
        }

        public static void ReadImg(Stream stream, Album album, long length) {
            stream.Seek(album.Offset, SeekOrigin.Begin);
            var albumFlag = stream.ReadString();
            if (albumFlag == IMG_FLAG) {
                album.IndexLength = stream.ReadLong();
                album.Version = (ImgVersion)stream.ReadInt();
                album.Count = stream.ReadInt();
                album.InitHandle(stream);
            } else  {
                if (albumFlag == IMAGE_FLAG) {
                    album.Version = ImgVersion.Ver1;
                } else {
                    if (length < 0) {
                        length = stream.Length;
                    }
                    album.Version = ImgVersion.Other;
                    stream.Seek(album.Offset, SeekOrigin.Begin);
                    if (album.Name.ToLower().EndsWith(".ogg")) {
                        album.Version = ImgVersion.Other;
                        album.IndexLength = length - stream.Position;
                    }
                }
                album.InitHandle(stream);
            }
        }



        #region 读取一个IMG
        public static void ReadImg(Stream stream,Album album) {
            ReadImg(stream, album, -1);
        }

        public static Album ReadImg(Stream stream, string path) {
            return ReadImg(stream, path, -1);
        }


        public static Album ReadImg(Stream stream, string path, long length) {
            var album = new Album() {
                Path=path
            };
            ReadImg(stream, album, length);
            return album;
        }


        public static void ReadImg(byte[] data, Album album) {
            ReadImg(data, album, -1);
        }


        public static Album ReadImg(byte[] data, string path) {
            return ReadImg(data, path, -1);
        }

        public static void ReadImg(byte[] data, Album album, long length) {
            using (var ms = new MemoryStream(data)) {
                ReadImg(ms, album, length);
            }
        }

        public static Album ReadImg(byte[] data, string path, long length) {
            using (var ms = new MemoryStream(data)) {
               return ReadImg(ms, path, length);
            }
        }

        #endregion


        /// <summary>
        ///     保存为NPK
        /// </summary>
        /// <param name="fileName"></param>
        public static void WriteNpk(Stream stream, List<Album> List) {
            var position = 52 + List.Count * 264;
            var length = 0;
            for (var i = 0; i < List.Count; i++) {
                List[i].Adjust();
                if (i > 0) {
                    if (List[i].Target != null) {
                        continue;
                    }
                    position += length;
                }
                List[i].Offset = position;
                length = List[i].Length;
            }
            List.ForEach(e => {
                if (e.Target != null) {
                    e.Offset = e.Target.Offset;
                    e.Length = e.Target.Length;
                }
            });
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
                if (album.Target == null) {
                    stream.Write(album.Data);
                }
            }
        }

        public static List<Album> Find(IEnumerable<Album> Items, params string[] args) {
            return Find(Items, false, args);
        }

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
            return list;
        }
 

        /// <summary>
        ///     根据文件路径得到NPK名
        /// </summary>
        /// <param name="album"></param>
        /// <returns></returns>
        public static string GetFilePath(this Album file) {
            var path = file.Path;
            var index = path.LastIndexOf("/");
            if (index > -1) {
                path = path.Substring(0, index);
            }
            path = path.Replace("/", "_");
            path += ".NPK";
            return path;
        }



        #region 加载保存

        public static List<Album> Load(string file) {
            return Load(false, file);
        }

        public static List<Album> Load(bool onlyPath, string file) {
            var list = new List<Album>();
            if (Directory.Exists(file)) {
                return Load(onlyPath, Directory.GetFiles(file));
            }
            if (!File.Exists(file)) {
                return list;
            }
            using (var stream = File.OpenRead(file)) {
                if (onlyPath) {
                    return ReadInfo(stream);
                }
                var enums = ReadNpk(stream, file);
                return enums;
            }
        }

        public static List<Album> Load(bool onlyPath, params string[] files) {
            var List = new List<Album>();
            foreach (var file in files) {
                List.AddRange(Load(onlyPath, file));
            }
            return List;
        }

        public static List<Album> Load(params string[] files) {
            return Load(false, files);
        }


        public static Album LoadWithPath(string file, string name) {
            using (var stream = File.OpenRead(file)) {
               return LoadWithPath(stream, name);
            }
        }

        public static Album LoadWithPath(Stream stream, string name) {
            var list = LoadWithPathArray(stream, name);
            if (list.Count > 0) {
                return list[0];
            }
            return null;
        }

        /// <summary>
        ///     根据已有的文件名获得img集合
        /// </summary>
        /// <param name="file"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public static List<Album> LoadWithPathArray(string file, params string[] args) {
            using (var fs = File.OpenRead(file)) {
                return LoadWithPathArray(fs, args);
            }
        }

        public static List<Album> LoadWithPathArray(Stream stream, params string[] paths) {
            return LoadAll(stream, e => paths.Contains(e.Path));
        }

        public static List<Album> LoadAll(string file, Predicate<Album> predicate) {
            using (var fs = File.OpenRead(file)) {
                return LoadAll(fs, predicate);
            }
        }

        public static List<Album> LoadAll(Stream stream, Predicate<Album> predicate) {
            var list = ReadInfo(stream);
            list = list.FindAll(predicate);
            foreach (var al in list) {
                stream.Seek(al.Offset, SeekOrigin.Begin);
                ReadImg(stream, al, stream.Length);
            }
            return list;
        }


        public static void Save(string file, List<Album> list) {
            using (var fs = File.Open(file, FileMode.Create)) {
                WriteNpk(fs, list);
            }
        }

        public static void SaveToDirectory(string dir, IEnumerable<Album> array) {
            foreach (var img in array) {
                img.Save($"{dir}/{img.Name}");
            }
        }

        #endregion

        #region 比较

        public static void Compare(string gamePath, Action<Album, Album> restore, params Album[] array) {
            Compare(gamePath, IMAGE_DIR, restore, array);
        }

        /// <summary>
        ///     与游戏原文件进行对比
        /// </summary>
        /// <param name="gamePath"></param>
        /// <param name="restore"></param>
        /// <param name="array"></param>
        public static void Compare(string gamePath, string dir, Action<Album, Album> restore, params Album[] array) {
            var dic = new Dictionary<string, List<string>>(); //将img按NPK分类
            foreach (var item in array) {
                var path = GetFilePath(item);
                path = $"{gamePath}/{dir}/{path}"; //得到游戏原文件路径
                if (!dic.ContainsKey(path)) {
                    dic.Add(path, new List<string>());
                }
                dic[path].Add(item.Path);
            }
            var list = new List<Album>();
            foreach (var item in dic.Keys) {
                list.AddRange(LoadWithPathArray(item, dic[item].ToArray())); //读取游戏原文件
            }
            foreach(var a2 in array) { //模型文件
                foreach (var a1 in list) { //游戏原文件
                    if (a2.Path.Equals(a1.Path)) {
                        restore.Invoke(a1, a2);
                    }
                }
            }
        }

        #endregion
    }
}