#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace YUIControl
{
    [CustomEditor(typeof(ResImage))]
    public class ResImage_Editor : UnityEditor.UI.ImageEditor
    {
        [MenuItem("GameObject/UI/ResImage")]
        private static void GenerateImage()
        {
            GameObject go = new GameObject("ResImage");
            go.layer = LayerMask.NameToLayer("UI");

            go.AddComponent<ResImage>();

            RectTransform rt = (RectTransform)go.transform;
            Transform parent;
            if (Selection.activeTransform)
            {
                parent = Selection.activeTransform;
            }
            else
            {
                parent = FindObjectOfType<Canvas>().transform;
            }
            if (parent != null)
            {
                rt.SetParent(parent);
            }
            rt.localPosition = Vector3.zero;
            rt.localScale = Vector3.one;
            rt.sizeDelta = new Vector2(100, 100);
            Selection.activeGameObject = go;
        }
    }
}
#endif