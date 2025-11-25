using UnityEngine;
using YUIControl;

public class TipsWnd : MonoBehaviour
{
    [SerializeField] private I18N_Text tipsText;

    public static TipsWnd Instance { get; private set; } = null;

    public void Init()
    {
        Instance = this;
    }
}