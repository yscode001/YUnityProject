using System;
using YUnity;

[Serializable]
public class CurUser : SingletonPersistentBaseY<CurUser>
{
    protected CurUser() { }

    public string id;
    public string token;
    public string name;
}