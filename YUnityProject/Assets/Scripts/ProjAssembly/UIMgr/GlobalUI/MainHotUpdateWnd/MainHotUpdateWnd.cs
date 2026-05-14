using System;
using UnityEngine;
using UnityEngine.UI;
using YUnity;

public partial class MainHotUpdateWnd : MonoBehaviour
{
    [SerializeField] private Image progressImg;
    [SerializeField] private Text tipsText;

    public static MainHotUpdateWnd Instance { get; private set; } = null;

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
public partial class MainHotUpdateWnd
{
    public void CheckAndDownload(Action complete)
    {
        this.SetAct(true);
        complete?.Invoke();
    }
}