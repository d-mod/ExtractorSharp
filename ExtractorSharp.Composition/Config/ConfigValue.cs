using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace ExtractorSharp.Composition.Config {
    /// <summary>
    ///     设置的值
    /// </summary>
    public class ConfigValue {
        public ConfigValue(object obj) {
            if(obj is Color c) {
                obj = $"{c.R},{c.G},{c.B},{c.A}";
            }
            this.Object = obj;
        }

        /// <summary>
        ///     空值
        /// </summary>
        public static readonly ConfigValue NullValue = new ConfigValue(null);

        /// <summary>
        ///     值
        /// </summary>
        public string Value => this.Object?.ToString();

        public object Object { get; }

        /// <summary>
        ///     int型
        /// </summary>
        public int Integer {
            get {
                if(this.Object is int i) {
                    return i;
                }
                int.TryParse(this.Value, out var rs);
                return rs;
            }
        }

        /// <summary>
        ///     double型
        /// </summary>
        public decimal Decimal {
            get {
                if(this.Object is decimal d) {
                    return d;
                }
                decimal.TryParse(this.Value, out var rs);
                return rs;
            }
        }

        /// <summary>
        ///     布尔型
        /// </summary>
        public bool Boolean {
            get {
                if(this.Object is bool bl) {
                    return bl;
                }
                if(this.Value != null) {
                    bool.TryParse(this.Value, out var rs);
                    return rs;
                }
                return false;
            }
        }

        /// <summary>
        ///     时间
        /// </summary>
        public DateTime DateTime {
            get {
                if(this.Object is DateTime time) {
                    return time;
                }
                DateTime.TryParse(this.Value, out var rs);
                return rs;
            }
        }

        public Color Color {
            get {
                if(this.Object is Color color) {
                    return color;
                }
                if(this.Value != null && this.Value.Length > 0) {
                    if(this.Value.StartsWith("#")) {
                        var argb = int.Parse(this.Value.Substring(1), NumberStyles.AllowHexSpecifier);
                        return Color.FromArgb(argb);
                    }
                    var arr = this.Value.Split(",");
                    if(arr.Length >= 3) {
                        int.TryParse(arr[0], out var r);
                        int.TryParse(arr[1], out var g);
                        int.TryParse(arr[2], out var b);
                        if(arr.Length > 3) {
                            int.TryParse(arr[3], out var a);
                            return Color.FromArgb(a, r, g, b);
                        }
                        return Color.FromArgb(r, g, b);
                    }
                    return Color.FromName(this.Value);
                }
                return Color.Empty;
            }
        }

        public Point Location {
            get {
                if(this.Object is Point point) {
                    return point;
                }
                if(this.Value != null && this.Value.Contains(",")) {
                    var arr = this.Value.Split(",");
                    var x = int.Parse(arr[0]);
                    var y = int.Parse(arr[1]);
                    return new Point(x, y);
                }
                return Point.Empty;
            }
        }

        public Size Size {
            get {
                if(this.Object is Size size) {
                    return size;
                }
                if(this.Value != null && this.Value.Contains(",")) {
                    var arr = this.Value.Split(",");
                    var width = int.Parse(arr[0]);
                    var height = int.Parse(arr[1]);
                    return new Size(width, height);
                }
                return Size.Empty;
            }
        }


        public Image Image {
            get {
                if(this.Object is Image image) {
                    return image;
                }
                if(string.IsNullOrEmpty(this.Value)) {
                    return null;
                }
                if(File.Exists(this.Value)) {
                    return Image.FromFile(this.Value);
                }
                var request = WebRequest.CreateHttp(this.Value);
                using(var response = request.GetResponse())
                using(var os = response.GetResponseStream()) {
                    return Image.FromStream(os ?? throw new InvalidOperationException());
                }
            }
        }

        public Guid Guid {
            get {
                if(this.Object is Guid guid) {
                    return guid;
                }
                if(this.Value != null && Guid.TryParse(this.Value, out guid)) {
                    return guid;
                }
                return Guid.Empty;
            }
        }

        public bool Saveable { set; get; } = true;

        public bool NotEmpty => !string.IsNullOrEmpty(this.Value);

        public override string ToString() {
            return this.Value;
        }

        public override bool Equals(object obj) {
            return this.Value != null && this.Value.Equals(obj?.ToString());
        }

        public override int GetHashCode() {
            return this.Value == null ? 0 : this.Value.GetHashCode();
        }
    }
}