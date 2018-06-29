using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ExtractorSharp.Core.Composition;
using ExtractorSharp.Core.Config;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Draw.Paint;
using ExtractorSharp.Core.Lib;
using ExtractorSharp.Core.Model;

namespace ExtractorSharp.Component {
    public partial class ESCavasBox : PictureBox {
        public ESCavasBox(IConnector connector) {
            Connector = connector;
            Language = connector?.Language;
            Config = connector?.Config;
            InitializeComponent();
        }

        private IConnector Connector { get; }
        public Language Language { set; get; }
        public IConfig Config { set; get; }

        /// <summary>
        ///     当前图层
        /// </summary>
        private IPaint CurrentLayer { get; } = new Canvas();

        /// <summary>
        ///     上一图层
        /// </summary>
        private IPaint LastLayer { set; get; }


        /// <summary>
        ///     绘图的临时图层
        /// </summary>
        private IPaint BufferLayer { set; get; }

        protected override void OnPaint(PaintEventArgs e) {
            var g = e.Graphics;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            var entity = Connector.SelectedImage; //获得当前选择的贴图
            var pos = CurrentLayer.Location;
            if (!Config["MultipleLayer"].Boolean && entity?.Picture != null) {
                if (entity.Type == ColorBits.LINK && entity.Target != null) entity = entity.Target;
                var pictrue = entity.Picture;
                var size = entity.Size.Star(Config["CanvasScale"].Decimal);
                if (Config["LinearDodge"].Boolean) pictrue = pictrue.LinearDodge();
                if (Config["OnionSkin"].Boolean) LastLayer?.Draw(g);
                CurrentLayer.Tag = entity;
                CurrentLayer.Size = size; //校正当前图层的宽高
                CurrentLayer.Image = pictrue; //校正贴图
                CurrentLayer.Draw(g); //绘制贴图
            }
        }
    }
}