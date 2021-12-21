using System.ComponentModel.Composition;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {
    /// <summary>
    ///     删除贴图
    /// </summary>
    [ExportCommand("DeleteImage")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED,ListenterKeys.REFRESH_FILE)]
    internal class DeleteImage : InjectService, IRollback, ISingleMacro {

        [CommandParameter("File", IsDefault =true)]
        private Album _album;

        private Sprite[] _array;

        [CommandParameter("Indices")]
        public int[] Indices { set; get; }

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {
            this._array = new Sprite[this.Indices.Length];
            for(var i = 0; i < this.Indices.Length; i++) {
                if(this.Indices[i] > this._album.List.Count - 1 || this.Indices[i] < 0) {
                    continue;
                }
                this._array[i] = this._album.List[this.Indices[i]];
            }
            foreach(var sprite in this._array) {
                if(sprite != null) {
                    var frist = this._album.List.Find(item => item?.Target == sprite);
                    if(frist != null) {
                        this._album.List[frist.Index] = sprite;
                    }
                    this._album.List[sprite.Index] = null;
                }
            }
            this._album.List.RemoveAll(e => e == null);
            this._album.AdjustIndex();
            this.Messager.Success(this.Language["<DeleteImage><Success>"]);
        }


        public void Undo() {
            for(var i = 0; i < this.Indices.Length; i++) {
                var entity = this._array[i];
                if(this.Indices[i] < this._album.List.Count) {
                    this._album.List.Insert(this.Indices[i], entity);
                } else {
                    entity.Index = this._album.List.Count;
                    this._album.List.Add(entity);
                }
            }
            if(this._array.Length > 0) {
                this._album.AdjustIndex();
            }
        }

        public void Action(Album album, int[] indexes) {
            var array = new Sprite[indexes.Length];
            for(var i = 0; i < array.Length; i++) {
                if(indexes[i] < album.List.Count && indexes[i] > -1) {
                    array[i] = album.List[indexes[i]];
                }
            }
            foreach(var entity in array) {
                album.List.Remove(entity);
            }
            album.AdjustIndex(); //校正索引
        }
    }
}