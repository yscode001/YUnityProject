using UnityEngine;

public class GlobalUIMgr : MonoBehaviour
{
    [Header("主模块热更：进游戏时")]
    [SerializeField] private MainHotUpdateWnd mainHotUpdateWnd;

    [Header("功能模块热更：进入某功能时")]
    [SerializeField] private ModuleHotupdateWnd moduleHotupdateWnd;

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
        mainHotUpdateWnd.Init();
        moduleHotupdateWnd.Init();
        loadingWnd.Init();
        sceneLoadingWnd.Init();
        tipsWnd.Init();
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}