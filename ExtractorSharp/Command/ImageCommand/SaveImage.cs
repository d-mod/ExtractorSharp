using ExtractorSharp.Core.Lib;
using ExtractorSharp.Data;
using System;
using System.IO;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    /// 保存贴图
    /// 不可撤销
    /// 可宏命令
    /// </summary>
    class SaveImage : ISingleAction {
        public int[] Indices { set; get; }
        private Album Album;
        private string Path;
        private int Increment = 0;
        private string Prefix = string.Empty;
        private int Digit = 0;
        /// <summary>
        /// 提取模式
        /// <para>0.单张贴图</para>
        /// <para>其他.多张贴图</para>
        /// </summary>
        private int Mode;
        public void Do(params object[] args) {
            Album = args[0] as Album;
            Mode = (int)args[1];
            Indices = args[2] as int[];
            Path = args[3] as string;
            if (args.Length > 3) {
                Prefix = args[4] as string;
                Increment = (int)args[5];
                Digit = (int)args[6];
            }
            Action(Album, Indices);
        }

        public void Redo() {
            // Method intentionally left empty.
        }

        public void Undo() {
            // Method intentionally left empty.
        }

        public void Action(Album album, int[] indexes) {
            if (Mode == 0) {//当保存模式为单张贴图时
                album.List[indexes[0]].Picture.Save(Path);
            } else {//是否加入文件的路径
                var suffix = Program.Config["SaveImageAllPath"].Boolean ? album.Path : album.Name;
                var dir = Path.Replace('\\', '/');
                dir = dir.Complete("/" + suffix);
                if (File.Exists(dir)) {//当已存在同名的文件时,文件夹加上下划线后缀
                    dir += "_";
                }
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }
                var max = Math.Min(indexes.Length, album.List.Count);
                for (var i = 0; i < max; i++) {
                    if (indexes[i] < 0 || i > album.List.Count - 1) {
                        continue;
                    }
                    var entity = album.List[indexes[i]];
                    var name = (Increment==-1?Indices[i]:Increment+i).ToString();
                    while (name.Length < Digit) {
                        name = "0" + name;
                    }
                    var path = $"{dir}/{Prefix}{name}.png";//文件名格式:文件路径/贴图索引.png
                    entity.Picture.Save(path);//保存贴图
                }
            }
        }

        public bool CanUndo => false;

        public bool IsChanged => false;

        public bool IsFlush => false;

        public string Name => "SaveImage";

    }
}
