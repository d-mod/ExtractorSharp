
using ExtractorSharp.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExtractorSharp.Users {
    internal class Work {
        static Dictionary<Encrypt_Version, IDecrypter> Map;
        static Work() {
            Map = new Dictionary<Encrypt_Version, IDecrypter>();
        }
        public long Id;
        /// <summary>
        /// 加密版本
        /// </summary>
        public Encrypt_Version Version;
        /// <summary>
        /// 作者
        /// </summary>
        public string Author;
        /// <summary>
        /// 名称
        /// </summary> 
        public string Name;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark;
        /// <summary>
        /// 密钥
        /// </summary>
        public byte[] Key;
        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime Update;
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expire;
        /// <summary>
        /// 能否提取出IMG
        /// </summary>
        public bool CanExtract;
        ///
        ///能否读取贴图
        ///
        public bool CanRead;
        /// <summary>
        /// 是否已经解开
        /// </summary>
        public bool IsDecrypt => isDecrypt;

        private bool isDecrypt;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid {
            get {
                if (Update < Expire)
                    return DateTime.Now < Expire;
                return true;
            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool Decrypt(string pwd) {
            return isDecrypt = Key.Check(Tools.Encrypt(pwd));
        }

        public static Work CreateNewWork(string Name, string AuthorName, string Key, string Remark, DateTime Update, DateTime Expire, bool CanExtract, bool CanRead, params Album[] Array) {
            var Work = new Work();
            Work.Id = DateTime.Now.ToBinary();
            Work.Version = Encrypt_Version.Ver1;
            Work.Name = Name;
            Work.Author = AuthorName;
            Work.Key = Tools.Encrypt(Key);
            Work.Remark = Remark;
            Work.Update = Update;
            Work.Expire = Expire;
            Work.CanExtract = CanExtract;
            Work.CanRead = CanRead;
            Work.isDecrypt = false;
            foreach (var item in Array)
                item.Work = Work;
            return Work;
        }

        public static Work CreateDefaultWork() {
            var Name = "未知";
            var key = new Random().Next().ToString();
            var update = DateTime.Now;
            var expire = DateTime.Now.AddDays(-1);
            return Work.CreateNewWork(Name, Name, key, Name, update, expire, true, false);
        }



        /// <summary>
        /// 模型信息
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return "模型名称:" + Name + "\r\n模型作者:" + Author + "\r\n更新日期:" + Update + "\r\n有效期至:" + Expire + "\r\n可否提取:" + (CanExtract ? "是" : "否") + "\r\n可否查看:" + (CanRead ? "是" : "否") + "\r\n备注:" + Remark + (IsValid ? "" : "\r\n模型已过期");
        }
    }
}
