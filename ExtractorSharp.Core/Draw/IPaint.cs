using System.Drawing;

namespace ExtractorSharp.Draw {
    /// <summary>
    /// 可绘制的物体
    /// </summary>
    public interface IPaint {

        string Name { set; get; }
        /// <summary>
        /// 是否全屏
        /// </summary>
        bool FullCanvas { set; get; }
        /// <summary>
        /// 是否可见
        /// </summary>
        bool Visible { set; get; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        bool Locked { set; get; }
        /// <summary>
        /// 图片
        /// </summary>
        Bitmap Image { set; get; }
        /// <summary>
        /// 大小
        /// </summary>
        Size Size { set; get; }
        /// <summary>
        /// 坐标
        /// </summary>
        Point Location { set; get; }
        /// <summary>
        /// 所在区域
        /// </summary>
        Rectangle Rectangle { get; }
        /// <summary>
        /// 判断物体是否被选中
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        bool Contains(Point point);
        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="g"></param>
        void Draw(Graphics g);

        object Tag { set; get; }
    }
}
