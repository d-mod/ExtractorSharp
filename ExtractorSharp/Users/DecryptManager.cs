using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Users {
    static class DecryptManager {
        private static Dictionary<Encrypt_Version, IDecrypter> Decryptes;

        static DecryptManager() {
            Decryptes = new Dictionary<Encrypt_Version, IDecrypter>();
            Decryptes.Add(Encrypt_Version.Ver2, new SecondDecrypter());
        }


        public static Work Decrypt(Encrypt_Version version, byte[] data) {
            if (Decryptes.ContainsKey(version)) 
                return Decryptes[version].Decrypt(data);           
            return Work.CreateDefaultWork();
        }

        public static void Encrypt(Stream stream, Work work) {
            if (Decryptes.ContainsKey(work.Version)) 
                Decryptes[work.Version].Encrypt(stream, work);           
        } 
    }
}
