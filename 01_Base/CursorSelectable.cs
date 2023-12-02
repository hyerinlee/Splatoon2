using UnityEngine;

namespace Splatoon2
{
    public class CursorSelectable : MonoBehaviour
    {
        [SerializeField] protected RectTransform rt;
        [SerializeField] protected RectTransform cursorPointRt;

        public RectTransform Rt { get => rt; }
        public RectTransform CursorPointRt { get => cursorPointRt; }
    }
}