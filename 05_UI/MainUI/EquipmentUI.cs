using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class EquipmentUI : MonoBehaviour, ICursorHandler
    {
        [Header("아바타")]
        [SerializeField] private Avatar avatar;

        [Header("커서")]
        [SerializeField] private Cursor cursor;

        [Header("카테고리 카드 그룹")]
        [SerializeField] private Transform categoryCardGroupTr;

        [Header("아이템 카드 그룹")]
        [SerializeField] private Transform itemCardGroupTr;

        [Header("선택 아이템명")]
        [SerializeField] private TMP_Text selectedItemNameTxt;

        [SerializeField] private CanvasGroup exitBtnCanvasGroup;
        [SerializeField] private CanvasGroup backBtnCanvasGroup;
        [SerializeField] private Image whitePanel;

        private int categoryIndex;
        private int focusedItemIndex;
        private int selectedItemIndex;
        private STATE state;
        private List<string> ownedItemKey;
        private List<CategoryCard> categoryCards;
        private List<EquipmentItemCard> itemCards;

        public enum STATE { MAIN, ITEM_SELECT_MODE }





        private void OnEnable()
        {
            InputActionHandler.onOK += Select;
            InputActionHandler.onCancel += MoveBack;
            InputActionHandler.onHorizontalMove += MoveHorizontal;
            InputActionHandler.onVerticalMove += MoveVertical;
        }

        private void Start()
        {

            categoryCards = new List<CategoryCard>();
            for (int i = 0; i < categoryCardGroupTr.childCount; i++)
            {
                categoryCards.Add(categoryCardGroupTr.GetChild(i).GetComponent<CategoryCard>());
            }
            categoryCards.Reverse();
            itemCards = new List<EquipmentItemCard>();
            for (int i = 0; i < itemCardGroupTr.childCount; i++)
            {
                itemCards.Add(itemCardGroupTr.GetChild(i).GetComponent<EquipmentItemCard>());
            }

            Init();
        }

        private void OnDisable()
        {
            InputActionHandler.onOK -= Select;
            InputActionHandler.onCancel -= MoveBack;
            InputActionHandler.onHorizontalMove -= MoveHorizontal;
            InputActionHandler.onVerticalMove -= MoveVertical;
        }



        private void Init()
        {
            categoryIndex = (int)ITEM_CATEGORY.WEAPON;
            for(int i=0; i<categoryCards.Count; i++)
            {
                categoryCards[i].SetCard(i);
            }
            SetItemPanel();
            UpdateCursorPos();
        }

        public void Select()
        {
            if (state.Equals(STATE.MAIN))
            {
                state = STATE.ITEM_SELECT_MODE;
                whitePanel.DOFade(0f, 0.2f);
                exitBtnCanvasGroup.DOFade(0f, 0.2f);
                backBtnCanvasGroup.DOFade(1f, 0.2f);
                itemCards[focusedItemIndex].Focus();
                UpdateCursorPos();
            }
            else if (state.Equals(STATE.ITEM_SELECT_MODE))
            {
                DataManager.Instance.EquipItem(ownedItemKey[focusedItemIndex]);
                categoryCards[categoryIndex].SetCard(categoryIndex);
                itemCards[selectedItemIndex].UnSelect();
                selectedItemIndex = focusedItemIndex;
                itemCards[selectedItemIndex].Select();
            }
        }

        public void MoveBack()
        {
            if (state.Equals(STATE.MAIN))
            {
                Exit();
            }
            else if (state.Equals(STATE.ITEM_SELECT_MODE))
            {
                state = STATE.MAIN;
                focusedItemIndex = 0;
                whitePanel.DOFade(0.2f, 0.2f);
                exitBtnCanvasGroup.DOFade(1f, 0.2f);
                backBtnCanvasGroup.DOFade(0f, 0.2f);
                itemCards[focusedItemIndex].Idle();
                UpdateCursorPos();
                avatar.Init();
            }
        }

        public void MoveHorizontal(int dir)
        {
            if (dir == 0) return;
            if (state.Equals(STATE.MAIN))
            {
                ChangeCategory(dir);
            }
            else if (state.Equals(STATE.ITEM_SELECT_MODE))
            {
                ChangeFocusedItem(dir);
            }
        }

        public void MoveVertical(int dir)
        {
            if (dir == 0) return;
            if (state.Equals(STATE.ITEM_SELECT_MODE))
            {
                ChangeFocusedItem(-dir * 4);
            }
        }



        private void ChangeFocusedItem(int offset)
        {
            if (focusedItemIndex + offset >= ownedItemKey.Count || focusedItemIndex + offset < 0) return;
            itemCards[focusedItemIndex].Idle();

            focusedItemIndex += offset;

            avatar.WearItem(ownedItemKey[focusedItemIndex], (ITEM_CATEGORY)categoryIndex);
            itemCards[focusedItemIndex].Focus();
            UpdateCursorPos();
        }

        private void ChangeCategory(int dir)
        {
            // 장비창은 기존의 신발>옷>헤드>무기 순서와 정반대로 배치된 상태이므로
            // 반대방향으로 인덱스 이동
            dir = -dir;

            if(categoryIndex + dir >= 0 && categoryIndex + dir < 4)
            {
                categoryCards[categoryIndex].Idle();

                categoryIndex += dir;

                categoryCards[categoryIndex].Focus();
                UpdateCursorPos();
                avatar.Init();
                SetItemPanel();
            }
        }

        public void UpdateCursorPos()
        {
            if (state.Equals(STATE.MAIN))
            {
                cursor.rt.position = categoryCards[categoryIndex].CursorPointRt.position;
            }
            if (state.Equals(STATE.ITEM_SELECT_MODE))
            {
                cursor.rt.position = itemCards[focusedItemIndex].CursorPointRt.position;
            }
        }

        private void SetItemPanel()
        {
            if (((ITEM_CATEGORY)categoryIndex).Equals(ITEM_CATEGORY.WEAPON)) avatar.InitWeapon();

            ownedItemKey = DataManager.Instance.GetOwnedItemsKey((ITEM_CATEGORY)categoryIndex);
            for(int i=0; i<ownedItemKey.Count; i++)
            {
                if (DataManager.Instance.IsPlayerEquipped(ownedItemKey[i]))
                {
                    var tmp = ownedItemKey[i];
                    ownedItemKey[i] = ownedItemKey[0];
                    ownedItemKey[0] = tmp;
                }
            }

            focusedItemIndex = 0;
            selectedItemIndex = 0;
            selectedItemNameTxt.text = DataManager.Instance.GetItemName(ownedItemKey[0]);

            for (int i = 0; i < ownedItemKey.Count; i++)
            {
                itemCards[i].SetActive(true);
                itemCards[i].SetItemCard(ownedItemKey[i]);
                if (DataManager.Instance.IsPlayerEquipped(ownedItemKey[i]))
                {
                    itemCards[i].Select();
                }
            }
            for(int i=ownedItemKey.Count; i<itemCards.Count; i++)
            {
                itemCards[i].SetActive(false);
            }

        }

        private void Exit()
        {
            SaveAndLoad.Instance.Save();
            GameManager.Instance.ChangeScene(SCENE.MODESELECT);
        }
    }

}