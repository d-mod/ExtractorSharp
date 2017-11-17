using ExtractorSharp.Draw;
using System.Collections.Generic;
using ExtractorSharp.Draw.Brush;
using ExtractorSharp.EventArguments;
using System;
using System.Drawing;
using ExtractorSharp.Config;
using ExtractorSharp.Data;

namespace ExtractorSharp.Core {
    /// <summary>
    /// 绘制器
    /// 控制着画布显示的所有内容
    /// </summary>
    class Drawer {
        /// <summary>
        /// 画笔集
        /// </summary>
        public Dictionary<string, IBrush> Brushes { get; }
        public List<List<Layer>> FlashList { get; }
        public List<Layer> LayerList { get; private set; }
        public int Count => FlashList.Count;

        public Layer CurrentLayer;

        public Layer LastLayer;

        public Dictionary<string,ConfigValue> Properties { get; }

        /// <summary>
        /// 当前选择的画笔<see cref="IBrush"/>
        /// </summary>
        public IBrush Brush { set; get; }

        public Point CusorLocation { set => Brush.Location = value; get => Brush.Location; }

        private Color _color=Color.White;
        public Color Color {
            set {
                var args = new ColorEventArgs();
                args.OldColor = this.Color;
                args.NewColor = value;
                ColorChanged?.Invoke(this, args);
                this._color = value;
            }
            get => _color;
        }

        public IBrush this[string key] {
            get {
                return Select(key);
            }
            set {
                if (Brushes.ContainsKey(key)) {
                    Brushes.Remove(key);
                }
                Brushes.Add(key, value);
            }
        }


#region event
        public delegate void ImageHandler(object sender, ImageEntityEventArgs e);
        public event ImageHandler ImageChanged;
        public void OnImageChanged(ImageEntityEventArgs e) => ImageChanged(this, e);

        public delegate void DrawHandler(object sender, DrawEventArgs e);

        public event DrawHandler BrushChanged;
        private void OnBrushChanged(DrawEventArgs e) => BrushChanged?.Invoke(this, e);

        public delegate void ColorChangeHandler(object sender, ColorEventArgs e);
        public event ColorChangeHandler ColorChanged;

#endregion


        public IBrush Select(string key) {
            if (Brushes.ContainsKey(key)) {
                Brush = Brushes[key];//切换画笔
                var arg = new DrawEventArgs();
                arg.Brush = Brush;
                OnBrushChanged(arg);
            }
            return Brush;
        }

        public void AddLayer(params Layer[] array) {
            LayerList.AddRange(array);
        }

        public void AddLayer(params ImageEntity[] array) {
            var list = new List<Layer>();
            foreach (var entity in array) {
                var isContains = false;
                foreach (Layer item in LayerList) {
                    if (item.Index == entity.Index) {
                        item.Replace(entity);
                        isContains = true;
                        break;
                    }
                }
                if (!isContains)
                    list.Add(Layer.CreateFrom(entity));
            }
            AddLayer(list.ToArray());
        }

        public void ReplaceLayer(params ImageEntity[] array) {
            foreach (Layer layer in LayerList)
                for (int j = 0; j < array.Length; j++)
                    if (layer.Index == array[j].Index)
                        layer.Replace(array[j]);
        }

        public int IndexOfLayer(Point point) {
            for (var i = LayerList.Count - 1; i > -1; i--)
                if (LayerList[i].Contains(point))
                    return i + 2;
            return -1;
        }

        public void DrawLayer(Graphics g) {
            LayerList.ForEach(l => l.Draw(g));
        }

        public void TabLayer(int index) {
            while (index >= Count)
                FlashList.Add(new List<Layer>());
            LayerList = FlashList[index];
        }

        public bool IsSelect(string name) {
            if (Brushes.ContainsKey(name)) {
                return Brushes[name] == Brush;
            }
            return false; 
        }
        


        public Drawer() {
            Properties = new Dictionary<string, ConfigValue>();
            Brushes = new Dictionary<string, IBrush>();
            this["MoveTool"] = new MoveTool();
            this["Straw"] = new Straw();
           // this["Eraser"] = new Eraser();
            this["Pencil"] = new Pencil();
            Brush = this["MoveTool"];

            FlashList = new List<List<Layer>>();
            LayerList = new List<Layer>();
            FlashList.Add(LayerList);
        }
    }
}
