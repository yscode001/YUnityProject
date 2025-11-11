using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace YUnity
{
    /// <summary>
    /// 单例基类(会持久化到硬盘中，不继承MonoBehaviour)
    /// </summary>
    [Serializable]
    public abstract partial class SingletonPersistentBaseY<T> where T : SingletonPersistentBaseY<T>
    {
        protected SingletonPersistentBaseY() { }

        public static readonly string LocalFilePath = Application.persistentDataPath + "/" + typeof(T).Name + ".fun";
        public static readonly string ClassName = typeof(T).Name;

        private static readonly Lazy<T> _instance = new Lazy<T>(() =>
        {
            T data = GetFromLocal();
            if (data == null)
            {
                var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                if (ctor == null)
                {
                    throw new Exception("Non-public ctor() not found");
                }
                data = ctor.Invoke(null) as T;
                data.Persistent();
            }
            return data;
        });
        public static T Instance => _instance.Value;
    }
    #region 从磁盘加载
    public abstract partial class SingletonPersistentBaseY<T>
    {
        /// <summary>
        /// 从磁盘加载
        /// </summary>
        /// <returns></returns>
        private static T GetFromLocal()
        {
            try
            {
                if (File.Exists(LocalFilePath))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream stream = new FileStream(LocalFilePath, FileMode.Open);
                    T data = formatter.Deserialize(stream) as T;
                    stream.Close();
                    return data;
                }
            }
            catch (Exception e)
            {
                LogTool.Error($"单例：{ClassName}，读取失败：{e}");
                DeleteCacheFile();
            }
            return null;
        }
    }
    #endregion
    #region 持久化
    public abstract partial class SingletonPersistentBaseY<T>
    {
        /// <summary>
        /// 持久化
        /// </summary>
        public void Persistent()
        {
            try
            {
                DoBeforePersistent();
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(LocalFilePath, FileMode.OpenOrCreate);
                formatter.Serialize(stream, this);
                stream.Close();
            }
            catch (Exception e)
            {
                LogTool.Error($"单例：{ClassName}，持久化失败：{e}");
                DeleteCacheFile();
            }
        }
        /// <summary>
        /// 持久化
        /// </summary>
        public void Save()
        {
            Persistent();
        }
    }
    #endregion
    #region 删除缓存文件
    public abstract partial class SingletonPersistentBaseY<T>
    {
        /// <summary>
        /// 移除缓存文件
        /// </summary>
        public static void DeleteCacheFile()
        {
            if (File.Exists(LocalFilePath))
            {
                File.Delete(LocalFilePath);
            }
        }

        /// <summary>
        /// 移除缓存文件
        /// </summary>
        public static void RemoveCacheFile()
        {
            DeleteCacheFile();
        }

        /// <summary>
        /// 移除缓存文件
        /// </summary>
        public static void ClearCacheFile()
        {
            DeleteCacheFile();
        }
    }
    #endregion
    #region 虚方法
    public abstract partial class SingletonPersistentBaseY<T>
    {
        /// <summary>
        /// 持久化前调用
        /// </summary>
        public virtual void DoBeforePersistent() { }
    }
    #endregion
}