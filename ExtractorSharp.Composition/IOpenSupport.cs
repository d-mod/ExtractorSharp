namespace ExtractorSharp.Composition {

    /// <summary>
    /// 可打开的支持
    /// </summary>
    public interface IOpenSupport {

        string Extension { get; }

        bool Open(string file);

    }
}
