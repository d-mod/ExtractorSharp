using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.View;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    /// 删除贴图
    /// </summary>
    class DeleteImage : ICommand,SingleAction {
        private Album Album;
        private ImageEntity[] Array;
        public int[] Indexes { set; get; }
        public string Name => "DeleteImage";
        public void Do( params object[] args) {
            Album = args[0] as Album;
            Indexes = args[1] as int[];
            Array = new ImageEntity[Indexes.Length];
            for (var i = 0; i < Indexes.Length; i++) {
                if (Indexes[i] > Album.List.Count - 1 || Indexes[i] < 0) {
                    continue;
                }
                Array[i] = Album.List[Indexes[i]];
            }
            foreach (var entity in Array)
                if (entity != null)
                    Album.List.Remove(entity);
            Album.AdjustIndex();
            Messager.ShowOperate("DeleteFile");
        }

        public void Redo() => Do( Album, Indexes);


        public void Undo() {
            for (var i = 0; i < Indexes.Length; i++) {
                var entity = Array[i];
                if (Indexes[i] < Album.List.Count) {
                    Album.List.Insert(Indexes[i], entity);
                } else {
                    entity.Index = Album.List.Count;
                    Album.List.Add(entity);
                }
            }
            if (Array.Length > 0) {
                Album.AdjustIndex();
            }
        }

        public void Action(Album Album, int[] indexes) {
            var array = new ImageEntity[indexes.Length];
            for (int i = 0; i < array.Length; i++) {
                if (indexes[i] < Album.List.Count && indexes[i] > -1) {
                    array[i] = Album.List[indexes[i]];
                }
            }
            foreach (var entity in array) {
                Album.List.Remove(entity);
            }
            Album.AdjustIndex();//校正索引
        }

        public void RunScript(string arg) { }

        public bool CanUndo => true;

        public bool Changed => true;

        public override string ToString() => Language.Default["DeleteImage"];
    }
}
