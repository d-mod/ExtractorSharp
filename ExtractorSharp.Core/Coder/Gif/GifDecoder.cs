#region .NET Disclaimer/Info

//===============================================================================
//
// gOODiDEA, uland.com
//===============================================================================
//
// $Header :		$  
// $Author :		$
// $Date   :		$
// $Revision:		$
// $History:		$  
//  
//===============================================================================

#endregion

#region Java

/**
 * Class GifDecoder - Decodes a GIF file into one or more frames.
 * <br><pre>
 * Example:
 *    GifDecoder d = new GifDecoder();
 *    d.read("sample.gif");
 *    int n = d.getFrameCount();
 *    for (int i = 0; i < n; i++) {
 *       BufferedImage frame = d.getFrame(i);  // frame i
 *       int t = d.getDelay(i);  // display duration of frame in milliseconds
 *       // do something with frame
 *    }
 * </pre>
 * No copyright asserted on the source code of this class.  May be used for
 * any purpose, however, refer to the Unisys LZW patent for any additional
 * restrictions.  Please forward any corrections to kweiner@fmsware.com.
 *
 * @author Kevin Weiner, FM Software; LZW decoder adapted from John Cristy's ImageMagick.
 * @version 1.03 November 2003
 *
 */

#endregion

using System;
using System.Collections;
using System.Drawing;
using System.IO;

namespace ExtractorSharp.Core.Coder.Gif {
    public class GifDecoder {
        /**
         * File read status: No errors.
         */
        public static readonly int STATUS_OK = 0;

        /**
         * File read status: Error decoding file (may be partially decoded)
         */
        public static readonly int STATUS_FORMAT_ERROR = 1;

        /**
         * File read status: Unable to open source.
         */
        public static readonly int STATUS_OPEN_ERROR = 2;

        protected static readonly int MaxStackSize = 4096;
        protected int[] act; // active color table
        protected int bgColor; // background color

        protected int bgIndex; // background color index
        protected Bitmap bitmap;

        protected byte[] block = new byte[256]; // current data block
        protected int blockSize; // block size
        protected int delay; // delay in milliseconds

        // last graphic control extension info
        protected int dispose;
        protected int frameCount;

        protected ArrayList frames; // frames read from current file

        protected int[] gct; // global color table
        protected bool gctFlag; // global color table used
        protected int gctSize; // size of global color table
        protected int height; // full image height
        protected Image image; // current frame

        protected Stream inStream;
        protected bool interlace; // interlace flag

        protected int ix, iy, iw, ih; // current image rectangle

        protected int lastBgColor; // previous bg color

        // 0=no action; 1=leave in place; 2=restore to bg; 3=restore to prev
        protected int lastDispose;
        protected Image lastImage; // previous frame
        protected Rectangle lastRect; // last image rect
        protected int[] lct; // local color table

        protected bool lctFlag; // local color table flag
        protected int lctSize; // local color table size
        protected int loopCount = 1; // iterations; 0 = repeat forever
        protected int pixelAspect; // pixel aspect ratio
        protected byte[] pixels;

        protected byte[] pixelStack;
        // max decoder pixel stack size

        // LZW decoder working arrays
        protected short[] prefix;
        protected int status;
        protected byte[] suffix;
        protected int transIndex; // transparent color index
        protected bool transparency; // use transparent color

        protected int width; // full image width

        /**
         * Gets display duration for specified frame.
         *
         * @param n int index of frame
         * @return delay in milliseconds
         */
        public int GetDelay(int n) {
            //
            this.delay = -1;
            if(n >= 0 && n < this.frameCount) {
                this.delay = ((GifFrame)this.frames[n]).delay;
            }

            return this.delay;
        }

        /**
         * Gets the number of frames read from file.
         * @return frame count
         */
        public int GetFrameCount() {
            return this.frameCount;
        }

        /**
         * Gets the first (or only) image read.
         *
         * @return BufferedImage containing first frame, or null if none.
         */
        public Image GetImage() {
            return this.GetFrame(0);
        }

        /**
         * Gets the "Netscape" iteration count, if any.
         * A count of 0 means repeat indefinitiely.
         *
         * @return iteration count if one was specified, else 1.
         */
        public int GetLoopCount() {
            return this.loopCount;
        }

        /**
         * Creates new frame image from current data (and previous
         * frames as specified by their disposition codes).
         */
        private int[] GetPixels(Bitmap bitmap) {
            var data = bitmap.ToArray();
            var len = this.image.Width * this.image.Height;
            var pixels = new int[4 * len];
            for(var i = 0; i < len; i++) {
                pixels[i * 3 + 0] = data[i * 4 + 0];
                pixels[i * 3 + 1] = data[i * 4 + 1];
                pixels[i * 3 + 2] = data[i * 4 + 2];
            }
            return pixels;
        }

        private void SetPixels(int[] pixels) {
            var len = this.image.Width * this.image.Height;
            var data = new byte[len * 4];
            for(var i = 0; i < len; i++) {
                var pixel = pixels[i];
                data[i * 4 + 0] = (byte)(pixel & 0xff);
                data[i * 4 + 1] = (byte)((pixel >> 8) & 0xff);
                data[i * 4 + 2] = (byte)((pixel >> 16) & 0xff);
                data[i * 4 + 3] = (byte)(pixel >> 24);
            }
            this.bitmap = Bitmaps.FromArray(data, this.image.Size);
        }

        protected void SetPixels() {
            // expose destination image's pixels as int array
            //		int[] dest =
            //			(( int ) image.getRaster().getDataBuffer()).getData();
            var dest = this.GetPixels(this.bitmap);

            // fill in starting image contents based on last image's dispose code
            if(this.lastDispose > 0) {
                if(this.lastDispose == 3) {
                    // use image before last
                    var n = this.frameCount - 2;
                    if(n > 0) {
                        this.lastImage = this.GetFrame(n - 1);
                    } else {
                        this.lastImage = null;
                    }
                }

                if(this.lastImage != null) {
                    //				int[] prev =
                    //					((DataBufferInt) lastImage.getRaster().getDataBuffer()).getData();
                    var prev = this.GetPixels(new Bitmap(this.lastImage));
                    Array.Copy(prev, 0, dest, 0, this.width * this.height);
                    // copy pixels

                    if(this.lastDispose == 2) {
                        // fill last image rect area with background color
                        var g = Graphics.FromImage(this.image);
                        var c = Color.Empty;
                        if(this.transparency) {
                            c = Color.FromArgb(0, 0, 0, 0); // assume background is transparent
                        } else {
                            c = Color.FromArgb(this.lastBgColor);
                        }
                        var brush = new SolidBrush(c);
                        g.FillRectangle(brush, this.lastRect);
                        brush.Dispose();
                        g.Dispose();
                    }
                }
            }

            // copy each source line to the appropriate place in the destination
            var pass = 1;
            var inc = 8;
            var iline = 0;
            for(var i = 0; i < this.ih; i++) {
                var line = i;
                if(this.interlace) {
                    if(iline >= this.ih) {
                        pass++;
                        switch(pass) {
                            case 2:
                                iline = 4;
                                break;
                            case 3:
                                iline = 2;
                                inc = 4;
                                break;
                            case 4:
                                iline = 1;
                                inc = 2;
                                break;
                        }
                    }
                    line = iline;
                    iline += inc;
                }
                line += this.iy;
                if(line < this.height) {
                    var k = line * this.width;
                    var dx = k + this.ix; // start of line in dest
                    var dlim = dx + this.iw; // end of dest line
                    if(k + this.width < dlim) {
                        dlim = k + this.width; // past dest edge
                    }

                    var sx = i * this.iw; // start of line in source
                    while(dx < dlim) {
                        // map color and insert in destination
                        var index = this.pixels[sx++] & 0xff;
                        var c = this.act[index];
                        if(c != 0) {
                            dest[dx] = c;
                        }

                        dx++;
                    }
                }
            }
            this.SetPixels(dest);
        }

        /**
         * Gets the image contents of frame n.
         *
         * @return BufferedImage representation of frame, or null if n is invalid.
         */
        public Image GetFrame(int n) {
            Image im = null;
            if(n >= 0 && n < this.frameCount) {
                im = ((GifFrame)this.frames[n]).image;
            }

            return im;
        }

        /**
         * Gets image size.
         *
         * @return GIF image dimensions
         */
        public Size GetFrameSize() {
            return new Size(this.width, this.height);
        }

        /**
         * Reads GIF image from stream
         *
         * @param BufferedInputStream containing GIF file.
         * @return read status code (0 = no errors)
         */
        public int Read(Stream inStream) {
            this.Init();
            if(inStream != null) {
                this.inStream = inStream;
                this.ReadHeader();
                if(!this.Error()) {
                    this.ReadContents();
                    if(this.frameCount < 0) {
                        this.status = STATUS_FORMAT_ERROR;
                    }
                }
                inStream.Close();
            } else {
                this.status = STATUS_OPEN_ERROR;
            }
            return this.status;
        }

        /**
         * Reads GIF file from specified file/URL source  
         * (URL assumed if name contains ":/" or "file:")
         *
         * @param name String containing source
         * @return read status code (0 = no errors)
         */
        public int Read(string name) {
            this.status = STATUS_OK;
            try {
                name = name.Trim().ToLower();
                this.status = this.Read(new FileInfo(name).OpenRead());
            } catch(IOException) {
                this.status = STATUS_OPEN_ERROR;
            }

            return this.status;
        }

        /**
         * Decodes LZW image data into pixel array.
         * Adapted from John Cristy's ImageMagick.
         */
        protected void DecodeImageData() {
            var NullCode = -1;
            var npix = this.iw * this.ih;
            int available,
                clear,
                code_mask,
                code_size,
                end_of_information,
                in_code,
                old_code,
                bits,
                code,
                count,
                i,
                datum,
                data_size,
                first,
                top,
                bi,
                pi;

            if(this.pixels == null || this.pixels.Length < npix) {
                this.pixels = new byte[npix]; // allocate new pixel array
            }

            if(this.prefix == null) {
                this.prefix = new short[MaxStackSize];
            }

            if(this.suffix == null) {
                this.suffix = new byte[MaxStackSize];
            }

            if(this.pixelStack == null) {
                this.pixelStack = new byte[MaxStackSize + 1];
            }

            //  Initialize GIF data stream decoder.

            data_size = this.Read();
            clear = 1 << data_size;
            end_of_information = clear + 1;
            available = clear + 2;
            old_code = NullCode;
            code_size = data_size + 1;
            code_mask = (1 << code_size) - 1;
            for(code = 0; code < clear; code++) {
                this.prefix[code] = 0;
                this.suffix[code] = (byte)code;
            }

            //  Decode GIF pixel stream.

            datum = bits = count = first = top = pi = bi = 0;

            for(i = 0; i < npix;) {
                if(top == 0) {
                    if(bits < code_size) {
                        //  Load bytes until there are enough bits for a code.
                        if(count == 0) {
                            // Read a new data block.
                            count = this.ReadBlock();
                            if(count <= 0) {
                                break;
                            }
                            bi = 0;
                        }
                        datum += (this.block[bi] & 0xff) << bits;
                        bits += 8;
                        bi++;
                        count--;
                        continue;
                    }

                    //  Get the next code.

                    code = datum & code_mask;
                    datum >>= code_size;
                    bits -= code_size;

                    //  Interpret the code

                    if(code > available || code == end_of_information) {
                        break;
                    }
                    if(code == clear) {
                        //  Reset decoder.
                        code_size = data_size + 1;
                        code_mask = (1 << code_size) - 1;
                        available = clear + 2;
                        old_code = NullCode;
                        continue;
                    }
                    if(old_code == NullCode) {
                        this.pixelStack[top++] = this.suffix[code];
                        old_code = code;
                        first = code;
                        continue;
                    }
                    in_code = code;
                    if(code == available) {
                        this.pixelStack[top++] = (byte)first;
                        code = old_code;
                    }
                    while(code > clear) {
                        this.pixelStack[top++] = this.suffix[code];
                        code = this.prefix[code];
                    }
                    first = this.suffix[code] & 0xff;

                    //  Add a new string to the string table,

                    if(available >= MaxStackSize) {
                        break;
                    }
                    this.pixelStack[top++] = (byte)first;
                    this.prefix[available] = (short)old_code;
                    this.suffix[available] = (byte)first;
                    available++;
                    if((available & code_mask) == 0
                        && available < MaxStackSize) {
                        code_size++;
                        code_mask += available;
                    }
                    old_code = in_code;
                }

                //  Pop a pixel off the pixel stack.

                top--;
                this.pixels[pi++] = this.pixelStack[top];
                i++;
            }

            for(i = pi; i < npix; i++) {
                this.pixels[i] = 0; // clear missing pixels
            }
        }

        /**
         * Returns true if an error was encountered during reading/decoding
         */
        protected bool Error() {
            return this.status != STATUS_OK;
        }

        /**
         * Initializes or re-initializes reader
         */
        protected void Init() {
            this.status = STATUS_OK;
            this.frameCount = 0;
            this.frames = new ArrayList();
            this.gct = null;
            this.lct = null;
        }

        /**
         * Reads a single byte from the input stream.
         */
        protected int Read() {
            var curByte = 0;
            try {
                curByte = this.inStream.ReadByte();
            } catch(IOException) {
                this.status = STATUS_FORMAT_ERROR;
            }
            return curByte;
        }

        /**
         * Reads next variable length block from input.
         *
         * @return number of bytes stored in "buffer"
         */
        protected int ReadBlock() {
            this.blockSize = this.Read();
            var n = 0;
            if(this.blockSize > 0) {
                try {
                    var count = 0;
                    while(n < this.blockSize) {
                        count = this.inStream.Read(this.block, n, this.blockSize - n);
                        if(count == -1) {
                            break;
                        }
                        n += count;
                    }
                } catch(IOException) { }

                if(n < this.blockSize) {
                    this.status = STATUS_FORMAT_ERROR;
                }
            }
            return n;
        }

        /**
         * Reads color table as 256 RGB integer values
         *
         * @param ncolors int number of colors to read
         * @return int array containing 256 colors (packed ARGB with full alpha)
         */
        protected int[] ReadColorTable(int ncolors) {
            var nbytes = 3 * ncolors;
            int[] tab = null;
            var c = new byte[nbytes];
            var n = 0;
            try {
                n = this.inStream.Read(c, 0, c.Length);
            } catch(IOException) { }
            if(n < nbytes) {
                this.status = STATUS_FORMAT_ERROR;
            } else {
                tab = new int[256]; // max size to avoid bounds checks
                var i = 0;
                var j = 0;
                while(i < ncolors) {
                    var r = c[j++] & 0xff;
                    var g = c[j++] & 0xff;
                    var b = c[j++] & 0xff;
                    tab[i++] = (0xff << 24) | (r << 16) | (g << 8) | b;
                }
            }
            return tab;
        }

        /**
         * Main file parser.  Reads GIF content blocks.
         */
        protected void ReadContents() {
            // read GIF file content blocks
            var done = false;
            while(!(done || this.Error())) {
                var code = this.Read();
                switch(code) {
                    case 0x2C: // image separator
                        this.ReadImage();
                        break;

                    case 0x21: // extension
                        code = this.Read();
                        switch(code) {
                            case 0xf9: // graphics control extension
                                this.ReadGraphicControlExt();
                                break;

                            case 0xff: // application extension
                                this.ReadBlock();
                                var app = "";
                                for(var i = 0; i < 11; i++) {
                                    app += (char)this.block[i];
                                }

                                if(app.Equals("NETSCAPE2.0")) {
                                    this.ReadNetscapeExt();
                                } else {
                                    this.Skip(); // don't care
                                }
                                break;

                            default: // uninteresting extension
                                this.Skip();
                                break;
                        }
                        break;

                    case 0x3b: // terminator
                        done = true;
                        break;

                    case 0x00: // bad byte, but keep going and see what happens
                        break;

                    default:
                        this.status = STATUS_FORMAT_ERROR;
                        break;
                }
            }
        }

        /**
         * Reads Graphics Control Extension values
         */
        protected void ReadGraphicControlExt() {
            this.Read(); // block size
            var packed = this.Read(); // packed fields
            this.dispose = (packed & 0x1c) >> 2; // disposal method
            if(this.dispose == 0) {
                this.dispose = 1; // elect to keep old image if discretionary
            }

            this.transparency = (packed & 1) != 0;
            this.delay = this.ReadShort() * 10; // delay in milliseconds
            this.transIndex = this.Read(); // transparent color index
            this.Read(); // block terminator
        }

        /**
         * Reads GIF file header information.
         */
        protected void ReadHeader() {
            var id = "";
            for(var i = 0; i < 6; i++) {
                id += (char)this.Read();
            }

            if(!id.StartsWith("GIF")) {
                this.status = STATUS_FORMAT_ERROR;
                return;
            }

            this.ReadLSD();
            if(this.gctFlag && !this.Error()) {
                this.gct = this.ReadColorTable(this.gctSize);
                this.bgColor = this.gct[this.bgIndex];
            }
        }

        /**
         * Reads next frame image
         */
        protected void ReadImage() {
            this.ix = this.ReadShort(); // (sub)image position & size
            this.iy = this.ReadShort();
            this.iw = this.ReadShort();
            this.ih = this.ReadShort();

            var packed = this.Read();
            this.lctFlag = (packed & 0x80) != 0; // 1 - local color table flag
            this.interlace = (packed & 0x40) != 0; // 2 - interlace flag
            // 3 - sort flag
            // 4-5 - reserved
            this.lctSize = 2 << (packed & 7); // 6-8 - local color table size

            if(this.lctFlag) {
                this.lct = this.ReadColorTable(this.lctSize); // read table
                this.act = this.lct; // make local table active
            } else {
                this.act = this.gct; // make global table active
                if(this.bgIndex == this.transIndex) {
                    this.bgColor = 0;
                }
            }
            var save = 0;
            if(this.transparency) {
                save = this.act[this.transIndex];
                this.act[this.transIndex] = 0; // set transparent color if specified
            }

            if(this.act == null) {
                this.status = STATUS_FORMAT_ERROR; // no color table defined
            }

            if(this.Error()) {
                return;
            }

            this.DecodeImageData(); // decode pixel data
            this.Skip();

            if(this.Error()) {
                return;
            }

            this.frameCount++;

            // create new image to receive frame data
            //		image =
            //			new BufferedImage(width, height, BufferedImage.TYPE_INT_ARGB_PRE);

            this.bitmap = new Bitmap(this.width, this.height);
            this.image = this.bitmap;
            this.SetPixels(); // transfer pixel data to image

            this.frames.Add(new GifFrame(this.bitmap, this.delay)); // add image to frame list

            if(this.transparency) {
                this.act[this.transIndex] = save;
            }

            this.ResetFrame();
        }

        /**
         * Reads Logical Screen Descriptor
         */
        protected void ReadLSD() {
            // logical screen size
            this.width = this.ReadShort();
            this.height = this.ReadShort();

            // packed fields
            var packed = this.Read();
            this.gctFlag = (packed & 0x80) != 0; // 1   : global color table flag
            // 2-4 : color resolution
            // 5   : gct sort flag
            this.gctSize = 2 << (packed & 7); // 6-8 : gct size

            this.bgIndex = this.Read(); // background color index
            this.pixelAspect = this.Read(); // pixel aspect ratio
        }

        /**
         * Reads Netscape extenstion to obtain iteration count
         */
        protected void ReadNetscapeExt() {
            do {
                this.ReadBlock();
                if(this.block[0] == 1) {
                    // loop count sub-block
                    var b1 = this.block[1] & 0xff;
                    var b2 = this.block[2] & 0xff;
                    this.loopCount = (b2 << 8) | b1;
                }
            } while(this.blockSize > 0 && !this.Error());
        }

        /**
         * Reads next 16-bit value, LSB first
         */
        protected int ReadShort() {
            // read 16-bit value, LSB first
            return this.Read() | (this.Read() << 8);
        }

        /**
         * Resets frame state for reading next image.
         */
        protected void ResetFrame() {
            this.lastDispose = this.dispose;
            this.lastRect = new Rectangle(this.ix, this.iy, this.iw, this.ih);
            this.lastImage = this.image;
            this.lastBgColor = this.bgColor;
            this.lct = null;
        }

        /**
         * Skips variable length blocks up to and including
         * next zero length block.
         */
        protected void Skip() {
            do {
                this.ReadBlock();
            } while(this.blockSize > 0 && !this.Error());
        }

        public class GifFrame {
            public int delay;
            public Image image;

            public GifFrame(Image im, int del) {
                this.image = im;
                this.delay = del;
            }
        }
    }
}