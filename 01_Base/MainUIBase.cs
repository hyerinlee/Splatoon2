using System.Collections;
using UnityEngine;

namespace Splatoon2
{
    /// <summary>
    /// �� ���� ���� UI ��ũ��Ʈ���� ��ӹ޽��ϴ�. <br/>
    /// (��, �� �̵� �ÿ� ������� UI ĵ������ �ش�)
    /// </summary>
    public class MainUIBase : MonoBehaviour
    {
        [Header("�ֻ��� ĵ����")]
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