using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Commands {

    [ExportMetadata("Name", "ChangeSize")]
    [ExportMetadata("FileChange", true)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(ICommand))]
    internal class ChangeSize : InjectService, IRollback {

        [CommandParameter("File")]
        private Album _album;

        private Bitmap[] _array;

        [CommandParameter("Indices")]
        private int[] indices;

        [CommandParameter("Scale")]
        private decimal _scale;

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {
            this._array = new Bitmap[this.indices.Length];
            for(var i = 0; i < this._array.Length; i++) {
                var sprite = this._album[this.indices[i]];
                var image = sprite.Image;
                this._array[i] = image;
                sprite.Image = image.Star(this._scale);
                sprite.Location = sprite.Location.Divide(this._scale);
            }
            this.Messager.Success(this.Language["<ChangeImageSize><Success>!"]);
        }

        public void Undo() {
            for(var i = 0; i < this._array.Length; i++) {
                var sprite = this._album[this.indices[i]];
                sprite.Image = this._array[i];
                sprite.Location = sprite.Location.Star(this._scale);
            }
        }
    }
}