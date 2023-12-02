using System.Collections.Generic;
using static Splatoon2.Define;

namespace Splatoon2
{
    public abstract class ItemData
    {
        public readonly int id;
        public readonly int price;
        public readonly string name;

        public ItemData(int id, int price, string name)
        {
            this.id = id;
            this.price = price;
            this.name = name;
        }
    }
    
    public class WeaponData : ItemData
    {
        public readonly WEAPONTYPE type;
        public readonly DECALTYPE decalType;
        public readonly Dictionary<SPEC, float> specs;
        public readonly SUBWEAPON subWeapon;

        public WeaponData(int id, int price, string name, WEAPONTYPE type, DECALTYPE decalType,
            Dictionary<SPEC, float> specs, SUBWEAPON subWeapon) : base(id, price, name)
        {
            this.type = type;
            this.decalType = decalType;
            this.specs = specs;
            this.subWeapon = subWeapon;
        }
    }

    public class GearData : ItemData
    {
        public readonly GEAR_CATEGORY type;
        public readonly ABILITY[] ability = new ABILITY[4];
        public readonly GEAR_BRAND brand;


        public GearData(int id, int price, string name, GEAR_CATEGORY type, ABILITY[] ability,
            GEAR_BRAND brand) : base(id, price, name)
        {
            this.type = type;
            for (int i = 0; i < ability.Length; i++)
            {
                this.ability[i] = ability[i];
            }
            this.brand = brand;
        }
    }

    public struct DecalData
    {
        public string decalTex;
        public int decalRow, decalCol;
    }
}