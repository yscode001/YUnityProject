using System;
using UnityEngine;
using YUnity;

public partial class ModuleHotupdateWnd : MonoBehaviour
{
    public static ModuleHotupdateWnd Instance { get; private set; } = null;

    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(Instance);
        }
        Instance = this;
        this.SetAct(false);
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}
public partial class ModuleHotupdateWnd
{
    public void CheckAndDownload(Action complete)
    {
        this.SetAct(true);
        complete?.Invoke();
    }
}