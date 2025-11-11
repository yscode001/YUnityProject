using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using YCSharp;

namespace YUnity
{
    internal partial class ABLoadUtil : MonoBehaviourBaseY
    {
        internal static ABLoadUtil Instance { get; private set; } = null;

        internal void Init()
        {
            Instance = this;
        }
    }
    #region 获取已加载的AssetBundle
    internal partial class ABLoadUtil
    {
        /// <summary>
        /// 获取已加载的所有AB的名字，不包含hashcode，示例：ui.unity3d
        /// </summary>
        /// <returns></returns>
        internal List<string> GetLoadedAssetBundleNameList()
        {
            List<string> list = new List<string>();
            var assetBundles = GetLoadedAssetBundleList();
            foreach (var ab in assetBundles)
            {
                if (ab != null && !list.Contains(ab.name))
                {
                    list.Add(ab.name);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取已加载的所有AB，不包含hashcode，示例：ui.unity3d
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<AssetBundle> GetLoadedAssetBundleList()
        {
            return AssetBundle.GetAllLoadedAssetBundles() ?? (new AssetBundle[] { });
        }

        /// <summary>
        /// 获取已加载的AB(参数：带不带扩展名都可以，带不带hashcode都可以)
        /// </summary>
        /// <param name="bundleName">带不带扩展名都可以，带不带hashcode都可以</param>
        /// <returns></returns>
        internal AssetBundle GetLoadedAssetBundle(string bundleName)
        {
            if (string.IsNullOrWhiteSpace(bundleName))
            {
                return null;
            }
            var loadABs = GetLoadedAssetBundleList();
            foreach (var loadAB in loadABs)
            {
                if (ABHelper.BundleNameEqual(loadAB.name, bundleName))
                {
                    return loadAB;
                }
            }
            return null;
        }
    }
    #endregion
    #region 加载AssetBundle
    internal partial class ABLoadUtil
    {
        internal void LoadAssetBundle(string abFullPath, Action<AssetBundle> complete)
        {
            if (string.IsNullOrWhiteSpace(abFullPath))
            {
                complete?.Invoke(null);
                return;
            }
            StartCoroutine(LoadAssetBundleAction(abFullPath, complete));
        }
        private IEnumerator LoadAssetBundleAction(string abFullPath, Action<AssetBundle> complete)
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(abFullPath);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                complete?.Invoke((request.downloadHandler as DownloadHandlerAssetBundle).assetBundle);
            }
            else
            {
                complete?.Invoke(null);
            }
        }
    }
    #endregion
    #region 加载Asset
    internal partial class ABLoadUtil
    {
        internal void LoadAsset<T>(AssetBundle assetBundle, string assetName, Action<T> complete) where T : UnityEngine.Object
        {
            if (assetBundle == null || string.IsNullOrWhiteSpace(assetName))
            {
                complete?.Invoke(null);
                return;
            }
            StartCoroutine(LoadAssetAction(assetBundle, assetName, complete));
        }
        private IEnumerator LoadAssetAction<T>(AssetBundle assetBundle, string assetName, Action<T> complete) where T : UnityEngine.Object
        {
            AssetBundleRequest request = assetBundle.LoadAssetAsync<T>(assetName);
            yield return request;
            if (request.asset != null && request.asset is T)
            {
                complete?.Invoke(request.asset as T);
            }
            else
            {
                complete?.Invoke(null);
            }
        }
    }
    #endregion
}