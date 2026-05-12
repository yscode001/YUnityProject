using UnityEngine;

public class LobbyRoot : MonoBehaviour
{
    private void Start()
    {
        SceneLoader.Instance.SceneSwitchFinishedAndInitDone(SceneNames.Lobby);
    }
}