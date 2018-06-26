using System.Drawing;
using ExtractorSharp.Core.Command;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.Command.DrawCommand {
    internal class MoveToolsDraw : ICommand {
        private Point Dest;
        private IPaint Entity;

        private Point Source;

        private IConnector Connector => Program.Connector;


        public string Name => "MoveTools";

        public bool CanUndo => false;

        public bool IsChanged => false;

        public bool IsFlush => false;

        public void Do(params object[] args) {
            Entity = args[0] as IPaint;
            Source = (Point) args[1];
            Dest = (Point) args[2];
            Entity.Location = Entity.Location.Add(Dest.Minus(Source));
            Connector.CanvasFlush();
        }

        public void Redo() { }

        public void Undo() { }
    }
}