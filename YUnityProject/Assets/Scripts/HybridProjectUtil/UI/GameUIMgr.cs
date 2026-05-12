using UnityEngine;

public class GameUIMgr : MonoBehaviour
{
    public static GameUIMgr Instance { get; private set; } = null;

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