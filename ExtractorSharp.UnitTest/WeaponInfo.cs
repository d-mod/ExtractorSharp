using System.Collections.Generic;

namespace ExtractorSharp.UnitTest {
    public class WeaponInfo {
        public string this[string key] {
            get {
                if (Alias != null && Alias.ContainsKey(key)) return Alias[key];
                if (Weapon.Contains(key)) return key;
                return null;
            }
        }

        public string Name { set; get; }
        public int Conut => Weapon.Count + (Alias == null ? 0 : Alias.Count);

        public bool HasAt { set; get; }
        public List<string> Weapon { set; get; }
        public Dictionary<string, string> Alias { get; set; } = new Dictionary<string, string>();
    }
}