using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YUnity
{
    [Serializable]
    public partial class ABLoadManifestFile
    {
        public List<string> AssetBundles = new List<string>();
        public ABLoadManifestFile() { }

        public byte[] Serialize()
        {
            if (AssetBundles == null || AssetBundles.Count == 0)
            {
                return null;
            }
            else
            {
                return System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
            }
        }

        /// <summary>
        /// 资源包清单文件保存在硬盘上
        /// </summary>
        public void SaveToHardDisk()
        {
            ABLoader.SaveManifestFile(this);
        }

        public void Add(string assetBundleName)
        {
            if (string.IsNullOrWhiteSpace(assetBundleName) || AssetBundles.Contains(assetBundleName))
            {
                return;
            }
            AssetBundles.Add(assetBundleName);
        }
        public void DeleteABInfo(string assetBundleName)
        {
            AssetBundles.RemoveAll(m => m == assetBundleName);
        }
    }

    public partial class ABLoadManifestFile
    {
        /// <summary>
        /// 比较本地和远端的bundle清单文件，获取需要下载的bundle清单
        /// </summary>
        /// <param name="local">本地的bundle清单文件</param>
        /// <param name="remote">远端的bundle清单文件</param>
        /// <returns></returns>
        public static List<string> CompareAndGetCanDownloadFiles(ABLoadManifestFile local, ABLoadManifestFile remote)
        {
            if (remote == null || remote.AssetBundles == null || remote.AssetBundles.Count <= 0)
            {
                // 远端没有资源，直接返回null
                return null;
            }
            if (local == null || local.AssetBundles == null || local.AssetBundles.Count <= 0)
            {
                // 本地没有资源，直接返回远端的所有资源
                return remote.AssetBundles;
            }
            List<string> result = new List<string>();
            foreach (var remoteItem in remote.AssetBundles)
            {
                if (string.IsNullOrWhiteSpace(remoteItem) || result.Contains(remoteItem) || local.AssetBundles.Contains(remoteItem))
                {
                    continue;
                }
                result.Add(remoteItem);
            }
            return result;
        }
    }
}