using System;
using System.IO;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    ///     保存贴图
    ///     不可撤销
    ///     可宏命令
    /// </summary>
    internal class SaveImage : ISingleAction, ICommandMessage {
        private Album Album { set; get; }
        private int Digit { set; get; }
        private bool FullPath { set; get; }
        private int Increment { set; get; }

        /// <summary>
        ///     提取模式
        ///     <para>0.单张贴图</para>
        ///     <para>其他.多张贴图</para>
        /// </summary>
        private int Mode { set; get; }

        private SpriteEffect OnSaving { set; get; }

        private string Path { set; get; }

        private string Prefix { set; get; } = string.Empty;

        public int[] Indices { set; get; }

        public void Do(params object[] args) {
            Album = args[0] as Album;
            Mode = (int) args[1];
            Indices = args[2] as int[];
            Path = args[3] as string;
            if (args.Length > 4) {
                Prefix = (args[4] as string).Replace("\\", "/");
                Increment = (int) args[5];
                Digit = (int) args[6];
                FullPath = (bool) args[7];
                OnSaving = args[8] as SpriteEffect;
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
            if (Mode == 0) {
                //当保存模式为单张贴图时
                album.List[indexes[0]].Picture.Save(Path);
            } else {
                //是否加入文件的路径
                var dir = $"{Path}/{(FullPath ? album.Path : album.Name)}/{Prefix}";
                dir = dir.Replace('\\', '/');
                var index = dir.LastIndexOf("/");
                dir = dir.Substring(0, index + 1);
                var prefix = dir.Substring(index);

                if (File.Exists(dir)) dir += "_";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                var max = Math.Min(indexes.Length, album.List.Count);
                for (var i = 0; i < max; i++) {
                    if (indexes[i] < 0 || i > Indices.Length - 1 || i > album.List.Count - 1) continue;
                    var entity = album.List[indexes[i]];
                    var name = (Increment == -1 ? Indices[i] : Increment + i).ToString();
                    while (name.Length < Digit) name = "0" + name;
                    var path = $"{dir}{prefix}{name}.png"; //文件名格式:文件路径/贴图索引.png
                    var image = entity.Picture;
                    ;
                    if (OnSaving != null) {
                        foreach (SpriteEffect action in OnSaving.GetInvocationList()) {
                            action.Invoke(entity, ref image);
                            image = image ?? entity.Picture;
                        }
                    }

                    var parent = System.IO.Path.GetDirectoryName(path);
                    if (Directory.Exists(parent)) { }

                    image.Save(path); //保存贴图
                }
            }
        }

        public bool CanUndo => false;

        public bool IsChanged => false;

        public string Name => "SaveImage";
    }
}