using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    /// </summary>
    internal class HideImage : ISingleAction {
        private Album _album;

        private Sprite[] _array;

        public int[] Indices { set; get; }

        public string Name => "HideImage";

        public void Do(params object[] args) {
            _album = args[0] as Album;
            Indices = args[1] as int[]; //需要修改的文件索引
            _array = new Sprite[Indices.Length];
            for (var i = 0; i < Indices.Length; i++) {
                //确保文件索引正确
                if (Indices[i] > _album.List.Count || Indices[i] < 0) continue;
                var entity = _album.List[Indices[i]];
                _array[i] = entity; //存下原文件对象
                _album.List.Remove(entity);
                var newEntity = new Sprite(_album); //插入一个新的空有贴图的文件对象
                newEntity.Index = Indices[i];
                _album.List.Insert(Indices[i], newEntity);
            }
        }

        public void Redo() {
            Do(_album, Indices);
        }


        public void Undo() {
            for (var i = 0; i < Indices.Length; i++) {
                _album.List.RemoveAt(Indices[i]);
                var entity = _array[i];
                entity.Index = entity.Index > _album.List.Count ? _album.List.Count - 1 : entity.Index;
                _album.List.Insert(entity.Index, entity);
            }
        }

        public void Action(Album album, int[] indexes) {
            foreach (var i in indexes) {
                if (i > -1 && i < album.List.Count) {
                    album.List[i] = new Sprite(album);
                }
            }
        }


        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => false;

        public override string ToString() {
            return Language.Default["HideImage"];
        }
    }
}