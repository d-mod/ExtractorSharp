using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExtractorSharp.Data;
using ExtractorSharp.Loose;
using ExtractorSharp.Properties;

namespace ExtractorSharp.Service {
    public class FitRoomService {
        private List<string> profession_list { get; set; }
        private Dictionary<string, WeaponInfo> weapon_info;
        private Dictionary<string, string> replace_dic;
        private Dictionary<string, string> alias_list;
        private string[] parts = { "hair", "cap", "face", "neck", "coat", "skin", "belt", "pants", "shoes" };
        private int index { set; get; }
        public bool Mask { get; set; }
        public bool Ban { get; set; }
        public Album[] Array { get; set; }
        private  string url = Program.Config["ApiUrl"].Value;


        public string[] Init() {
            replace_dic = new Dictionary<string, string>();
            var builder = new LSBuilder();
            var obj = builder.ReadJson(Resources.Weapon);
            weapon_info = new Dictionary<string, WeaponInfo>();
            obj.GetValue(ref weapon_info);
            profession_list = new List<string>();
            foreach (var entry in weapon_info) {
                var info = entry.Value;
                profession_list.Add(entry.Key);
                if (info.HasAt) {
                    profession_list.Add(entry.Key + "_at");
                }
                if (info.Alias != null) {
                    foreach (var en in info.Alias) {
                        replace_dic.Add(en.Key, en.Value);
                    }
                }
                info.Name = entry.Key;
                info.Weapon.AddRange(info.Alias.Keys);
            }
            alias_list = new Dictionary<string, string>();
            var array = new string[profession_list.Count];
            obj = builder.ReadJson(Resources.Alias_Weapon);
            obj.GetValue(ref alias_list);
            for (var i = 0; i < array.Length; i++) {
                array[i] = alias_list[profession_list[i]];
            }
            return array;
        }

        public int SelectProfessionByName(string name) {
            return profession_list.FindIndex(item => item.Equals(name));
        }

        public string[] GetParts() {
            var array = new string[parts.Length];
            for (var i = 0; i < array.Length; i++) {
                array[i] = alias_list[parts[i]];
            }
            return array;
        }

        public string[] GetWeapon(int index) {
            this.index = index;
            var profession = profession_list[index];
            profession = profession.Replace("_at", "");
            var array = weapon_info[profession].Weapon.ToArray();
            for (var i = 0; i < array.Length; i++) {
                array[i] = alias_list[array[i]];
            }
            return array;
        }

        public void ExtractImg(int[] values, int weaponValue, int wepaonIndex) {
            var list = new List<Album>();
            for (var i = 0; i < parts.Length; i++) {
                var item = FindImg(profession_list[index], parts[i], values[i]);
                list.AddRange(item);
            }
            Array = list.ToArray();
        }

        private IEnumerable<Album> FindImg(string profession, string part, int code) {
            if (code == -1)
                return new List<Album>();
            profession = profession.Contains("_at") ? profession : profession + "_";
            var name = part;
            if (replace_dic.ContainsKey(name))
                name = replace_dic[name];
            string path = $"{ Program.ResourcePath }sprite_character_{ profession }equipment_avatar_{ name }.NPK";
            if (!File.Exists(path))
                return new List<Album>();
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
            var builder = new LSBuilder();
            var list = new List<TempImage>();
            for (var i = 0; i < parts.Length; i++) {
                if (values[i] == -1)
                    continue;
                var value = values[i].Completed();
                var temp = GetTempImage(profession_list[index], parts[i], value);
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
        private TempImage[] GetTempImage(string profession, string part, string value){
            var suffix = $"{profession_list[index]}/{part}/{value}.json";
            var tempPath = "temp/" + suffix;
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
                var file = url + suffix;
                client.DownloadFile(file, tempPath);
                var o = builder.Read(tempPath);
                var arr = o.GetValue(typeof(TempImage[])) as TempImage[];
                for (var i = 0; i < arr.Length; i++) {
                    var path = dir + "/" + arr[i].Name;
                    var uri = url + suffix.Replace(tempPath.GetName(), arr[i].Name);
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
            if (code == -1)
                return new List<Album>();
            var path = $"{ Program.ResourcePath }sprite_character_{ prefession }equipment_weapon_{ type }.NPK";
            if (!File.Exists(path))
                return new List<Album>();
            var list = Tools.Load(path);
            list = new List<Album>(Tools.Find(list, code.Completed()));
            return FindImg(list, code, type);
        }
    }
}
