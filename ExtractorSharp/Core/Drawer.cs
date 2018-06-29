using System.Collections.Generic;
using System.Drawing;
using ExtractorSharp.Core.Config;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Draw.Paint;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Draw.Brush;
using ExtractorSharp.Draw.Paint;
using ExtractorSharp.EventArguments;

namespace ExtractorSharp.Core {
    /// <summary>
    ///     绘制器
    ///     控制着画布显示的所有内容
    /// </summary>
    internal class Drawer {
        private Color _color = Color.White;

        private bool _lastLayerVisible;


        public Drawer() {
            Properties = new Dictionary<string, ConfigValue>();
            Brushes = new Dictionary<string, IBrush>();
            this["MoveTool"] = new MoveTool();
            this["Straw"] = new Straw();
            this["Eraser"] = new Eraser();
            this["Pencil"] = new Pencil();
            Brush = this["MoveTool"];
            LayerList = new List<IPaint> {
                new Canvas {Name = "LastLayer"},
                new Canvas {Name = "CurrentLayer"}
            };
            CurrentLayer.Visible = true;
            FlashList = new List<List<IPaint>> {LayerList};
        }

        /// <summary>
        ///     画笔集
        /// </summary>
        public Dictionary<string, IBrush> Brushes { get; }

        public List<List<IPaint>> FlashList { get; }
        public List<IPaint> LayerList { get; private set; }
        public int Count => FlashList.Count;
        public int CustomLayerCount { set; get; }
        public decimal ImageScale { set; get; }


        public List<CompareLayer> CompareLayers {
            get {
                var list = new List<CompareLayer>();
                foreach(var layer in LayerList) {
                    if(layer is CompareLayer compareLayer) {
                        list.Add(compareLayer);
                    }
                }
                return list;
            }
        }


        public Canvas CurrentLayer {
            set {
                var lastVisible = LastLayer.Visible;
                var lastRealPosition = LastLayer.RealPosition;

                var curPoint = CurrentLayer.Location;
                var curVisible = CurrentLayer.Visible;
                var curRealPosition = CurrentLayer.RealPosition;

                LayerList[0] = LayerList[1]; //图层更新
                LayerList[0].Location = curPoint;
                LayerList[0].Name = "LastLayer";
                LayerList[0].Visible = lastVisible;
                (LayerList[0] as Canvas).RealPosition = lastRealPosition;
                LayerList[1] = value;
                LayerList[1].Location = curPoint;
                LayerList[1].Name = "CurrentLayer";
                LayerList[1].Visible = curVisible;
                (LayerList[1] as Canvas).RealPosition = curRealPosition;
            }
            get => LayerList[1] as Canvas;
        }

        public Canvas LastLayer {
            set => LayerList[0] = value;
            get => LayerList[0] as Canvas;
        }

        public bool LastLayerVisible {
            set {
                if (LayerList[0].Visible != value) {
                    LayerList[0].Visible = value;
                    _lastLayerVisible = value;
                    OnLayerVisibleChanged(new LayerEventArgs {
                        ChangedIndex = 0
                    });
                }
            }
            get => LayerList[0].Visible;
        }


        public Dictionary<string, ConfigValue> Properties { get; }

        /// <summary>
        ///     当前选择的画笔<see cref="IBrush" />
        /// </summary>
        public IBrush Brush { set; get; }

        public Point CusorLocation {
            set => Brush.Location = value;
            get => Brush.Location;
        }

        public Color Color {
            set {
                ColorChanged?.Invoke(this, new ColorEventArgs {
                    OldColor = Color,
                    NewColor = value
                });
                _color = value;
            }
            get => _color;
        }

        public IBrush this[string key] {
            get => Select(key);
            set {
                if (Brushes.ContainsKey(key)) Brushes.Remove(key);
                Brushes.Add(key, value);
            }
        }


        public IBrush Select(string key) {
            if (key != null && Brushes.ContainsKey(key)) {
                Brush = Brushes[key]; //切换画笔
                OnBrushChanged(new DrawEventArgs {
                    Brush = Brush
                });
            }

            return Brush;
        }

        public void AddLayer(params IPaint[] array) {
            LayerList.AddRange(array);
            OnLayerChanged(new LayerEventArgs());
        }

        public void MoveLayer(int soureIndex, int targetIndex) {
            var list = LayerList;
            var temp = list[soureIndex];
            list[soureIndex] = list[targetIndex];
            list[targetIndex] = temp;
            OnLayerChanged(new LayerEventArgs());
        }


        public void ReplaceLayer(params Sprite[] array) { }

        public int IndexOfLayer(Point point) {
            for (var i = LayerList.Count - 1; i > -1; i--) {
                var layer = LayerList[i];
                if (!layer.Visible || layer.Locked) {
                    continue;
                }
                if (LayerList[i].Contains(point)) {
                    return i;
                }
            }

            return -1;
        }

        public void DrawLayer(Graphics g) {
            OnLayerDrawing(new LayerEventArgs());
            for (var i = LayerList.Count - 1; i > 0; i--) {
                if (LayerList[i].Visible) {
                    LayerList[i].Draw(g);
                }
            }
        }

        public void TabLayer(int index) {
            while (index >= Count) {
                FlashList.Add(new List<IPaint>());
            }
            LayerList = FlashList[index];
        }

        public bool IsSelect(string name) {
            if (Brushes.ContainsKey(name)) {
                return Brushes[name] == Brush;
            }
            return false;
        }


        #region event

        public delegate void FileHandler(object sender, FileEventArgs e);

        public event FileHandler PalatteChanged;

        public void OnPalatteChanged(FileEventArgs e) {
            PalatteChanged?.Invoke(this, e);
        }

        public delegate void DrawHandler(object sender, DrawEventArgs e);

        public event DrawHandler BrushChanged;

        private void OnBrushChanged(DrawEventArgs e) {
            BrushChanged?.Invoke(this, e);
        }

        public delegate void ColorHandler(object sender, ColorEventArgs e);

        public event ColorHandler ColorChanged;

        public delegate void LayerHandler(object sender, LayerEventArgs e);

        public event LayerHandler LayerChanged;
        public event LayerHandler LayerVisibleChanged;

        public void OnLayerChanged(LayerEventArgs e) {
            LayerChanged?.Invoke(this, e);
        }

        public void OnLayerVisibleChanged(LayerEventArgs e) {
            LayerVisibleChanged?.Invoke(this, e);
        }

        public event LayerHandler LayerDrawing;

        public void OnLayerDrawing(LayerEventArgs e) {
            LayerDrawing?.Invoke(this, e);
        }

        #endregion
    }
}