using System;
using UnityEngine;
using YUnity;

public class GameRoot : MonoBehaviour
{
    [Header("游戏UI")]
    [SerializeField] private GameUIMgr gameUIMgr;

    [Header("新手引导UI")]
    [SerializeField] private NewbieGuideUIMgr newbieGuideUIMgr;

    [Header("全局UI：loading、tips之类")]
    [SerializeField] private GlobalUIMgr globalUIMgr;

    private static GameRoot Instance = null;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            Init(InitDone);
        }
        else
        {
            DestroyImmediate(this);
        }
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    private void Init(Action complete)
    {
        // 不销毁
        DontDestroyOnLoad(gameObject);
        // 打印日志设置
        Debug.unityLogger.logEnabled = AppCfg.IsEnableLog;
        // 设置帧率
        Application.targetFrameRate = AppCfg.TargetFrameRateDefault;
        // 降分辨率(提升渲染性能)
        // 获取设备原生分辨率
        Resolution nativeRes = Screen.currentResolution;
        // 基于原生分辨率，设置游戏分辨率
        Screen.SetResolution((int)(nativeRes.width * 0.8f), (int)(nativeRes.height * 0.8f), false);
        // 锁定为标准竖屏
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        // 类库初始化
        YSRoot.Init(AppCfg.IsEnableLog);
        // 一些全局类初始化
        gameObject.AddComponent<SceneLoader>().Init();
        // UI管理器初始化
        gameUIMgr.Init();
        newbieGuideUIMgr.Init();
        globalUIMgr.Init();
        // 所有初始化完成后，下载热更资源
        HotUpdateWnd.Instance.CheckAndDownload(complete);
    }
    private void InitDone()
    {
        // 初始化华佗
        gameObject.AddComponent<HotUpdateAssembly>().Init();
        // 跳转场景
        string targetScene = string.IsNullOrWhiteSpace(CurUser.Instance.token) ? SceneNames.Login : SceneNames.Lobby;
        SceneMag.Instance.LoadSceneAsync(targetScene, null, null, null);
    }
}