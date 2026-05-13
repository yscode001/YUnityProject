using UnityEngine;

public class NewbieGuideUIMgr : MonoBehaviour
{
    public static NewbieGuideUIMgr Instance { get; private set; } = null;

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