using System.Linq;
using System.Reflection;
using UnityEngine;

#if !UNITY_EDITOR
using System.IO;
#endif

public class HotUpdateAssembly : MonoBehaviour
{
    public static HotUpdateAssembly Instance { get; private set; } = null;

    public Assembly HybridHotupdateAss { get; private set; } = null;

    public void Init()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(Instance);
        }
        Instance = this;
#if UNITY_EDITOR
        // Editor下，已经被自动加载，不需要手动加载，重复加载反而会出问题。直接查找获得HotUpdate程序集。
        HybridHotupdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HybridHotupdate");
#else
        HybridHotupdateAss = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/Hybrid/HybridHotupdate.dll.bytes"));
#endif
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}