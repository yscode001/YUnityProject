using System.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(ProgressBarImage))]
public class ProgressBarImageInspector : ImageEditor
{
    SerializedProperty m_FillMethod;
    SerializedProperty m_FillOrigin;
    SerializedProperty m_FillAmount;
    SerializedProperty m_FillClockwise;
    SerializedProperty m_Type;
    SerializedProperty m_FillCenter;
    SerializedProperty m_Sprite;
    SerializedProperty m_PreserveAspect;
    GUIContent m_SpriteContent;
    GUIContent m_SpriteTypeContent;
    GUIContent m_ClockwiseContent;
    AnimBool m_ShowSlicedOrTiled;
    AnimBool m_ShowSliced;
    AnimBool m_ShowFilled;
    AnimBool m_ShowType;

    void SetShowNativeSize(bool instant)
    {
        ProgressBarImage.Type type = (ProgressBarImage.Type)m_Type.enumValueIndex;
        bool showNativeSize = (type == ProgressBarImage.Type.Simple || type == ProgressBarImage.Type.Filled);
        base.SetShowNativeSize(showNativeSize, instant);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        m_SpriteContent = new GUIContent("Source childImage");
        m_SpriteTypeContent = new GUIContent("childImage Type");
        m_ClockwiseContent = new GUIContent("Clockwise");

        m_Sprite = serializedObject.FindProperty("m_Sprite");
        m_Type = serializedObject.FindProperty("m_Type");
        m_FillCenter = serializedObject.FindProperty("m_FillCenter");
        m_FillMethod = serializedObject.FindProperty("m_FillMethod");
        m_FillOrigin = serializedObject.FindProperty("m_FillOrigin");
        m_FillClockwise = serializedObject.FindProperty("m_FillClockwise");
        m_FillAmount = serializedObject.FindProperty("m_FillAmount");
        m_PreserveAspect = serializedObject.FindProperty("m_PreserveAspect");

        m_ShowType = new AnimBool(m_Sprite.objectReferenceValue != null);
        m_ShowType.valueChanged.AddListener(Repaint);

        var typeEnum = (ProgressBarImage.Type)m_Type.enumValueIndex;
        m_ShowSlicedOrTiled = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == ProgressBarImage.Type.Sliced);
        m_ShowSliced = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == ProgressBarImage.Type.Sliced);
        m_ShowFilled = new AnimBool(!m_Type.hasMultipleDifferentValues && typeEnum == ProgressBarImage.Type.Filled);
        m_ShowSlicedOrTiled.valueChanged.AddListener(Repaint);
        m_ShowSliced.valueChanged.AddListener(Repaint);
        m_ShowFilled.valueChanged.AddListener(Repaint);

        SetShowNativeSize(true);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SpriteGUI();
        AppearanceControlsGUI();
        RaycastControlsGUI();

        m_ShowType.target = m_Sprite.objectReferenceValue != null;
        if (EditorGUILayout.BeginFadeGroup(m_ShowType.faded))
        {
            EditorGUILayout.PropertyField(m_Type, m_SpriteTypeContent);

            ++EditorGUI.indentLevel;
            {
                ProgressBarImage.Type typeEnum = (ProgressBarImage.Type)m_Type.enumValueIndex;
                bool showSlicedOrTiled = (!m_Type.hasMultipleDifferentValues && (typeEnum == ProgressBarImage.Type.Sliced || typeEnum == ProgressBarImage.Type.Tiled));
                if (showSlicedOrTiled && targets.Length > 1)
                    showSlicedOrTiled = targets.Select(obj => obj as ProgressBarImage).All(img => img.hasBorder);

                m_ShowSlicedOrTiled.target = showSlicedOrTiled;
                m_ShowSliced.target = (showSlicedOrTiled && !m_Type.hasMultipleDifferentValues && typeEnum == ProgressBarImage.Type.Sliced);
                m_ShowFilled.target = (!m_Type.hasMultipleDifferentValues && typeEnum == ProgressBarImage.Type.Filled);

                ProgressBarImage cImage = target as ProgressBarImage;

                if (EditorGUILayout.BeginFadeGroup(m_ShowSlicedOrTiled.faded))
                {
                    if (cImage.hasBorder)
                    {
                        EditorGUILayout.PropertyField(m_FillCenter);
                        EditorGUILayout.PropertyField(m_FillAmount);
                    }

                }
                EditorGUILayout.EndFadeGroup();

                if (EditorGUILayout.BeginFadeGroup(m_ShowSliced.faded))
                {
                    if (cImage.sprite != null && !cImage.hasBorder)
                        EditorGUILayout.HelpBox("This childImage doesn't have a border.", MessageType.Warning);
                }
                EditorGUILayout.EndFadeGroup();

                if (EditorGUILayout.BeginFadeGroup(m_ShowFilled.faded))
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(m_FillMethod);
                    if (EditorGUI.EndChangeCheck())
                    {
                        m_FillOrigin.intValue = 0;
                    }
                    switch ((ProgressBarImage.FillMethod)m_FillMethod.enumValueIndex)
                    {
                        case ProgressBarImage.FillMethod.Horizontal:
                            m_FillOrigin.intValue = (int)(ProgressBarImage.OriginHorizontal)EditorGUILayout.EnumPopup("Fill Origin", (ProgressBarImage.OriginHorizontal)m_FillOrigin.intValue);
                            break;
                        case ProgressBarImage.FillMethod.Vertical:
                            m_FillOrigin.intValue = (int)(ProgressBarImage.OriginVertical)EditorGUILayout.EnumPopup("Fill Origin", (ProgressBarImage.OriginVertical)m_FillOrigin.intValue);
                            break;
                        case ProgressBarImage.FillMethod.Radial90:
                            m_FillOrigin.intValue = (int)(ProgressBarImage.Origin90)EditorGUILayout.EnumPopup("Fill Origin", (ProgressBarImage.Origin90)m_FillOrigin.intValue);
                            break;
                        case ProgressBarImage.FillMethod.Radial180:
                            m_FillOrigin.intValue = (int)(ProgressBarImage.Origin180)EditorGUILayout.EnumPopup("Fill Origin", (ProgressBarImage.Origin180)m_FillOrigin.intValue);
                            break;
                        case ProgressBarImage.FillMethod.Radial360:
                            m_FillOrigin.intValue = (int)(ProgressBarImage.Origin360)EditorGUILayout.EnumPopup("Fill Origin", (ProgressBarImage.Origin360)m_FillOrigin.intValue);
                            break;
                    }
                    EditorGUILayout.PropertyField(m_FillAmount);
                    if ((ProgressBarImage.FillMethod)m_FillMethod.enumValueIndex > ProgressBarImage.FillMethod.Vertical)
                    {
                        EditorGUILayout.PropertyField(m_FillClockwise, m_ClockwiseContent);
                    }
                }
                EditorGUILayout.EndFadeGroup();
            }
            --EditorGUI.indentLevel;
        }

        EditorGUILayout.EndFadeGroup();

        SetShowNativeSize(false);
        if (EditorGUILayout.BeginFadeGroup(m_ShowNativeSize.faded))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_PreserveAspect);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();
        NativeSizeButtonGUI();

        serializedObject.ApplyModifiedProperties();
    }
}