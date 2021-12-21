using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {
    /// <summary>
    /// </summary>
    [ExportCommand("HideImage")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED,ListenterKeys.REFRESH_FILE)]
    internal class HideImage : InjectService, ISingleMacro, IRollback {

        [CommandParameter(IsDefault = true)]
        private Album file;

        private Sprite[] _array;

        [CommandParameter]
        public int[] Indices { set; get; }

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {
            this._array = new Sprite[this.Indices.Length];
            for(var i = 0; i < this.Indices.Length; i++) {
                //确保文件索引正确
                if(this.Indices[i] > this.file.List.Count || this.Indices[i] < 0) {
                    continue;
                }
                var sprite = this.file.List[this.Indices[i]];
                this._array[i] = sprite; //存下原文件对象
                this.file.List.Remove(sprite);
                var newSprite = new Sprite(this.file) {
                    Index = this.Indices[i]
                }; //插入一个新的空有贴图的文件对象
                this.file.List.Insert(this.Indices[i], newSprite);
            }
            this.Messager.Success(this.Language["<HideImage><Success>"]);
        }


        public void Undo() {
            for(var i = 0; i < this.Indices.Length; i++) {
                this.file.List.RemoveAt(this.Indices[i]);
                var sprite = this._array[i];
                sprite.Index = sprite.Index > this.file.List.Count ? this.file.List.Count - 1 : sprite.Index;
                this.file.List.Insert(sprite.Index, sprite);
            }
        }

        public void Action(Album album, int[] indexes) {
            foreach(var i in indexes) {
                if(i > -1 && i < album.List.Count) {
                    album.List[i] = new Sprite(album);
                }
            }
        }

    }
}