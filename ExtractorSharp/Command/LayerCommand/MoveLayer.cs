using ExtractorSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Command.LayerCommand {
    class MoveLayer : ICommand {
        public string Name =>"MoveLayer";

        public bool CanUndo => true;

        public bool IsChanged => false;

        public bool IsFlush => true;

        private int SoureIndex { set; get; }

        private int TargetIndex { set; get; }



        private Drawer Drawer => Program.Drawer;

        public void Do(params object[] args) {
            SoureIndex = (int)args[0];
            TargetIndex = (int)args[1];
            var list = Drawer.LayerList;
            Drawer.MoveLayer(SoureIndex,TargetIndex);
        }

        public void Redo() {
            Drawer.MoveLayer(SoureIndex, TargetIndex);
        }
        public void Undo() {
            Drawer.MoveLayer(SoureIndex, TargetIndex);
        }
    }
}
