namespace ExtractorSharp.Command.MergeCommand {
    class RunMerge : ICommand {
        public bool CanUndo => false;

        public bool Changed => false;

        public void Do(params object[] args) =>Program.Merger.RunMerge();

        public void Redo() { }
        public void Undo() { }

        public string Name => "RunMerge";
    }
}
