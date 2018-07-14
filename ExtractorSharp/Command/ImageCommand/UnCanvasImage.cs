using System.Drawing;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    ///     去画布化
    /// </summary>
    internal class UnCanvasImage : ISingleAction, ICommandMessage {
        private Album Album { set; get; }

        private Bitmap[] Images { set; get; }

        private Point[] Locations { set; get; }
        public int[] Indices { set; get; }

        public string Name => "UnCanvasImage";

        public void Do(params object[] args) {
            Album = args[0] as Album;
            Indices = args[1] as int[];
            Images = new Bitmap[Indices.Length];
            Locations = new Point[Indices.Length];
            for (var i = 0; i < Indices.Length; i++) {
                if (Indices[i] > Album.List.Count - 1 || Indices[i] < 0) {
                    continue;
                }
                var entity = Album.List[Indices[i]];
                Images[i] = entity.Picture;
                Locations[i] = entity.Location;
                entity.UnCanvasImage();
            }
        }

        public void Redo() {
            Do(Album, Indices);
        }


        public void Undo() {
            for (var i = 0; i < Indices.Length && i < Images.Length; i++) {
                var entity = Album.List[Indices[i]];
                entity.ReplaceImage(entity.Type, false, Images[i]);
                entity.Location = Locations[i];
            }
        }

        public void Action(Album album, int[] indexes) {
            foreach (var i in indexes) {
                if (i < album.List.Count && i > -1) {
                    album.List[i].UnCanvasImage();
                }
            }
        }


        public bool CanUndo => true;

        public bool IsChanged => true;
    }
}