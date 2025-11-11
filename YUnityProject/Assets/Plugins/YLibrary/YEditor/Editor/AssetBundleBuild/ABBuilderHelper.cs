using System;
using YCSharp;

namespace YUtilEditor
{
    public class ABBuilderHelper
    {
        /// <summary>
        /// manifest.unity3d
        /// </summary>
        /// <returns></returns>
        internal static string GetManifestBundleName()
        {
            return ABHelper.Manifest + ABHelper.BundleExt;
        }

        /// <summary>
        /// 获取AB包名称(小写带扩展名，如：audio.unity3d)
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <returns>小写带扩展名，如：audio.unity3d</returns>
        /// <exception cref="Exception"></exception>
        internal static string GetAssetBundleName(string assetBundleName)
        {
            if (string.IsNullOrWhiteSpace(assetBundleName))
            {
                throw new Exception("assetBundleName不能为空");
            }
            if (assetBundleName.EndsWith(ABHelper.BundleExt))
            {
                return assetBundleName.ToLower();
            }
            else
            {
                return assetBundleName.ToLower() + ABHelper.BundleExt;
            }
        }
    }
}