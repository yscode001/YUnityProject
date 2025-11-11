using UnityEngine;
using UnityEngine.UI;

namespace YUnity
{
    public enum ScrollDirection
    {
        Top, Bottom, Left, Right
    }

    /// <summary>
    /// ScrollRect帮助类
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public partial class ScrollRectHelper : MonoBehaviourBaseY
    {
        public ScrollRect SR { get; private set; } = null;
        private readonly float smoothing = 4f;
        private float verDest = -1; // 取值-1  0  1
        private float horDest = -1; // 取值-1  0  1

        protected void Awake()
        {
            SR = gameObject.GetComponent<ScrollRect>();
        }

        private void Update()
        {
            if (verDest == 0)
            {
                if (SR.verticalNormalizedPosition < 0.001) { SR.verticalNormalizedPosition = 0; verDest = -1; }
                else { SR.verticalNormalizedPosition = Mathf.Lerp(SR.verticalNormalizedPosition, verDest, Time.deltaTime * smoothing); }
            }
            else if (verDest == 1)
            {
                if (SR.verticalNormalizedPosition > 0.999) { SR.verticalNormalizedPosition = 1; verDest = -1; }
                else { SR.verticalNormalizedPosition = Mathf.Lerp(SR.verticalNormalizedPosition, verDest, Time.deltaTime * smoothing); }
            }
            if (horDest == 0)
            {
                if (SR.horizontalNormalizedPosition < 0.001) { SR.horizontalNormalizedPosition = 0; horDest = -1; }
                else { SR.horizontalNormalizedPosition = Mathf.Lerp(SR.horizontalNormalizedPosition, horDest, Time.deltaTime * smoothing); }
            }
            else if (horDest == 1)
            {
                if (SR.horizontalNormalizedPosition > 0.999) { SR.horizontalNormalizedPosition = 1; horDest = -1; }
                else { SR.horizontalNormalizedPosition = Mathf.Lerp(SR.horizontalNormalizedPosition, horDest, Time.deltaTime * smoothing); }
            }
        }
    }

    public partial class ScrollRectHelper
    {
        public void ScrollTo(ScrollDirection direction, bool animate)
        {
            if ((direction == ScrollDirection.Top && SR.verticalNormalizedPosition == 1) ||
                (direction == ScrollDirection.Bottom && SR.verticalNormalizedPosition == 0) ||
                (direction == ScrollDirection.Left && SR.horizontalNormalizedPosition == 0) ||
                (direction == ScrollDirection.Right && SR.horizontalNormalizedPosition == 1))
            {
                return;
            }
            if (!animate)
            {
                switch (direction)
                {
                    case ScrollDirection.Top:
                        SR.verticalNormalizedPosition = 1; return;
                    case ScrollDirection.Bottom:
                        SR.verticalNormalizedPosition = 0; return;
                    case ScrollDirection.Left:
                        SR.horizontalNormalizedPosition = 0; return;
                    case ScrollDirection.Right:
                        SR.horizontalNormalizedPosition = 1; return;
                    default: return;
                }
            }
            switch (direction)
            {
                case ScrollDirection.Top:
                    verDest = 1; return;
                case ScrollDirection.Bottom:
                    verDest = 0; return;
                case ScrollDirection.Left:
                    horDest = 0; return;
                case ScrollDirection.Right:
                    horDest = 1; return;
                default: return;
            }
        }
    }
}