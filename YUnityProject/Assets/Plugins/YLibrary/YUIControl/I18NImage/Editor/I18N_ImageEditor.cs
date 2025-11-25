using UnityEditor;
using UnityEngine;

namespace YUIControl
{
    public class I18N_ImageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        [MenuItem("GameObject/UI/I18N_Image")]
        private static void GenerateImageY()
        {
            GameObject go = new GameObject("I18N_Image");
            go.layer = LayerMask.NameToLayer("UI");

            go.AddComponent<I18N_Image>();

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