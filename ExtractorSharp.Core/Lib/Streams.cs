using System.Text;

namespace System.IO {
    public static class Streams {
        #region 基本语法糖

        public static int Read(this Stream stream, byte[] buf) {
            return stream.Read(buf, 0, buf.Length);
        }

        public static int Read(this Stream stream, int length, out byte[] buf) {
            buf = new byte[length];
            return stream.Read(buf, 0, length);
        }

        public static byte[] Read(this Stream stream, int length) {
            var buf = new byte[length];
            stream.Read(buf, 0, length);
            return buf;
        }

        public static void Write(this Stream stream, byte[] buf) {
            stream.Write(buf, 0, buf.Length);
        }


        public static void Seek(this Stream stream, long offset) {
            stream.Seek(offset, SeekOrigin.Current);
        }

        #endregion

        #region 基本类型拓展

        /// <summary>
        ///     读取一个int
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static int ReadInt(this Stream stream) {
            stream.Read(4, out var buf);
            return BitConverter.ToInt32(buf, 0);
        }

        public static uint ReadUInt(this Stream stream) {
            stream.Read(4, out var buf);
            return BitConverter.ToUInt32(buf, 0);
        }

        /// <summary>
        ///     写入一个int
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="data"></param>
        public static void WriteInt(this Stream stream, int data) {
            stream.Write(BitConverter.GetBytes(data));
        }

        public static void WriteUInt(this Stream stream, uint data) {
            stream.Write(BitConverter.GetBytes(data));
        }

        public static short ReadShort(this Stream stream) {
            var buf = new byte[2];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToInt16(buf, 0);
        }

        public static ushort ReadUShort(this Stream stream) {
            var buf = new byte[2];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToUInt16(buf, 0);
        }


        public static void WriteShort(this Stream stream, short s) {
            stream.Write(BitConverter.GetBytes(s));
        }


        /// <summary>
        ///     读取一个long
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static long ReadLong(this Stream stream) {
            var buf = new byte[8];
            stream.Read(buf, 0, buf.Length);
            return BitConverter.ToInt64(buf, 0);
        }

        /// <summary>
        ///     写入一个long
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="l"></param>
        public static void WriteLong(this Stream stream, long l) {
            stream.Write(BitConverter.GetBytes(l));
        }

        public static string ReadString(this Stream stream) {
            return ReadString(stream, Encoding.Default);
        }

        public static void WriteString(this Stream stream, string str) {
            WriteString(stream, str, Encoding.Default, true);
        }

        /// <summary>
        ///     读出一个字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadString(this Stream stream, Encoding encoding) {
            var ms = new MemoryStream();
            var j = 0;
            while((j = stream.ReadByte()) != 0 && j != -1) {
                ms.WriteByte((byte)j);
            }
            ms.Close();
            return encoding.GetString(ms.ToArray());
        }

        public static void WriteString(this Stream stream, string str, Encoding encoding) {
            WriteString(stream, str, encoding, true);
        }

        /// <summary>
        ///     写入一个字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="str"></param>
        public static void WriteString(this Stream stream, string str, Encoding encoding, bool split) {
            stream.Write(encoding.GetBytes(str));
            if(split) {
                stream.WriteByte(0);
            }
        }

        public static byte[] ReadToEnd(this Stream stream) {
            var buf = new byte[stream.Length - stream.Position];
            stream.Read(buf, 0, buf.Length);
            return buf;
        }

        public static void ReadToEnd(this Stream stream, out byte[] buf) {
            buf = new byte[stream.Length - stream.Position];
            stream.Read(buf, 0, buf.Length);
        }

        #endregion
    }
}