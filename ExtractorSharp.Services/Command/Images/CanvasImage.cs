using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {
    /// <inheritdoc cref="" />
    /// <summary>
    ///     画布化
    ///     可撤销
    ///     可宏命令
    /// </summary>
    /// 
    [ExportCommand("CanvasImage")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    internal class CanvasImage : InjectService, IRollback, ISingleMacro {

        [CommandParameter("File")]
        private Album _album;

        [CommandParameter("Size")]
        private Size _size;

        [CommandParameter("Indices")]
        public int[] Indices { set; get; }

        private Bitmap[] _images;

        private Point[] _locations;

        public void Do(CommandContext context) {
            context.Export(this);

            this._images = new Bitmap[0];
            this._locations = new Point[0];
            if(this.Indices == null) {
                this.Indices = new int[this._album.List.Count];
                for(var i = 0; i < this.Indices.Length; i++) {
                    this.Indices[i] = i;
                }
            }
            this._images = new Bitmap[this.Indices.Length];
            this._locations = new Point[this.Indices.Length];
            this.Redo();
        }

        public void Redo() {
            for(var i = 0; i < this.Indices.Length; i++) {
                if(this._album == null || this.Indices[i] > this._album.List.Count - 1 || this.Indices[i] < 0) {
                    continue;
                }
                var sprite = this._album.List[this.Indices[i]];
                this._images[i] = sprite.Image;
                this._locations[i] = sprite.Location;
                sprite.CanvasImage(new Rectangle(Point.Empty, this._size));
            }
            this.Messager.Success(this.Language["<CustomFrameSize><Success>!"]);
        }


        public void Undo() {
            for(var i = 0; i < this.Indices.Length && i < this._images.Length; i++) {
                var entity = this._album.List[this.Indices[i]];
                entity.ReplaceImage(entity.ColorFormat, this._images[i]);
                entity.Location = this._locations[i];
            }
        }

        public void Action(Album album, int[] indexes) {
            foreach(var i in indexes) {
                if(i < album.List.Count && i > -1) {
                    album.List[i].CanvasImage(new Rectangle(Point.Empty, this._size));
                }
            }
        }

    }
}