namespace ExtractorSharp.Core.Model {
    /// <summary>
    ///     压缩类型
    /// </summary>
    public enum CompressMode {
        /// <summary>
        /// ZLIB压缩
        /// </summary>
        ZLIB = 0x06,
        /// <summary>
        /// 不压缩
        /// </summary>
        NONE = 0x05,
        /// <summary>
        /// DDS专用的ZLIB压缩
        /// </summary>
        DDS_ZLIB = 0x07,
        /// <summary>
        /// 未知
        /// </summary>
        UNKNOWN = 0x01

    }
}
