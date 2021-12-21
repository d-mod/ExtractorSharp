using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;
using Point = System.Drawing.Point;

namespace ExtractorSharp.Services.Commands {

    /// <summary>
    ///  粘贴单张图片
    /// </summary>
    [ExportCommand("PasteSingleImage")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED)]
    public class PasteSingleImage : InjectService, IRollback {

        private ColorFormats bits;

        [CommandParameter]
        private Sprite target;

        [CommandParameter]
        private Point location;

        private Point oldLocation;

        private Bitmap image;

        public void Do(CommandContext context) {
            context.Export(this);
            this.Redo();
        }

        public void Redo() {
            Bitmap image = null;
            /*            if(clipboard != null) {
                            var source = clipboard.Album;
                            var indexes = clipboard.Indexes;
                            if(indexes.Length > 0 && indexes[0] < source.List.Count) {
                                image = source[indexes[0]].Image;
                            }
                        } else if(Clipboard.ContainsFileDropList()) {
                            var collection = Clipboard.GetFileDropList();
                            if(collection.Count > 0 && File.Exists(collection[0])) {
                                image = Image.FromFile(collection[0]) as Bitmap;
                            }
                        }*/
            
            this.oldLocation = this.target.Location;
            this.image = this.target.Image;
            this.bits = this.target.ColorFormat;
            this.target.ReplaceImage(this.bits, image);
            this.target.Location = this.location;
            this.Messager.Success(this.Language["<PasteImage><Success>"]);
        }

        public void Undo() {
            this.target.ReplaceImage(this.bits, this.image);
            this.target.Location = this.oldLocation;
        }
    }
}