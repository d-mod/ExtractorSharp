using System.Drawing;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Draw.Paint {
    /// <summary>
    ///     图层
    /// </summary>
    public class Layer : AbstractPaint {

        public int Index {
            get => this.Sprite.Index;
            set { }
        }

        public Sprite Sprite { set; get; }

        public decimal ImageScale { set; get; }


        public override Bitmap Image {
            get => this.Sprite.Image;
            set { }
        }


        public override string Name { get; set; }

        public override Size Size {
            get => this.Sprite.Size.Star(this.ImageScale);
            set { }
        }

        public override Rectangle Rectangle => new Rectangle(this.Location, this.Size);

        /// <summary>
        ///     是否在图层范围之内
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool Contains(Point point) {
            return this.Rectangle.Contains(point);
        }

        /// <summary>
        ///     绘制
        /// </summary>
        /// <param name="Controller"></param>
        /// <param name="g"></param>
        public override void Draw(Graphics g) {
            g.DrawImage(this.Image, this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height);
        }

    }
}