using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using ExtractorSharp.Core.Lib;

namespace ExtractorSharp.Core.Config {
    /// <summary>
    ///     设置的值
    /// </summary>
    public class ConfigValue {
        public ConfigValue(object obj) {
            if (obj is Color c) obj = $"{c.R},{c.G},{c.B},{c.A}";
            Object = obj;
        }

        /// <summary>
        ///     空值
        /// </summary>
        public static ConfigValue NullValue { get; } = new ConfigValue(null);

        /// <summary>
        ///     值
        /// </summary>
        public string Value => Object?.ToString();

        public object Object { get; }

        /// <summary>
        ///     int型
        /// </summary>
        public int Integer {
            get {
                if (Object is int i) {
                    return i;
                }
                int.TryParse(Value, out var rs);
                return rs;
            }
        }

        /// <summary>
        ///     double型
        /// </summary>
        public decimal Decimal {
            get {
                if (Object is decimal d) {
                    return d;
                }
                decimal.TryParse(Value, out var rs);
                return rs;
            }
        }

        /// <summary>
        ///     布尔型
        /// </summary>
        public bool Boolean {
            get {
                if (Object is bool bl) {
                    return bl;
                }
                if (Value != null) {
                    bool.TryParse(Value, out var rs);
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
                if (Object is DateTime time) {
                    return time;
                }
                DateTime.TryParse(Value, out var rs);
                return rs;
            }
        }

        public Color Color {
            get {
                if (Object is Color color) {
                    return color;
                }
                if (Value != null && Value.Length > 0) {
                    if (Value.StartsWith("#")) {
                        var argb = int.Parse(Value.Substring(1), NumberStyles.AllowHexSpecifier);
                        return Color.FromArgb(argb);
                    }
                    var arr = Value.Split(",");
                    if (arr.Length >= 3) {
                        int.TryParse(arr[0], out var r);
                        int.TryParse(arr[1], out var g);
                        int.TryParse(arr[2], out var b);
                        if (arr.Length > 3) {
                            int.TryParse(arr[3], out var a);
                            return Color.FromArgb(a, r, g, b);
                        }
                        return Color.FromArgb(r, g, b);
                    }
                    return Color.FromName(Value);
                }
                return Color.Empty;
            }
        }

        public Point Location {
            get {
                if (Object is Point point) {
                    return point;
                }
                if (Value.Contains(",")) {
                    var arr = Value.Split(",");
                    var x = int.Parse(arr[0]);
                    var y = int.Parse(arr[1]);
                    return new Point(x, y);
                }
                return Point.Empty;
            }
        }

        public Size Size {
            get {
                if (Object is Size size) {
                    return size;
                }
                if (Value.Contains(",")) {
                    var arr = Value.Split(",");
                    var width = int.Parse(arr[0]);
                    var height = int.Parse(arr[1]);
                    return new Size(width, height);
                }
                return Size.Empty;
            }
        }


        public Image Image {
            get {
                if (Object is Image image) {
                    return image;
                }
                if (string.IsNullOrEmpty(Value)) {
                    return null;
                }
                if (File.Exists(Value)) {
                    return Image.FromFile(Value);
                }
                var request = WebRequest.CreateHttp(Value);
                using (var response = request.GetResponse())
                using (var os = response.GetResponseStream()) {
                    return Image.FromStream(os ?? throw new InvalidOperationException());
                }
            }
        }

        public Guid Guid {
            get {
                if (Object is Guid guid) {
                    return guid;
                }
                if (Value != null && Guid.TryParse(Value, out guid)) {
                    return guid;
                }
                return Guid.Empty;
            }
        }

        public bool Saveable { set; get; } = true;

        public bool NotEmpty => !string.IsNullOrEmpty(Value);

        public override string ToString() {
            return Value;
        }

        public override bool Equals(object obj) {
            return Value != null && Value.Equals(obj?.ToString());
        }

        public override int GetHashCode() {
            return Value == null ? 0 : Value.GetHashCode();
        }
    }
}