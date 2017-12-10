using ExtractorSharp.Data;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    /// 
    /// </summary>
    class HideImage : SingleAction {
        public int[] Indexes { set; get; }
        private Album Album;
        private ImageEntity[] Array;

        public string Name => "HideImage";
        public void Do(params object[] args) {
            Album = args[0] as Album;
            Indexes = args[1] as int[];//需要修改的文件索引
            Array = new ImageEntity[Indexes.Length];
            for (var i = 0; i < Indexes.Length; i++) {//确保文件索引正确
                if (Indexes[i] > Album.List.Count || Indexes[i] < 0) {
                    continue;
                }
                var entity = Album.List[Indexes[i]];
                Array[i] = entity;//存下原文件对象
                Album.List.Remove(entity);
                var newEntity = new ImageEntity(Album);//插入一个新的空有贴图的文件对象
                newEntity.Index = Indexes[i];
                Album.List.Insert(Indexes[i], newEntity);
            }
        }

        public void Redo() => Do(Album, Indexes);
        

        public void Undo() {
            for (var i = 0; i < Indexes.Length; i++) {
                Album.List.RemoveAt(Indexes[i]);
                var entity = Array[i];
                entity.Index = entity.Index > Album.List.Count ? Album.List.Count - 1 : entity.Index;
                Album.List.Insert(entity.Index, entity);
            }
        }

        public void Action(Album Album, int[] indexes) {
            foreach (var i in indexes) {
                if (i > -1 && i < Album.List.Count) {
                    Album.List[i] = new ImageEntity(Album);
                }
            }
        }
        

        public bool CanUndo => true;

        public bool Changed => true;

        public override string ToString() => Language.Default["HideImage"];
        
    }
}
