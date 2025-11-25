using UnityEngine;

public class GlobalUI : MonoBehaviour
{
    [SerializeField] private SceneLoadingWnd sceneLoadingWnd;
    [SerializeField] private LoadingWnd loadingWnd;
    [SerializeField] private HotUpdateWnd hotUpdateWnd;
    [SerializeField] private TipsWnd tipsWnd;

    public static GlobalUI Instance { get; private set; } = null;

    public void Init()
    {
        Instance = this;
        sceneLoadingWnd.Init();
        loadingWnd.Init();
        hotUpdateWnd.Init();
        tipsWnd.Init();
    }
}