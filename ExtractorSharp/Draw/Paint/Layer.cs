using System.Drawing;
using System;
using ExtractorSharp.Data;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.Draw {
    /// <summary>
    /// 图层
    /// </summary>
    
    enum Model_Version {
        Ver1=0x01
    }

    public class Layer:IPaint{
        public Point Location { set; get; }
        public Bitmap Image { get => Sprite.Picture; set { } }
        public int Index {
            get => Sprite.Index; set { }
        }
        public bool Locked { set; get; }
        public bool Visible { set; get; }

        public bool FullCanvas { set; get; }

        public string Name { get; set; }

        public Sprite Sprite { set; get; }

        public Size Size { get => Sprite.Size.Star(ImageScale); set { } }
        public decimal ImageScale { set; get; }
        public Rectangle Rectangle => new Rectangle(Location, Size);

        public object Tag { set; get; }



        /// <summary>
        /// 是否在图层范围之内
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Point point) {
            return new Rectangle(Location, Size).Contains(point);
        }

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="Controller"></param>
        /// <param name="g"></param>
        public void Draw(Graphics g) {
            g.DrawImage(Image, Location.X, Location.Y, Size.Width, Size.Height);
        }


        public override string ToString() {
            return $"{Name},{Language.Default["Position"]}:({Location.X},{Location.Y}),{Language.Default["Size"]}({Size.Width},{Size.Height})";
        }

    }
}
