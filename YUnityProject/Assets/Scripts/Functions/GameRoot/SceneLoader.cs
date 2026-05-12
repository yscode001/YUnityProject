using System;
using UniRx;
using UnityEngine;

public partial class SceneLoader : MonoBehaviour
{

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
    private string _prevSceneName = null;

    private readonly Subject<SceneLoadedModel> _sceneSwitchFinished = new Subject<SceneLoadedModel>();
    public IObservable<SceneLoadedModel> SceneSwitchFinished => _sceneSwitchFinished;

    public void SceneSwitchFinishedAndInitDone(string newSceneName)
    {
        SceneLoadedModel loadedModel = new SceneLoadedModel(_prevSceneName, newSceneName);
        _prevSceneName = newSceneName;
        _sceneSwitchFinished.OnNext(loadedModel);
    }
}
#endregion