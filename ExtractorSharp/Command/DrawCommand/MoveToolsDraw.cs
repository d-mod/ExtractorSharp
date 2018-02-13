using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Draw;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ExtractorSharp.Command.DrawCommand {
    class MoveToolsDraw : ICommand {
        private IPaint Entity;

        private Point Source;

        private Point Dest;
        

        public string Name => "MoveTools";

        public bool CanUndo => false;

        public bool IsChanged => false;

        public bool IsFlush => false;

        private IConnector Connector => Program.Connector;

        public void Do(params object[] args) {
            Entity = args[0] as IPaint;
            Source = (Point)args[1];
            Dest = (Point)args[2];
            Entity.Location = Entity.Location.Add(Dest.Minus(Source));
            Connector.CanvasFlush();
        }

        public void Redo() {
        }

        public void Undo() {
        }
    }
}
