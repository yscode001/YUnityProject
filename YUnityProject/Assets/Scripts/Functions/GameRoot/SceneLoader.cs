using System;
using UniRx;
using UnityEngine;

public partial class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; } = null;

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
#region 切换完成
public readonly struct SceneLoadedModel
{
    public readonly string PrevSceneName;
    public readonly string CurSceneName;

    public SceneLoadedModel(string prevSceneName, string curSceneName)
    {
        PrevSceneName = prevSceneName;
        CurSceneName = curSceneName;
    }
}
public partial class SceneLoader
{
    public string PrevSceneName { get; private set; } = null;
    public string CurSceneName { get; private set; } = null;

    private readonly Subject<SceneLoadedModel> _sceneSwitchFinished = new Subject<SceneLoadedModel>();
    public IObservable<SceneLoadedModel> SceneSwitchFinished => _sceneSwitchFinished;

    public void SceneSwitchFinishedAndInitDone(string newSceneName)
    {
        PrevSceneName = CurSceneName;
        CurSceneName = newSceneName;
        _sceneSwitchFinished.OnNext(new SceneLoadedModel(PrevSceneName, CurSceneName));
    }
}
#endregion