#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using YEditor;
using System.IO;
using System.Collections.Generic;

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
        List<string> paths = new List<string>()
        {
            Path.Combine(Application.persistentDataPath, "PersistentData", "CurUser.fun"),
        };
        foreach (var path in paths)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        Debug.Log("清理本地缓存数据：清理完成\n");
    }
}
public static partial class YSEditor
{
    [MenuItem("ys/AB资源管理/1-清理AB资源包")]
    public static void ClearABRes()
    {
        Debug.Log("正在清理AB资源包...\n");
        ABBuilder.Init(Paths.AB.SourceDir);
        ABBuilder.ClearBundles();
        Debug.Log($"AB资源包清理完毕\n");
    }

    [MenuItem("ys/AB资源管理/2.1-生成AB资源包-OSX")]
    public static void BuildABResOSX()
    {
        Debug.Log("正在生成AB资源包...\n");
        ABBuilder.Init(Paths.AB.SourceDir);
        ABBuilder.BuildAssetBundles(BuildTarget.StandaloneOSX);
        Debug.Log($"AB资源包生成完毕\n");
    }

    [MenuItem("ys/AB资源管理/2.2-生成AB资源包-IOS")]
    public static void BuildABResIOS()
    {
        Debug.Log("正在生成AB资源包...\n");
        ABBuilder.Init(Paths.AB.SourceDir);
        ABBuilder.BuildAssetBundles(BuildTarget.iOS);
        Debug.Log($"AB资源包生成完毕\n");
    }

    [MenuItem("ys/AB资源管理/2.3-生成AB资源包-Windows64")]
    public static void BuildABResWindows64()
    {
        Debug.Log("正在生成AB资源包...\n");
        ABBuilder.Init(Paths.AB.SourceDir);
        ABBuilder.BuildAssetBundles(BuildTarget.StandaloneWindows64);
        Debug.Log($"AB资源包生成完毕\n");
    }

    [MenuItem("ys/AB资源管理/2.4-生成AB资源包-Android")]
    public static void BuildABResAndroid()
    {
        Debug.Log("正在生成AB资源包...\n");
        ABBuilder.Init(Paths.AB.SourceDir);
        ABBuilder.BuildAssetBundles(BuildTarget.Android);
        Debug.Log($"AB资源包生成完毕\n");
    }

    [MenuItem("ys/AB资源管理/2.5-生成AB资源包-WebGL")]
    public static void BuildABResWebGL()
    {
        Debug.Log("正在生成AB资源包...\n");
        ABBuilder.Init(Paths.AB.SourceDir);
        ABBuilder.BuildAssetBundles(BuildTarget.WebGL);
        Debug.Log($"AB资源包生成完毕\n");
    }
}
#endif