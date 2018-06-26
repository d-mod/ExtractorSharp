namespace ExtractorSharp.Composition {
    public interface IMetadata {
        /// <summary>
        ///     名称
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     插件版本
        /// </summary>
        string Version { get; }

        /// <summary>
        ///     作者
        /// </summary>
        string Author { get; }

        /// <summary>
        ///     程序最低版本要求
        /// </summary>
        string Since { get; }

        /// <summary>
        ///     注释
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     唯一标识码
        /// </summary>
        string Guid { get; }
    }
}