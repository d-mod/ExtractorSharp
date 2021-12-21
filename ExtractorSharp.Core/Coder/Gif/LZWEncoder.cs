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

//==============================================================================
//  Adapted from Jef Poskanzer's Java port by way of J. M. G. Elliott.
//  K Weiner 12/00

#endregion

using System;
using System.IO;

namespace ExtractorSharp.Core.Coder.Gif {
    public class LZWEncoder {
        private static readonly int EOF = -1;

        // GIFCOMPR.C       - GIF Image compression routines
        //
        // Lempel-Ziv compression based on 'compress'.  GIF modifications by
        // David Rowley (mgardi@watdcsu.waterloo.edu)

        // General DEFINEs

        private static readonly int BITS = 12;

        private static readonly int HSIZE = 5003; // 80% occupancy

        // Define the storage for the packet accumulator
        private readonly byte[] accum = new byte[256];
        private readonly int[] codetab = new int[HSIZE];

        private readonly int hsize = HSIZE; // for dynamic table sizing

        private readonly int[] htab = new int[HSIZE];
        private readonly int imgH;

        private readonly int imgW;
        private readonly int initCodeSize;

        private readonly int[] masks = {
            0x0000,
            0x0001,
            0x0003,
            0x0007,
            0x000F,
            0x001F,
            0x003F,
            0x007F,
            0x00FF,
            0x01FF,
            0x03FF,
            0x07FF,
            0x0FFF,
            0x1FFF,
            0x3FFF,
            0x7FFF,
            0xFFFF
        };

        private readonly int maxbits = BITS; // user settable max # bits/code
        private readonly int maxmaxcode = 1 << BITS; // should NEVER generate this code
        private readonly byte[] pixAry;

        // Number of characters so far in this 'packet'
        private int a_count;

        // block compression parameters -- after all codes are used up,
        // and compression rate changes, start over.
        private bool clear_flg;

        private int ClearCode;

        // output
        //
        // Output the given code.
        // Inputs:
        //      code:   A n_bits-bit integer.  If == -1, then EOF.  This assumes
        //              that n_bits =< wordsize - 1.
        // Outputs:
        //      Outputs code to the file.
        // Assumptions:
        //      Chars are 8 bits long.
        // Algorithm:
        //      Maintain a BITS character long buffer (so that 8 codes will
        // fit in it exactly).  Use the VAX insv instruction to insert each
        // code in turn.  When the buffer fills up empty it and start over.

        private int cur_accum;
        private int cur_bits;
        private int curPixel;
        private int EOFCode;

        private int free_ent; // first unused entry

        // Algorithm:  use open addressing double hashing (no chaining) on the
        // prefix code / next character combination.  We do a variant of Knuth's
        // algorithm D (vol. 3, sec. 6.4) along with G. Knott's relatively-prime
        // secondary probe.  Here, the modular division first probe is gives way
        // to a faster exclusive-or manipulation.  Also do block compression with
        // an adaptive reset, whereby the code table is cleared when the compression
        // ratio decreases, but after the table fills.  The variable-length output
        // codes are re-sized at this point, and a special CLEAR code is generated
        // for the decompressor.  Late addition:  construct the table according to
        // file size for noticeable speed improvement on small files.  Please direct
        // questions about this implementation to ames!jaw.

        private int g_init_bits;
        private int maxcode; // maximum code, given n_bits

        // GIF Image compression - modified 'compress'
        //
        // Based on: compress.c - File compression ala IEEE Computer, June 1984.
        //
        // By Authors:  Spencer W. Thomas      (decvax!harpo!utah-cs!utah-gr!thomas)
        //              Jim McKie              (decvax!mcvax!jim)
        //              Steve Davies           (decvax!vax135!petsd!peora!srd)
        //              Ken Turkowski          (decvax!decwrl!turtlevax!ken)
        //              James A. Woods         (decvax!ihnp4!ames!jaw)
        //              Joe Orost              (decvax!vax135!petsd!joe)

        private int n_bits; // number of bits/code
        private int remaining;

        //----------------------------------------------------------------------------
        public LZWEncoder(int width, int height, byte[] pixels, int color_depth) {
            this.imgW = width;
            this.imgH = height;
            this.pixAry = pixels;
            this.initCodeSize = Math.Max(2, color_depth);
        }

        // Add a character to the end of the current packet, and if it is 254
        // characters, flush the packet to disk.
        private void Add(byte c, Stream outs) {
            this.accum[this.a_count++] = c;
            if(this.a_count > 254) {
                this.Flush(outs);
            }
        }

        // Clear out the hash table

        // table clear for block compress
        private void ClearTable(Stream outs) {
            this.ResetCodeTable(this.hsize);
            this.free_ent = this.ClearCode + 2;
            this.clear_flg = true;

            this.Output(this.ClearCode, outs);
        }

        // reset code table
        private void ResetCodeTable(int hsize) {
            for(var i = 0; i < hsize; ++i) {
                this.htab[i] = -1;
            }
        }

        private void Compress(int init_bits, Stream outs) {
            int fcode;
            int i /* = 0 */;
            int c;
            int ent;
            int disp;
            int hsize_reg;
            int hshift;

            // Set up the globals:  g_init_bits - initial number of bits
            this.g_init_bits = init_bits;

            // Set up the necessary values
            this.clear_flg = false;
            this.n_bits = this.g_init_bits;
            this.maxcode = this.MaxCode(this.n_bits);

            this.ClearCode = 1 << (init_bits - 1);
            this.EOFCode = this.ClearCode + 1;
            this.free_ent = this.ClearCode + 2;

            this.a_count = 0; // clear packet

            ent = this.NextPixel();

            hshift = 0;
            for(fcode = this.hsize; fcode < 65536; fcode *= 2) {
                ++hshift;
            }
            hshift = 8 - hshift; // set hash code range bound

            hsize_reg = this.hsize;
            this.ResetCodeTable(hsize_reg); // clear hash table

            this.Output(this.ClearCode, outs);

        outer_loop:
            while((c = this.NextPixel()) != EOF) {
                fcode = (c << this.maxbits) + ent;
                i = (c << hshift) ^ ent; // xor hashing

                if(this.htab[i] == fcode) {
                    ent = this.codetab[i];
                    continue;
                }
                if(this.htab[i] >= 0) // non-empty slot
                {
                    disp = hsize_reg - i; // secondary hash (after G. Knott)
                    if(i == 0) {
                        disp = 1;
                    }
                    do {
                        if((i -= disp) < 0) {
                            i += hsize_reg;
                        }

                        if(this.htab[i] == fcode) {
                            ent = this.codetab[i];
                            goto outer_loop;
                        }
                    } while(this.htab[i] >= 0);
                }
                this.Output(ent, outs);
                ent = c;
                if(this.free_ent < this.maxmaxcode) {
                    this.codetab[i] = this.free_ent++; // code -> hashtable
                    this.htab[i] = fcode;
                } else {
                    this.ClearTable(outs);
                }
            }
            // Put out the final code.
            this.Output(ent, outs);
            this.Output(this.EOFCode, outs);
        }

        //----------------------------------------------------------------------------
        public void Encode(Stream os) {
            os.WriteByte(Convert.ToByte(this.initCodeSize)); // write "initial code size" byte

            this.remaining = this.imgW * this.imgH; // reset navigation variables
            this.curPixel = 0;

            this.Compress(this.initCodeSize + 1, os); // compress and write the pixel data

            os.WriteByte(0); // write block terminator
        }

        // Flush the packet to disk, and reset the accumulator
        private void Flush(Stream outs) {
            if(this.a_count > 0) {
                outs.WriteByte(Convert.ToByte(this.a_count));
                outs.Write(this.accum, 0, this.a_count);
                this.a_count = 0;
            }
        }

        private int MaxCode(int n_bits) {
            return (1 << n_bits) - 1;
        }

        //----------------------------------------------------------------------------
        // Return the next pixel from the image
        //----------------------------------------------------------------------------
        private int NextPixel() {
            var upperBound = this.pixAry.GetUpperBound(0);

            return this.curPixel <= upperBound ? this.pixAry[this.curPixel++] & 0xff : EOF;
        }

        private void Output(int code, Stream outs) {
            this.cur_accum &= this.masks[this.cur_bits];

            if(this.cur_bits > 0) {
                this.cur_accum |= code << this.cur_bits;
            } else {
                this.cur_accum = code;
            }

            this.cur_bits += this.n_bits;

            while(this.cur_bits >= 8) {
                this.Add((byte)(this.cur_accum & 0xff), outs);
                this.cur_accum >>= 8;
                this.cur_bits -= 8;
            }

            // If the next entry is going to be too big for the code size,
            // then increase it, if possible.
            if(this.free_ent > this.maxcode || this.clear_flg) {
                if(this.clear_flg) {
                    this.maxcode = this.MaxCode(this.n_bits = this.g_init_bits);
                    this.clear_flg = false;
                } else {
                    ++this.n_bits;
                    if(this.n_bits == this.maxbits) {
                        this.maxcode = this.maxmaxcode;
                    } else {
                        this.maxcode = this.MaxCode(this.n_bits);
                    }
                }
            }

            if(code == this.EOFCode) {
                // At EOF, write the rest of the buffer.
                while(this.cur_bits > 0) {
                    this.Add((byte)(this.cur_accum & 0xff), outs);
                    this.cur_accum >>= 8;
                    this.cur_bits -= 8;
                }

                this.Flush(outs);
            }
        }
    }
}