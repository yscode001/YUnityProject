#if UNITY_EDITOR
using UnityEditor;

namespace YUnity
{
    [CustomEditor(typeof(EmptyRaycast))]
    public class EmptyRaycastEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_RaycastTarget"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif