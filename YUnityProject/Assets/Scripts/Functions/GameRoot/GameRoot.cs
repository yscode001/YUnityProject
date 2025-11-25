using UnityEngine;
using YUnity;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private GlobalUI globalUI;
    [SerializeField] private GameUIMgr gameUIMgr;

    private void Start()
    {
        Init();
        InitDone();
    }
    private void Init()
    {
        DontDestroyOnLoad(gameObject);
        YSRoot.Init(false);
        globalUI.Init();
        gameUIMgr.Init();
    }
    private void InitDone()
    {
        SceneMag.Instance.LoadSceneAsync(SceneNames.Lobby, null, null, null);
    }
}