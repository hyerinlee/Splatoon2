using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Splatoon2
{
    public class EquipmentItemCard : CursorSelectable
    {
        [SerializeField] private Image cardColorImg;
        [SerializeField] private Image itemImg;
        [SerializeField] private Image SelectEffectImg;

        [Header("부 무기")]
        [SerializeField] private GameObject subWeapons;
        [SerializeField] private Image subWeaponImg;

        [Header("기어 어빌리티")]
        [SerializeField] private GameObject gearAbilities;
        [SerializeField] private List<Image> gearAbilityImg = new List<Image>();

        private Vector3 scaleOffset = Vector3.one * 1.1f;





        public void SetItemCard(string key)
        {
            subWeapons.SetActive(false);
            gearAbilities.SetActive(false);
            SelectEffectImg.enabled = false;

            itemImg.sprite = ResourceManager.Instance.ItemSpriteDict[key];

            WeaponData weaponData;
            GearData gearData;
            if (DataManager.Instance.WeaponDataDict.TryGetValue(key, out weaponData))
            {
                subWeapons.SetActive(true);
                subWeaponImg.sprite = ResourceManager.Instance.SubWeaponSpriteDict[weaponData.subWeapon];
            }
            else if (DataManager.Instance.GearDataDict.TryGetValue(key, out gearData))
            {
                gearAbilities.SetActive(true);
                gearAbilityImg[0].sprite = ResourceManager.Instance.AbilitySpriteDict[gearData.ability[0]];
                gearAbilityImg[1].sprite = ResourceManager.Instance.AbilitySpriteDict[gearData.ability[1]];
            }
            else Debug.Log(key + "에 해당하는 아이템 데이터가 없습니다.");
        }
        
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void Idle()
        {
            Rt.DOScale(Vector3.one, 0.2f);
        }

        public void Focus()
        {
            Rt.DOScale(scaleOffset, 0.2f);
        }

        public void Select()
        {
            SelectEffectImg.enabled = true;
        }

        public void UnSelect()
        {
            SelectEffectImg.enabled = false;
        }
    }

}