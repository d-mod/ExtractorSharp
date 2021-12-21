using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {
    /// <summary>
    ///     裁剪贴图的透明部分
    /// </summary>
    [ExportCommand("TrimImage")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    internal class TrimImage : InjectService, IRollback, ISingleMacro {

        [CommandParameter("File", IsDefault = true)]
        private Album album;

        private Bitmap[] images;

        private Point[] locations;

        [CommandParameter]
        public int[] Indices { set; get; }

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {
            if(this.Indices == null) {
                this.Indices = new int[this.album.List.Count];
                for(var i = 0; i < this.Indices.Length; i++) {
                    this.Indices[i] = i;
                }
            }
            this.images = new Bitmap[this.Indices.Length];
            this.locations = new Point[this.Indices.Length];
            for(var i = 0; i < this.Indices.Length; i++) {
                if(this.Indices[i] > this.album.List.Count - 1 || this.Indices[i] < 0) {
                    continue;
                }
                var entity = this.album.List[this.Indices[i]];
                this.images[i] = entity.Image;
                this.locations[i] = entity.Location;
                entity.TrimImage();
            }
            this.Messager.Success(this.Language["<TrimImage><Success>"]);
        }


        public void Undo() {
            for(var i = 0; i < this.Indices.Length && i < this.images.Length; i++) {
                var entity = this.album.List[this.Indices[i]];
                entity.ReplaceImage(entity.ColorFormat, this.images[i]);
                entity.Location = this.locations[i];
            }
        }

        public void Action(Album album, int[] indexes) {
            foreach(var i in indexes) {
                if(i < album.List.Count && i > -1) {
                    album.List[i].TrimImage();
                }
            }
        }

    }
}