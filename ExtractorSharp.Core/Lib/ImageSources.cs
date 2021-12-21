using System.Windows.Media.Imaging;

namespace System.Windows.Media {
    public static class ImageSources {

        public static ImageSource CreateImageSource(byte[] data, int width, int height) {
            var imageSource = BitmapSource.Create(width, height, 0, 0, PixelFormats.Bgra32, null, data, 0);
            return null;
        }

    }
}
