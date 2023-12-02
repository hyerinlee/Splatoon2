using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class CategoryCard : CursorSelectable
    {
        [SerializeField] private Image cardImg;
        [SerializeField] private Image itemThumbnailImg;

        [Header("부 무기")]
        [SerializeField] private GameObject subWeapons;
        [SerializeField] private Image subWeaponImg;

        [Header("기어 어빌리티")]
        [SerializeField] private GameObject gearAbilities;
        [SerializeField] private List<Image> gearAbilityImg = new List<Image>();

        private float duration = 0.2f;
        private int moveYOffset = 20;
        private Vector3 scaleOffset = Vector3.one * 1.2f;
        private Vector3 rotateOffset = new Vector3(0, 0, 5);





        public void SetCard(int categoryIndex)
        {
            subWeapons.SetActive(false);
            gearAbilities.SetActive(false);

            if (((ITEM_CATEGORY)categoryIndex).Equals(ITEM_CATEGORY.WEAPON))
            {
                string key = DataManager.Instance.GetEquippedWeaponKey();
                itemThumbnailImg.sprite = ResourceManager.Instance.ItemSpriteDict[key];
                WeaponData data = DataManager.Instance.WeaponDataDict[key];
                subWeapons.SetActive(true);
                subWeaponImg.sprite = ResourceManager.Instance.SubWeaponSpriteDict[data.subWeapon];
            }
            else
            {
                string key = DataManager.Instance
                    .GetEquippedGearKey(ITEM_TO_GEAR_CATEGORY[(ITEM_CATEGORY)categoryIndex]);
                itemThumbnailImg.sprite = ResourceManager.Instance.ItemSpriteDict[key];
                GearData data = DataManager.Instance.GearDataDict[key];
                gearAbilities.SetActive(true);
                gearAbilityImg[0].sprite = ResourceManager.Instance.AbilitySpriteDict[data.ability[0]];
                gearAbilityImg[1].sprite = ResourceManager.Instance.AbilitySpriteDict[data.ability[1]];
            }
            cardImg.color = ITEM_CATEGORY_COLOR[categoryIndex];
        }

        public void Idle()
        {
            Rt.DOAnchorPosY(-moveYOffset, duration);
            Rt.DOScale(Vector3.one, duration);
            Rt.DOLocalRotate(Vector3.zero, duration);
        }

        public void Focus()
        {
            Rt.DOAnchorPosY(moveYOffset, duration);
            Rt.DOScale(scaleOffset, 0.2f);
            Rt.DOLocalRotate(rotateOffset, duration);
        }
    }

}