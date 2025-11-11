using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YUtilEditor
{
    [Serializable]
    public class ABBuildManifestFile
    {
        public List<string> AssetBundles { get; private set; } = new List<string>();
        public ABBuildManifestFile() { }

        public void Add(string assetBundleName)
        {
            if (string.IsNullOrWhiteSpace(assetBundleName) || AssetBundles.Contains(assetBundleName))
            {
                return;
            }
            AssetBundles.Add(assetBundleName);
        }
        public string Serialize()
        {
            if (AssetBundles == null || AssetBundles.Count == 0)
            {
                throw new Exception("没有要保存的bundle清单，无法序列化");
            }
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}