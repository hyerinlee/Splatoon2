using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace Splatoon2
{
    public partial class PartialShopUI : MainUIBase
    {
        [Header("커서")]
        [SerializeField] private Cursor cursor;

        [Header("코인")]
        [SerializeField] private TMP_Text cashTxt;

        [Header("가게 정보")]
        [SerializeField] private RectTransform shopNameRt;
        [SerializeField] private Image shopNameColorImg;
        [SerializeField] private TMP_Text shopNameTxt, shopCategoryTxt;
        [SerializeField] private Image leftShopIconImg, rightShopIconImg;
        [SerializeField] private Sprite[] shopIcons = new Sprite[4];

        [Header("NPC 말풍선")]
        [SerializeField] private CanvasGroup npcBubbleCanvasGroup;
        [SerializeField] private TMP_Text npcNameTxt;
        [SerializeField] private TMP_Text npcDialogueTxt;
        [SerializeField] private TextMeshProUGUI npcDialogueTMProUGUI;
        [SerializeField] private Button aButton;

        [Header("아이템 정보 카드(공통)")]
        [SerializeField] private GameObject itemDataObject;
        [SerializeField] private GameObject lockedObject;
        [SerializeField] private Image gearItemDetailCardImg;
        [SerializeField] private Image itemThumbnailImg;
        [SerializeField] private TMP_Text itemNameTxt, priceTxt;

        [Header("아이템 정보 카드(기어)")]
        [SerializeField] private GameObject gearData;
        [SerializeField] private Image brandLogoImg, mainAbilityImg;
        [SerializeField] private Image[] subAbilityImg = new Image[3];
        [SerializeField] private TMP_Text brandNameTxt, mainAbilityNameTxt;

        [Header("아이템 정보 카드(무기)")]
        [SerializeField] private GameObject weaponData;
        [SerializeField] private Image subWeaponImg;
        [SerializeField] private TMP_Text subWeaponNameTxt;
        [SerializeField] private TMP_Text[] specNameTxt = new TMP_Text[3];
        [SerializeField] private Slider[] specPercentage = new Slider[3];

        [Header("판매 물품 그룹")]
        [SerializeField] private RectTransform itemContent;
        private List<ShopItemCard> itemUIList;

        [Header("하단 물결 패턴 이미지")]
        [SerializeField] private Image bottomWavePatternImg;

        [Header("페이드 UI")]
        [SerializeField] private RectTransform fadableRt;
        private CanvasGroup fadableCanvasGroup;

        [Header("구매 팝업 및 버튼")]
        [SerializeField] private CanvasGroup buyPopupCanvasGroup;
        [SerializeField] private CursorSelectable buyButton, cancelBuyButton;
        private Image buyButtonImg, cancelBuyButtonImg;

        private Shop shop;

        public enum STATE { SHOP_MAIN, SHOP_BUY }

        private static STATE state;

        private bool onBuy = false;

        private int currentDialogueIndex = 0;

        public static STATE State
        {
            get => state;
            set => state = value;
        }





        private void OnEnable()
        {
            shop = gameObject.GetComponent<Shop>();

            UpdateCashTxt();
            shop.OnBuy += UpdateCashTxt;
            shop.OnBuy += SetItemCardUI;
        }

        protected override void Start()
        {
            base.Start();

            itemUIList = new List<ShopItemCard>(itemContent.GetComponentsInChildren<ShopItemCard>());
            fadableCanvasGroup = fadableRt.GetComponent<CanvasGroup>();
            buyButtonImg = buyButton.GetComponent<Image>();
            cancelBuyButtonImg = cancelBuyButton.GetComponent<Image>();

            shop.MoveShopAvatar();
            SetShopUI();
            DOVirtual.DelayedCall(0.1f, SetItemCardUI);
            DOVirtual.DelayedCall(1f, StartDialogue);
        }

        private void OnDisable()
        {
            shop.OnBuy -= UpdateCashTxt;
            shop.OnBuy -= SetItemCardUI;
            InputActionHandler.onHorizontalMove -= MoveHorizontal;
            InputActionHandler.onMoveByLR -= GoToAnotherShop;
            InputActionHandler.onOK -= Select;
            InputActionHandler.onCancel -= MoveBack;
            InputActionHandler.onTry -= shop.Try;
        }



        private void StartDialogue()
        {
            npcDialogueTxt.ForceMeshUpdate();

            InputActionHandler.onOK += aButton.Click;
            aButton.onClicked = PrintNextNpcDialogue;
            ShowNpcBubble();
        }

        /// <summary>
        /// 입력 이벤트를 업데이트하고, 상점 기능을 활성화합니다.
        /// </summary>
        private void EndDialogue()
        {
            InputActionHandler.onHorizontalMove += MoveHorizontal;
            InputActionHandler.onMoveByLR += GoToAnotherShop;
            InputActionHandler.onOK -= aButton.Click;
            InputActionHandler.onOK += Select;
            InputActionHandler.onCancel += MoveBack;
            InputActionHandler.onTry += shop.Try;

            HideNpcBubble(ShowFadableObject);
            InitCursor();
        }

        public void GoToAnotherShop(int dir)
        {
            if (state.Equals(STATE.SHOP_MAIN))
            {
                InputActionHandler.Instance.Active(false);
                Shop.CurrentShop += dir;
                shop.SelectedItemIndex = 0;
                shop.MoveShopAvatar();
                onBuy = false;

                shop.ResetAvatar();
                RotateShopNamecard(dir);
                ShakeItemCard(dir);
                SetShopUI();
                SetItemCardUI();
                HideFadableObject(ShowFadableObject);
                InitCursor();
                itemUIList[shop.SelectedItemIndex].Focus();
            }
        }
    }

}