using System.Collections.Generic;

namespace Splatoon2
{
    public class PlayerData
    {
        public int exp;
        public int cash;
        public string name;
        public int level;
        public HashSet<string> ownedGears, ownedWeapons, equippedGears;
        public string equippedWeapon;
    }
}