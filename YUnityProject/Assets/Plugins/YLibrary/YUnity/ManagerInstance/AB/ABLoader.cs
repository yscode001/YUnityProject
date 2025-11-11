using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.U2D;
using YCSharp;

namespace YUnity
{
    #region Init
    public static partial class ABLoader
    {
        /// <summary>
        /// 真正存放bundle包的路径
        /// </summary>
        public static string BundlePath { get; private set; }

        public static AssetBundleManifest Manifest { get; private set; }

        /// <summary>
        /// 所有的Bundle包名字的集合，带扩展名，带hashcode，示例：ui_hashcode.unity3d
        /// </summary>
        public static string[] AllAssetBundle { get; private set; }

        public static string[] AllAssetBundleWithVariant { get; private set; }

        /// <summary>
        /// 热更前初始化存放Bundle包的路径
        /// </summary>
        /// <param name="bundlePath">bundle包所在路径</param>
        /// <param name="createBundleDirectory">是否需要强制创建bundle包所在的目录</param>
        /// <exception cref="System.Exception"></exception>
        public static void InitBundlePathBeforeHotUpdate(string bundlePath, bool createBundleDirectory = true)
        {
            if (string.IsNullOrWhiteSpace(bundlePath))
            {
                throw new System.Exception("bundlePath不能为空");
            }
            BundlePath = bundlePath;
            if (createBundleDirectory && !Directory.Exists(BundlePath))
            {
                Directory.CreateDirectory(BundlePath);
            }
        }

        /// <summary>
        /// 热更后初始化manifest依赖文件
        /// </summary>
        /// <param name="manifestBundleFullName">manifestBundleFullName(带hashCode)</param>
        /// <param name="complete">完成回调</param>
        /// <exception cref="System.Exception"></exception>
        public static void InitManifestAfterHotUpdate(string manifestBundleFullName, Action complete)
        {
            if (string.IsNullOrWhiteSpace(manifestBundleFullName))
            {
                throw new Exception("manifestBundleFullName不能为空");
            }
            if (manifestBundleFullName.EndsWith(ABHelper.BundleExt))
            {
                manifestBundleFullName = manifestBundleFullName.ToLower();
            }
            else
            {
                manifestBundleFullName = manifestBundleFullName.ToLower() + ABHelper.BundleExt;
            }
            ABLoadUtil.Instance.LoadAssetBundle(Path.Combine(BundlePath, manifestBundleFullName), (manifestAB) =>
            {
                if (manifestAB == null)
                {
                    throw new System.Exception($"{manifestBundleFullName}，这个bundle包不存在");
                }
                else
                {
                    Manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    if (Manifest == null)
                    {
                        throw new System.Exception($"{manifestBundleFullName}，这个bundle包错误，取不到里面的AssetBundleManifest");
                    }
                    manifestAB.Unload(false);
                    AllAssetBundle = Manifest.GetAllAssetBundles();
                    AllAssetBundleWithVariant = Manifest.GetAllAssetBundlesWithVariant();
                }
                complete?.Invoke();
            });
        }
    }
    #endregion

    #region 工具函数，获取bundle包的名字
    public static partial class ABLoader
    {
        /// <summary>
        /// 获取Bundle包的完整名字，结果带hashcode，带扩展名，如果不存在此Bundle包，返回空
        /// </summary>
        /// <param name="bundleName">带不带扩展名都可以，带不带hashcode都可以</param>
        /// <returns></returns>
        public static string GetAssetBundleName(string bundleName)
        {
            if (string.IsNullOrWhiteSpace(bundleName))
            {
                return null;
            }
            bundleName = bundleName.ToLower();
            if (bundleName.Contains("_"))
            {
                if (bundleName.EndsWith(ABHelper.BundleExt))
                {
                    // 带hashcode，带扩展名
                    return AllAssetBundle.FirstOrDefault(m => m == bundleName);
                }
                else
                {
                    // 带hashcode，不带扩展名
                    return AllAssetBundle.FirstOrDefault(m => m == bundleName + ABHelper.BundleExt);
                }
            }
            else
            {
                // 不带hashcode，先把扩展名给去掉
                bundleName = ABHelper.RemoveHashcodeAndBundleExt(bundleName);
                // 判断时加上 _
                return AllAssetBundle.FirstOrDefault(m => m.StartsWith(bundleName + "_"));
            }
        }
    }
    #endregion

    #region 获取依赖项
    public static partial class ABLoader
    {
        public static string[] GetAllDependencies(string bundleName)
        {
            if (Manifest == null)
            {
                throw new System.Exception($"Manifest未初始化，请先调用InitManifestAfterHotUpdate进行初始化");
            }
            else
            {
                string abName = GetAssetBundleName(bundleName);
                if (string.IsNullOrWhiteSpace(abName))
                {
                    return null;
                }
                else
                {
                    return Manifest.GetAllDependencies(abName);
                }
            }
        }
    }
    #endregion

    #region LoadAssetBundle
    public static partial class ABLoader
    {
        private static void LoadDependencies(string[] dependencies, Action complete)
        {
            if (dependencies == null || dependencies.Length == 0)
            {
                complete?.Invoke();
                return;
            }
            // 先排除已加载的
            List<string> depList = new List<string>();
            var loadedList = ABLoadUtil.Instance.GetLoadedAssetBundleNameList();
            foreach (var dep in dependencies)
            {
                if (!ABHelper.LoadedContains(loadedList, dep) && !depList.Contains(dep))
                {
                    depList.Add(dep);
                }
            }
            // 没有需要加载的依赖项
            if (depList.Count == 0)
            {
                complete?.Invoke();
                return;
            }
            // 开始加载依赖项
            int loadCount = 0;
            foreach (var dep in depList)
            {
                ABLoadUtil.Instance.LoadAssetBundle(Path.Combine(BundlePath, dep), (assetbundle) =>
                {
                    // 全部加载后完成回调
                    loadCount += 1;
                    if (loadCount >= depList.Count)
                    {
                        complete?.Invoke();
                        return;
                    }
                });
            }
        }

        private static readonly object lock_loadAssetBundle = new object();
        public static void LoadAssetBundle(string bundleName, Action<AssetBundle> complete)
        {
            if (string.IsNullOrWhiteSpace(bundleName) || complete == null)
            {
                complete?.Invoke(null);
                return;
            }
            // 先从缓存中查找
            AssetBundle cacheBundle = ABLoadUtil.Instance.GetLoadedAssetBundle(bundleName);
            if (cacheBundle != null)
            {
                complete?.Invoke(cacheBundle);
                return;
            }
            // 获取完整包名，示例：ui_hashcode.unity3d
            string abName = GetAssetBundleName(bundleName);
            if (string.IsNullOrWhiteSpace(abName))
            {
                complete?.Invoke(null);
                return;
            }
            // 先锁起来
            Monitor.Enter(lock_loadAssetBundle);
            // 解锁后先从缓存中查找
            cacheBundle = ABLoadUtil.Instance.GetLoadedAssetBundle(bundleName);
            if (cacheBundle != null)
            {
                Monitor.Exit(lock_loadAssetBundle);
                complete?.Invoke(cacheBundle);
                return;
            }
            LoadDependencies(GetAllDependencies(abName), () =>
            {
                // 加载完依赖，再次判断缓存
                cacheBundle = ABLoadUtil.Instance.GetLoadedAssetBundle(bundleName);
                if (cacheBundle == null)
                {
                    ABLoadUtil.Instance.LoadAssetBundle(Path.Combine(BundlePath, abName), (assetbundle) =>
                    {
                        Monitor.Exit(lock_loadAssetBundle);
                        complete?.Invoke(assetbundle);
                    });
                }
                else
                {
                    Monitor.Exit(lock_loadAssetBundle);
                    complete?.Invoke(cacheBundle);
                }
            });
        }
    }
    #endregion

    #region LoadAsset
    public static partial class ABLoader
    {
        public static void LoadAsset<T>(string bundleName, string assetName, Action<AssetBundle, T> loaded) where T : UnityEngine.Object
        {
            if (string.IsNullOrWhiteSpace(bundleName) || string.IsNullOrWhiteSpace(assetName) || loaded == null)
            {
                loaded?.Invoke(null, null);
                return;
            }
            LoadAssetBundle(bundleName, (ab) =>
            {
                ABLoadUtil.Instance.LoadAsset<T>(ab, assetName, (asset) =>
                {
                    loaded?.Invoke(ab, asset);
                });
            });
        }
        public static void LoadPrefab(string bundleName, string assetName, Action<AssetBundle, GameObject> loaded)
        {
            if (!assetName.EndsWith(".prefab"))
            {
                assetName += ".prefab";
            }
            LoadAsset<GameObject>(bundleName, assetName, loaded);
        }
        public static void LoadGameObject(string bundleName, string assetName, Action<AssetBundle, GameObject> loaded)
        {
            LoadAsset<GameObject>(bundleName, assetName, loaded);
        }
        public static void LoadAudioClip(string bundleName, string assetName, Action<AssetBundle, AudioClip> loaded)
        {
            LoadAsset<AudioClip>(bundleName, assetName, loaded);
        }
        public static void LoadMaterial(string bundleName, string assetName, Action<AssetBundle, Material> loaded)
        {
            LoadAsset<Material>(bundleName, assetName, loaded);
        }
        public static void LoadSprite(string bundleName, string assetName, Action<AssetBundle, Sprite> loaded)
        {
            LoadAsset<Sprite>(bundleName, assetName, loaded);
        }
        public static void LoadSpriteAtlas(string bundleName, string assetName, Action<AssetBundle, SpriteAtlas> loaded)
        {
            LoadAsset<SpriteAtlas>(bundleName, assetName, loaded);
        }
        public static void LoadTexture(string bundleName, string assetName, Action<AssetBundle, Texture> loaded)
        {
            LoadAsset<Texture>(bundleName, assetName, loaded);
        }
        public static void LoadTextAsset(string bundleName, string assetName, Action<AssetBundle, TextAsset> loaded)
        {
            LoadAsset<TextAsset>(bundleName, assetName, loaded);
        }
        public static void LoadTextAssetContent(string bundleName, string assetName, Action<AssetBundle, string> loaded)
        {
            LoadAsset<TextAsset>(bundleName, assetName, (ab, textAsset) =>
            {
                if (textAsset != null && !string.IsNullOrWhiteSpace(textAsset.text))
                {
                    loaded?.Invoke(ab, textAsset.text);
                }
                else
                {
                    loaded?.Invoke(ab, null);
                }
            });
        }
    }
    #endregion

    #region LoadInstantiate
    public static partial class ABLoader
    {
        public static void LoadGameObjectInstantiate(string bundleName, string assetName, Transform parent, Action<AssetBundle, GameObject> loaded)
        {
            LoadPrefab(bundleName, assetName, (ab, prefab) =>
            {
                if (prefab == null)
                {
                    loaded?.Invoke(ab, null);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(prefab, parent, false);
                    go.name = assetName;
                    go.transform.ResetLocal();
                    loaded?.Invoke(ab, go);
                }
            });
        }
        public static void LoadGameObjectInstantiate(string bundleName, string assetName, Transform parent, Vector3 localPosition, Vector3 localEulerAngles, Vector3 localScale, Action<AssetBundle, GameObject> loaded)
        {
            LoadPrefab(bundleName, assetName, (ab, prefab) =>
            {
                if (prefab == null)
                {
                    loaded?.Invoke(ab, null);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(prefab, parent, false);
                    go.name = assetName;
                    go.transform.localPosition = localPosition;
                    go.transform.localEulerAngles = localEulerAngles;
                    go.transform.localScale = localScale;
                    loaded?.Invoke(ab, go);
                }
            });
        }
    }
    #endregion

    #region Clear
    public static partial class ABLoader
    {
        /// <summary>
        /// 清理(删除)所有bundle包资源
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void ClearBundle()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(BundlePath);
            if (directoryInfo == null)
            {
                Debug.Log($"{BundlePath}对应的目录不存在，无需清理");
                return;
            }
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            if (fileInfos == null || fileInfos.Length <= 0)
            {
                return;
            }
            for (int i = fileInfos.Length - 1; i >= 0; i--)
            {
                File.Delete(fileInfos[i].FullName);
            }
        }

        /// <summary>
        /// 清理指定名称的bundle资源
        /// </summary>
        /// <param name="bundleName">指定的bundle的名称</param>
        public static void ClearBundle(string bundleName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(BundlePath);
            if (directoryInfo == null)
            {
                Debug.Log($"{BundlePath}对应的目录不存在，无需清理");
                return;
            }
            string path = Path.Combine(BundlePath, GetAssetBundleName(bundleName));
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
    #endregion

    #region 清单文件
    public static partial class ABLoader
    {
        /// <summary>
        /// 生成的资源包清单
        /// </summary>
        public static string ManifestFileFullPath => Path.Combine(BundlePath, ABHelper.ManifestTextFileName);

        /// <summary>
        /// 保存资源包清单文件
        /// </summary>
        /// <param name="bytes"></param>
        public static void SaveManifestFile(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                return;
            }
            if (File.Exists(ManifestFileFullPath))
            {
                File.Delete(ManifestFileFullPath);
            }
            File.WriteAllBytes(ManifestFileFullPath, bytes);
        }

        /// <summary>
        /// 保存资源包清单文件
        /// </summary>
        /// <param name="manifestFile"></param>
        public static void SaveManifestFile(ABLoadManifestFile manifestFile)
        {
            SaveManifestFile(manifestFile.Serialize());
        }

        /// <summary>
        /// 加载bundle清单文件
        /// </summary>
        /// <returns></returns>
        public static ABLoadManifestFile LoadManifestFile()
        {
            ABLoadManifestFile manifestFile = new ABLoadManifestFile();
            if (File.Exists(ManifestFileFullPath))
            {
                try
                {
                    manifestFile = JsonConvert.DeserializeObject<ABLoadManifestFile>(File.ReadAllText(ManifestFileFullPath, Encoding.UTF8));
                }
                catch (Exception e)
                {
                    Debug.Log($"加载ManifestFile时失败：{e}");
                }
            }
            return manifestFile;
        }
    }
    #endregion

    #region 获取已加载的Bundle包
    public static partial class ABLoader
    {
        public static List<string> GetLoadedAssetBundleNameList()
        {
            return ABLoadUtil.Instance.GetLoadedAssetBundleNameList();
        }

        public static IEnumerable<AssetBundle> GetLoadedAssetBundleList()
        {
            return ABLoadUtil.Instance.GetLoadedAssetBundleList();
        }

        public static AssetBundle GetLoadedAssetBundle(string bundleName)
        {
            if (string.IsNullOrWhiteSpace(bundleName))
            {
                return null;
            }
            return ABLoadUtil.Instance.GetLoadedAssetBundle(bundleName);
        }
    }
    #endregion
    #region 卸载已加载的Bundle包
    public static partial class ABLoader
    {
        /// <summary>
        /// 同步卸载AssetBundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="unloadAllLoadedObjects"></param>
        public static void UnloadAssetBundle(string bundleName, bool unloadAllLoadedObjects)
        {
            if (string.IsNullOrWhiteSpace(bundleName))
            {
                return;
            }
            AssetBundle ab = ABLoadUtil.Instance.GetLoadedAssetBundle(bundleName);
            if (ab == null)
            {
                return;
            }
            ab.Unload(unloadAllLoadedObjects);
        }

        /// <summary>
        /// 异步卸载AssetBundle，返回值可能为空
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="unloadAllLoadedObjects"></param>
        /// <returns></returns>
        public static AsyncOperation UnloadAsyncAssetBundle(string bundleName, bool unloadAllLoadedObjects)
        {
            if (string.IsNullOrWhiteSpace(bundleName))
            {
                return null;
            }
            AssetBundle ab = ABLoadUtil.Instance.GetLoadedAssetBundle(bundleName);
            if (ab == null)
            {
                return null;
            }
            return ab.UnloadAsync(unloadAllLoadedObjects);
        }

        /// <summary>
        /// 同步卸载所有的AssetBundle
        /// </summary>
        /// <param name="unloadAllLoadedObjects"></param>
        public static void UnloadAllAssetBundle(bool unloadAllLoadedObjects)
        {
            var ablist = ABLoadUtil.Instance.GetLoadedAssetBundleList();
            foreach (var ab in ablist)
            {
                ab.Unload(unloadAllLoadedObjects);
            }
        }
    }
    #endregion
}