using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    ///     删除贴图
    /// </summary>
    internal class DeleteImage : ISingleAction, ICommandMessage {
        private Album _album;

        private Sprite[] _array;

        public int[] Indices { set; get; }

        public string Name => "DeleteImage";

        public void Do(params object[] args) {
            _album = args[0] as Album;
            Indices = args[1] as int[];
            _array = new Sprite[Indices.Length];
            for (var i = 0; i < Indices.Length; i++) {
                if (Indices[i] > _album.List.Count - 1 || Indices[i] < 0) {
                    continue;
                }
                _array[i] = _album.List[Indices[i]];
            }
            foreach (var entity in _array) {
                if (entity != null) {
                    var frist = _album.List.Find(item => item?.Target == entity);
                    if (frist != null) {
                        _album.List[frist.Index] = entity;
                    }
                    _album.List[entity.Index] = null;
                }
            }
            _album.List.RemoveAll(e => e == null);
            _album.AdjustIndex();
        }

        public void Redo() {
            Do(_album, Indices);
        }


        public void Undo() {
            for (var i = 0; i < Indices.Length; i++) {
                var entity = _array[i];
                if (Indices[i] < _album.List.Count) {
                    _album.List.Insert(Indices[i], entity);
                } else {
                    entity.Index = _album.List.Count;
                    _album.List.Add(entity);
                }
            }
            if (_array.Length > 0) {
                _album.AdjustIndex();
            }
        }

        public void Action(Album album, int[] indexes) {
            var array = new Sprite[indexes.Length];
            for (var i = 0; i < array.Length; i++) {
                if (indexes[i] < album.List.Count && indexes[i] > -1) {
                    array[i] = album.List[indexes[i]];
                }
            }
            foreach (var entity in array) {
                album.List.Remove(entity);
            }
            album.AdjustIndex(); //校正索引
        }


        public bool CanUndo => true;

        public bool IsChanged => true;
    }
}