using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class ShopItemCard : CursorSelectable
    {
        [SerializeField] private GameObject priceTag;
        [SerializeField] private GameObject disableTag;
        [SerializeField] private Image bgImg;
        [SerializeField] private Image cardColorImg;
        [SerializeField] private Image itemImg;
        [SerializeField] private TMP_Text priceTxt, disableTxt;

        private bool canBuy;



        public Image ItemImg { get => itemImg; }
        public TMP_Text PriceTxt { get => priceTxt; }
        public bool CanBuy { get => canBuy; }





        public void SetCardColor(Color color)
        {
            cardColorImg.color = color;
        }

        public void SetItemData(string key)
        {
            ItemImg.sprite = ResourceManager.Instance.ItemSpriteDict[key];

            if (DataManager.Instance.IsPlayerOwned(key))
            {
                canBuy = false;
                bgImg.color = Color.gray;
                priceTag.SetActive(false);
                disableTag.SetActive(true);
                disableTxt.text = OWNED;
            }
            else
            {
                int price = DataManager.Instance.GetItemPrice(key);
                disableTag.SetActive(false);
                priceTag.SetActive(true);
                PriceTxt.text = price.ToString();

                if (DataManager.Instance.GetMyData().cash < price)
                {
                    canBuy = false;
                    bgImg.color = Color.gray;
                    PriceTxt.color = Color.gray;
                }
                else
                {
                    canBuy = true;
                    bgImg.color = Color.white;
                    PriceTxt.color = Color.white;
                }
            }
        }

        public void SetLocked()
        {
            canBuy = false;
            bgImg.color = Color.gray;
            priceTag.SetActive(false);
            disableTag.SetActive(true);
            disableTxt.text = LOCKED;
            itemImg.sprite = ResourceManager.Instance.ItemSpriteDict[LOCKED];
        }

        public void Idle()
        {
            rt.DOScale(1f, 0.2f);
        }
        public void Focus()
        {
            rt.DOScale(1.1f, 0.2f);
        }
    }

}