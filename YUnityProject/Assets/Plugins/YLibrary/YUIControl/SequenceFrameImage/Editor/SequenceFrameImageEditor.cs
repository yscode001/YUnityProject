#if UNITY_EDITOR
using UnityEditor;

namespace YUIControl
{
    [CustomEditor(typeof(SequenceFrameImage))]
    public class SequenceFrameImageEditor : UnityEditor.UI.ImageEditor
    {

    }
}
#endif