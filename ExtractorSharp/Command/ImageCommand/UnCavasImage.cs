using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.View;
using System;
using System.Drawing;

namespace ExtractorSharp.Command.ImageCommand {
    /// <summary>
    /// 去画布化
    /// </summary>
    class UnCanvasImage : SingleAction {
        public int[] Indices { set; get; }

        public string Name => "UnCanvas";

        private Album Album;

        private Bitmap[] Images;

        private Point[] Locations;

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
            Messager.ShowOperate("UnCanvasImage");
        }

        public void Redo() => Do(Album, Indices);


        public void Undo() {
            for (var i = 0; i < Indices.Length && i < Images.Length; i++) {
                var entity = Album.List[Indices[i]];
                entity.ReplaceImage(entity.Type, false, Images[i]);
                entity.Location = Locations[i];
            }
        }

        public void Action(Album Album, int[] indexes) {
            foreach (var i in indexes) {
                if (i < Album.List.Count && i > -1) {
                    Album.List[i].UnCanvasImage();
                }
            }
        }
        

        public bool CanUndo => true;

        public bool IsChanged => true;

        public bool IsFlush => false;
    }
}
