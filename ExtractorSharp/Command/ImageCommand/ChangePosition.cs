using ExtractorSharp.Data;
using System.Drawing;

namespace ExtractorSharp.Command.ImageCommand {
    class ChangePosition : ICommand,SingleAction{
        Album Album;
        public int[] Indexes { set; get; }
        int[] Ins;
        bool[] Checkes;
        bool relative;
        Point[] old_Locations;
        Size[] old_Max_Sizes;
        public void Do(params object[] args) {
            Album = args[0] as Album;
            Indexes = args[1] as int[];
            Ins = args[2] as int[];
            Checkes = args[3] as bool[];
            relative = Checkes[4];
            old_Locations = new Point[Indexes.Length];
            old_Max_Sizes = new Size[Indexes.Length];
            for (var i = 0; i < Indexes.Length; i++) {
                if (Indexes[i] > Album.List.Count - 1 || Indexes[i] < 0)
                    continue;
                var entity = Album.List[Indexes[i]];
                old_Locations[i] = entity.Location;
                old_Max_Sizes[i] = entity.Cavas_Size;
                if (Checkes[0]) {
                    if (!relative)
                        entity.X = 0;
                    entity.X += Ins[0];
                }
                if (Checkes[1]) {
                    if (!relative)
                        entity.Y = 0;
                    entity.Y += Ins[1];
                }
                if (Checkes[2]) {
                    if (!relative)
                        entity.Cavas_Width = 0;
                    entity.Cavas_Width += Ins[2];
                }
                if (Checkes[3]) {
                    if (!relative)
                        entity.Cavas_Height = 0;
                    entity.Cavas_Height += Ins[3];
                }
            }
        }

        public void Redo() => Do(Album, Indexes, Ins, Checkes);
        

        public void Undo() {
            for (var i = 0; i < Indexes.Length; i++) {
                if (Indexes[i] > Album.List.Count - 1 || Indexes[i] < 0) {
                    continue;
                }
                var entity = Album.List[Indexes[i]];
                entity.Location = old_Locations[i];
                entity.Cavas_Size = old_Max_Sizes[i];
            }
        }

        public void Action(Album Album,int[] indexes) {        
            foreach(var i in indexes) {
                if (i > Album.List.Count - 1 || i < 0) {
                    continue;
                }
                var entity = Album.List[i];
                if (Checkes[0]) {
                    if (!relative)
                        entity.X = 0;
                    entity.X += Ins[0];
                }
                if (Checkes[1]) {
                    if (!relative)
                        entity.Y = 0;
                    entity.Y += Ins[1];
                }
                if (Checkes[2]) {
                    if (!relative)
                        entity.Cavas_Width = 0;
                    entity.Cavas_Width += Ins[2];
                }
                if (Checkes[3]) {
                    if (!relative)
                        entity.Cavas_Height = 0;
                    entity.Cavas_Height += Ins[3];
                }
            }
        }

        public bool CanUndo => true;

        public bool Changed => true;

        public override string ToString() => Language.Default["ChangeImagePosition"];

    }
}
