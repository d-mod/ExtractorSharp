using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {


    /// <summary>
    /// 贴图染色
    /// </summary>
    [ExportCommand("DyeImage")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    internal class DyeImage : InjectService, IRollback, ISingleMacro {

        [CommandParameter]
        private Album file;

        [CommandParameter]
        private Color color;

        private Bitmap[] images;

        [CommandParameter]
        public int[] Indices { set; get; }

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {
            this.images = new Bitmap[this.Indices.Length];
            for(var i = 0; i < this.images.Length; i++) {
                var sprite = this.file.List[this.Indices[i]];
                this.images[i] = sprite.Image;
                sprite.Image = this.images[i].Dye(this.color);
            }
            this.Messager.Success(this.Language["<Dye><Success>"]);
        }

        public void Undo() {
            for(var i = 0; i < this.Indices.Length; i++) {
                this.file.List[this.Indices[i]].Image = this.images[i];
            }
        }

        public void Action(Album album, int[] indices) {
            foreach(var i in indices) {
                if(i > -1 && i < album.List.Count) {
                    album[i].Image = album[i].Image.Dye(this.color);
                }
            }
        }
    }
}