namespace ExtractorSharp.Composition {
    public interface ISetting {

        /// <summary>
        /// UI
        /// </summary>
        object View { get; }

        /// <summary>
        ///     初始化
        /// </summary>
        void Initialize();

        /// <summary>
        ///     保存
        /// </summary>
        void Save();
    }
}