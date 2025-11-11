using System.IO;

namespace YCSharp
{
    public class DirecoryUtil
    {
        /// <summary>
        /// 获取最后一级文件夹的名字
        /// </summary>
        /// <param name="directoryPath">文件夹完整路径(带不带/都可以)</param>
        /// <returns></returns>
        public static string GetLastDirectoryName(string directoryPath)
        {
            // 有可能是一个网址目录，所以这里不能判断文件夹是否存在
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            return directoryInfo.Name;
        }

        /// <summary>
        /// 删除文件夹下的所有文件和子文件夹
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void DeleteAllFilesAndSubDirectorys(string directoryPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            if (!directoryInfo.Exists)
            {
                return;
            }

            // 删除所有文件
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                // 去除文件的只读属性
                file.Attributes = FileAttributes.Normal;
                file.Delete();
            }

            // 删除所有子文件夹
            foreach (DirectoryInfo subDir in directoryInfo.GetDirectories())
            {
                // 去除文件夹和子文件的只读属性
                subDir.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                subDir.Delete();
            }

            // 删除这个空文件夹
            directoryInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
            directoryInfo.Delete();
        }

        /// <summary>
        /// 获取文件夹下的所有文件
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static FileInfo[] GetAllFiles(string directoryPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            if (!directoryInfo.Exists)
            {
                return null;
            }
            return directoryInfo.GetFiles("*", SearchOption.AllDirectories);
        }

        /// <summary>
        /// 文件夹或子文件夹下是否有文件
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool HasFiles(string directoryPath)
        {
            FileInfo[] fileInfos = GetAllFiles(directoryPath);
            return fileInfos != null && fileInfos.Length > 0;
        }
    }
}