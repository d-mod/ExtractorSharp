using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorSharp.Users {
    class SecondDecrypter : IDecrypter {
        private byte[] Key {
            get {
                if (_Key != null)
                    return _Key;
                var data = new byte[32];
                var tp = Encoding.UTF8.GetBytes("虾球受,女装Z");
                Array.Copy(tp, data, Math.Min(tp.Length, 32));
                return data;
            }
        }
        private byte[] _Key;
        public Work Decrypt(byte[] data) {
            data = DecryptData(data);
            var ms = new MemoryStream(data);
            var work = new Work();
            work.Id = ms.ReadLong();
            work.CanExtract = ms.ReadByte() == 1;
            work.CanRead = ms.ReadByte() == 1;
            work.Name = ms.ReadString();
            work.Author = ms.ReadString();
            work.Remark = ms.ReadString();
            work.Update = DateTime.FromBinary(ms.ReadLong());
            work.Expire = DateTime.FromBinary(ms.ReadLong());
            var key = new byte[256];
            ms.Read(key);
            Array.Reverse(key);
            work.Key = key;
            return work;
        }

        public byte[] DecryptData(byte[] source) {
            var aes = new AesCryptoServiceProvider();
            aes.Mode = CipherMode.CBC;
            aes.Key = Key;
            aes.Padding = PaddingMode.Zeros;
            aes.IV = new byte[16];
            var diff = source.Length + aes.BlockSize - source.Length % aes.BlockSize;
            var data = new byte[diff];
            Array.Copy(source, data, source.Length);
            data = aes.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
            return data;
        }

        public byte[] EncryptData(byte[] source) {
            var aes = new AesCryptoServiceProvider();
            aes.Mode = CipherMode.CBC;
            aes.Key = Key;
            aes.Padding = PaddingMode.Zeros;
            aes.IV = new byte[16];
            var diff = source.Length + aes.BlockSize - source.Length % aes.BlockSize;
            var data = new byte[diff];
            Array.Copy(source, data, source.Length);
            data = aes.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
            return data;
        }



        public void Encrypt(Stream mm, Work work) {
            var ms = new MemoryStream();
            ms.WriteLong(work.Id);
            ms.WriteByte((byte)(work.CanExtract ? 1 : 0));
            ms.WriteByte((byte)(work.CanRead ? 1 : 0));
            ms.WriteString(work.Name);
            ms.WriteString(work.Author);
            ms.WriteString(work.Remark);
            ms.WriteLong(work.Update.ToBinary());
            ms.WriteLong(work.Expire.ToBinary());
            var key = new byte[work.Key.Length];
            Array.Copy(work.Key, key, key.Length);
            Array.Reverse(key);
            ms.Write(key);
            ms.Close();
            var data=ms.ToArray();
            data = EncryptData(data);
            mm.Write(data);
        }
    }
}
