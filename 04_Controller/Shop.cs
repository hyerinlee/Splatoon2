using System;
using System.Collections.Generic;
using UnityEngine;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private Transform avatarTr;
        [SerializeField] private Avatar avatar;
        [SerializeField] private List<Transform> shopAvatarTr = new List<Transform>();

        private static int currentShop = 0;

        private event Action onBuy;
        private int selectedItemIndex = 0;
        private List<string> itemKeys = new List<string>();



        public event Action OnBuy
        {
            add => onBuy += value;
            remove => onBuy -= value;
        }
        public static int CurrentShop
        {
            get => currentShop;
            set
            {
                currentShop = value;
                if (currentShop < 0) currentShop = (int)ITEM_CATEGORY.WEAPON;
                else if (currentShop > (int)ITEM_CATEGORY.WEAPON) currentShop = 0;
            }
        }
        public int SelectedItemIndex
        {
            get => selectedItemIndex;
            set
            {
                selectedItemIndex = value;
                if (selectedItemIndex < 0) selectedItemIndex = 5;
                if (selectedItemIndex > 5) selectedItemIndex = 0;
            }
        }
        public List<string> ItemKeys
        {
            get => itemKeys;
            set => itemKeys = value;
        }





        public void MoveShopAvatar()
        {
            avatarTr.position = shopAvatarTr[CurrentShop].position;
            avatarTr.rotation = shopAvatarTr[CurrentShop].rotation;
            avatarTr.localScale = shopAvatarTr[CurrentShop].localScale;
        }

        public void Try()
        {
            if (SelectedItemIndex < ItemKeys.Count)
            {
                avatar.WearItem(ItemKeys[SelectedItemIndex], (ITEM_CATEGORY)CurrentShop);
            }
        }

        public void ResetAvatar()
        {
            avatar.Init();
            if (CurrentShop.Equals((int)ITEM_CATEGORY.WEAPON))
            {
                avatar.InitWeapon();
            }
        }

        public void Buy(string key)
        {
            DataManager.Instance.GetMyData().cash -= DataManager.Instance.GetItemPrice(key);
            DataManager.Instance.AddOwnedItems(key);

            onBuy?.Invoke();
            SaveAndLoad.Instance.Save();
        }

        public void Exit()
        {
            SaveAndLoad.Instance.Save();
            GameManager.Instance.ChangeScene(SCENE.INKOPOLIS);
        }
    }
}