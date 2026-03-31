using UnityEngine;

namespace YUnity
{
    public enum HorizontalType
    {
        Left, Cener, Right
    }
    public enum VerticalType
    {
        Left, Cener, Right
    }
    public static class UILayoutUtil
    {
        /// <summary>
        /// 四周拉伸(内置：Inset向里为正，向外为负)
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="leftInset">内置：Inset向里为正，向外为负</param>
        /// <param name="rightInset">内置：Inset向里为正，向外为负</param>
        /// <param name="topInset">内置：Inset向里为正，向外为负</param>
        /// <param name="bottomInset">内置：Inset向里为正，向外为负</param>
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
        /// 垂直拉伸，水平固定(内置：Inset向里为正，向外为负)
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="horizontalType"></param>
        /// <param name="topInset">(内置：Inset向里为正，向外为负)</param>
        /// <param name="bottomInset">(内置：Inset向里为正，向外为负)</param>
        /// <param name="posX"></param>
        /// <param name="width"></param>
        public static void StretchVertical(RectTransform rt, HorizontalType horizontalType, float topInset, float bottomInset, float posX, float width)
        {
            if (rt != null)
            {
                switch (horizontalType)
                {
                    case HorizontalType.Cener:
                        rt.pivot = Vector2.one * 0.5f;
                        rt.anchorMin = new Vector2(0.5f, 0);
                        rt.anchorMax = new Vector2(0.5f, 1);
                        break;
                    case HorizontalType.Left:
                        rt.pivot = new Vector2(0, 0.5f);
                        rt.anchorMin = Vector2.zero;
                        rt.anchorMax = new Vector2(0, 1);
                        break;
                    case HorizontalType.Right:
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
        /// 水平拉伸、垂直固定(内置：Inset向里为正，向外为负)
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="verticalType"></param>
        /// <param name="leftInset">内置：Inset向里为正，向外为负</param>
        /// <param name="rightInset">内置：Inset向里为正，向外为负</param>
        /// <param name="posY"></param>
        /// <param name="height"></param>
        public static void StretchHorizontal(RectTransform rt, VerticalType verticalType, float leftInset, float rightInset, float posY, float height)
        {
        }
    }
}