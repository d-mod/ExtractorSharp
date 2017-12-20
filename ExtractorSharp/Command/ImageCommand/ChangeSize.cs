using ExtractorSharp.Data;
using System.Drawing;

namespace ExtractorSharp.Command.ImageCommand {
    class ChangeSize : ICommand {
        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => false;

        public string Name => "ChangeImageSize";

        private Album Album;

        private int[] Indexes;

        private decimal Scale;

        private Bitmap[] Array;

        public void Do(params object[] args) {
            Album = args[0] as Album;
            Indexes = args[1] as int[];
            Scale = (decimal)args[2];
            Array = new Bitmap[Indexes.Length];
            for (var i = 0; i < Array.Length; i++) {
                var entity = Album[Indexes[i]];
                var image = entity.Picture;
                var point = entity.Location;
                Array[i] = image;
                entity.Picture = image.Star(Scale);
                entity.Location = entity.Location.Divide(Scale);
            }
        }

        public void Redo() {
            Do(Album,Indexes,Scale);
        }

        public void Undo() {
            for (var i = 0; i < Array.Length; i++) {
                var entity = Album[Indexes[i]];
                entity.Picture = Array[i];
                entity.Location = entity.Location.Star(Scale);
            }
        }

    }
}
