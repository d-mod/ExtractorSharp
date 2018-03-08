using ExtractorSharp.Data;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    /// 
    /// </summary>
    class HideImage : ISingleAction {

        public int[] Indices { set; get; }

        private Album Album;

        private Sprite[] Array;

        public string Name => "HideImage";
        public void Do(params object[] args) {
            Album = args[0] as Album;
            Indices = args[1] as int[];//需要修改的文件索引
            Array = new Sprite[Indices.Length];
            for (var i = 0; i < Indices.Length; i++) {//确保文件索引正确
                if (Indices[i] > Album.List.Count || Indices[i] < 0) {
                    continue;
                }
                var entity = Album.List[Indices[i]];
                Array[i] = entity;//存下原文件对象
                Album.List.Remove(entity);
                var newEntity = new Sprite(Album);//插入一个新的空有贴图的文件对象
                newEntity.Index = Indices[i];
                Album.List.Insert(Indices[i], newEntity);
            }
        }

        public void Redo() => Do(Album, Indices);
        

        public void Undo() {
            for (var i = 0; i < Indices.Length; i++) {
                Album.List.RemoveAt(Indices[i]);
                var entity = Array[i];
                entity.Index = entity.Index > Album.List.Count ? Album.List.Count - 1 : entity.Index;
                Album.List.Insert(entity.Index, entity);
            }
        }

        public void Action(Album Album, int[] indexes) {
            foreach (var i in indexes) {
                if (i > -1 && i < Album.List.Count) {
                    Album.List[i] = new Sprite(Album);
                }
            }
        }
        

        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => false;

        public override string ToString() => Language.Default["HideImage"];
        
    }
}
