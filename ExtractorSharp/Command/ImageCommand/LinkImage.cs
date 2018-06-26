using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    ///     链接贴图
    ///     可撤销
    ///     可宏命令
    /// </summary>
    internal class LinkImage : ISingleAction {
        private Album Album;

        private int Index;

        private ColorBits[] Types;

        public int[] Indices { set; get; }

        public string Name => "LinkImage";

        public void Do(params object[] args) {
            Album = args[0] as Album;
            Index = (int) args[1];
            Indices = args[2] as int[];
            Types = new ColorBits[Indices.Length];
            for (var i = 0; i < Indices.Length; i++) {
                if (Index > Album.List.Count - 1 || Index < 0 || Index == Indices[i]) continue;
                var entity = Album.List[Indices[i]];
                Types[i] = entity.Type;
                entity.Type = ColorBits.Link;
                entity.Target = Album.List[Index];
            }
        }

        public void Undo() {
            for (var i = 0; i < Indices.Length; i++) {
                var entity = Album.List[Indices[i]];
                entity.Type = Types[i];
                entity.Target = null;
            }
        }

        public void Redo() {
            Do(Album, Index, Indices);
        }


        public void Action(Album album, int[] indexes) {
            foreach (var i in indexes) {
                //确保链接指向的文件索引正确
                if (Index > album.List.Count - 1 || Index < 0 || Index == i) {
                    continue; //确保链接文件索引正确
                }
                if (i > album.List.Count - 1 || i < 0) {
                    continue;
                }
                var entity = album.List[i];
                entity.Picture = null; //删除贴图对象
                entity.Type = ColorBits.Link; //修改为链接类型
                entity.Target = album.List[Index]; //指向指定文件
            }
        }


        public bool CanUndo => true;

        public bool IsChanged => true;
    }
}