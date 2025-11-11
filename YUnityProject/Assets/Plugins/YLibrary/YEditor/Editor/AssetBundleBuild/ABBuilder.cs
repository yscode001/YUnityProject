using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using YCSharp;

namespace YUtilEditor
{
    #region Init
    public static partial class ABBuilder
    {
        /// <summary>
        /// 资源所在的根目录
        /// </summary>
        private static string? ResSourceDirectory;

        /// <summary>
        /// 资源输出目录
        /// </summary>
        private static readonly string ResOutputDirectory = "./AssetBundleFiles/";

        /// <summary>
        /// 忽略文件的扩展名集合
        /// </summary>
        private static readonly List<string> IgnoreExts = new List<string>() { ".meta", ".tpsheet", ".unity", ".cs" };

        /// <summary>
        /// 初始化，注意，资源根目录下面的子目录的名字不要包含"_"，否则可能会有意想不到的错误
        /// </summary>
        /// <param name="resSourceDirectory">资源所在的根目录(示例：Assets/Editor/ABRes)</param>
        /// <exception cref="System.Exception"></exception>
        public static void Init(string resSourceDirectory)
        {
            if (string.IsNullOrWhiteSpace(resSourceDirectory) || !Directory.Exists(resSourceDirectory))
            {
                throw new System.Exception("resSourceDirectory 错误");
            }
            ResSourceDirectory = resSourceDirectory.EndsWith("/") ? resSourceDirectory : resSourceDirectory + "/";
        }
    }
    #endregion

    #region Build
    public static partial class ABBuilder
    {
        public static void BuildAssetBundles(BuildTarget buildTarget)
        {
            if (string.IsNullOrWhiteSpace(ResSourceDirectory) || !Directory.Exists(ResSourceDirectory))
            {
                throw new System.Exception("ResSourceDirectory 错误");
            }
            if (!HasFilesWillBeBuiled(new DirectoryInfo(ResSourceDirectory)))
            {
                throw new System.Exception("ResSourceDirectory 不存在任何文件，无法build");
            }
            if (Directory.Exists(ResOutputDirectory))
            {
                Directory.Delete(ResOutputDirectory, true);
            }
            Directory.CreateDirectory(ResOutputDirectory);

            List<AssetBundleBuild> list = new List<AssetBundleBuild>();

            DirectoryInfo sourceDir = new DirectoryInfo(ResSourceDirectory);
            DirectoryInfo[] childrenDirs = sourceDir.GetDirectories();
            foreach (var childDir in childrenDirs)
            {
                AssetBundleBuild childBuildInfo = GetBuildInfo(childDir);
                if (!string.IsNullOrWhiteSpace(childBuildInfo.assetBundleName) && childBuildInfo.assetNames != null && childBuildInfo.assetNames.Length > 0)
                {
                    list.Add(childBuildInfo);
                }
            }
            if (list.Count <= 0)
            {
                throw new System.Exception("ResSourceDirectory 不存在任何符合条件可以打包的文件，无法build");
            }

            DirectoryInfo outputDirInfo = new DirectoryInfo(ResOutputDirectory);

            // EditorUserBuildSettings.activeBuildTarget
            BuildPipeline.BuildAssetBundles(outputDirInfo.FullName, list.ToArray(), BuildAssetBundleOptions.AppendHashToAssetBundleName | BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.None, buildTarget);
            GenerateManifestTextFileAfterBuild();
        }

        // 判断文件夹里面是否有需要build的文件
        private static bool HasFilesWillBeBuiled(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null || !directoryInfo.Exists) { return false; }
            FileInfo[] fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            if (fileInfos == null || fileInfos.Length <= 0) { return false; }

            // 有文件
            if (IgnoreExts == null || IgnoreExts.Count <= 0) { return true; }
            foreach (var fileinfo in fileInfos)
            {
                string ext = Path.GetExtension(fileinfo.Name).ToLower();
                if (!IgnoreExts.Contains(ext))
                {
                    return true;
                }
            }
            return false;
        }

        // 获取文件夹中可以build的文件集合(包括子文件夹)
        private static List<FileInfo>? GetFilesWillBeBuiled(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null || !directoryInfo.Exists) { return null; }
            FileInfo[] fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            if (fileInfos == null || fileInfos.Length <= 0) { return null; }

            // 有文件
            List<FileInfo> list = new List<FileInfo>();
            foreach (var fileinfo in fileInfos)
            {
                if (IgnoreExts != null && IgnoreExts.Count > 0 && IgnoreExts.Contains(Path.GetExtension(fileinfo.Name).ToLower()))
                {
                    // 被忽略的文件过滤掉
                    continue;
                }
                list.Add(fileinfo);
            }
            return list;
        }

        private static AssetBundleBuild GetBuildInfo(DirectoryInfo directoryInfo)
        {
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = ABBuilderHelper.GetAssetBundleName(directoryInfo.Name);

            List<FileInfo>? fileInfolist = GetFilesWillBeBuiled(directoryInfo);
            List<string> fileNames = new List<string>();
            if (fileInfolist != null)
            {
                foreach (var fileinfo in fileInfolist)
                {
                    // 获取文件的相对路径
                    string filepath = "Assets" + fileinfo.FullName.Replace("\\", "/").Replace(Application.dataPath.Replace("\\", "/"), "");
                    fileNames.Add(filepath);
                }
            }
            build.assetNames = fileNames.ToArray();

            return build;
        }

        private static void GenerateManifestTextFileAfterBuild()
        {
            DirectoryInfo outputDir = new DirectoryInfo(ResOutputDirectory);
            FileInfo[] fileInfos = outputDir.GetFiles();
            if (fileInfos == null || fileInfos.Length <= 0)
            {
                PrintLogAfterBuildEnd(false);
                return;
            }
            List<FileInfo> filelist = new List<FileInfo>();
            for (int i = fileInfos.Length - 1; i >= 0; i--)
            {
                FileInfo fileInfo = fileInfos[i];
                if (Path.GetExtension(fileInfo.Name) == ".manifest")
                {
                    // 删除manifest
                    File.Delete(fileInfo.FullName);
                }
                else
                {
                    if (fileInfo.Name == DirecoryUtil.GetLastDirectoryName(ResOutputDirectory))
                    {
                        // 修改主清单文件的bundle包的名字：manifest.unity3d
                        string newName = $"{fileInfo.Directory}/{ABBuilderHelper.GetManifestBundleName()}";
                        fileInfo.MoveTo(newName);
                    }
                    // 添加至清单列表
                    filelist.Add(fileInfo);
                }
            }
            if (filelist.Count == 0)
            {
                PrintLogAfterBuildEnd(false);
                return;
            }
            // 生成bundle包的清单
            ABBuildManifestFile manifestFile = new ABBuildManifestFile();
            for (int i = 0; i < filelist.Count; i++)
            {
                manifestFile.Add(filelist[i].Name);
            }
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(manifestFile.Serialize());
            if (bytes == null || bytes.Length == 0)
            {
                PrintLogAfterBuildEnd(false);
                return;
            }
            File.WriteAllBytes(Path.Combine(ResOutputDirectory, ABHelper.ManifestTextFileName), bytes);
            PrintLogAfterBuildEnd(true);
        }

        private static void PrintLogAfterBuildEnd(bool success)
        {
            AssetDatabase.Refresh();
            string result = success ? "成功" : "失败";
            Debug.Log($"AssetBundle 打包结束：{result}");
        }
    }
    #endregion

    #region Clear
    public static partial class ABBuilder
    {
        /// <summary>
        /// 清理(删除)所有bundle资源
        /// </summary>
        public static void ClearBundles()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(ResOutputDirectory);
            if (!directoryInfo.Exists)
            {
                Debug.Log($"ResOutputDirectory 对应的目录不存在，无需清理");
                return;
            }
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            if (fileInfos == null || fileInfos.Length == 0)
            {
                return;
            }
            for (int i = fileInfos.Length - 1; i >= 0; i--)
            {
                File.Delete(fileInfos[i].FullName);
            }
            AssetDatabase.Refresh();
        }
    }
    #endregion
}