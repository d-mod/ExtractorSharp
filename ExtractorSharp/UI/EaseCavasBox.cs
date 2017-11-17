using ExtractorSharp.Config;
using ExtractorSharp.Core;
using ExtractorSharp.Data;
using ExtractorSharp.Draw;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.UI {
    class EaseCavasBox : PictureBox {
        private IConfig Config => Program.Config;
        private Drawer Drawer => Program.Drawer;
        private Controller Controller => Program.Controller;
        private Dictionary<string, ConfigValue> Properties => Drawer.Properties;
        private Language Language => Language.Default;
        private Layer CurrentLayer {
            set => Drawer.CurrentLayer = value;
            get => Drawer.CurrentLayer;
        }
        private Layer LastLayer {
            set => Drawer.LastLayer = value;
            get => Drawer.LastLayer;
        }

        public Bitmap BackImage;
        public Color BackBoxColor;
        public EaseCavasBox() {
        }

        protected void OnPain(PaintEventArgs e) {
            var g = e.Graphics;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            var backmode = Properties["DisplayBackColor"].Integer;
            if (backmode == 0) {
                BackColor = BackBoxColor;
            } else if (backmode == 1 && BackImage != null) {
                BackgroundImage = BackImage;
            }
            var entity = Controller.SelectImage;//获得当前选择的贴图
            var pos = CurrentLayer.Location;
            if (Properties["MutipleLayer"].Boolean && entity?.Picture != null) {
                if (entity.Type == ColorBits.LINK && entity.Target != null)
                    entity = entity.Target;
                var pictrue = entity.Picture;
                if (pictrue == null)
                    return;
                var size = entity.Size.Star(Properties["CavasScale"].Decimal);
                if (Properties["LinearDodge"].Boolean)
                    pictrue = pictrue.LinearDodge();
                if (Properties["Onionskin"].Boolean) {
                    LastLayer?.Draw(g);
                }
                CurrentLayer.Tag = entity;
                CurrentLayer.Size = size;//校正当前图层的宽高
                CurrentLayer.Image = pictrue;//校正贴图
                CurrentLayer.Draw(g);//绘制贴图
            } else {//多图层模式
                Drawer.DrawLayer(g);
            }
            if (Properties["DisplayRule"].Boolean) {//显示标尺
                var rule_point = Properties["rulePoint"].Location;
                if (!Properties["LockRule"].Boolean) {
                    Properties["RuleRealPoint"] = new ConfigValue(rule_point.Add(pos));
                }
                var rp = Properties["RuleRealPoint"].Location;
                var rule_radius = Properties["RuleRadius"].Integer;
                g.DrawString(Language["AbsolutePosition"] + ":" + rp.GetString(), DefaultFont, Brushes.White, new Point(rp.X + rule_radius, rp.Y - rule_radius - DefaultFont.Height));
                g.DrawString(Language["RealativePosition"] + ":" + rule_point.Reverse().GetString(), DefaultFont, Brushes.White, new Point(rp.X + rule_radius, rp.Y - rule_radius - DefaultFont.Height * 2));
                g.DrawLine(Pens.White, new Point(rp.X, 0), new Point(rp.X, Height));
                g.DrawLine(Pens.White, new Point(0, rp.Y), new Point(Width, rp.Y));
                if (Properties["DisplayRuleCrossHair"].Boolean) {
                    var x = rp.X - rule_radius;
                    var y = rp.Y - rule_radius;
                    g.DrawEllipse(Pens.WhiteSmoke, x, y, rule_radius * 2, rule_radius * 2);
                }
            }
            if (Properties["DisplayGridGap"].Boolean) {//显示网格
                var grap = Config["GridGap"].Integer;
                for (var i = 0; i < Width || i < Height; i += grap) {
                    if (i < Width)
                        g.DrawLine(Pens.White, new Point(i, 0), new Point(i, Height));
                    if (i < Height)
                        g.DrawLine(Pens.White, new Point(0, i), new Point(Width, i));
                }
            }
        }
    }
}
