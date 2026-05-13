#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using YEditor;
using System.IO;
using System.Collections.Generic;

#region 常用目录
public static partial class YSEditor
{
    [MenuItem("ys/1-常用路径/1-打印常用路径", false, 1000)]
    public static void PrintPath()
    {
        Debug.Log($"persistentDataPath：\n{Application.persistentDataPath}\n");
        Debug.Log($"streamingAssetsPath：\n{Application.streamingAssetsPath}\n");
    }

    private static void OpenFolder(string path)
    {
        if (File.Exists(path) || Directory.Exists(path))
        {
            EditorUtility.RevealInFinder(path);
        }
        else
        {
            Directory.CreateDirectory(path);
            EditorUtility.RevealInFinder(path);
        }
    }

    [MenuItem("ys/1-常用路径/2.1-打开Assets", false, 1100)]
    public static void OpenFolderDataPath()
    {
        OpenFolder(Application.dataPath);
    }
    [MenuItem("ys/1-常用路径/2.2-打开Assets-Plugins", false, 1101)]
    public static void OpenFolderPlugins()
    {
        OpenFolder(Path.Combine(Application.dataPath, "Plugins"));
    }
    [MenuItem("ys/1-常用路径/2.3-打开Assets-Editor", false, 1102)]
    public static void OpenFolderEditor()
    {
        OpenFolder(Path.Combine(Application.dataPath, "Editor"));
    }
    [MenuItem("ys/1-常用路径/2.4-打开Assets-Resources", false, 1103)]
    public static void OpenFolderResources()
    {
        OpenFolder(Path.Combine(Application.dataPath, "Resources"));
    }
    [MenuItem("ys/1-常用路径/2.5-打开Assets-StreamingAssetsPath", false, 1104)]
    public static void OpenFolderStreamingAssetsPath()
    {
        OpenFolder(Application.streamingAssetsPath);
    }
    [MenuItem("ys/1-常用路径/3.1-打开Library", false, 1200)]
    public static void OpenFolderLibrary()
    {
        OpenFolder(Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Library"));
    }
    [MenuItem("ys/1-常用路径/3.2-打开Library-ScriptAssemblies", false, 1201)]
    public static void OpenFolderScriptAssemblies()
    {
        OpenFolder(Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Library", "ScriptAssemblies"));
    }
    [MenuItem("ys/1-常用路径/3.3-打开UniRx.dll", false, 1202)]
    public static void OpenFolderUniRx()
    {
        OpenFolder(Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Library", "ScriptAssemblies", "UniRx.dll"));
    }
    [MenuItem("ys/1-常用路径/3.4-打开SuperScrollView.dll", false, 1202)]
    public static void OpenFolderSuperScrollView()
    {
        OpenFolder(Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Library", "ScriptAssemblies", "SuperScrollView.dll"));
    }
    [MenuItem("ys/1-常用路径/3.5-打开DoTweenModules.dll", false, 1202)]
    public static void OpenFolderDoTweenModules()
    {
        OpenFolder(Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Library", "ScriptAssemblies", "DoTweenModules.dll"));
    }
    [MenuItem("ys/1-常用路径/3.6-打开YEditor.dll", false, 1202)]
    public static void OpenFolderYEditor()
    {
        OpenFolder(Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Library", "ScriptAssemblies", "YEditor.dll"));
    }
    [MenuItem("ys/1-常用路径/3.7-打开YLibrary.dll", false, 1202)]
    public static void OpenFolderYLibrary()
    {
        OpenFolder(Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Library", "ScriptAssemblies", "YLibrary.dll"));
    }
    [MenuItem("ys/1-常用路径/3.8-打开热更Login.dll", false, 1202)]
    public static void OpenFolderLogin()
    {
        OpenFolder(Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Library", "ScriptAssemblies", "Login.dll"));
    }
    [MenuItem("ys/1-常用路径/3.9-打开热更Lobby.dll", false, 1203)]
    public static void OpenFolderLobby()
    {
        OpenFolder(Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Library", "ScriptAssemblies", "Lobby.dll"));
    }
    [MenuItem("ys/1-常用路径/4-打开Packages", false, 1300)]
    public static void OpenFolderPackages()
    {
        OpenFolder(Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Packages"));
    }
    [MenuItem("ys/1-常用路径/5-打开PersistentDataPath", false, 1400)]
    public static void OpenFolderPersistentDataPath()
    {
        OpenFolder(Application.persistentDataPath);
    }
}
#endregion
#region 缓存数据
public static partial class YSEditor
{
    [MenuItem("ys/2-清理本地缓存数据", false, 2000)]
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
#endregion
#region AB包
public static partial class YSEditor
{
    [MenuItem("ys/3-AB资源管理/1-清理AB资源包", false, 3000)]
    public static void ClearABRes()
    {
        Debug.Log("正在清理AB资源包...\n");
        ABBuilder.Init(Paths.AB.SourceDir);
        ABBuilder.ClearBundles();
        Debug.Log($"AB资源包清理完毕\n");
    }

    [MenuItem("ys/3-AB资源管理/2.1-生成AB资源包-OSX", false, 3100)]
    public static void BuildABResOSX()
    {
        Debug.Log("正在生成AB资源包...\n");
        ABBuilder.Init(Paths.AB.SourceDir);
        ABBuilder.BuildAssetBundles(BuildTarget.StandaloneOSX);
        Debug.Log($"AB资源包生成完毕\n");
    }

    [MenuItem("ys/3-AB资源管理/2.2-生成AB资源包-iOS", false, 3101)]
    public static void BuildABResiOS()
    {
        Debug.Log("正在生成AB资源包...\n");
        ABBuilder.Init(Paths.AB.SourceDir);
        ABBuilder.BuildAssetBundles(BuildTarget.iOS);
        Debug.Log($"AB资源包生成完毕\n");
    }

    [MenuItem("ys/3-AB资源管理/2.3-生成AB资源包-Windows64", false, 3102)]
    public static void BuildABResWindows64()
    {
        Debug.Log("正在生成AB资源包...\n");
        ABBuilder.Init(Paths.AB.SourceDir);
        ABBuilder.BuildAssetBundles(BuildTarget.StandaloneWindows64);
        Debug.Log($"AB资源包生成完毕\n");
    }

    [MenuItem("ys/3-AB资源管理/2.4-生成AB资源包-Android", false, 3103)]
    public static void BuildABResAndroid()
    {
        Debug.Log("正在生成AB资源包...\n");
        ABBuilder.Init(Paths.AB.SourceDir);
        ABBuilder.BuildAssetBundles(BuildTarget.Android);
        Debug.Log($"AB资源包生成完毕\n");
    }

    [MenuItem("ys/3-AB资源管理/2.5-生成AB资源包-WebGL", false, 3104)]
    public static void BuildABResWebGL()
    {
        Debug.Log("正在生成AB资源包...\n");
        ABBuilder.Init(Paths.AB.SourceDir);
        ABBuilder.BuildAssetBundles(BuildTarget.WebGL);
        Debug.Log($"AB资源包生成完毕\n");
    }
}
#endregion
#endif