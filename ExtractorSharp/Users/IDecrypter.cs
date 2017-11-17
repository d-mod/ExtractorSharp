using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Users {
    interface IDecrypter {
        Work Decrypt(byte[] data);

        void Encrypt(Stream stream,Work work);
    }


}
