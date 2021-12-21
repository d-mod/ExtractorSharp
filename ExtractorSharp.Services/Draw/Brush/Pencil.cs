using System;
using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Services.Properties;

namespace ExtractorSharp.Draw.Brush {


    [Export(typeof(IBrush))]
    internal class Pencil : InjectService, IBrush {

        public string Name => "Pencil";

        public IntPtr Cursor => Resources.pencil.GetHicon();

        public int Radius { set; get; } = 1;

        public Point Location { set; get; }




        public void Draw(IPaint paint, Point point, decimal scale) {
            point = point.Minus(paint.Location).Divide(scale);
            if(paint.Tag != null) {
                this.Controller.Do("PencilDraw", new CommandContext(paint.Tag) {
                    { "Location",point },
                    { "Color",Color.Red}
                });
            }
        }
    }
}