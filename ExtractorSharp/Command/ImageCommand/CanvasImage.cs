using ExtractorSharp.Core;
using ExtractorSharp.Data;
using System.Drawing;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    /// 画布化
    /// 可撤销
    /// 可宏命令
    /// </summary>
    class CanvasImage : SingleAction {
        private Album Album;

        private Size Size;

        private Bitmap[] Images;

        private Point[] Locations;

        public int[] Indices { set; get; }

        public void Do(params object[] args) {
            Album = args[0] as Album;
            Size = (Size)args[1];
            Indices = args[2] as int[];
            Images = new Bitmap[0];
            Locations = new Point[0];
            Images = new Bitmap[Indices.Length];
            Locations = new Point[Indices.Length];
            for (var i = 0; i < Indices.Length; i++) {
                if (Indices[i] > Album.List.Count - 1 || Indices[i] < 0)
                    continue;
                var entity = Album.List[Indices[i]];
                Images[i] = entity.Picture;
                Locations[i] = entity.Location;
                entity.CanvasImage(Size);
            }
            Messager.ShowOperate("CanvasImage");
        }

        public void Redo() => Do(Album, Size, Indices);


        public void Undo() {
            for (var i = 0; i < Indices.Length && i < Images.Length; i++) {
                var entity = Album.List[Indices[i]];
                entity.ReplaceImage(entity.Type, false, Images[i]);
                entity.Location = Locations[i];
            }
        }

        public void Action(Album Album, int[] indexes) {
            foreach (var i in indexes)
                if (i < Album.List.Count && i > -1)
                    Album.List[i].CanvasImage(Size);
        }

        public bool IsFlush => false;

        public bool CanUndo => true;

        public bool IsChanged => true;

        public string Name => "Canvas";
    }
}
