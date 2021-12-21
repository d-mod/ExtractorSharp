using System;
using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition;
using ExtractorSharp.Composition.Control;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Draw.Paint;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Draw.Paint;

namespace ExtractorSharp.Draw.Brush {
    /// <summary>
    ///     移动工具
    /// </summary>
    /// 
    [Export(typeof(IBrush))]
    internal class MoveTool : InjectService, IBrush {
        public Color Color { set; get; }

        public string Name => "MoveTool";

        public IntPtr Cursor { get; }

        public int Radius { get; set; }

        public Point Location { get; set; }


        public void Draw(IPaint paint, Point newPoint, decimal scale) {

            this.Controller.Do("moveTools", new CommandContext(paint){
                { "Source", this.Location },
                { "Target", newPoint }
            });
            var layer = new Layer();

            if(this.Config["AutoChangePosition"].Boolean && paint.Equals(layer)) {
                if(layer.Tag is Sprite sprite && paint is Canvas canvas) {
                    var album = sprite.Parent;
                    var index = sprite.Index;
                    var location = canvas.RealLocation;
                    if(canvas.RealPosition) {
                        location = location.Minus(sprite.Location);
                        canvas.Location = Point.Empty;
                    }
                    this.Controller.Do("ChangePosition", new CommandContext {
                        { "File", album },
                        { "Indices",new []{index} },
                        { "X" , location.X },
                        { "Y" , location.Y },
                        { "FrameWidth",null },
                        { "FrameHeight",null },
                        { "Relative" , canvas.RealPosition }
                    });

                }
            }
        }
    }
}