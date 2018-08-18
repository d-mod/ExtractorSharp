using System.Drawing;

namespace ExtractorSharp.Core.Lib {
    public static class Drawings {
        public static string GetString(this Size size) {
            return $"[{size.Width},{size.Height}]";
        }

        public static Size Star(this Size size, decimal step) {
            var width = (int) (size.Width * step);
            var height = (int) (size.Height * step);
            return new Size(width, height);
        }


        #region Point拓展        

        public static string GetString(this Point point) {
            return $"[{point.X},{point.Y}]";
        }

        public static Point Star(this Point point, decimal step) {
            var x = (int) (point.X * step);
            var y = (int) (point.Y * step);
            return new Point(x, y);
        }

        public static Point Add(this Point p1, Point p2) {
            var x = p1.X + p2.X;
            var y = p1.Y + p2.Y;
            return new Point(x, y);
        }

        public static Point Divide(this Point point, decimal step) {
            var x = (int) (point.X / step);
            var y = (int) (point.Y / step);
            return new Point(x, y);
        }

        public static Point Minus(this Point p1, Point p2) {
            var x = p1.X - p2.X;
            var y = p1.Y - p2.Y;
            return new Point(x, y);
        }

        public static Point Reverse(this Point point) {
            return new Point(-point.X, -point.Y);
        }

        public static Rectangle Add(this Rectangle rect1, Rectangle rect2) {
            var x = rect1.X + rect2.X;
            var y = rect1.X + rect2.Y;
            var w = rect1.Width + rect2.Width;
            var h = rect1.Height + rect2.Height;
            return new Rectangle(x, y, w, h);
        }

        #endregion
    }
}