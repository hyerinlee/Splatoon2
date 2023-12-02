using Cinemachine;
using UnityEngine;
using DG.Tweening;
using static Splatoon2.Define;

namespace Splatoon2
{
    public class InkopolisUI : MainUIBase
    {
        [SerializeField] private bool isTitleActive;
        [SerializeField] private CanvasGroup titleCanvasGroup;
        [SerializeField] private GameObject mainCanvas;
        [SerializeField] private CinemachineBrain cameraBrain;
        [SerializeField] private CinemachineFreeLook playerCam;

        private CinemachineInputProvider mainCaminputProvider;





        private void OnEnable()
        {
            mainCaminputProvider = playerCam.GetComponent<CinemachineInputProvider>();
            GameManager.Instance.onSceneChange += DisableCameraInput;

            if (GameManager.Instance.IsTitlePassed) isTitleActive = false;
            if (!isTitleActive)
            {
                GameManager.Instance.IsTitlePassed = true;
                titleCanvasGroup.alpha = 0f;
                EnableMainUI();
            }
            else
            {
                InputActionHandler.onStart += EndTitle;
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        private void OnDisable()
        {
            GameManager.Instance.onSceneChange -= DisableCameraInput;
        }



        public void DisableCameraInput()
        {
            mainCaminputProvider.enabled = false;
        }

        private void EndTitle()
        {
            InputActionHandler.onStart -= EndTitle;
            EnableMainUI();
        }

        /// <summary>
        /// 1) Ÿ��Ʋ UI ��Ȱ��ȭ<br />
        /// 2) ī�޶� ��ȯ(Ÿ��Ʋ ī�޶� -> �÷��̾� ī�޶�)<br />
        /// 3) ���� UI Ȱ��ȭ<br />
        /// 4) 1�� �� => �÷��̾� ī�޶� ��ǲ Ȱ��ȭ, �׼Ǹ� ����, OnSceneLoaded �׼� ���
        /// </summary>
        private void EnableMainUI()
        {
            titleCanvasGroup.DOFade(0, 0.2f)
                   .OnComplete(() =>
                   {
                       isTitleActive = false;
                       titleCanvasGroup.gameObject.SetActive(false);
                       cameraBrain.ActiveVirtualCamera.Priority = 0;
                       playerCam.Priority = 10;
                       mainCanvas.SetActive(true);
                       DOVirtual.DelayedCall(1f, () => {
                           mainCaminputProvider.enabled = true;
                           GameManager.Instance.IsTitlePassed = true;
                           InputActionHandler.Instance.Active(true);
                           InputActionHandler.Instance.ChangeActionMap(ACTION_MAP.PLAYER_INKOPOLIS);
                       });
                   });
        }
    }
}