using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : MonoBehaviour
{
    [SerializeField] private Image iconImg;

    public static LoadingWnd Instance { get; private set; } = null;

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