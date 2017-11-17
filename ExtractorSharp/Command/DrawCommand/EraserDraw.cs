using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Command.DrawCommand {
    class EraserDraw : ICommand {
        private ImageEntity Entity;
        private Point Location;
        private Color Color;
        private Bitmap Image;
        public bool CanUndo => true;

        public bool Changed => false;

        public void Do(params object[] args) {

        }

        public void Redo() {

        }

        public void Undo() {

        }
    }
}
