using System.Collections.Generic;

namespace YCSharp
{
    public partial class ABHelper
    {
        /// <summary>
        /// 清单文本文件名称(ManifestFile.txt)
        /// </summary>
        public const string ManifestTextFileName = "ManifestFile.txt";

        /// <summary>
        /// ab包扩展名(.unity3d)
        /// </summary>
        public const string BundleExt = ".unity3d";

        /// <summary>
        /// Manifest(manifest)
        /// </summary>
        public const string Manifest = "manifest";

        /// <summary>
        /// 移除hashcode，移除包扩展名，返回小写
        /// </summary>
        /// <param name="bundleName">带不带hashcode都可以，带不带扩展名都可以</param>
        /// <returns></returns>
        public static string RemoveHashcodeAndBundleExt(string bundleName)
        {
            if (string.IsNullOrWhiteSpace(bundleName))
            {
                return null;
            }
            string firstCharacter = bundleName.Contains("_") ? "_" : ".";
            int firstIdx = bundleName.IndexOf(firstCharacter);
            if (firstIdx >= 0)
            {
                return bundleName.Remove(firstIdx).ToLower();
            }
            else
            {
                return bundleName.ToLower();
            }
        }

        /// <summary>
        /// bundle包名字是否相同
        /// </summary>
        /// <param name="bundleName1"></param>
        /// <param name="bundleName2"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static bool BundleNameEqual(string bundleName1, string bundleName2)
        {
            if (string.IsNullOrWhiteSpace(bundleName1) || string.IsNullOrWhiteSpace(bundleName2))
            {
                throw new System.Exception("bundleName1 和 bundleName2 都不能为空");
            }
            if (bundleName1 == bundleName2)
            {
                return true;
            }
            else
            {
                return RemoveHashcodeAndBundleExt(bundleName1) == RemoveHashcodeAndBundleExt(bundleName2);
            }
        }

        /// <summary>
        /// 已加载的Bundle名字集合，是否包含将要加载的Bundle名字
        /// </summary>
        /// <param name="loaded"></param>
        /// <param name="willLoadBundleName"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static bool LoadedContains(List<string> loaded, string willLoadBundleName)
        {
            if (string.IsNullOrWhiteSpace(willLoadBundleName))
            {
                throw new System.Exception("willLoadBundleName 不能为空");
            }
            if (loaded == null || loaded.Count == 0)
            {
                return false;
            }
            foreach (var loadedItem in loaded)
            {
                if (BundleNameEqual(loadedItem, willLoadBundleName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}