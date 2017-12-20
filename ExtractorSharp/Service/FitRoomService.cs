using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using ExtractorSharp.Data;
using ExtractorSharp.Loose;
using ExtractorSharp.Properties;

namespace ExtractorSharp.Service {
    public class FitRoomService {
        public List<string> ProfessionList { get; set; }
        public string Profession => ProfessionList[Index];
        private string[] WeaponArray => weapon_info[Profession.RemoveSuffix("_at")].Weapon.ToArray();
        private Dictionary<string, WeaponInfo> weapon_info;
        private Dictionary<string, string> replace_dic;
        public readonly string[] Parts = { "hair", "cap", "face", "neck", "coat", "skin", "belt", "pants", "shoes" };
        private int Index { set; get; }
        public bool Mask { get; set; }
        public bool Ban { get; set; }
        public Album[] Array { get; set; }
        private readonly string RESOUCES_URL = Program.Config["ResourceUrl"].Value;
        private readonly string API_URL = Program.Config["ApiUrl"].Value;


        public FitRoomService() {
            replace_dic = new Dictionary<string, string>();
            var builder = new LSBuilder();
            var obj = builder.ReadJson(Resources.Weapon);
            weapon_info = new Dictionary<string, WeaponInfo>();
            obj.GetValue(ref weapon_info);
            ProfessionList = new List<string>();
            foreach (var entry in weapon_info) {
                var info = entry.Value;
                ProfessionList.Add(entry.Key);
                if (info.HasAt) {
                    ProfessionList.Add(entry.Key + "_at");
                }
                if (info.Alias != null) {
                    foreach (var en in info.Alias) {
                        replace_dic.Add(en.Key, en.Value);
                    }
                }
                info.Name = entry.Key;
                info.Weapon.AddRange(info.Alias.Keys);
            }
        }

        public int SelectProfessionByName(string name) {
            return ProfessionList.FindIndex(item => item.Equals(name));
        }


        public string[] GetWeapon(int index) {
            this.Index = index;
            var profession = ProfessionList[index];
            profession = profession.Replace("_at", "");
            return weapon_info[profession].Weapon.ToArray();
        }

        public void ExtractImg(int[] values, int weaponValue, int wepaonIndex) {
            var list = new List<Album>();
            for (var i = 0; i < Parts.Length; i++) {
                var item = FindImg(Profession, Parts[i], values[i]);
                list.AddRange(item);
            }
            var weapon = FindWeapon(Profession, WeaponArray[wepaonIndex], weaponValue);
            list.AddRange(weapon);
            Array = list.ToArray();
        }

        private IEnumerable<Album> FindImg(string profession, string part, int code) {
            if (code == -1) {
                return new List<Album>();
            }
            profession = profession.Contains("_at") ? profession : profession + "_";
            var name = part;
            if (replace_dic.ContainsKey(name)) {
                name = replace_dic[name];
            }
            string path = $"{ Program.Config["ResourcePath"]}/sprite_character_{ profession }equipment_avatar_{ name }.NPK";
            if (!File.Exists(path)) {
                return new List<Album>();
            }
            var list = Tools.Load(path);
            list = new List<Album>(Tools.Find(list, code.Completed()));   //检索
            part = part.Equals("skin") ? "body" : part;
            return FindImg(list, code, part);
        }

        /// <summary>
        /// 二次过滤
        /// </summary>
        /// <param name="list"></param>
        /// <param name="code"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        private IEnumerable<Album> FindImg(List<Album> list, int code, string part) {
            foreach (var album in list) {//二次过滤
                var s = int.Parse(Regex.Match(album.Name, @"\d+").Value);
                if (!Mask && album.Name.Contains("mask"))//过滤掉染色图层
                    continue;
                if (!Ban && Regex.IsMatch(album.Name, @"\(.*\)+")) //过滤掉和谐图层
                    continue;
                if ((code == s || code / 100 * 100 == s) && album.Name.Contains(part)) {//过滤掉非同部位的
                    album.TableIndex = code % 100;
                    yield return album;
                }
            }
        }


        /// <summary>
        /// 载入贴图
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        internal List<TempImage> LoadImage(int[] values) {
            var list = new List<TempImage>();
            for (var i = 0; i < Parts.Length; i++) {
                if (values[i] == -1)
                    continue;
                var value = values[i].Completed();
                var temp = GetTempImage(Parts[i], value);
                list.AddRange(temp);
            }
            list.Sort((left, right) => left.Index - right.Index);
            return list;
        }

        /// <summary>
        /// 获得临时贴图
        /// </summary>
        /// <param name="profession"></param>
        /// <param name="part"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private TempImage[] GetTempImage(string part, string value) {
            var tempPath = $"temp/{ ProfessionList[Index]}/{part}/{value}.json";
            var builder = new LSBuilder();
            var dir = Path.GetDirectoryName(tempPath);
            if (File.Exists(tempPath)) {
                var obj = builder.Read(tempPath);
                var array = obj.GetValue(typeof(TempImage[])) as TempImage[];
                var exist = true;
                for (var i = 0; i < array.Length; i++) {
                    var p = dir + "/" + array[i].Name;
                    if (!File.Exists(p)) {
                        exist = false;
                        break;
                    }
                    array[i].Image = Image.FromFile(p);
                }
                if (exist) return array;
            } else {
                Directory.CreateDirectory(dir);
            }
            using (var client = new WebClient()) {
                var file = $"{API_URL}/image?profession={Profession}&part={part}&code={value}";
                client.DownloadFile(file, tempPath);
                var o = builder.Read(tempPath);
                var arr = o.GetValue(typeof(TempImage[])) as TempImage[];
                for (var i = 0; i < arr.Length; i++) {
                    var path = $"{dir}/{arr[i].Name}";
                    var uri = $"{RESOUCES_URL}/{Profession}/{part}/{arr[i].Name}";
                    client.DownloadFile(uri, path);
                    arr[i].Image = Image.FromFile(path);
                }
                return arr;
            }
        }

        /// <summary>
        /// 检索武器
        /// </summary>
        /// <param name="prefession"></param>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private IEnumerable<Album> FindWeapon(string prefession, string type, int code) {
            if (code == -1) {
                return new List<Album>();
            }
            if (!prefession.EndsWith("_at")) {
                prefession += "_";
            }
            var path = $"{ Program.Config["ResourcePath"]}/sprite_character_{ prefession }equipment_weapon_{ type }.NPK";
            if (!File.Exists(path)) {
                return new List<Album>();
            }
            var list = Tools.Load(path);
            list = new List<Album>(Tools.Find(list, code.Completed()));
            return FindImg(list, code, type);
        }
    }
}
