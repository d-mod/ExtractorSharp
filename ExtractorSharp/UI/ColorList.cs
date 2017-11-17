using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractorSharp.UI {
    public partial class ColorList : ListView {
        public Color[] Colors {
            set {
                SmallImageList.Images.Clear();
                Items.Clear();
                for(var i = 0; i < value.Length; i++) {
                    var item = new ListViewItem();
                    Items.Add(item);
                    SetColor(item, value[i]);
                    item.ImageIndex = i;
                }
            }
        }

        public void SetColor(ListViewItem item, Color color) {
            var image = new Bitmap(30, 30);
            using (var g = Graphics.FromImage(image))
                g.FillRectangle(new SolidBrush(color), new Rectangle(0, 0, 30, 30));
            if (item.Index < SmallImageList.Images.Count) {
                SmallImageList.Images[item.Index] = image;
            } else {
                SmallImageList.Images.Add(image);
            }
            item.Text = ColorTranslator.ToHtml(color);
            item.Tag = color;
        }


        public ColorList() {
            InitializeComponent();
        }


    }
}
