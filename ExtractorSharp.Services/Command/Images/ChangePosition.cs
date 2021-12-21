using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Services.Constants;

namespace ExtractorSharp.Services.Commands {

    [ExportCommand("ChangePosition")]
    [CommandListeners(ListenterKeys.SAVE_CHANGED,ListenterKeys.REFRESH_FILE)]
    internal class ChangePosition : InjectService, IRollback, ISingleMacro {

        [CommandParameter("File", IsDefault =true,IsRequired = true)]
        private Album _album;

        [CommandParameter(IsRequired = false)]
        private int? x;

        [CommandParameter(IsRequired = false)]
        private int? y;

        [CommandParameter(IsRequired = false)]
        private int? frameWidth;

        [CommandParameter(IsRequired = false)]
        private int? frameHeight;

        [CommandParameter(IsRequired = false)]
        private bool relative;

        private Point[] _oldLocations;

        private Size[] _oldMaxSizes;


        [CommandParameter("Indices", IsRequired = true)]
        public int[] Indices { set; get; }

        public void Do(CommandContext context) {
            context.Export(this);

            this.Redo();
        }

        public void Redo() {
            this._oldLocations = new Point[this.Indices.Length];
            this._oldMaxSizes = new Size[this.Indices.Length];
            for(var i = 0; i < this.Indices.Length; i++) {
                if(this._album == null || this.Indices[i] > this._album.List.Count - 1 || this.Indices[i] < 0) {
                    continue;
                }
                var sprite = this._album.List[this.Indices[i]];
                if(sprite.ColorFormat == ColorFormats.LINK) {
                    continue;
                }
                this._oldLocations[i] = sprite.Location;
                this._oldMaxSizes[i] = sprite.FrameSize;
                this.Change(sprite);
            }
            this.Messager.Success("<ChangeImagePosition><Success>!");
        }


        public void Undo() {
            for(var i = 0; i < this.Indices.Length; i++) {
                if(this.Indices[i] > this._album.List.Count - 1 || this.Indices[i] < 0) {
                    continue;
                }
                var entity = this._album.List[this.Indices[i]];
                entity.Location = this._oldLocations[i];
                entity.FrameSize = this._oldMaxSizes[i];
            }
        }

        private void Change(Sprite sprite) {
            if(this.x != null) {
                if(!this.relative) {
                    sprite.X = 0;
                }
                sprite.X += this.x.Value;
            }
            if(this.y != null) {
                if(!this.relative) {
                    sprite.Y = 0;
                }
                sprite.Y += this.y.Value;
            }
            if(this.frameWidth != null) {
                if(!this.relative) {
                    sprite.FrameWidth = 0;
                }
                sprite.FrameWidth += this.frameWidth.Value;
            }
            if(this.frameHeight != null) {
                if(!this.relative) {
                    sprite.FrameHeight = 0;
                }
                sprite.FrameHeight += this.frameHeight.Value;
            }
        }

        public void Action(Album album, int[] indexes) {
            foreach(var i in indexes) {
                if(i > album.List.Count - 1 || i < 0) {
                    continue;
                }
                var sprite = album.List[i];
                this.Change(sprite);
            }
        }

    }
}