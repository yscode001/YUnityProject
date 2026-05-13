using UnityEngine;
using YUnity;

public class LoginEntrance : MonoBehaviour
{
    private void Start()
    {
        ABLoader.LoadGameObjectInstantiate("login_9f61c2fb4f178c8401a70c16bb715c73.unity3d", ABNames.Login.LoginRoot, transform.parent, null);
        DestroyImmediate(gameObject);
    }
}