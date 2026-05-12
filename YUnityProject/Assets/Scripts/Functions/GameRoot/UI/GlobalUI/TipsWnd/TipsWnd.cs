using UnityEngine;
using UnityEngine.UI;

public class TipsWnd : MonoBehaviour
{
    [SerializeField] private Text tipsText;

    public static TipsWnd Instance { get; private set; } = null;

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