using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using static Splatoon2.Define;

namespace Splatoon2
{
    /// PartialShopUI(Transform): Shop UI의 모양, 색, 위치 등을 변경시키는 메소드를 갖습니다.
    public partial class PartialShopUI
    {
        /// <summary>
        /// 대화창 활성화 후 다이얼로그를 출력하기 시작합니다.
        /// </summary>
        private void ShowNpcBubble()
        {
            npcNameTxt.color = ITEM_CATEGORY_COLOR[Shop.CurrentShop];
            npcBubbleCanvasGroup.DOFade(1, 0.5f);
            PrintNextNpcDialogue();
        }

        private void HideNpcBubble(TweenCallback afterHiding)
        {
            npcBubbleCanvasGroup.DOFade(0, 0.5f).OnComplete(afterHiding);
        }

        public void PrintNextNpcDialogue()
        {
            StartCoroutine(PrintNpcDialogueCrt());
        }

        public void UpdateCashTxt()
        {
            cashTxt.text = DataManager.Instance.GetMyData().cash.ToString();
        }

        /// <summary>
        /// 상점의 간판, 색상 등의 UI를 세팅합니다.
        /// </summary>
        public void SetShopUI()
        {
            shopNameColorImg.color = ITEM_CATEGORY_COLOR[Shop.CurrentShop];
            bottomWavePatternImg.DOColor(ITEM_CATEGORY_COLOR[Shop.CurrentShop], 0.2f);
            for (int i = 0; i < 6; i++) itemUIList[i].SetCardColor(SHOP_COLOR_DARK[Shop.CurrentShop]);
            gearItemDetailCardImg.DOColor(ITEM_CATEGORY_COLOR[Shop.CurrentShop], 0f).SetDelay(0.5f);

            ChangeSelectedBuyButton(false);

            // 가게 색상, 이름, 카테고리 변경
            shopNameTxt.text = SHOP_NAME[Shop.CurrentShop];
            shopCategoryTxt.text = SHOP_CATEGORY[Shop.CurrentShop];
        }

        private void HideFadableObject(TweenCallback nextEvent = null)
        {
            fadableCanvasGroup.DOFade(0, 0.2f)
                .OnComplete(nextEvent);
        }

        public void ShowFadableObject()
        {
            InputActionHandler.Instance.Active(false);

            leftShopIconImg.sprite = shopIcons[(Shop.CurrentShop + 3) % 4];
            rightShopIconImg.sprite = shopIcons[(Shop.CurrentShop + 1) % 4];

            SetItemDetailCard();
            fadableCanvasGroup.DOFade(1, 0.2f).SetDelay(0.6f)
            .OnComplete(() => { InputActionHandler.Instance.Active(true); });
        }

        public void SetItemDetailCard()
        {
            if (shop.SelectedItemIndex >= shop.ItemKeys.Count)
            {
                itemDataObject.SetActive(false);
                lockedObject.SetActive(true);
                return;
            }
            itemDataObject.SetActive(true);
            lockedObject.SetActive(false);

            itemThumbnailImg.sprite = itemUIList[shop.SelectedItemIndex].ItemImg.sprite;
            priceTxt.text = itemUIList[shop.SelectedItemIndex].PriceTxt.text;
            if (Shop.CurrentShop.Equals((int)ITEM_CATEGORY.WEAPON))
            {
                gearData.SetActive(false);
                weaponData.SetActive(true);

                var itemData = DataManager.Instance.WeaponDataDict[shop.ItemKeys[shop.SelectedItemIndex]];
                itemNameTxt.text = itemData.name;

                // 무기 스펙(3가지) 세팅
                int index = 0;
                foreach(KeyValuePair<SPEC, float> spec in itemData.specs)
                {
                    specNameTxt[index].text = SPEC_TO_STR[spec.Key];
                    specPercentage[index].value = spec.Value;
                    index++;
                }

                // 부무기 이미지,텍스트 세팅
                subWeaponImg.sprite = ResourceManager.Instance.SubWeaponSpriteDict[itemData.subWeapon];
                subWeaponNameTxt.text = SUBWEAPON_TO_STR[itemData.subWeapon];
            }
            else
            {
                weaponData.SetActive(false);
                gearData.SetActive(true);

                var itemData = DataManager.Instance.GearDataDict[shop.ItemKeys[shop.SelectedItemIndex]];
                itemNameTxt.text = itemData.name;

                brandNameTxt.text = BRAND_TO_STR[itemData.brand];
                try
                {
                    brandLogoImg.sprite = ResourceManager.Instance.BrandSpriteDict[itemData.brand];
                }
                catch(KeyNotFoundException)
                {
                    brandLogoImg.sprite = ResourceManager.Instance.BrandSpriteDict[GEAR_BRAND.NO_BRAND];
                    brandNameTxt.text = BRAND_TO_STR[GEAR_BRAND.NO_BRAND];
                }
                mainAbilityImg.sprite = ResourceManager.Instance.AbilitySpriteDict[itemData.ability[0]];
                mainAbilityNameTxt.text = ABILITY_TO_STR[itemData.ability[0]];
                subAbilityImg[0].sprite = ResourceManager.Instance.AbilitySpriteDict[itemData.ability[1]];
            }
        }

        public void SetItemCardUI()
        {
            shop.ItemKeys = DataManager.Instance.GetItemsKey((ITEM_CATEGORY)Shop.CurrentShop);
            for (int i=0; i<shop.ItemKeys.Count; i++) { itemUIList[i].SetItemData(shop.ItemKeys[i]); }
            for(int i= shop.ItemKeys.Count; i<itemUIList.Count; i++) { itemUIList[i].SetLocked(); }
        }

        public void ShakeItemCard(int dir)
        {
            for (int i = 0; i < itemUIList.Count; i++)
            {
                Quaternion a = Quaternion.Euler(0f, 0f, -30f);
                Quaternion b = Quaternion.Euler(0f, 0f, 30f);
                Quaternion midValue = Quaternion.Lerp(a, b, 0.5f); // a와 b의 중간값

                // a와 b 값을 왕복하면서 중간값으로 수렴
                itemUIList[i].Rt.DOBlendablePunchRotation(Vector3.forward * -30f * dir, 1f, 4, 0.5f)
                    .SetRelative()
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() => { itemUIList[i].Rt.rotation = midValue; })
                    .SetDelay(i * 0.05f);
            }
        }

        private void RotateShopNamecard(int dir)
        {
            Quaternion initRot = Quaternion.Euler(0, 0, 0);

            shopNameRt.DORotate(new Vector3(0, 0, 180f * dir), 0.1f)
                .OnComplete(() =>
                {
                    shopNameRt.DORotate(new Vector3(0, 0, 360f * dir), 0.1f)
                    .OnComplete(() => { shopNameRt.rotation = initRot; });
                });
        }

        private void SetActiveBuyPopup(bool isActive)
        {
            var endValue = (isActive ? 1 : 0);
            if (isActive)
            {
                buyPopupCanvasGroup.gameObject.SetActive(true);
                buyPopupCanvasGroup.DOFade(endValue, 0.2f);
            }
            else
            {
                buyPopupCanvasGroup.DOFade(endValue, 0.2f)
                    .OnComplete(() => { buyPopupCanvasGroup.gameObject.SetActive(false); });
            }
        }

        private void ChangeSelectedBuyButton(bool onBuy)
        {
            if (onBuy)
            {
                buyButtonImg.color = BUY_POPUP_SELECTED_COLOR;
                buyButton.Rt.localScale = Vector3.one*1.8f;
                cancelBuyButtonImg.color = ITEM_CATEGORY_COLOR[Shop.CurrentShop];
                cancelBuyButton.Rt.localScale = Vector3.one;
            }
            else
            {
                buyButtonImg.color = ITEM_CATEGORY_COLOR[Shop.CurrentShop];
                buyButton.Rt.localScale = Vector3.one;
                cancelBuyButtonImg.color = BUY_POPUP_SELECTED_COLOR;
                cancelBuyButton.Rt.localScale = Vector3.one*1.8f;
            }
        }



        private IEnumerator PrintNpcDialogueCrt()
        {
            WaitForSeconds wait = new WaitForSeconds(0.03f);
            Color32 halfClear = new Color32(255, 255, 255, 128);
            Vector3 offset = new Vector3(0, 5f, 0);

            // 출력되는 동안은 A버튼을 숨기고, 출력이 끝나면 A버튼 활성화
            aButton.Hide();
            npcDialogueTxt.text = SHOP_WELCOME_MSG[Shop.CurrentShop][currentDialogueIndex++];
            npcDialogueTxt.color = Color.clear;

            var info = npcDialogueTMProUGUI.textInfo;
            yield return new WaitForSeconds(1f);

            // 해당 인덱스의 문장을 한글자씩 표시
            for (int i = 0; i < npcDialogueTxt.text.Length; i++)
            {
                if (npcDialogueTxt.text[i] == ' ') continue;
                var charInfo = info.characterInfo[i];
                var meshInfo = info.meshInfo[charInfo.materialReferenceIndex];
                var color32 = meshInfo.colors32;
                var vertices = meshInfo.vertices;
                var vertexIndex = charInfo.vertexIndex;

                color32[vertexIndex] = halfClear;
                color32[vertexIndex + 1] = halfClear;
                color32[vertexIndex + 2] = halfClear;
                color32[vertexIndex + 3] = halfClear;

                vertices[vertexIndex] += offset;
                vertices[vertexIndex + 1] -= offset;
                vertices[vertexIndex + 2] -= offset;
                vertices[vertexIndex + 3] += offset;

                npcDialogueTMProUGUI.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                npcDialogueTMProUGUI.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                yield return wait;

                color32[vertexIndex] = Color.white;
                color32[vertexIndex + 1] = Color.white;
                color32[vertexIndex + 2] = Color.white;
                color32[vertexIndex + 3] = Color.white;
                vertices[vertexIndex] -= offset;
                vertices[vertexIndex + 1] += offset;
                vertices[vertexIndex + 2] += offset;
                vertices[vertexIndex + 3] -= offset;
                npcDialogueTMProUGUI.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                npcDialogueTMProUGUI.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

                yield return wait;
            }
            npcDialogueTxt.color = Color.white;
            aButton.Show();

            if (currentDialogueIndex == SHOP_WELCOME_MSG[Shop.CurrentShop].Count)
            {
                aButton.onClicked = EndDialogue;
            }
        }
    }

}