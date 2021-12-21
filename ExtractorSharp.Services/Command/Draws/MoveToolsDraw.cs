using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Draw.Paint;

namespace ExtractorSharp.Services.Commands {


    [ExportMetadata("Name", "MoveTools")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(ICommand))]
    public class MoveToolsDraw : ICommand {

        [CommandParameter(IsDefault = true)]
        private IPaint Tag;

        [CommandParameter]
        private Point Target;

        [CommandParameter]
        private Point Source;


        public void Do(CommandContext context) {
            context.Export(this);
            var offset = this.Target.Minus(this.Source);
            if(this.Tag is Canvas canvas) {
                canvas.RealLocation = canvas.RealLocation.Add(offset);
            } else {
                this.Tag.Location = this.Tag.Location.Add(offset);
            }
        }
    }
}