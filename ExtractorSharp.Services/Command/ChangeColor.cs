using System.Drawing;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Services.Commands {
    internal class ChangeColor : ICommand {

        [CommandParameter]
        private int[] Indices;

        [CommandParameter("Color")]
        private Color NewColor;

        private Color[] OldColor;

        [CommandParameter]
        private int TableIndex = -1;

        [CommandParameter("File", IsDefault = true)]
        private Album Album { set; get; }

        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => false;

        public void Do(CommandContext context) {
            context.Export(this);
            this.TableIndex = this.Album.PaletteIndex;
            this.Redo();
        }

        public void Redo() {
            this.OldColor = new Color[this.Indices.Length];
            var table = this.Album.Palettes[this.TableIndex];
            for(var i = 0; i < this.Indices.Length; i++) {
                var index = this.Indices[i];
                this.OldColor[i] = table[index];
                table[index] = this.NewColor;
            }
            this.Album.Refresh();
        }

        public void Undo() {
            for(var i = 0; i < this.Indices.Length; i++) {
                this.Album.Palettes[this.TableIndex][this.Indices[i]] = this.OldColor[i];
            }
            this.Album.Refresh();
        }
    }
}