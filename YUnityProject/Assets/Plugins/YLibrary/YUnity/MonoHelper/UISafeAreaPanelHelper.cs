using UnityEngine;

namespace YUnity
{
    /// <summary>
    /// UI安全区面板
    /// </summary>
    public class UISafeAreaPanelHelper : MonoBehaviour
    {
        private RectTransform target;

        public float HasNoSafeAreaHorizontalTotalPadding = 0f;
        public float HasNoSafeAreaVerticalTotalPadding = 0f;

        private void Awake()
        {
            target = GetComponent<RectTransform>();

            Rect safearea = Screen.safeArea;
            if (safearea.width != Screen.width || safearea.height != Screen.height)
            {
                // 曲面屏
                var anchorMin = safearea.position;
                var anchorMax = safearea.position + safearea.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;
                target.anchorMin = anchorMin;
                target.anchorMax = anchorMax;
            }
            else if (HasNoSafeAreaHorizontalTotalPadding != 0 || HasNoSafeAreaVerticalTotalPadding != 0)
            {
                // 非曲面屏，根据设置的值进行缩进
                float x = Mathf.Abs(HasNoSafeAreaHorizontalTotalPadding) * 0.5f;
                float y = Mathf.Abs(HasNoSafeAreaVerticalTotalPadding) * 0.5f;
                float wid = Screen.width - Mathf.Abs(HasNoSafeAreaHorizontalTotalPadding);
                float hei = Screen.height - Mathf.Abs(HasNoSafeAreaVerticalTotalPadding);
                safearea = new Rect(x, y, wid, hei);

                var anchorMin = safearea.position;
                var anchorMax = safearea.position + safearea.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;
                target.anchorMin = anchorMin;
                target.anchorMax = anchorMax;
            }
        }
    }
}