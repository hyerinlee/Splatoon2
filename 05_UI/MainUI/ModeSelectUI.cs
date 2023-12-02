using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class ModeSelectUI : MainUIBase
    {
        [Header("커서")]
        [SerializeField] private RectTransform cursorRt;

        [Header("모드 카드 & 설명")]
        [SerializeField] private RectTransform cardPanelRt;
        [SerializeField] private List<RectTransform> cardRt = new List<RectTransform>();
        [SerializeField] private List<RectTransform> detailRt = new List<RectTransform>();

        [Header("전광판")]
        [SerializeField] private List<Texture> billboardColors = new List<Texture>();
        [SerializeField] private GameObject billboard;

        public enum CARD { RANK, REGULAR, FRIENDS, PRIVATE }

        private Material billboardMat;
        private int selectedCardIndex;
        private int moveCardPanelOffset = 360;
        private int moveDetailPanelOffset = 270;
        private Vector3 scaleOffset = new Vector3(1.2f, 1.3f, 1.2f);
        private Vector3 rotateOffset = new Vector3(0, 0, 7);





        private void OnEnable()
        {
            InputActionHandler.onOK += GoToIngame;
            InputActionHandler.onCancel += Exit;
            InputActionHandler.onPressE += GoToEquip;
            InputActionHandler.onVerticalMove += MovePanel;
        }

        protected override void Start()
        {
            base.Start();
            billboardMat = billboard.GetComponent<Renderer>().material;
            selectedCardIndex = (int)CARD.REGULAR;

            StartCoroutine(ShowMainCanvas());
        }

        private void OnDisable()
        {
            InputActionHandler.onOK -= GoToIngame;
            InputActionHandler.onCancel -= Exit;
            InputActionHandler.onPressE -= GoToEquip;
            InputActionHandler.onVerticalMove -= MovePanel;
        }



        private void MovePanel(int dir)
        {
            dir = -dir; // 위방향 +1, 아래방향 -1이므로 뒤집어 주어야 함
            if (selectedCardIndex + dir >= 0 && selectedCardIndex + dir <= 3)
            {
                InputActionHandler.Instance.Active(false);
                cardPanelRt.DOLocalMoveY(cardPanelRt.localPosition.y + dir * moveCardPanelOffset, 0.2f)
                    .OnComplete(() =>
                    {
                        InputActionHandler.Instance.Active(true);
                    });
                SetCardIdle();
                selectedCardIndex += dir;
                SetCardFocus();
                billboardMat.mainTexture = billboardColors[selectedCardIndex];
            }
        }

        private void SetCardIdle()
        {
            detailRt[selectedCardIndex].gameObject.SetActive(false);
            detailRt[selectedCardIndex].DOAnchorPosX(moveDetailPanelOffset, 0);

            cardRt[selectedCardIndex].DOScale(1f, 0.2f);
            cardRt[selectedCardIndex].DOLocalRotate(Vector3.zero, 0.2f);
        }

        private void SetCardFocus()
        {
            detailRt[selectedCardIndex].gameObject.SetActive(true);
            detailRt[selectedCardIndex].DOAnchorPosX(-moveDetailPanelOffset, 0.2f);

            cardRt[selectedCardIndex].DOScale(scaleOffset, 0.2f);
            cardRt[selectedCardIndex].DOLocalRotate(rotateOffset, 0.2f);
        }

        private void GoToIngame()
        {
            if (!selectedCardIndex.Equals((int)CARD.PRIVATE)) return;
            GameManager.Instance.ChangeScene(SCENE.INGAME);
        }

        private void GoToEquip()
        {
            GameManager.Instance.ChangeScene(SCENE.EQUIPMENT);
        }

        private void Exit()
        {
            GameManager.Instance.ChangeScene(SCENE.INKOPOLIS);
        }



        private IEnumerator ShowMainCanvas()
        {
            yield return ShowAll();

            cursorRt.gameObject.SetActive(true);
            SetCardFocus();
        }
    }
}