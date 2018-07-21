using System.Collections.Generic;

namespace ExtractorSharp.UnitTest {
    internal class Profession {
        public string Label;
        public string Name;
        public List<Weapon> Weapons=new List<Weapon>();

        public IEnumerable<string> WeaponNames {
            get {
                foreach (var weapon in Weapons) {
                    yield return weapon.Name;
                }
            }
        }

        public override string ToString() {
            return Name;
        }
    }
}