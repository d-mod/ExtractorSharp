using ExtractorSharp.Core;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Config;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Draw.Paint;
using ExtractorSharp.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Command.LayerCommand {
    class AddCompareLayer :ICommand{
        private Album[] Array { set; get; }

        private CompareLayer[] Layers { set; get; }

        private Drawer Drawer => Program.Drawer;

        private IConfig Config => Program.Config;

        public string Name => "AddCompareLayer";

        public bool CanUndo => true;

        public bool IsChanged => false;

        public void Do(params object[] args) {
            Array = args as Album[];
            Layers = new CompareLayer[Array.Length];
            for (var i = 0; i < Layers.Length; i++) {
                Layers[i] = new CompareLayer();
                Layers[i].Name = $"{Language.Default["CompareLayer"]}[{Array[i].Name}]";
                Layers[i].Tag = Array[i];
                Layers[i].ImageScale = Drawer.ImageScale;
                Layers[i].Visible = true;
                Layers[i].Index = -1;
                Layers[i].RealPosition = Config["RealPosition"].Boolean;
            }

            Drawer.AddLayer(Layers);
        }

        public void Redo() {
            Do(Array);
        }

        public void Undo() {
            var list = Drawer.LayerList;
            foreach (var layer in Layers) {
                list.Remove(layer);
            }
            Drawer.OnLayerChanged(new LayerEventArgs());
        }
    }
}
