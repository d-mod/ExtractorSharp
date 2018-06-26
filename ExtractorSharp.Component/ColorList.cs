using System;
using System.Drawing;
using System.Windows.Forms;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.Component {
    public partial class ColorList : ListView {
        public ColorList() {
            InitializeComponent();
        }

        public Color[] Colors {
            get => _colors;
            set {
                if (_colors.Compare(value)) {
                    return;
                }
                _colors = value;
                SmallImageList.Images.Clear();
                Items.Clear();
                for (var i = 0; i < value.Length; i++) {
                    var item = new ListViewItem();
                    Items.Add(item);
                    SetColor(item, value[i]);
                    item.ImageIndex = i;
                }
            }
        }

        private Color[] _colors=new Color[0];

        public void SetColor(ListViewItem item, Color color) {
            var image = new Bitmap(30, 30);
            using (var g = Graphics.FromImage(image)) {
                g.FillRectangle(new SolidBrush(color), new Rectangle(0, 0, 30, 30));
            }
            if (item.Index < SmallImageList.Images.Count) {
                SmallImageList.Images[item.Index] = image;
            } else {
                SmallImageList.Images.Add(image);
            }
            item.Text = ColorTranslator.ToHtml(color);
            item.Tag = color;
        }
    }
}