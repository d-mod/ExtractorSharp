namespace ExtractorSharp.Core.Coder {
    public enum DdsFormat {
        RgbS3TcDxt1Format = 33776,
        RgbaS3TcDxt3Format = 33778,
        RgbaS3TcDxt5Format = 33779
    }

    public class DdsTexture {
        public int Length { get; set; }
        public int Flags { get; set; }
        public int Width { get; set; } = 4;
        public int Height { get; set; } = 4;
        public int Count { set; get; } = 1;
        public DdsFormat Format { set; get; }
        public bool IsCubemap { get; internal set; }
        public DdsMipmap[] DdsMipmaps { get; internal set; }
        public int Pitch { get; internal set; }
        public int Depth { get; internal set; }
        public byte[] Reverse { set; get; } = new byte[11];
        public int PixelFormatSize { get; internal set; }
    }

    public class DdsMipmap {
        public int Width { set; get; }
        public int Height { set; get; }
        public byte[] Data { set; get; }
    }
}