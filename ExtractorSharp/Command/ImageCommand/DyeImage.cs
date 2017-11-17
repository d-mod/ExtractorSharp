using ExtractorSharp.Data;
using System.Drawing;

namespace ExtractorSharp.Command.ImageCommand {
    class DyeImage : ICommand,SingleAction{
        Bitmap[] Images;
        public int[] Indexes { set; get; }
        Album Album;
        Color Color;
        public void Do(params object[] args) {
            Album = args[0] as Album;
            Color = (Color)args[1];
            Indexes = args[2] as int[];
            Images = new Bitmap[Indexes.Length];
            for (var i = 0; i < Indexes.Length; i++) {
                var index = Indexes[i];
                if (index > Album.List.Count - 1 || index < 0)
                    continue;
                var entity = Album.List[index];
                if (entity.Type == ColorBits.LINK)
                    continue;
                Images[i] = entity.Picture;
                var data = entity.Picture.ToArray();
                for (var j = 0; j < data.Length; j += 4) {
                    data[j + 0] = (byte)Complie(data[j + 0], Color.B);
                    data[j + 1] = (byte)Complie(data[j + 1], Color.G);
                    data[j + 2] = (byte)Complie(data[j + 2], Color.R);
                    data[j + 3] = (byte)Complie(data[j + 3], Color.A);
                }
                entity.Picture = Tools.FromArray(data, entity.Size);
            }
        }

        public static int Complie(int up, int down) {
            var result = up + down - 255;
            result = result < 0 ? 0 : result;
            return result;
        }

        public override string ToString() {
            return "染色：#"+Color;
        }

        public void Undo() {
            for (int i = 0; i < Indexes.Length; i++) {
                if (Indexes[i] > Album.List.Count - 1 && Indexes[i] < 0)
                    continue;
                var entity = Album.List[Indexes[i]];
                if (entity.Type == ColorBits.LINK)
                    continue;
                entity.ReplaceImage(entity.Type, false, Images[i]);
            }       
        }

        public void Redo() {
            Do(Album, Color, Indexes);
        }

        public void Action(Album Album ,int[] indexes) {
            for (var i = 0; i < Indexes.Length; i++) {
                var index = Indexes[i];
                if (index > Album.List.Count - 1 || index < 0)
                    continue;
                var entity = Album.List[index];
                if (entity.Type == ColorBits.LINK)
                    continue;
                var data = entity.Picture.ToArray();
                for (var j = 0; j < data.Length; j += 4) {
                    data[j + 0] = (byte)Complie(data[j + 0], Color.B);
                    data[j + 1] = (byte)Complie(data[j + 1], Color.G);
                    data[j + 2] = (byte)Complie(data[j + 2], Color.R);
                    data[j + 3] = (byte)Complie(data[j + 3], Color.A);
                }
                entity.Picture = Tools.FromArray(data, entity.Size);
            }
        }

        public void RunScript(string arg) { }

        public bool CanUndo => true;

        public bool Changed => true;

    }
}
