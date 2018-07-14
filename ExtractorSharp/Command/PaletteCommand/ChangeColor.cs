using System.Drawing;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.PaletteCommand {
    internal class ChangeColor : ICommand {
        private int[] Indexes;

        private Color NewColor;

        private Color[] OldColor;

        private int TableIndex;

        private Album Album { set; get; }

        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => false;

        public string Name => "ChangeColor";

        public void Do(params object[] args) {
            Album = args[0] as Album;
            TableIndex = (int) args[1];
            Indexes = args[2] as int[];
            NewColor = (Color) args[3];
            OldColor = new Color[Indexes.Length];
            var table = Album.Tables[TableIndex];
            for (var i = 0; i < Indexes.Length; i++) {
                var index = Indexes[i];
                OldColor[i] = table[index];
                table[index] = NewColor;
            }
            Album.Refresh();
        }

        public void Redo() {
            Do(Album, TableIndex, Indexes, NewColor);
            Album.Refresh();
        }

        public void Undo() {
            for (var i = 0; i < Indexes.Length; i++) {
                Album.Tables[TableIndex][Indexes[i]] = OldColor[i];
            }
            Album.Refresh();
        }
    }
}