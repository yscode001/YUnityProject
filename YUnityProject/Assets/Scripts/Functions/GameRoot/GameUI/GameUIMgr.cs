using UnityEngine;

public class GameUIMgr : MonoBehaviour
{
    public static GameUIMgr Instance { get; private set; } = null;

    public void Init()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}