using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Command.LayerCommand {
    class AddLayer : ICommand {
        public string Name => throw new NotImplementedException();

        public bool CanUndo => true;

        public bool IsChanged => false;

        public bool IsFlush => true;

        private Sprite[] Array { set; get; }

        private Layer[] Layers { set; get; }
        
        private Drawer Drawer => Program.Drawer;

        private List<IPaint> List => Drawer.LayerList;

        public void Do(params object[] args) {
            Array = args as Sprite[];
            Layers = new Layer[Array.Length];
            for (var i = 0; i < Layers.Length; i++) {
                Layers[i] = new Layer();
                Layers[i].Name = $"{Language.Default["NewLayer"]}{Drawer.CustomLayerCount++}";
                Layers[i].Sprite = Array[i];
                Layers[i].ImageScale = Drawer.ImageScale;
                Layers[i].Visible = true;
            }
            Drawer.AddLayer(Layers);
        }
        public void Redo() {
            Do(Array);
        }
        public void Undo() {
            foreach (var layer in Layers) {
                List.Remove(layer);
            }
            Drawer.CustomLayerCount -= Layers.Length;
            Drawer.OnLayerChanged(new EventArguments.LayerEventArgs());
        }
    }
}
