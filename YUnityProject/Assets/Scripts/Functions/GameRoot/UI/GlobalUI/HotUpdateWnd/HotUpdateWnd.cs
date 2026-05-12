using UnityEngine;
using UnityEngine.UI;
using YUIControl;

public class HotUpdateWnd : MonoBehaviour
{
    [SerializeField] private ProgressBarImage progressImg;
    [SerializeField] private Text tipsText;

    public static HotUpdateWnd Instance { get; private set; } = null;

    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(Instance);
        }
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}