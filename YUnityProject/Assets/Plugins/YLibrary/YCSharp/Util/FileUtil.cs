using System;
using System.IO;
using System.Text;

namespace YCSharp
{
    public class FileUtil
    {
        /// <summary>
        /// 获取文件名字(带扩展名)
        /// </summary>
        /// <param name="fileFullPath">文件完整路径</param>
        /// <returns></returns>
        public static string GetFileName(string fileFullPath)
        {
            return Path.GetFileName(fileFullPath);
        }

        /// <summary>
        /// 保存bytes至硬盘
        /// </summary>
        /// <param name="fileFullPath">文件完整路径</param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static bool SaveBytes(string fileFullPath, byte[] bytes)
        {
            if (string.IsNullOrWhiteSpace(fileFullPath) || bytes == null || bytes.Length <= 0)
            {
                return false;
            }
            try
            {
                DeleteFile(fileFullPath);
                using (FileStream fs = new FileStream(fileFullPath, FileMode.OpenOrCreate))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(bytes);
                        bw.Close();
                        fs.Close();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 从硬盘读取数据
        /// </summary>
        /// <param name="fileFullPath">文件完整路径</param>
        /// <returns></returns>
        public static byte[] ReadBytes(string fileFullPath)
        {
            if (string.IsNullOrWhiteSpace(fileFullPath) || !File.Exists(fileFullPath))
            {
                return null;
            }
            try
            {
                using (FileStream fs = new FileStream(fileFullPath, FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        return br.ReadBytes((int)fs.Length);
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="directoryFullPath"></param>
        /// <returns></returns>
        public static bool CreateDirectory(string directoryFullPath)
        {
            try
            {
                if (!Directory.Exists(directoryFullPath))
                {
                    Directory.CreateDirectory(directoryFullPath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileFullPath">文件完整路径</param>
        public static bool DeleteFile(string fileFullPath)
        {
            try
            {
                if (File.Exists(fileFullPath))
                {
                    // 先去除文件的只读属性
                    File.SetAttributes(fileFullPath, FileAttributes.Normal);
                    File.Delete(fileFullPath);
                    return true;
                }
                else
                {
                    // 这里也返回true，文件不存在，一样达到了效果
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除文件夹及其下面的所有文件
        /// </summary>
        /// <param name="directoryFullPath">文件夹路径</param>
        public static bool DeleteDirectory(string directoryFullPath)
        {
            try
            {
                if (Directory.Exists(directoryFullPath))
                {
                    // 去除文件夹和子文件的只读属性
                    DirectoryInfo dirInfo = new DirectoryInfo(directoryFullPath);
                    dirInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

                    // 去除文件的只读属性
                    File.SetAttributes(directoryFullPath, FileAttributes.Normal);

                    foreach (string path in Directory.GetFileSystemEntries(directoryFullPath))
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        else
                        {
                            DeleteDirectory(path);
                        }
                    }

                    Directory.Delete(directoryFullPath);
                    return true;
                }
                else
                {
                    // 这里也返回true，文件夹不存在，一样达到了效果
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static string GetMD5HashFromFile(string filepath)
        {
            try
            {
                FileStream file = new FileStream(filepath, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("ABBuildUtil-GetMD5HashFromFile() fail, error:" + ex.Message);
            }
        }
    }
}