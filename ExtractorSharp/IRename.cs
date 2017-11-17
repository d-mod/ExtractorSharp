namespace ExtractorSharp {
    /// <summary>
    /// 可重命名,可以使用重命名窗口
    /// <see cref="View.RenameDialog"/>
    /// </summary>
    interface IName {
        /// <summary>
        /// 名字
        /// </summary>
        string Name { get; set; }
    }
}
