using System.Collections;
using UnityEngine;

namespace Splatoon2
{
    /// <summary>
    /// 각 씬의 메인 UI 스크립트에서 상속받습니다. <br/>
    /// (단, 씬 이동 시에 사라지는 UI 캔버스만 해당)
    /// </summary>
    public class MainUIBase : MonoBehaviour
    {
        [Header("최상위 캔버스")]
        [SerializeField] private CanvasGroup topCanvasGroup;

        protected virtual void Start()
        {
            GameManager.Instance.hideMainCanvasCrt = HideAll();
        }

        protected IEnumerator ShowAll()
        {
            float time = 0f;
            while (time < 0.5f)
            {
                time += Time.deltaTime;
                topCanvasGroup.alpha = time * 2;
                yield return null;
            }
            topCanvasGroup.alpha = 1;
        }

        protected IEnumerator HideAll()
        {
            float time = 0.5f;
            while (time > 0f)
            {
                time -= Time.deltaTime;
                topCanvasGroup.alpha = time * 2;
                yield return null;
            }
            topCanvasGroup.alpha = 0;
        }
    }
}