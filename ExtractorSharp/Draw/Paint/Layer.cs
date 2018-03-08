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
        public Bitmap Image { get => entity.Picture; set { } }
        public int Index {
            get => entity.Index; set { }
        }
        public bool Locked { set; get; }
        public bool Visible { set; get; }

        public bool FullCanvas { set; get; }
        public string Name { get; set; }
        public Sprite entity;
        public Size Size { get => entity.Size; set { } }
        public Rectangle Rectangle => new Rectangle(Location, Size);

        public object Tag { set; get; }
    
        public Point AbsoluteLocation;

        /// <summary>
        /// 替换图层
        /// </summary>
        /// <param name="entity"></param>
        public void Replace(Sprite entity) {
            Location = AbsoluteLocation.Add(entity.Location);
            this.entity = entity;
        }

        /// <summary>
        /// 是否在图层范围之内
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Point point) => Visible && new Rectangle(Location, entity.Size).Contains(point);


        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="Controller"></param>
        /// <param name="g"></param>
        public void Draw(Graphics g) {
            if (Visible)
                g.DrawImage(Image, Location);
        }
        

        public override string ToString() => Name;
        

        public static Layer CreateFrom(Sprite image) {
            var layer = new Layer();
            layer.Name="新建图层" + image.Index;
            layer.entity = image;
            layer.Visible = true;
            return layer;
        }

        public void Adjust() {
            entity.X = Location.X - AbsoluteLocation.X;
            entity.Y = Location.Y - AbsoluteLocation.Y;
        }

    }
}
