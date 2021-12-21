using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Commands {

    [ExportMetadata("Name", "Lineardodge")]
    [ExportMetadata("FileChange", true)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(ICommand))]
    internal class Lineardodge : InjectService, ISingleMacro {

        [CommandParameter]
        private Album file;

        [CommandParameter]
        public int[] Indices { set; get; }

        private Bitmap[] images;

        public void Action(Album album, int[] indices) {
            foreach(var i in indices) {
                if(i > -1 && i < album.List.Count) {
                    album[i].Image = album[i].Image.LinearDodge();
                }
            }
        }

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {
            this.images = new Bitmap[this.Indices.Length];
            for(var i = 0; i < this.Indices.Length; i++) {
                var sprite = this.file.List[this.Indices[i]];
                this.images[i] = sprite.Image;
                sprite.Image = this.images[i].LinearDodge();
            }
            this.Messager.Success(this.Language["<LinearDodge><Success>"]);
        }

        public void Undo() {
            for(var i = 0; i < this.Indices.Length; i++) {
                this.file.List[this.Indices[i]].Image = this.images[i];
            }
        }
    }
}