using System.Drawing;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Command.ImageCommand {
    internal class ChangePosition : ISingleAction {
        private Album _album;
        private bool[] _checkes;
        private int[] _ins;
        private Point[] _oldLocations;
        private Size[] _oldMaxSizes;
        private bool _relative;
        public int[] Indices { set; get; }

        public void Do(params object[] args) {
            _album = args[0] as Album;
            Indices = args[1] as int[];
            _ins = args[2] as int[];
            _checkes = args[3] as bool[];
            if (_checkes == null) {
                return;
            }
            _relative = _checkes[4];
            if (Indices == null) {
                return;
            }
            if (_ins == null) {
                return ;
            }
            _oldLocations = new Point[Indices.Length];
            _oldMaxSizes = new Size[Indices.Length];
            for (var i = 0; i < Indices.Length; i++) {
                if (_album == null || Indices[i] > _album.List.Count - 1 || Indices[i] < 0) {
                    continue;
                }
                var entity = _album.List[Indices[i]];
                if (entity.Type == ColorBits.Link) {
                    continue;
                }
                _oldLocations[i] = entity.Location;
                _oldMaxSizes[i] = entity.CanvasSize;
                    if (_checkes[0]) {
                    if (!_relative) {
                        entity.X = 0;
                    }
                     entity.X += _ins[0];
                }

                if (_checkes[1]) {
                    if (!_relative) {
                        entity.Y = 0;
                    }
                    entity.Y += _ins[1];
                }

                if (_checkes[2]) {
                    if (!_relative) {
                        entity.CanvasWidth = 0;
                    }
                    entity.CanvasWidth += _ins[2];
                }

                if (_checkes[3]) {
                    if (!_relative) {
                        entity.CanvasHeight = 0;
                    }
                    entity.CanvasHeight += _ins[3];
                }
            }
        }

        public void Redo() {
            Do(_album, Indices, _ins, _checkes);
        }


        public void Undo() {
            for (var i = 0; i < Indices.Length; i++) {
                if (Indices[i] > _album.List.Count - 1 || Indices[i] < 0) continue;
                var entity = _album.List[Indices[i]];
                entity.Location = _oldLocations[i];
                entity.CanvasSize = _oldMaxSizes[i];
            }
        }

        public void Action(Album album, int[] indexes) {
            foreach (var i in indexes) {
                if (i > album.List.Count - 1 || i < 0) {
                    continue;
                }
                var entity = album.List[i];
                if (_checkes[0]) {
                    if (!_relative) entity.X = 0;
                    entity.X += _ins[0];
                }

                if (_checkes[1]) {
                    if (!_relative) entity.Y = 0;
                    entity.Y += _ins[1];
                }

                if (_checkes[2]) {
                    if (!_relative) entity.CanvasWidth = 0;
                    entity.CanvasWidth += _ins[2];
                }

                if (_checkes[3]) {
                    if (!_relative) entity.CanvasHeight = 0;
                    entity.CanvasHeight += _ins[3];
                }
            }
        }

        public bool CanUndo => true;      

        public bool IsChanged => true;

        public string Name => "ChangeImagePosition";
    }
}