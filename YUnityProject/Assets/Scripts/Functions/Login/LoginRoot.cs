using UnityEngine;
using YUnity;

public class LoginRoot : MonoBehaviour
{
    private void Start()
    {
        SceneLoader.Instance.SceneSwitchFinishedAndInitDone(SceneNames.Login);

        ABLoader.LoadGameObject("ui_3bfcf6066527c1a2d798675a16348ca9", "LoginWnd.prefab", (ab, prefab) =>
        {
            if (prefab != null)
            {
                GameObject.Instantiate(prefab, GameUIMgr.Instance.transform);
            }
        });
    }
}