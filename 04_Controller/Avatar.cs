using System.Collections.Generic;
using UnityEngine;
using static Splatoon2.Define;

namespace Splatoon2
{
    [RequireComponent(typeof(AnimController))]
    public class Avatar : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer bodySkinnedMeshRenderer;
        [SerializeField] private GameObject armature;
        [SerializeField] private Transform headGearTr, weaponTr;
        [SerializeField] private AnimController playerAnim;

        private int pid;
        private Dictionary<ITEM_CATEGORY, string> currentItems = new Dictionary<ITEM_CATEGORY, string>
        {
            {ITEM_CATEGORY.SHOES,   null },
            {ITEM_CATEGORY.CLOTHES, null },
            {ITEM_CATEGORY.HEAD,    null },
            {ITEM_CATEGORY.WEAPON,  null }
        };
        private Dictionary<string, GameObject> cachedItemDict = new Dictionary<string, GameObject>();



        public int Pid
        {
            get => pid;
            set
            {
                pid = value;
                Init();
            }
        }





        private void Start()
        {
            // Armature 회전각 리셋
            armature.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }



        /// <summary>
        /// 현재 씬의 종류에 따라 플레이어의 아바타 애니메이션과 장착아이템 활성화 유무를 설정합니다.
        /// </summary>
        public void Init()
        {
            WearItem(DataManager.Instance.GetEquippedGearKey(Pid, GEAR_CATEGORY.SHOES),     ITEM_CATEGORY.SHOES);
            WearItem(DataManager.Instance.GetEquippedGearKey(Pid, GEAR_CATEGORY.CLOTHES),   ITEM_CATEGORY.CLOTHES);
            WearItem(DataManager.Instance.GetEquippedGearKey(Pid, GEAR_CATEGORY.HEAD),      ITEM_CATEGORY.HEAD);
            if (currentItems[ITEM_CATEGORY.WEAPON] != null)
            {
                cachedItemDict[currentItems[ITEM_CATEGORY.WEAPON]].SetActive(false);
            }
            switch (GameManager.Instance.CurrentScene)
            {
                case SCENE.INGAME:
                case SCENE.MODESELECT:
                    InitWeapon();
                    break;
                default:
                    playerAnim.SetBool(AnimationParam.IS_SHOOTER_WEAPON_ACTIVE, false);
                    playerAnim.SetBool(AnimationParam.IS_ROLLER_WEAPON_ACTIVE, false);
                    break;
            }
        }

        public void InitWeapon()
        {
            WearItem(DataManager.Instance.GetEquippedWeaponKey(), ITEM_CATEGORY.WEAPON);
        }

        public void WearItem(string key, ITEM_CATEGORY category)
        {
            string prev = currentItems[category];
            Transform tr = transform;
            if (category.Equals(ITEM_CATEGORY.WEAPON)) tr = weaponTr;
            if (category.Equals(ITEM_CATEGORY.HEAD)) tr = headGearTr;

            if (!cachedItemDict.ContainsKey(key))
            {
                ResourceManager.Instance.LoadItemModel(key, category, tr, cachedItemDict, WearItem);
            }
            else
            {
                currentItems[category] = key;
                cachedItemDict[key].transform.SetParent(tr);
                cachedItemDict[key].transform.localPosition = Vector3.zero;
                switch (category)
                {
                    case ITEM_CATEGORY.WEAPON:
                        switch (DataManager.Instance.WeaponDataDict[key].type)
                        {
                            case WEAPONTYPE.GUN:
                                playerAnim.SetBool(AnimationParam.IS_SHOOTER_WEAPON_ACTIVE, true);
                                playerAnim.SetBool(AnimationParam.IS_ROLLER_WEAPON_ACTIVE, false);
                                break;
                            case WEAPONTYPE.ROLLER:
                                playerAnim.SetBool(AnimationParam.IS_SHOOTER_WEAPON_ACTIVE, false);
                                playerAnim.SetBool(AnimationParam.IS_ROLLER_WEAPON_ACTIVE, true);
                                break;
                        }
                        break;
                    case ITEM_CATEGORY.HEAD:
                        cachedItemDict[key].transform.localRotation = Quaternion.Euler(Vector3.zero);
                        break;
                    default:
                        cachedItemDict[key].GetComponent<SkinnedMeshRenderer>().rootBone = bodySkinnedMeshRenderer.rootBone;
                        cachedItemDict[key].GetComponent<SkinnedMeshRenderer>().bones = bodySkinnedMeshRenderer.bones;
                        break;
                }
                if (prev != null) cachedItemDict[prev].SetActive(false);
                cachedItemDict[key].SetActive(true);
            }
        }
    }
}