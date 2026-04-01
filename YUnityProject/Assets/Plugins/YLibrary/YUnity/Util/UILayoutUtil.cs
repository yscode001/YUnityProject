using UnityEngine;

namespace YUnity
{
    public enum XAxisPos
    {
        Left, Cener, Right
    }
    public enum YAxisPos
    {
        Top, Cener, Bottom
    }
    public static class UILayoutUtil
    {
        /// <summary>
        /// 四周拉伸(Inset向里为正，向外为负)
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="leftInset">Inset向里为正，向外为负</param>
        /// <param name="rightInset">Inset向里为正，向外为负</param>
        /// <param name="topInset">Inset向里为正，向外为负</param>
        /// <param name="bottomInset">Inset向里为正，向外为负</param>
        public static void Stretch(RectTransform rt, float leftInset, float rightInset, float topInset, float bottomInset)
        {
            if (rt != null)
            {
                rt.pivot = Vector2.one * 0.5f;
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = new Vector2(leftInset, bottomInset);
                rt.offsetMax = new Vector2(-rightInset, -topInset);
            }
        }

        /// <summary>
        /// 垂直拉伸，水平固定(Inset向里为正，向外为负)
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="xAxisPos"></param>
        /// <param name="topInset">Inset向里为正，向外为负</param>
        /// <param name="bottomInset">Inset向里为正，向外为负</param>
        /// <param name="posX"></param>
        /// <param name="width"></param>
        public static void StretchVertical(RectTransform rt, XAxisPos xAxisPos, float topInset, float bottomInset, float posX, float width)
        {
            if (rt != null)
            {
                switch (xAxisPos)
                {
                    case XAxisPos.Cener:
                        rt.pivot = Vector2.one * 0.5f;
                        rt.anchorMin = new Vector2(0.5f, 0);
                        rt.anchorMax = new Vector2(0.5f, 1);
                        break;
                    case XAxisPos.Left:
                        rt.pivot = new Vector2(0, 0.5f);
                        rt.anchorMin = Vector2.zero;
                        rt.anchorMax = new Vector2(0, 1);
                        break;
                    case XAxisPos.Right:
                        rt.pivot = new Vector2(1, 0.5f);
                        rt.anchorMin = new Vector2(1, 0);
                        rt.anchorMax = new Vector2(1, 1);
                        break;
                    default: break;
                }

                rt.offsetMin = new Vector2(0, bottomInset);
                rt.offsetMax = new Vector2(0, -topInset);

                Vector2 anchoredPos = rt.anchoredPosition;
                anchoredPos.x = posX;
                rt.anchoredPosition = anchoredPos;

                Vector2 sizeDelta = rt.sizeDelta;
                sizeDelta.x = width;
                rt.sizeDelta = sizeDelta;
            }
        }

        /// <summary>
        /// 水平拉伸、垂直固定(Inset向里为正，向外为负)
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="yAxisPos"></param>
        /// <param name="leftInset">Inset向里为正，向外为负</param>
        /// <param name="rightInset">Inset向里为正，向外为负</param>
        /// <param name="posY"></param>
        /// <param name="height"></param>
        public static void StretchHorizontal(RectTransform rt, YAxisPos yAxisPos, float leftInset, float rightInset, float posY, float height)
        {
            if (rt != null)
            {
                switch (yAxisPos)
                {
                    case YAxisPos.Cener:
                        rt.pivot = Vector2.one * 0.5f;
                        rt.anchorMin = new Vector2(0, 0.5f);
                        rt.anchorMax = new Vector2(1, 0.5f);
                        break;
                    case YAxisPos.Top:
                        rt.pivot = new Vector2(0.5f, 1);
                        rt.anchorMin = new Vector2(0, 1);
                        rt.anchorMax = Vector2.one;
                        break;
                    case YAxisPos.Bottom:
                        rt.pivot = new Vector2(0.5f, 0);
                        rt.anchorMin = Vector2.zero;
                        rt.anchorMax = new Vector2(1, 0);
                        break;
                    default: break;
                }

                rt.offsetMin = new Vector2(leftInset, 0);
                rt.offsetMax = new Vector2(-rightInset, 0);

                Vector2 anchoredPos = rt.anchoredPosition;
                anchoredPos.y = posY;
                rt.anchoredPosition = anchoredPos;

                Vector2 sizeDelta = rt.sizeDelta;
                sizeDelta.y = height;
                rt.sizeDelta = sizeDelta;
            }
        }
    }
}