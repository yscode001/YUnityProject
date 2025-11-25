using UnityEngine;
using YUIControl;

public class HotUpdateWnd : MonoBehaviour
{
    [SerializeField] private I18N_Image progressImg;
    [SerializeField] private I18N_Text tipsText;

    public static HotUpdateWnd Instance { get; private set; } = null;

    public void Init()
    {
        Instance = this;
    }
}