using System;
using System.Drawing;
using System.IO;

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
 * Class AnimatedGifEncoder - Encodes a GIF file consisting of one or
 * more frames.
 * <pre>
 * Example:
 *    AnimatedGifEncoder e = new AnimatedGifEncoder();
 *    e.start(outputFileName);
 *    e.setDelay(1000);   // 1 frame per sec
 *    e.addFrame(image1);
 *    e.addFrame(image2);
 *    e.finish();
 * </pre>
 * No copyright asserted on the source code of this class.  May be used
 * for any purpose, however, refer to the Unisys LZW patent for restrictions
 * on use of the associated LZWEncoder class.  Please forward any corrections
 * to kweiner@fmsware.com.
 *
 * @author Kevin Weiner, FM Software
 * @version 1.03 November 2003
 *
 */

#endregion

#region Modified By Phil Garcia

/* 
 * Modified by Phil Garcia (phil@thinkedge.com) 
	1. Add support to output the Gif to a MemoryStream (9/2/2005)
 
*/

#endregion

namespace ExtractorSharp.Core.Coder.Gif {
    public class AnimatedGifEncoder {
        protected bool closeStream; // close stream when finished
        protected int colorDepth; // number of bit planes
        protected byte[] colorTab; // RGB palette
        protected int delay; // frame delay (hundredths)
        protected int dispose = -1; // disposal code (-1 = use default)
        protected bool firstFrame = true;

        protected int height;
        //		protected FileStream fs;

        protected Image image; // current frame

        protected byte[] indexedPixels; // converted frame indexed to palette

        //	protected BinaryWriter bw;
        protected MemoryStream ms;
        protected int palSize = 7; // color table size (bits-1)
        protected byte[] pixels; // BGR byte array from frame
        protected int repeat = -1; // no repeat
        protected int sample = 10; // default sample interval for quantizer
        protected bool sizeSet; // if false, get size from first frame
        protected bool started; // ready to output frames
        protected int transIndex; // transparent index in color table
        protected Color transparent = Color.Empty; // transparent color if given
        protected bool[] usedEntry = new bool[256]; // active palette entries
        protected int width; // image size

        /**
		 * Sets the delay time between each frame, or changes it
		 * for subsequent frames (applies to last frame added).
		 *
		 * @param ms int delay time in milliseconds
		 */
        public void SetDelay(int ms) {
            this.delay = (int)Math.Round(ms / 10.0f);
        }

        /**
		 * Sets the GIF frame disposal code for the last added frame
		 * and any subsequent frames.  Default is 0 if no transparent
		 * color has been set, otherwise 2.
		 * @param code int disposal code.
		 */
        public void SetDispose(int code) {
            if(code >= 0) {
                this.dispose = code;
            }
        }

        /**
		 * Sets the number of times the set of GIF frames
		 * should be played.  Default is 1; 0 means play
		 * indefinitely.  Must be invoked before the first
		 * image is added.
		 *
		 * @param iter int number of iterations.
		 * @return
		 */
        public void SetRepeat(int iter) {
            if(iter >= 0) {
                this.repeat = iter;
            }
        }

        /**
		 * Sets the transparent color for the last added frame
		 * and any subsequent frames.
		 * Since all colors are subject to modification
		 * in the quantization process, the color in the final
		 * palette for each frame closest to the given color
		 * becomes the transparent color for that frame.
		 * May be set to null to indicate no transparent color.
		 *
		 * @param c Color to be treated as transparent on display.
		 */
        public void SetTransparent(Color c) {
            this.transparent = c;
        }

        /**
		 * Adds next GIF frame.  The frame is not written immediately, but is
		 * actually deferred until the next frame is received so that timing
		 * data can be inserted.  Invoking <code>finish()</code> flushes all
		 * frames.  If <code>setSize</code> was not invoked, the size of the
		 * first image is used for all subsequent frames.
		 *
		 * @param im BufferedImage containing frame to write.
		 * @return true if successful.
		 */
        public bool AddFrame(Image im) {
            if(im == null || !this.started) {
                return false;
            }

            var ok = true;
            try {
                if(!this.sizeSet) {
                    this.SetSize(im.Width, im.Height);
                }

                this.image = im;
                this.GetImagePixels(); // convert to correct format if necessary
                this.AnalyzePixels(); // build color table & map pixels
                if(this.firstFrame) {
                    this.WriteLSD(); // logical screen descriptior
                    this.WritePalette(); // global color table
                    if(this.repeat >= 0) {
                        this.WriteNetscapeExt();
                    }
                }
                this.WriteGraphicCtrlExt(); // write graphic control extension
                this.WriteImageDesc(); // image descriptor
                if(!this.firstFrame) {
                    this.WritePalette(); // local color table
                }

                this.WritePixels(); // encode and write pixel data
                this.firstFrame = false;
            } catch(IOException) {
                ok = false;
            }

            return ok;
        }

        /**
		 * Flushes any pending data and closes output file.
		 * If writing to an OutputStream, the stream is not
		 * closed.
		 */
        public bool Finish() {
            if(!this.started) {
                return false;
            }
            var ok = true;
            this.started = false;
            try {
                this.ms.WriteByte(0x3b); // gif trailer
                this.ms.Flush();
                if(this.closeStream) {
                    //					ms.Close();
                }
            } catch(IOException) {
                ok = false;
            }

            // reset for subsequent use
            this.transIndex = 0;
            //			fs = null;
            this.image = null;
            this.pixels = null;
            this.indexedPixels = null;
            this.colorTab = null;
            this.closeStream = false;
            this.firstFrame = true;

            return ok;
        }

        /**
		 * Sets frame rate in frames per second.  Equivalent to
		 * <code>setDelay(1000/fps)</code>.
		 *
		 * @param fps float frame rate (frames per second)
		 */
        public void SetFrameRate(float fps) {
            if(fps != 0f) {
                this.delay = (int)Math.Round(100f / fps);
            }
        }

        /**
		 * Sets quality of color quantization (conversion of images
		 * to the maximum 256 colors allowed by the GIF specification).
		 * Lower values (minimum = 1) produce better colors, but slow
		 * processing significantly.  10 is the default, and produces
		 * good color mapping at reasonable speeds.  Values greater
		 * than 20 do not yield significant improvements in speed.
		 *
		 * @param quality int greater than 0.
		 * @return
		 */
        public void SetQuality(int quality) {
            if(quality < 1) {
                quality = 1;
            }

            this.sample = quality;
        }

        /**
		 * Sets the GIF frame size.  The default size is the
		 * size of the first frame added if this method is
		 * not invoked.
		 *
		 * @param w int frame width.
		 * @param h int frame width.
		 */
        public void SetSize(int w, int h) {
            if(this.started && !this.firstFrame) {
                return;
            }

            this.width = w;
            this.height = h;
            if(this.width < 1) {
                this.width = 320;
            }

            if(this.height < 1) {
                this.height = 240;
            }

            this.sizeSet = true;
        }

        /**
		 * Initiates GIF file creation on the given stream.  The stream
		 * is not closed automatically.
		 *
		 * @param os OutputStream on which GIF images are written.
		 * @return false if initial write failed.
		 */

        public bool Start(MemoryStream os) {
            if(os == null) {
                return false;
            }

            var ok = true;
            this.closeStream = false;
            this.ms = os;
            try {
                this.WriteString("GIF89a"); // header
            } catch(IOException) {
                ok = false;
            }
            return this.started = ok;
        }

        /**
		 * Initiates writing of a GIF file to a memory stream.
		 *
		 * @return false if open or initial write failed.
		 */
        public bool Start() {
            var ok = true;
            try {
                ok = this.Start(new MemoryStream(10 * 1024));
                this.closeStream = true;
            } catch(IOException) {
                ok = false;
            }
            return this.started = ok;
        }

        /**
		 * Initiates writing of a GIF file with the specified name.
		 *
		 * @return false if open or initial write failed.
		 */
        public bool Output(string file) {
            try {
                var fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                fs.Write(this.ms.ToArray(), 0, (int)this.ms.Length);
                fs.Close();
            } catch(IOException) {
                return false;
            }
            return true;
        }

        public MemoryStream Output() {
            return this.ms;
        }

        /**
		 * Analyzes image colors and creates color map.
		 */
        protected void AnalyzePixels() {
            var len = this.pixels.Length;
            var nPix = len / 3;
            this.indexedPixels = new byte[nPix];
            var nq = new NeuQuant(this.pixels, len, this.sample);
            // initialize quantizer
            this.colorTab = nq.Process(); // create reduced palette
            // convert map from BGR to RGB
            //			for (int i = 0; i < colorTab.Length; i += 3) 
            //			{
            //				byte temp = colorTab[i];
            //				colorTab[i] = colorTab[i + 2];
            //				colorTab[i + 2] = temp;
            //				usedEntry[i / 3] = false;
            //			}
            // map image pixels to new palette
            var k = 0;
            for(var i = 0; i < nPix; i++) {
                var index =
                    nq.Map(this.pixels[k++] & 0xff,
                        this.pixels[k++] & 0xff,
                        this.pixels[k++] & 0xff);
                this.usedEntry[index] = true;
                this.indexedPixels[i] = (byte)index;
            }
            this.pixels = null;
            this.colorDepth = 8;
            this.palSize = 7;
            // get closest match to transparent color if specified
            if(this.transparent != Color.Empty) {
                this.transIndex = nq.Map(this.transparent.B, this.transparent.G, this.transparent.R);
            }
        }

        /**
		 * Returns index of palette color closest to c
		 *
		 */
        protected int FindClosest(Color c) {
            if(this.colorTab == null) {
                return -1;
            }

            int r = c.R;
            int g = c.G;
            int b = c.B;
            var minpos = 0;
            var dmin = 256 * 256 * 256;
            var len = this.colorTab.Length;
            for(var i = 0; i < len;) {
                var dr = r - (this.colorTab[i++] & 0xff);
                var dg = g - (this.colorTab[i++] & 0xff);
                var db = b - (this.colorTab[i] & 0xff);
                var d = dr * dr + dg * dg + db * db;
                var index = i / 3;
                if(this.usedEntry[index] && d < dmin) {
                    dmin = d;
                    minpos = index;
                }
                i++;
            }
            return minpos;
        }

        /**
		 * Extracts image pixels into byte array "pixels"
		 */
        protected void GetImagePixels() {
            var w = this.image.Width;
            var h = this.image.Height;
            //		int type = image.GetType().;
            if(w != this.width
                || h != this.height
            ) {
                // create new image with right size/format
                Image temp =
                    new Bitmap(this.width, this.height);
                var g = Graphics.FromImage(temp);
                g.DrawImage(this.image, 0, 0);
                this.image = temp;
                g.Dispose();
            }
            /*
				ToDo:
				improve performance: use unsafe code 
			*/
            this.pixels = new byte[3 * this.image.Width * this.image.Height];
            var tempBitmap = new Bitmap(this.image);
            var data = tempBitmap.ToArray();
            var len = this.image.Width * this.image.Height;
            for(var i = 0; i < len; i++) {
                this.pixels[i * 3 + 0] = data[i * 4 + 2];
                this.pixels[i * 3 + 1] = data[i * 4 + 1];
                this.pixels[i * 3 + 2] = data[i * 4 + 0];
            }

            //		pixels = ((DataBufferByte) image.getRaster().getDataBuffer()).getData();
        }

        /**
		 * Writes Graphic Control Extension
		 */
        protected void WriteGraphicCtrlExt() {
            this.ms.WriteByte(0x21); // extension introducer
            this.ms.WriteByte(0xf9); // GCE label
            this.ms.WriteByte(4); // data block size
            int transp, disp;
            if(this.transparent == Color.Empty) {
                transp = 0;
                disp = 0; // dispose = no action
            } else {
                transp = 1;
                disp = 2; // force clear if using transparent color
            }
            if(this.dispose >= 0) {
                disp = this.dispose & 7; // user override
            }

            disp <<= 2;

            // packed fields
            this.ms.WriteByte(Convert.ToByte(0 | // 1:3 reserved
                                        disp | // 4:6 disposal
                                        0 | // 7   user input - 0 = none
                                        transp)); // 8   transparency flag

            this.WriteShort(this.delay); // delay x 1/100 sec
            this.ms.WriteByte(Convert.ToByte(this.transIndex)); // transparent color index
            this.ms.WriteByte(0); // block terminator
        }

        /**
		 * Writes Image Descriptor
		 */
        protected void WriteImageDesc() {
            this.ms.WriteByte(0x2c); // image separator
            this.WriteShort(0); // image position x,y = 0,0
            this.WriteShort(0);
            this.WriteShort(this.width); // image size
            this.WriteShort(this.height);
            // packed fields
            if(this.firstFrame) {
                this.ms.WriteByte(0);
            } else {
                this.ms.WriteByte(Convert.ToByte(0x80 | // 1 local color table  1=yes
                                            0 | // 2 interlace - 0=no
                                            0 | // 3 sorted - 0=no
                                            0 | // 4-5 reserved
                                            this.palSize)); // 6-8 size of color table
            }
        }

        /**
		 * Writes Logical Screen Descriptor
		 */
        protected void WriteLSD() {
            // logical screen size
            this.WriteShort(this.width);
            this.WriteShort(this.height);
            // packed fields
            this.ms.WriteByte(Convert.ToByte(0x80 | // 1   : global color table flag = 1 (gct used)
                                        0x70 | // 2-4 : color resolution = 7
                                        0x00 | // 5   : gct sort flag = 0
                                        this.palSize)); // 6-8 : gct size

            this.ms.WriteByte(0); // background color index
            this.ms.WriteByte(0); // pixel aspect ratio - assume 1:1
        }

        /**
		 * Writes Netscape application extension to define
		 * repeat count.
		 */
        protected void WriteNetscapeExt() {
            this.ms.WriteByte(0x21); // extension introducer
            this.ms.WriteByte(0xff); // app extension label
            this.ms.WriteByte(11); // block size
            this.WriteString("NETSCAPE" + "2.0"); // app id + auth code
            this.ms.WriteByte(3); // sub-block size
            this.ms.WriteByte(1); // loop sub-block id
            this.WriteShort(this.repeat); // loop count (extra iterations, 0=repeat forever)
            this.ms.WriteByte(0); // block terminator
        }

        /**
		 * Writes color table
		 */
        protected void WritePalette() {
            this.ms.Write(this.colorTab, 0, this.colorTab.Length);
            var n = 3 * 256 - this.colorTab.Length;
            for(var i = 0; i < n; i++) {
                this.ms.WriteByte(0);
            }
        }

        /**
		 * Encodes and writes pixel data
		 */
        protected void WritePixels() {
            var encoder =
                new LZWEncoder(this.width, this.height, this.indexedPixels, this.colorDepth);
            encoder.Encode(this.ms);
        }

        /**
		 *    Write 16-bit value to output stream, LSB first
		 */
        protected void WriteShort(int value) {
            this.ms.WriteByte(Convert.ToByte(value & 0xff));
            this.ms.WriteByte(Convert.ToByte((value >> 8) & 0xff));
        }

        /**
		 * Writes string to output stream
		 */
        protected void WriteString(string s) {
            var chars = s.ToCharArray();
            for(var i = 0; i < chars.Length; i++) {
                this.ms.WriteByte((byte)chars[i]);
            }
        }
    }
}