using ExtractorSharp.Data;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    /// 链接贴图
    /// 可撤销
    /// 可宏命令
    /// </summary>
    class LinkImage : ICommand,SingleAction{
        int Index;
        public int[] Indexes { set; get; }
        ColorBits[] types;
        Album Album;
        public void Do(params object[] args) {
            Album = args[0] as Album;
            Index = (int)args[1];
            Indexes = args[2] as int[];
            types = new ColorBits[Indexes.Length];
            for (var i = 0; i < Indexes.Length; i++) {
                if (Index > Album.List.Count - 1 || Index < 0 || Index == Indexes[i])
                    continue;
                var entity = Album.List[Indexes[i]];
                types[i] = entity.Type;
                entity.Type = ColorBits.LINK;
                entity.Target = Album.List[Index];
            }
        }

        public void Undo() {
            for (int i = 0; i < Indexes.Length; i++) {
                var entity = Album.List[Indexes[i]];
                entity.Type = types[i];
                entity.Target = null;
            }
        }

        public void Redo() => Do(Album, Index, Indexes);
        

        public void Action(Album Album,int[] indexes) {
            foreach (var i in indexes) {//确保链接指向的文件索引正确
                if (Index > Album.List.Count - 1 || Index < 0 || Index == i)
                    continue;           //确保链接文件索引正确
                if (i > Album.List.Count - 1 || i < 0)
                    continue;
                var entity = Album.List[i];     
                entity.Picture = null;          //删除贴图对象
                entity.Type = ColorBits.LINK;   //修改为链接类型
                entity.Target = Album.List[Index];//指向指定文件
            }
        }

        public void RunScript(string arg) { }

        public bool CanUndo => true;

        public bool Changed => true;

        public override string ToString() => Language.Default["LinkImage"];
    }
}
