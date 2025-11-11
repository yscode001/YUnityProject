using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class I18N_Image : MonoBehaviour
{
    [Header("简体")]
    [SerializeField] private Sprite Jian;

    [Header("繁体")]
    [SerializeField] private Sprite Fan;

    [Header("Editor预览：是否显示简体")]
    [SerializeField] private bool ShowJian = true;

    private Image image;
    private Image Image
    {
        get
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }
            return image;
        }
    }

    public void Init(bool is_zhcn)
    {
        if (is_zhcn && Jian != null && Image.sprite != Jian)
        {
            Image.sprite = Jian;
        }
        else if (!is_zhcn && Fan != null && Image.sprite != Fan)
        {
            Image.sprite = Fan;
        }
    }

#if UNITY_EDITOR
    // 在编辑器中实时预览效果
    private void OnValidate()
    {
        if (UnityEditor.EditorApplication.isPlaying == false)
        {
            if (ShowJian && Jian != null && Image.sprite != Jian)
            {
                Image.sprite = Jian;
            }
            else if (!ShowJian && Fan != null && Image.sprite != Fan)
            {
                Image.sprite = Fan;
            }
        }
    }
#endif
}