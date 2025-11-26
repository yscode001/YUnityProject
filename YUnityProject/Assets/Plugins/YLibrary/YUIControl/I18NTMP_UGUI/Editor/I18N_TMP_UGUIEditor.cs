using UnityEditor;
using UnityEngine;

namespace YUIControl
{
    [CustomEditor(typeof(I18N_TMP_UGUI))]
    public class I18N_TMP_UGUIEditor : TMPro.EditorUtilities.TMP_EditorPanelUI
    {
        private SerializedProperty _jianProp;
        private SerializedProperty _fanProp;
        private SerializedProperty _showjianProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            // 关联字段
            _jianProp = serializedObject.FindProperty("Jian");
            _fanProp = serializedObject.FindProperty("Fan");
            _showjianProp = serializedObject.FindProperty("ShowJian");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("国际化属性", EditorStyles.boldLabel);
            serializedObject.Update();
            EditorGUILayout.PropertyField(_jianProp, new GUIContent("Jian"), true, GUILayout.ExpandWidth(true));
            EditorGUILayout.PropertyField(_fanProp, new GUIContent("Fan"), true, GUILayout.ExpandWidth(true));
            EditorGUILayout.PropertyField(_showjianProp);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("基础属性", EditorStyles.boldLabel);
            base.OnInspectorGUI();
        }

        [MenuItem("GameObject/UI/I18N_TMP_UGUI")]
        private static void GenerateText()
        {
            GameObject go = new GameObject("I18N_TMP_UGUI");
            go.layer = LayerMask.NameToLayer("UI");

            I18N_TMP_UGUI text = go.AddComponent<I18N_TMP_UGUI>();
            // text.font = Resources.Load<TMP_FontAsset>("Fonts/Font SDF");
            text.fontSize = 26;

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
            rt.sizeDelta = new Vector2(200, 40);
            Selection.activeGameObject = go;
        }
    }
}