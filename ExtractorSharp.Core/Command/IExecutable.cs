namespace ExtractorSharp.Core.Command {
    public interface IExecutable {
        string Name { set; get; }

        object Execute(params object[] args);
    }
}
