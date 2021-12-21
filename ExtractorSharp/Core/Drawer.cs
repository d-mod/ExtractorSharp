using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using ExtractorSharp.Composition.Config;
using ExtractorSharp.Composition.Core;
using ExtractorSharp.Core.Draw;
using ExtractorSharp.Core.Draw.Paint;
using ExtractorSharp.Core.Model;
using ExtractorSharp.Draw.Paint;
using ExtractorSharp.EventArguments;

namespace ExtractorSharp.Core {
    /// <summary>
    ///     绘制器
    ///     控制着画布显示的所有内容
    /// </summary>
    /// 
    [Export]
    public class Drawer : IPartImportsSatisfiedNotification {

        private Color _color = Color.White;

        [Import]
        public Store Store { set; get; }

        [Import]
        private Language Language;


        public Drawer() {
            Properties = new Dictionary<string, ConfigValue>();

            LayerList = new List<IPaint> {
                new Canvas {Name = "LastLayer"},
                new Canvas {Name = "CurrentLayer"}
            };

            CurrentLayer.Visible = true;
        }

        /// <summary>
        ///     画笔集
        /// </summary>
        [ImportMany(typeof(IBrush))]
        public List<IBrush> Brushes { set; get; } = new List<IBrush>();

        private List<IPaint> LayerList;

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
                var lastPoint = LastLayer.Location;
                var lastRealPosition = LastLayer.RealPosition;

                var curPoint = CurrentLayer.Location;
                var curVisible = CurrentLayer.Visible;
                var curRealPosition = CurrentLayer.RealPosition;

                LayerList[0] = LayerList[1]; //图层更新
                LayerList[0].Location = lastPoint;
                LayerList[0].Name = "LastLayer";
                LastLayerVisible = lastVisible;
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
                if(LayerList[0].Visible != value) {
                    LayerList[0].Visible = value;
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



        /// <summary>
        /// 根据key选择画笔工具
        /// </summary>
        /// <param name="key">画笔代号</param>
        /// <returns></returns>
        public IBrush Select(string key) {
            IBrush _brush = null;
            if(!string.IsNullOrEmpty(key)) {
                _brush = Brushes?.Find(e => key.Equals(e.Name));
                if(_brush != null) {
                    Brush = _brush; //切换画笔
                    OnBrushChanged(new DrawEventArgs {
                        Brush = _brush
                    });
                }
            }
            return _brush;
        }

        /// <summary>
        /// 添加图层
        /// </summary>
        /// <param name="array"></param>
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
            for(var i = LayerList.Count - 1; i > -1; i--) {
                var layer = LayerList[i];
                if(!layer.Visible || layer.Locked) {
                    continue;
                }
                if(LayerList[i].Contains(point)) {
                    return i;
                }
            }

            return -1;
        }

        public void DrawLayer(Graphics g) {
            OnLayerDrawing(new LayerEventArgs());
            LayerList.ForEach(e => {
                if(e.Visible) {
                    e.Draw(g);
                }
            });
        }

        public void Move(int index, Point point) {
            var layer = LayerList[index];
            //point = GetPoint(layer, point);
            Brush.Draw(layer, point, ImageScale);
            CusorLocation = point;
        }

        public Point GetPoint(IPaint layer, Point point) {
            if(layer is IAttractable att) {
                var range = att.Range;
                var rx = layer.Location.X;
                var ry = layer.Location.Y;
                if(layer is Canvas can) {
                    rx = can.RealLocation.X;
                    ry = can.RealLocation.Y;
                }
                var x = point.X;
                var y = point.Y;
                var x0 = point.X;
                var y0 = point.Y;

                foreach(var paint in LayerList) {
                    if(paint.Equals(layer) || !paint.Visible) {
                        continue;
                    }
                    var x1 = paint.Location.X;
                    var y1 = paint.Location.Y;
                    var width = paint.Size.Width;
                    var heihgt = paint.Size.Height;
                    if(paint is Canvas canvas) {
                        x1 = canvas.RealLocation.X;
                        y1 = canvas.RealLocation.Y;
                    }
                    if(x0 > x1 - range && x0 < x1 + range) {
                        x = x1;
                    }
                    if(x0 > y1 - range && x0 < y1 + range) {
                        y = y1;
                    }
                }
                point = new Point(x, y);
            }
            return point;
        }


        public bool IsSelect(string name) {
            return !string.IsNullOrEmpty(name) && name.Equals(Brush?.Name);
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

        public void Dispose() {
        }

        public void OnImportsSatisfied() {
            Store.Compute<List<IPaint>>("/draw/layers", () => LayerList, _ => LayerList = _)
                .Compute("/draw/current-layer", () => CurrentLayer)
                .Compute("/draw/last-layer", () => LastLayer);
            this.Select("MoveTool");
        }

        #endregion
    }
}