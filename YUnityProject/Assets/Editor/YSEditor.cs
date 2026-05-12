using UnityEngine;
using UnityEditor;
using YEditor;

public static partial class YSEditor
{
    [MenuItem("ys/打印常用路径")]
    public static void PrintPath()
    {
        Debug.Log($"persistentDataPath：\n{Application.persistentDataPath}\n");
        Debug.Log($"streamingAssetsPath：\n{Application.streamingAssetsPath}\n");
    }

    [MenuItem("ys/清理本地缓存数据")]
    public static void ClearLocalData()
    {
        Debug.Log("清理本地缓存数据：开始清理\n");
        CurUser.ClearCacheFile();
        Debug.Log("清理本地缓存数据：清理完成\n");
    }
}
public static partial class YSEditor
{
    [MenuItem("ys/AB资源管理/1-清理AB资源包")]
    public static void ClearABRes()
    {
        Debug.Log("正在清理AB资源包...\n");
        ABBuilder.Init("Assets/Editor/ABRes/");
        ABBuilder.ClearBundles();
        Debug.Log($"AB资源包清理完毕\n");
    }

    [MenuItem("ys/AB资源管理/2-生成AB资源包")]
    public static void BuildABRes()
    {
        Debug.Log("正在生成AB资源包...\n");
        ABBuilder.Init("Assets/Editor/ABRes/");
        ABBuilder.BuildAssetBundles(BuildTarget.StandaloneOSX);
        Debug.Log($"AB资源包生成完毕\n");
    }
}