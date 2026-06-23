#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YUnity
{
    public static class EmptyRaycastMenu
    {
        [MenuItem("GameObject/UI/EmptyRaycast 隐形点击热区", false, 10)]
        private static void CreateEmptyRaycastObj()
        {
            Canvas canvas = Object.FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<GraphicRaycaster>();

                GameObject eventSys = new GameObject("EventSystem");
                eventSys.AddComponent<EventSystem>();
                eventSys.AddComponent<StandaloneInputModule>();
            }

            GameObject hotObj = new GameObject("EmptyHotZone");
            hotObj.transform.SetParent(canvas.transform, false);
            RectTransform rt = hotObj.AddComponent<RectTransform>();
            rt.localPosition = Vector3.zero;
            rt.localScale = Vector3.one;
            rt.sizeDelta = new Vector2(100, 100);
            hotObj.AddComponent<EmptyRaycast>();

            Selection.activeGameObject = hotObj;
        }
    }
}
#endif