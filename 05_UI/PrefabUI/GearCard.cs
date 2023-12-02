using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class GearCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text gearNameTxt, brandNameTxt;
        [SerializeField] private Image gearCardImg, gearThumbnailImg, brandImg;
        [SerializeField] private List<Image> abilityImg;





        public void SetGearCard(GEAR_CATEGORY category)
        {
            string gearKey = DataManager.Instance.GetEquippedGearKey(category);
            GearData gearData = DataManager.Instance.GearDataDict[gearKey];
            gearNameTxt.text = gearData.name;
            brandNameTxt.text = BRAND_TO_STR[gearData.brand];

            gearCardImg.color = ITEM_CATEGORY_COLOR[(int)category];
            gearThumbnailImg.sprite = ResourceManager.Instance.ItemSpriteDict[gearKey];
            brandImg.sprite = ResourceManager.Instance.BrandSpriteDict[gearData.brand];
            for (int i = 0; i < abilityImg.Count; i++)
            {
                abilityImg[i].sprite = ResourceManager.Instance.AbilitySpriteDict[gearData.ability[i]];
            }

        }
    }

}