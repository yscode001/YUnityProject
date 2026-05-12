using UnityEngine;

public class GlobalUIMgr : MonoBehaviour
{
    [Header("热更面板")]
    [SerializeField] private HotUpdateWnd hotUpdateWnd;

    [Header("加载中loading")]
    [SerializeField] private LoadingWnd loadingWnd;

    [Header("场景加载loading")]
    [SerializeField] private SceneLoadingWnd sceneLoadingWnd;

    [Header("tips")]
    [SerializeField] private TipsWnd tipsWnd;

    public static GlobalUIMgr Instance { get; private set; } = null;

    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(Instance);
        }
        Instance = this;
        hotUpdateWnd.Init();
        loadingWnd.Init();
        sceneLoadingWnd.Init();
        tipsWnd.Init();
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}