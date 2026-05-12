using UnityEngine;

public class LoginRoot : MonoBehaviour
{
    private void Start()
    {
        SceneLoader.Instance.SceneSwitchFinishedAndInitDone(SceneNames.Login);
    }
}