using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Draw.Paint;

namespace ExtractorSharp.Services.Commands {

    [ExportCommand("AddLayer")]
    internal class AddLayer : InjectService, IRollback {

        private Sprite[] sprites;

        private Layer[] layers;

        public void Do(CommandContext context) {
            context.Get(out this.sprites);
            this.layers = new Layer[this.sprites.Length];
            this.Redo();
        }

        public void Redo() {
            this.Store.Get("/drawer/custom-layers-count", out int count)
                .Get("/drawer/image-scale", out decimal imageScale);

            for(var i = 0; i < this.layers.Length; i++) {
                this.layers[i] = new Layer {
                    Name = $"{this.Language["NewLayer"]}{++count}",
                    Sprite = this.sprites[i],
                    ImageScale = imageScale,
                    Visible = true
                };
            }
            this.Store
                .Set("/drawer/custom-layers-count", count)
                .Use<List<IPaint>>("/draw/layers", list => {
                    list.AddRange(this.layers);
                    return list;
                });
        }

        public void Undo() {
            this.Store.Use<List<IPaint>>("/draw/layers", list => {
                list.RemoveAll(this.layers.Contains);
                return list;
            });
        }

        public string Name => "AddLayer";
    }
}