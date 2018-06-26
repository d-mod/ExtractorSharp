namespace ExtractorSharp.Json.Converter {
    /// <summary>
    ///     转换器
    /// </summary>
    internal interface IConverter {
        LSType Type { get; }
        string Pattern { get; }
        bool Convert<T>(object Value, out T t);
    }
}