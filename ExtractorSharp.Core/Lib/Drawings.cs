using System.Drawing;

namespace ExtractorSharp.Core.Lib {
    public static class Drawings {

        public static string GetString(this Size size) => size.Width + "," + size.Height;


        #region Point拓展        
        public static string GetString(this Point point) {
            return point.X + "," + point.Y;
        }

        public static Point Star(this Point Point, decimal step) {
            var x = (int)(Point.X * step);
            var y = (int)(Point.Y * step);
            return new Point(x, y);
        }

        public static Point Add(this Point p1, Point p2) {
            var x = p1.X + p2.X;
            var y = p1.Y + p2.Y;
            return new Point(x, y);
        }

        public static Point Divide(this Point Point, decimal step) {
            var x = (int)(Point.X / step);
            var y = (int)(Point.Y / step);
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

        #endregion
        public static Size Star(this Size Size, decimal step) {
            var width = (int)(Size.Width * step);
            var height = (int)(Size.Height * step);
            return new Size(width, height);
        }

    }
}
