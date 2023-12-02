using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Newtonsoft.Json;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        [Header("JSON AssetReference")]
        [SerializeField] private AssetReference abilityDataRef;
        [SerializeField] private AssetReference gearDataRef;
        [SerializeField] private AssetReference weaponDataRef;
        [SerializeField] private AssetReference decalDataRef;

        [Header("Sprite AssetReference")]
        [SerializeField] private AssetReferenceSprite abilityRef;
        [SerializeField] private AssetReferenceSprite brandRef;
        [SerializeField] private AssetReferenceSprite subWeaponRef;
        [SerializeField] private AssetReferenceSprite itemRef;

        [Header("BattleScene AssetReference")]
        [SerializeField] private AssetReference drawDecalShaderRef;
        [SerializeField] private List<AssetReference> matchColorSORefs;
        [SerializeField] private List<AssetReferenceTexture> decalTexRefs;

        private AsyncOperationHandle<TextAsset> abilityDataHandle, gearDataHandle, weaponDataHandle, decalDataHandle;
        private AsyncOperationHandle<RuntimeAnimatorController> playerAnimControllerHandle;
        private AsyncOperationHandle<Sprite[]> abilitySpriteHandle, brandSpriteHandle, subWeaponSpriteHandle;
        private AsyncOperationHandle<SpriteAtlas> itemSpriteHandle;
        private AsyncOperationHandle<GameObject> itemPrefabHandle;

        private AsyncOperationHandle<Shader> drawDecalShaderHandle;
        private List<AsyncOperationHandle<MatchColorSO>> matchColorSOHandles;
        private List<AsyncOperationHandle<Texture>> decalTextureHandles;

        private Dictionary<ANIM_CONTROLLER, RuntimeAnimatorController> playerAnimControllerDict;
        private Dictionary<ABILITY, Sprite> abilitySpriteDict;
        private Dictionary<GEAR_BRAND, Sprite> brandSpriteDict;
        private Dictionary<SUBWEAPON, Sprite> subWeaponSpriteDict;
        private Dictionary<string, string> originalItemNameDict;
        private Dictionary<string, Sprite> itemSpriteDict;

        private Shader drawDecalShader;
        private List<MatchColorSO> matchColors;
        private Dictionary<string, Texture> decalTextureDict;



        public Dictionary<ANIM_CONTROLLER, RuntimeAnimatorController> PlayerAnimControllerDict { get => playerAnimControllerDict; }
        public Dictionary<ABILITY, Sprite> AbilitySpriteDict { get => abilitySpriteDict; }
        public Dictionary<GEAR_BRAND, Sprite> BrandSpriteDict { get => brandSpriteDict; }
        public Dictionary<SUBWEAPON, Sprite> SubWeaponSpriteDict { get => subWeaponSpriteDict; }
        public Dictionary<string, Sprite> ItemSpriteDict { get => itemSpriteDict; }

        public Shader DrawDecalShader { get => drawDecalShader; }
        public List<MatchColorSO> MatchColors { get => matchColors; }
        public Dictionary<string, Texture> DecalTextureDict { get => decalTextureDict; }





        protected override void Awake()
        {
            base.Awake();
            playerAnimControllerDict = new Dictionary<ANIM_CONTROLLER, RuntimeAnimatorController>();
        }



        public void LoadItemModel(string key, ITEM_CATEGORY category, Transform transform,
            Dictionary<string, GameObject> cachedItemDict, Action<string, ITEM_CATEGORY> onComplete)
        {
            itemPrefabHandle = Addressables.InstantiateAsync(key, transform);
            itemPrefabHandle.Completed += (asyncOpHandle) =>
            {
                if (asyncOpHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    cachedItemDict.Add(key, asyncOpHandle.Result);
                    onComplete?.Invoke(key, category);
                }
            };
        }

        public void ReleaseResources(SCENE currentScene)
        {
            if (currentScene.Equals(SCENE.INGAME))
            {
                Addressables.Release(decalDataHandle);
                Addressables.Release(drawDecalShaderHandle);
                for(int i=0; i<decalTextureHandles.Count; i++)
                {
                    Addressables.Release(decalTextureHandles[i]);
                }
                for(int i=0; i<matchColorSOHandles.Count; i++)
                {
                    Addressables.Release(matchColorSOHandles[i]);
                }
            }
        }



        private void LoadAbilityData()
        {
            if (!abilityDataHandle.IsValid())
            {
                abilityDataHandle = abilityDataRef.LoadAssetAsync<TextAsset>();
                abilityDataHandle.Completed += (asyncOperationHandle) =>
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        DataManager.Instance.CacheData(
                            JsonConvert.DeserializeObject<Dictionary<string, AbilityData>>(asyncOperationHandle.Result.text));
                    }
                };
            }
        }

        private void LoadGearData()
        {
            if (!gearDataHandle.IsValid())
            {
                gearDataHandle = gearDataRef.LoadAssetAsync<TextAsset>();
                gearDataHandle.Completed += (asyncOperationHandle) =>
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        DataManager.Instance.CacheData(
                            JsonConvert.DeserializeObject<Dictionary<string, GearData>>(asyncOperationHandle.Result.text));
                    }
                };
            }
        }

        private void LoadWeaponData()
        {
            if (!weaponDataHandle.IsValid())
            {
                weaponDataHandle = weaponDataRef.LoadAssetAsync<TextAsset>();
                weaponDataHandle.Completed += (asyncOperationHandle) =>
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        DataManager.Instance.CacheData(
                            JsonConvert.DeserializeObject<Dictionary<string, WeaponData>>(asyncOperationHandle.Result.text));
                    }
                };
            }
        }

        private void LoadDecalData()
        {
            if (!decalDataHandle.IsValid())
            {
                decalDataHandle = decalDataRef.LoadAssetAsync<TextAsset>();
                decalDataHandle.Completed += (asyncOperationHandle) =>
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        DataManager.Instance.CacheData(
                            JsonConvert.DeserializeObject<Dictionary<DECALTYPE, DecalData>>(asyncOperationHandle.Result.text));
                    }
                };
            }
        }

        private void LoadAnimController(ANIM_CONTROLLER animController)
        {
            if (!PlayerAnimControllerDict.ContainsKey(animController))
            {
                playerAnimControllerHandle = Addressables.LoadAssetAsync<RuntimeAnimatorController>
                    (ANIM_CONTROLLER_ASSET_ADDRESS[animController]);
                playerAnimControllerHandle.Completed += (asyncOperationHandle) =>
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        PlayerAnimControllerDict.Add(animController, asyncOperationHandle.Result);
                    }
                };
            }
        }

        private void LoadAbilitySprites()
        {
            if (!abilitySpriteHandle.IsValid())
            {
                abilitySpriteDict = new Dictionary<ABILITY, Sprite>();
                abilitySpriteHandle = abilityRef.LoadAssetAsync<Sprite[]>();
                abilitySpriteHandle.Completed += (asyncOperationHandle) =>
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        foreach (Sprite s in asyncOperationHandle.Result)
                        {
                            if (Enum.GetNames(typeof(ABILITY)).Contains(s.name))
                            {
                                abilitySpriteDict.Add(Enum.Parse<ABILITY>(s.name), s);
                            }
                        }
                    }
                };
            }
        }

        private void LoadBrandSprites()
        {
            if (!brandSpriteHandle.IsValid())
            {
                brandSpriteDict = new Dictionary<GEAR_BRAND, Sprite>();
                brandSpriteHandle = brandRef.LoadAssetAsync<Sprite[]>();
                brandSpriteHandle.Completed += (asyncOperationHandle) =>
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        foreach (Sprite s in asyncOperationHandle.Result)
                        {
                            BrandSpriteDict.Add(Enum.Parse<GEAR_BRAND>(s.name), s);
                        }
                    }
                };
            }
        }

        private void LoadSubWeaponSprites()
        {
            if (!subWeaponSpriteHandle.IsValid())
            {
                subWeaponSpriteDict = new Dictionary<SUBWEAPON, Sprite>();
                subWeaponSpriteHandle = subWeaponRef.LoadAssetAsync<Sprite[]>();
                subWeaponSpriteHandle.Completed += (asyncOperationHandle) =>
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        foreach (Sprite s in asyncOperationHandle.Result)
                        {
                            subWeaponSpriteDict.Add(Enum.Parse<SUBWEAPON>(s.name), s);
                        }
                    }
                };
            }
        }

        private void LoadItemSprites()
        {
            if (!itemSpriteHandle.IsValid())
            {
                itemSpriteDict = new Dictionary<string, Sprite>();
                itemSpriteHandle = itemRef.LoadAssetAsync<SpriteAtlas>();
                itemSpriteHandle.Completed += (asyncOperationHandle) =>
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        Sprite[] sprites = new Sprite[asyncOperationHandle.Result.spriteCount];
                        asyncOperationHandle.Result.GetSprites(sprites);
                        foreach (Sprite s in sprites)
                        {
                            if (!originalItemNameDict.ContainsKey(s.name))
                            {
                                originalItemNameDict.Add(s.name, s.name.Replace("(Clone)", ""));
                            }
                            ItemSpriteDict.Add(originalItemNameDict[s.name], s);
                        }
                    }
                };
            }
        }

        private void LoadDecalShader()
        {
            if (!drawDecalShaderHandle.IsValid())
            {
                drawDecalShaderHandle = drawDecalShaderRef.LoadAssetAsync<Shader>();
                drawDecalShaderHandle.Completed += (opHandle) =>
                {
                    if (opHandle.Status.Equals(AsyncOperationStatus.Succeeded))
                    {
                        drawDecalShader = opHandle.Result;
                    }
                };
            }
        }

        private void LoadMatchColor()
        {
            matchColors = new List<MatchColorSO>();
            matchColorSOHandles = new List<AsyncOperationHandle<MatchColorSO>>();
            for (int i = 0; i < matchColorSORefs.Count; i++)
            {
                matchColorSOHandles.Add(matchColorSORefs[i].LoadAssetAsync<MatchColorSO>());
                matchColorSOHandles[i].Completed += (opHandle) =>
                {
                    if (opHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        matchColors.Insert(int.Parse(opHandle.Result.name), opHandle.Result);
                    }
                };
            }
        }

        private void LoadDecalTex()
        {
            decalTextureDict = new Dictionary<string, Texture>();
            decalTextureHandles = new List<AsyncOperationHandle<Texture>>();
            for (int i = 0; i < decalTexRefs.Count; i++)
            {
                decalTextureHandles.Add(decalTexRefs[i].LoadAssetAsync<Texture>());
                decalTextureHandles[i].Completed += (opHandle) =>
                {
                    if (opHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        decalTextureDict[opHandle.Result.name] = opHandle.Result;
                    }
                };
            }
        }

        private bool AreAllResourcesLoaded(SCENE nextScene)
        {
            bool isDone = true;

            isDone &= gearDataHandle.IsDone;
            isDone &= playerAnimControllerHandle.IsDone;

            if (!nextScene.Equals(SCENE.INKOPOLIS))
            {
                isDone &= weaponDataHandle.IsDone;

                if (!nextScene.Equals(SCENE.MODESELECT))
                {
                    isDone &= abilityDataHandle.IsDone;
                    isDone &= abilitySpriteHandle.IsDone;
                    isDone &= brandSpriteHandle.IsDone;
                    isDone &= itemSpriteHandle.IsDone;
                    isDone &= subWeaponSpriteHandle.IsDone;

                    if (nextScene.Equals(SCENE.INGAME))
                    {
                        isDone &= decalDataHandle.IsDone;
                        for (int i = 0; i < decalTextureHandles.Count; i++)
                        {
                            isDone &= decalTextureHandles[i].IsDone;
                        }
                        isDone &= drawDecalShaderHandle.IsDone;
                        for(int i=0; i<matchColorSOHandles.Count; i++)
                        {
                            isDone &= matchColorSOHandles[i].IsDone;
                        }
                    }
                }
            }

            return isDone;
        }

        



        public IEnumerator LoadSceneResourcesCrt(SCENE nextScene)
        {
            LoadGearData();
            LoadAnimController
                (nextScene.Equals(SCENE.INKOPOLIS)? ANIM_CONTROLLER.INKOPOLIS :
                (nextScene.Equals(SCENE.INGAME)?    ANIM_CONTROLLER.INGAME :
                                                    ANIM_CONTROLLER.DEFAULT));

            if (!nextScene.Equals(SCENE.INKOPOLIS))
            {
                LoadWeaponData();

                if (!nextScene.Equals(SCENE.MODESELECT))
                {
                    LoadAbilityData();
                    originalItemNameDict = new Dictionary<string, string>();
                    LoadAbilitySprites();
                    LoadBrandSprites();
                    LoadItemSprites();
                    LoadSubWeaponSprites();

                    if (nextScene.Equals(SCENE.INGAME))
                    {
                        LoadDecalData();
                        LoadDecalTex();
                        LoadDecalShader();
                        LoadMatchColor();
                    }
                }
            }

            while (!AreAllResourcesLoaded(nextScene)) yield return null;
        }

    }
}