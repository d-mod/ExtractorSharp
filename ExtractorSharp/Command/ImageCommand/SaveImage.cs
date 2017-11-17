using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    /// 保存贴图
    /// 不可撤销
    /// 可宏命令
    /// </summary>
    class SaveImage : ICommand,SingleAction{
        public int[] Indexes { set; get; }
        Album Album;
        string Path;
        /// <summary>
        /// 提取模式
        /// <para>0.单张贴图</para>
        /// <para>其他.多张贴图</para>
        /// </summary>
        int Mode;
        public void Do(params object[] args) {
            Album = args[0] as Album;
            Mode = (int)args[1];
            Indexes = args[2] as int[];
            Path = args[3] as string;
            Action(Album, Indexes);
        }

        public void Redo() {}

        public void Undo() {}

        public void Action(Album album,int[] indexes) {
            if (Mode == 0) {//当保存模式为单张贴图时
                album.List[indexes[0]].Picture.Save(Path);
                Messager.ShowOperate("SaveFile");
            } else {//是否加入文件的路径
                var suffix = Program.Config["SaveImageAllPath"].Boolean ? album.Path : album.Name;
                var dir = Path.Replace('\\','/');
                dir = dir.Complete("/"+suffix);
                if (File.Exists(dir))//当已存在同名的文件时,文件夹加上下划线后缀
                    dir += "_";
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                for (int i = 0; i < indexes.Length && i < album.List.Count; i++) {
                    if (indexes[i] > album.List.Count - 1 || indexes[i] < 0)
                        continue;
                    var entity = album.List[indexes[i]];
                    var path = $"{dir}/{indexes[i]}.png";//文件名格式:文件路径/贴图索引.png
                    entity.Picture.Save(path);//保存贴图
                }
            }
        }

        public bool CanUndo => false;

        public bool Changed => false;

        public override string ToString() => Language.Default["SaveImage"];

        public void RunScript(string arg) { }

    }
}
