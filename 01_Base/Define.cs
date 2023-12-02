using System.Collections.Generic;
using UnityEngine;

namespace Splatoon2
{
    public static class Define
    {
        public static readonly int DECAL_WIDTH = 2048;
        public static readonly int DECAL_HEIGHT = 2048;
        public static readonly float GRAVITY = 9.8f;
        public static readonly string DEFEAT = "DEFEAT";
        public static readonly string LOCKED = "Locked";
        public static readonly string NONE = "None";
        public static readonly string OWNED = "Owned";
        public static readonly string PID = "pid";
        public static readonly string VICTORY = "VICTORY";
        public static readonly Color BUY_POPUP_SELECTED_COLOR = new Color(0.51f, 0.96f, 0.03f); // #82f508, LAWN GREEN

        public static readonly string[] SHOP_CATEGORY = { "Shoes", "Clothing", "Headgear", "Weapons" };
        public static readonly string[] SHOP_NAME = { "Shella Fresh", "Ye Olde Cloth Shoppe", "Headspace", "Ammo Knights" };
        public static readonly Color[] ITEM_CATEGORY_COLOR =
        {
            new Color(0.95f,0.56f,0.03f),   // #f28f08, ORANGE
            new Color(0.31f,0.86f,0.63f),   // #4fdba1, TURQUOISE
            new Color(1f,0.33f,0.33f),      // #ff5454, CORAL
            new Color(0.84f,0.89f,0)        // #d6e300, LIME
        };
        public static Color[] SHOP_COLOR_DARK =
        {
            new Color(0.75f,0.36f,0.00f),
            new Color(0.11f,0.66f,0.43f),
            new Color(0.8f,0.13f,0.13f),
            new Color(0.64f,0.69f,0)
        };
        public static readonly List<string>[] SHOP_WELCOME_MSG =
        {
            new List<string>{"AYYYYYY!\nWelcome to Shella Fresh." },
            new List<string>{"How dost thou, cousin?" },
            new List<string>{"Hmm? Ah, welcome, friend." },
            new List<string>{"Hello, hello!\nWhat kind of weapon are you in the market for?" }
        };

        public struct AnimationParam
        {
            public static readonly string IS_ATTACK = "isAttack";
            public static readonly string IS_JUMP = "isJump";
            public static readonly string IS_ROLLER_WEAPON_ACTIVE = "isRollerWeaponActive";
            public static readonly string IS_SHOOTER_WEAPON_ACTIVE = "isShooterWeaponActive";
            public static readonly string SPEED = "speed";
        }

        public struct MaterialProperty
        {
            public static readonly string DECALTEX = "_DecalTex";
            public static readonly string MAINTEX = "_MainTex";
            public static readonly string COLOR = "_Color";
            public static readonly string ROTATION = "_Rotation";
        }


        // ¡å Enum -------------------------------------------------------------------------------------------------------------

        public enum ABILITY
        {
            BOMB_DEFENSE_UP_DX, INK_RECOVERY_UP, INK_RESISTANCE_UP, INK_SAVER_MAIN, INK_SAVER_SUB, LAST_DITCH_EFFORT,
            NOT_FOUND, OPENING_GAMBIT, RANDOM, RUN_SPEED_UP, SPECIAL_CHARGE_UP, STEALTH_JUMP, SWIM_SPEED_UP
        }
        public enum ACTION_MAP { UI, PLAYER_INGAME, PLAYER_INKOPOLIS }
        public enum ANIM_CONTROLLER { DEFAULT, INKOPOLIS, INGAME }
        public enum DECALTYPE { SHOOT, SPREAD, NONE }
        public enum GEAR_BRAND
        {
            AMIIBO, ANNAKI, CUTTLEGEAR, ENPERRY, FIREFIN, FORGE, GRIZZCO, INKLINE, KRAK_ON, ROCKENBERG,
            SKALOP, SPLASH_MOB, SQUIDFORCE, TAKOROKA, TENTATEK, TONI_KENSA, ZEKKO, ZINK, NO_BRAND
        }
        public enum GEAR_CATEGORY { HEAD, CLOTHES, SHOES, NONE }
        public enum ITEM_CATEGORY { SHOES, CLOTHES, HEAD, WEAPON, NONE }
        public enum SCENE { INKOPOLIS, SHOP, MODESELECT, EQUIPMENT, INGAME, LOADING, }
        public enum SPEC { RANGE, DAMAGE, FIRE_RATE, INK_SPEED, HANDLING }
        public enum SUBWEAPON { QUICK_BOMB, CURLING_BOMB, AUTO_BOMB }
        public enum WEAPONTYPE { GUN, ROLLER, NONE }



        // ¡å Dictionary -------------------------------------------------------------------------------------------------------------

        public static Dictionary<SCENE, string> SCENE_TO_STR = new Dictionary<SCENE, string>
        {
            {SCENE.INKOPOLIS,       "INKOPOLIS"},
            {SCENE.SHOP,            "SHOP"},
            {SCENE.MODESELECT,      "MODESELECT"},
            {SCENE.EQUIPMENT,       "EQUIPMENT" },
            {SCENE.INGAME,          "INGAME" }
        };

        public static Dictionary<ACTION_MAP, string> ACTION_MAP_TO_STR = new Dictionary<ACTION_MAP, string>
        {
            { ACTION_MAP.UI,                "UI" },
            { ACTION_MAP.PLAYER_INGAME,     "PLAYER_INGAME" },
            { ACTION_MAP.PLAYER_INKOPOLIS,  "PLAYER_INKOPOLIS" },
        };

        public static Dictionary<ANIM_CONTROLLER, string> ANIM_CONTROLLER_ASSET_ADDRESS = new Dictionary<ANIM_CONTROLLER, string>
        {
            { ANIM_CONTROLLER.DEFAULT,      "DefaultAnimController" },
            { ANIM_CONTROLLER.INKOPOLIS,    "InkopolisAnimController" },
            { ANIM_CONTROLLER.INGAME,       "IngameAnimController" }
        };

        public static Dictionary<ITEM_CATEGORY, GEAR_CATEGORY> ITEM_TO_GEAR_CATEGORY = new Dictionary<ITEM_CATEGORY, GEAR_CATEGORY>
        {
            {ITEM_CATEGORY.SHOES,   GEAR_CATEGORY.SHOES},
            {ITEM_CATEGORY.CLOTHES, GEAR_CATEGORY.CLOTHES},
            {ITEM_CATEGORY.HEAD,    GEAR_CATEGORY.HEAD},
            {ITEM_CATEGORY.NONE,    GEAR_CATEGORY.NONE},
        };

        public static Dictionary<GEAR_BRAND, string> BRAND_TO_STR = new Dictionary<GEAR_BRAND, string>
        {
            {GEAR_BRAND.AMIIBO,     "Amiibo"},
            {GEAR_BRAND.ANNAKI,     "Annaki" },
            {GEAR_BRAND.CUTTLEGEAR, "Cuttlegear" },
            {GEAR_BRAND.ENPERRY,    "Enperry" },
            {GEAR_BRAND.FIREFIN,    "Firefin"},
            {GEAR_BRAND.FORGE,      "Forge" },
            {GEAR_BRAND.GRIZZCO,    "Grizzco"},
            {GEAR_BRAND.INKLINE,    "Inkline" },
            {GEAR_BRAND.KRAK_ON,    "Krak-On"},
            {GEAR_BRAND.ROCKENBERG, "Rockenberg" },
            {GEAR_BRAND.SKALOP,     "Skalop"},
            {GEAR_BRAND.SPLASH_MOB, "Splash Mob" },
            {GEAR_BRAND.SQUIDFORCE, "Squidforce"},
            {GEAR_BRAND.TAKOROKA,   "Takoroka" },
            {GEAR_BRAND.TENTATEK,   "Tentatek"},
            {GEAR_BRAND.TONI_KENSA, "Toni Kensa" },
            {GEAR_BRAND.ZEKKO,      "Zekko"},
            {GEAR_BRAND.ZINK,       "Zink" },
            {GEAR_BRAND.NO_BRAND,   "No-Brand"}
        };

        public static Dictionary<ABILITY, string> ABILITY_TO_STR = new Dictionary<ABILITY, string>
        {
            {ABILITY.BOMB_DEFENSE_UP_DX,    "Bomb Defense Up DX" },
            {ABILITY.INK_RECOVERY_UP,       "Ink Recovery Up"},
            {ABILITY.INK_RESISTANCE_UP,     "Ink Resistance Up" },
            {ABILITY.INK_SAVER_MAIN,        "Ink Saver(Main)" },
            {ABILITY.INK_SAVER_SUB,         "Ink Saver(Sub)" },
            {ABILITY.LAST_DITCH_EFFORT,     "Last Ditch Effort" },
            {ABILITY.NOT_FOUND,             "Not Found" },
            {ABILITY.OPENING_GAMBIT,        "Opening Gambit" },
            {ABILITY.RANDOM,                "Random" },
            {ABILITY.RUN_SPEED_UP,          "Run Speed Up" },
            {ABILITY.SPECIAL_CHARGE_UP,     "Special Charge Up" },
            {ABILITY.STEALTH_JUMP,          "Stealth Jump" },
            {ABILITY.SWIM_SPEED_UP,         "Swim Speed Up" }
        };

        public static Dictionary<SUBWEAPON, string> SUBWEAPON_TO_STR = new Dictionary<SUBWEAPON, string>
        {
            {SUBWEAPON.AUTO_BOMB,           "Auto Bomb" },
            {SUBWEAPON.CURLING_BOMB,        "Curling Bomb"},
            {SUBWEAPON.QUICK_BOMB,          "Quick Bomb" }
        };

        public static Dictionary<SPEC, string> SPEC_TO_STR = new Dictionary<SPEC, string>
        {
            {SPEC.DAMAGE,       "Damage"},
            {SPEC.FIRE_RATE,    "Fire Rate" },
            {SPEC.HANDLING,     "Handling" },
            {SPEC.INK_SPEED,    "Ink Speed" },
            {SPEC.RANGE,        "Range" }
        };
    }
}
