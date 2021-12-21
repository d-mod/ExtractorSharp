using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Draw.Paint;

namespace ExtractorSharp.Services.Commands {

    [ExportMetadata("Name", "AddCompareLayer")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(ICommand))]
    internal class AddCompareLayer : InjectService, IRollback {

        private Album[] files;

        private CompareLayer[] Layers { set; get; }

        private decimal ImageScale;

        private bool IsRealPosition;

        public void Do(CommandContext context) {
            context.Get(out this.files);
            this.Store.Get("/draw/image-scale", out this.ImageScale);
            this.IsRealPosition = this.Config["RealPosition"].Boolean;
            this.Redo();
        }

        public void Redo() {
            this.Layers = new CompareLayer[this.files.Length];

            for(var i = 0; i < this.Layers.Length; i++) {
                this.Layers[i] = new CompareLayer {
                    Name = this.files[i].Name,
                    Tag = this.files[i],
                    ImageScale = this.ImageScale / 100,
                    Visible = true,
                    Index = -1,
                    RealPosition = IsRealPosition
                };
            }

            this.Store.Use<List<IPaint>>("/draw/layers", list => {
                list.AddRange(this.Layers);
                return list;
            });
        }

        public void Undo() {
            this.Store.Use<List<IPaint>>("/draw/layers", list => {
                list.RemoveAll(this.Layers.Contains);
                return list;
            });
        }
    }
}
