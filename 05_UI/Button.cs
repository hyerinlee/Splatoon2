using UnityEngine;
using DG.Tweening;

namespace Splatoon2
{
    public class Button : MonoBehaviour
    {
        [SerializeField] private RectTransform buttonRt;
        [SerializeField] private CanvasGroup buttonCanvasGroup;

        public TweenCallback onShow, onHide, onClicked;

        private bool canClick = true;
        private Vector3 shrinkScale = Vector3.one * -0.2f;





        private void Start()
        {
            onShow += ()=> { canClick = true; };
        }



        public void Show()
        {
            canClick = false;
            buttonCanvasGroup.DOFade(1, 0.5f)
                .OnComplete(onShow);
        }

        public void Hide()
        {
            canClick = false;
            buttonCanvasGroup.DOFade(0, 0.5f)
                .OnComplete(onHide);
        }
        
        public void Click()
        {
            if(canClick)
            {
                canClick = false;
                buttonRt.DOPunchScale(shrinkScale, 0.3f, 1, 1)
                    .OnComplete(() =>
                    {
                        onClicked?.Invoke();
                        canClick = true;
                    });
            }
        }
    }

}