using UnityEngine;
using UnityEngine.UI;
using YUnity;

public class SceneLoadingWnd : MonoBehaviour
{
    [SerializeField] private Image progressImg;
    [SerializeField] private Text tipsText;

    public static SceneLoadingWnd Instance { get; private set; } = null;

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