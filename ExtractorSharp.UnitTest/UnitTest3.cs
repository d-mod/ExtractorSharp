using ExtractorSharp.Data;
using ExtractorSharp.Handle;
using ExtractorSharp.Loose;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExtractorSharp.UnitTest {
    [TestClass]
    public class UnitTest3 {

        public string[] part_array = {"cap","coat","belt","neck","hair","face","skin","pants","shoes" };
        public string[] profession_array ={"swordman","swordman_at","gunner","gunner_at","fighter","fighter_at","mage","mage_at","priest" ,"priest_at","thief","knight", "demoniclancer" };
        [TestMethod]
        public void Test01() {
            var curData=GetData(@"C:\Users\krits\Desktop\sm_acoat.img");
            var lastData = GetData(@"E:\avatar\icon\swordman\coat");
            var curLen = curData.Count;
            var lastLen = lastData.Count;
            foreach(var cur in curData.Keys.ToArray()) {
                var data1 = curData[cur];
                foreach(var last in lastData.Keys) {
                    var data2 = lastData[last];
                    if (Check(data1, data2)) {
                        curData.Remove(cur);
                    }
                }
            }
            foreach(var cur in curData.Keys.ToArray()) { 
                var file = Path.GetFileName(cur);
                File.Copy(cur, $"E:/avatar/icon_new/swordman/coat/{file}", true);
            }
        }


        [TestMethod]
        public void Test02() {
            Program.RegistyHandler();
            foreach (var profession in profession_array) {
                var dir = $"E:/avatar/icon_new_image/{profession}";
                if (Directory.Exists(dir)) {
                    Directory.Delete(dir,true);
                }
                Directory.CreateDirectory(dir);
                foreach (var part in part_array) {
                    var list = Tools.Load($@"D:\地下城与勇士\ImagePacks2\sprite_character_{profession + (profession.Contains("_") ? "" : "_")}equipment_avatar_{part}.NPK");
                    var builder = new LSBuilder();
                    var obj = builder.Get($"http://localhost/api/avatar/icon?profession={profession}&part={part}");
                    var cur_list = new List<string>();
                    foreach (var child in obj) {
                        var code = child["code"].Value.ToString();
                        cur_list.Add(code);
                    }
                    var rs = new List<Album>();
                    foreach (var img in list.ToArray()) {
                        var name = img.Name;
                        var regex = new Regex("\\d+");
                        var match = regex.Match(name);
                        if (match.Success) {
                            var code = match.Value;
                            if (!cur_list.Contains(code)) {
                                rs.Add(img);
                            }
                        }
                    }
                    Tools.WriteNPK($"E:/avatar/icon_new_image/{profession}/{part}.NPK", rs);
                }
            }
        }

        [TestMethod]
        public void Test03() {
            var data = (Image.FromFile("d:/2.png") as Bitmap).ToArray();
            data=RgbToHsv(data);
            var fs = new FileStream("d:/t2.ps", FileMode.Create);
            fs.Write(data);
            fs.Close();
        }

        public byte[] RgbToHsv(byte[] data) {
            for (var i = 0; i < data.Length; i+=4) {
                float min, max, tmp, H, S, V;
                float R = data[i + 0] * 1.0f / 255;
                var G = data[i + 1] * 1.0f / 255;
                var B = data[i + 2] * 1.0f / 255;
                tmp = Math.Min(R, G);
                min = Math.Min(tmp, B);
                tmp = Math.Max(R, G);
                max = Math.Max(tmp, B);
                // H  
                H = 0;
                if (max == min) {
                    H = 0;
                } else if (max == R && G > B) {
                    H = 60 * (G - B) * 1.0f / (max - min) + 0;
                } else if (max == R && G < B) {
                    H = 60 * (G - B) * 1.0f / (max - min) + 360;
                } else if (max == G) {
                    H = 60 * (B - R) * 1.0f / (max - min) + 120;
                } else if (max == B) {
                    H = 60 * (R - G) * 1.0f / (max - min) + 240;
                }
                // S  
                if (max == 0) {
                    S = 0;
                } else {
                    S = (max - min) * 1.0f / max;
                }
                // V  
                V = max;
                S *= 255;
                V *= 255;
                data[i + 0] = (byte)H;
                data[i + 1] = (byte)S;
                data[i + 2] = (byte)V;
            }
            return data;
        }


        public static Dictionary<string,Color[]> GetData(string dir) {
            var data = new Dictionary<string, Color[]>();
            foreach (var file in Directory.GetFiles(dir)) {
                var bmp = Image.FromFile(file) as Bitmap;
                var bmpData = GetPalette(bmp);
                data.Add(file, bmpData);
            }
            return data;
        }


        public static Color[] GetPalette(Bitmap bmp) {
            var data = ToArray(bmp);
            var array = new Color[data.Length / 4];
            for(var i = 0; i < data.Length; i+=4) {
                var r = data[i + 0];
                var g = data[i + 1];
                var b = data[i + 2];
                var a = data[i + 3];
                array[i / 4] = Color.FromArgb(r, g, b, a);
            }
            return array;
        }


        public static bool Check(Color[] color1, Color[] color2) {
            double total = Math.Max(color1.Length, color2.Length);
            double count = 0;
            for (var i = 0; i < color1.Length && i < color2.Length; i++) {
                if (color1[i] == color2[i]) {
                    count++;
                }
            }
            return count / total > 0.32;
        }


        public static byte[] ToArray( Bitmap bmp) {
            var bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var data = new byte[bmp.Width * bmp.Height * 4];
            Marshal.Copy(bmpData.Scan0, data, 0, data.Length);
            bmp.UnlockBits(bmpData);
            return data;
        }
    }
}
