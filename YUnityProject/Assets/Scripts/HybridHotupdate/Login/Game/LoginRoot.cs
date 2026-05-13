using UnityEngine;
using YUnity;

public class LoginRoot : MonoBehaviour
{
    private void Start()
    {
        SceneLoader.Instance.SceneSwitchFinishedAndInitDone(SceneNames.Login);
        ABLoader.LoadGameObjectInstantiate("login_00862d06a5e1cfd2372dbbe29c6f48b3.unity3d", ABNames.Login.LoginWnd, GameUIMgr.Instance.transform, (ab, go) =>
        {
            Debug.Log(go);
        });
    }
}