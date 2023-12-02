using System.Linq;
using System.Collections.Generic;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class DataManager : Singleton<DataManager>
    {
        private int myID = 1234;
        private List<int> maxExp = new List<int> { 0, 1000, 2000 };
        private Dictionary<string, AbilityData> abilityDataDict;
        private Dictionary<string, GearData> gearDataDict;
        private Dictionary<string, WeaponData> weaponDataDict;
        private Dictionary<DECALTYPE, DecalData> decalDataDict;
        private Dictionary<int, PlayerData> playerDataDict;



        public int MyID { get => myID; }
        public Dictionary<int, PlayerData> PlayerDataDict { get => playerDataDict; }
        public Dictionary<string, AbilityData> AbilityDataDict { get => abilityDataDict; }
        public Dictionary<string, GearData> GearDataDict { get => gearDataDict; }
        public Dictionary<string, WeaponData> WeaponDataDict { get => weaponDataDict; }
        public Dictionary<DECALTYPE, DecalData> DecalDataDict { get => decalDataDict; }





        protected override void Awake()
        {
            base.Awake();
        }



        public PlayerData GetMyData()
        {
            return PlayerDataDict[MyID];
        }

        public void AddOwnedItems(string key)
        {
            if (WeaponDataDict.ContainsKey(key)) playerDataDict[MyID].ownedWeapons.Add(key);
            else if (GearDataDict.ContainsKey(key)) playerDataDict[MyID].ownedGears.Add(key);
        }

        public void EquipItem(string key)
        {
            if (WeaponDataDict.ContainsKey(key)) PlayerDataDict[MyID].equippedWeapon = key;
            else if (GearDataDict.ContainsKey(key))
            {
                GEAR_CATEGORY category = GearDataDict[key].type;
                foreach (string equippedGear in PlayerDataDict[MyID].equippedGears)
                {
                    if (GearDataDict[equippedGear].type.Equals(category))
                    {
                        PlayerDataDict[MyID].equippedGears.Remove(equippedGear);
                        PlayerDataDict[MyID].equippedGears.Add(key);
                        return;
                    }
                }
            }
        }

        public bool IsPlayerOwned(string key)
        {
            return (playerDataDict[MyID].ownedGears.Contains(key) ||
                    playerDataDict[MyID].ownedWeapons.Contains(key));
        }

        public bool IsPlayerEquipped(string key)
        {
            return (PlayerDataDict[MyID].equippedGears.Contains(key) ||
                    PlayerDataDict[MyID].equippedWeapon.Equals(key));
        }
        public int GetItemPrice(string key)
        {
            if (GearDataDict.ContainsKey(key)) return GearDataDict[key].price;
            if (WeaponDataDict.ContainsKey(key)) return WeaponDataDict[key].price;
            else return 0;
        }

        public int GetMaxExp(int level)
        {
            if(level<maxExp.Count) return maxExp[level];

            for (int i = 0; i < level - 2; i++)
            {
                maxExp.Add(maxExp[i - 1] + maxExp[i - 2]);
            }

            return maxExp[level];
        }

        public string GetItemName(string key)
        {
            GearData gearData;
            WeaponData weaponData;
            if (GearDataDict.TryGetValue(key, out gearData)) return gearData.name;
            else if (WeaponDataDict.TryGetValue(key, out weaponData)) return weaponData.name;
            else
            {
                //Debug.Log($"{key} 에 해당하는 아이템 정보가 없습니다.");
                return NONE;
            }
        }

        public string GetEquippedWeaponKey()
        {
            return GetEquippedWeaponKey(MyID);
        }

        public string GetEquippedWeaponKey(int pid)
        {
            return PlayerDataDict[pid].equippedWeapon;
        }

        public string GetEquippedGearKey(GEAR_CATEGORY gearCategory)
        {
            return GetEquippedGearKey(MyID, gearCategory);
        }

        public string GetEquippedGearKey(int pid, GEAR_CATEGORY gearCategory)
        {
            foreach (var gear in PlayerDataDict[pid].equippedGears)
            {
                if (GearDataDict[gear].type.Equals(gearCategory))
                {
                    return gear;
                }
            }
            return null;
        }

        public DecalData GetDecalData(string weaponKey)
        {
            return DecalDataDict[GetDecalType(weaponKey)];
        }

        public DECALTYPE GetDecalType(string weaponKey)
        {
            return WeaponDataDict[weaponKey].decalType;
        }

        public List<string> GetItemsKey(ITEM_CATEGORY category)
        {
            if (category.Equals(ITEM_CATEGORY.WEAPON))
                return weaponDataDict.Keys.ToList();
            else
            {
                return GearDataDict
                    .Where(pair => pair.Value.type.Equals(ITEM_TO_GEAR_CATEGORY[category]))
                    .ToDictionary(pair => pair.Key, pair => pair.Value)
                    .Keys.ToList();
            }
        }

        public List<string> GetOwnedItemsKey(ITEM_CATEGORY category)
        {
            List<string> items = new List<string>();
            if (category.Equals(ITEM_CATEGORY.WEAPON))
            {
                items = WeaponDataDict
                    .Where(n => playerDataDict[MyID].ownedWeapons.Contains(n.Key))
                    .Select(n => n.Key)
                    .ToList();
            }
            else
            {
                items = GearDataDict
                    .Where(n => n.Value.type.Equals(ITEM_TO_GEAR_CATEGORY[category]))
                    .Where(n => playerDataDict[MyID].ownedGears.Contains(n.Key))
                    .Select(n => n.Key)
                    .ToList();
            }
            return items;
        }

        public void CacheData(Dictionary<string, AbilityData> data)
        {
            abilityDataDict = data;
        }
        public void CacheData(Dictionary<string, GearData> data)
        {
            gearDataDict = data;
        }
        public void CacheData(Dictionary<string, WeaponData> data)
        {
            weaponDataDict = data;
        }
        public void CacheData(Dictionary<DECALTYPE, DecalData> data)
        {
            decalDataDict = data;
        }
        public void CacheData(Dictionary<int, PlayerData> data)
        {
            playerDataDict = data;
        }
        public void CachePlayerData(int pid, PlayerData data)
        {
            if(playerDataDict == null) playerDataDict = new Dictionary<int, PlayerData>();
            playerDataDict.Add(pid, data);
        }
    }
}