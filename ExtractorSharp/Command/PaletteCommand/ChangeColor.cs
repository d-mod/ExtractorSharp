using System.Drawing;
using ExtractorSharp.Data;

namespace ExtractorSharp.Command.PaletteCommand {
    class ChangeColor : ICommand {

        private Album Album { set; get; }

        private int TableIndex;

        private int[] Indexes;

        private Color[] OldColor;

        private Color NewColor;

        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => false;

        public string Name => "ChangeColor";

        public void Do(params object[] args) {
            Album = args[0] as Album;
            TableIndex = (int)args[1];
            Indexes = args[2] as int[];  
            NewColor = (Color)args[3];
            OldColor = new Color[Indexes.Length];
            for (var i =0;i<Indexes.Length;i++) {
                var index = Indexes[i];
                var table = Album.Tables[TableIndex];
                OldColor[i] = table[index];
                table[i] = NewColor;
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
