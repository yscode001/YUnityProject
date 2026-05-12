using System;
using UnityEngine;
using YUnity;

public class LoginRoot : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("logic scene");
        SceneLoader.Instance.SceneSwitchFinishedAndInitDone(SceneNames.Login);

        try
        {
            ABLoader.InitBundlePathBeforeHotUpdate("file://" + Application.persistentDataPath + "/" + "ABRes");
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
        try
        {
            ABLoader.InitManifestAfterHotUpdate("manifest.unity3d", () =>
            {
                ABLoader.LoadGameObject("ui_3bfcf6066527c1a2d798675a16348ca9", "LoginWnd.prefab", (ab, prefab) =>
                {
                    if (prefab != null)
                    {
                        GameObject.Instantiate(prefab, GameUIMgr.Instance.transform);
                    }
                });
            });
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}