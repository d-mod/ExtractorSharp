using ExtractorSharp.Data;
using System.Drawing;

namespace ExtractorSharp.Command.ImageCommand {
    class ChangePosition : ISingleAction {
        public int[] Indices { set; get; }

        private Album Album;
        private int[] Ins;
        private bool[] Checkes;
        private bool relative;
        private Point[] old_Locations;
        private Size[] old_Max_Sizes;

        public void Do(params object[] args) {
            Album = args[0] as Album;
            Indices = args[1] as int[];
            Ins = args[2] as int[];
            Checkes = args[3] as bool[];
            relative = Checkes[4];
            old_Locations = new Point[Indices.Length];
            old_Max_Sizes = new Size[Indices.Length];
            for (var i = 0; i < Indices.Length; i++) {
                if (Indices[i] > Album.List.Count - 1 || Indices[i] < 0) {
                    continue;
                }
                var entity = Album.List[Indices[i]];
                if (entity.Type == ColorBits.LINK) {
                    continue;
                }
                old_Locations[i] = entity.Location;
                old_Max_Sizes[i] = entity.Canvas_Size;
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
                        entity.Canvas_Width = 0;
                    entity.Canvas_Width += Ins[2];
                }
                if (Checkes[3]) {
                    if (!relative)
                        entity.Canvas_Height = 0;
                    entity.Canvas_Height += Ins[3];
                }
            }
        }

        public void Redo() => Do(Album, Indices, Ins, Checkes);
        

        public void Undo() {
            for (var i = 0; i < Indices.Length; i++) {
                if (Indices[i] > Album.List.Count - 1 || Indices[i] < 0) {
                    continue;
                }
                var entity = Album.List[Indices[i]];
                entity.Location = old_Locations[i];
                entity.Canvas_Size = old_Max_Sizes[i];
            }
        }

        public void Action(Album Album,int[] indexes) {        
            foreach(var i in indexes) {
                if (i > Album.List.Count - 1 || i < 0) {
                    continue;
                }
                var entity = Album.List[i];
                if (Checkes[0]) {
                    if (!relative) {
                        entity.X = 0;
                    }
                    entity.X += Ins[0];
                }
                if (Checkes[1]) {
                    if (!relative) {
                        entity.Y = 0;
                    }
                    entity.Y += Ins[1];
                }
                if (Checkes[2]) {
                    if (!relative) {
                        entity.Canvas_Width = 0;
                    }
                    entity.Canvas_Width += Ins[2];
                }
                if (Checkes[3]) {
                    if (!relative) {
                        entity.Canvas_Height = 0;
                    }
                    entity.Canvas_Height += Ins[3];
                }
            }
        }

        public bool CanUndo => true;

        public bool IsFlush => true;

        public bool IsChanged => true;

        public string Name => "ChangeImagePosition";

    }
}
