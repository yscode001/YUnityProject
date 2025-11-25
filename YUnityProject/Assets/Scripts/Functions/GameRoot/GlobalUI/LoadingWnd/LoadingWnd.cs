using UnityEngine;
using YUIControl;

public class LoadingWnd : MonoBehaviour
{
    [SerializeField] private I18N_Image iconImg;

    public static LoadingWnd Instance { get; private set; } = null;

    public void Init()
    {
        Instance = this;
    }
}