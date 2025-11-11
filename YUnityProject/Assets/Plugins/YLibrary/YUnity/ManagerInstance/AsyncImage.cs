using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using YCSharp;

namespace YUnity
{
    public partial class AsyncImage : MonoBehaviourBaseY
    {
        private AsyncImage() { }
        public static AsyncImage Instance { get; private set; } = null;

        private readonly bool isWebGL = Application.platform == RuntimePlatform.WebGLPlayer;
        private string _webImgDirectory = null;
        private string webImgDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_webImgDirectory))
                {
                    _webImgDirectory = Application.persistentDataPath + "/WebImgDirectory/";
                    if (!isWebGL && !Directory.Exists(_webImgDirectory))
                    {
                        Directory.CreateDirectory(_webImgDirectory);
                    }
                }
                return _webImgDirectory;
            }
        }

        internal void Init()
        {
            Instance = this;
        }
    }

    #region 工具方法，网络请求
    public partial class AsyncImage
    {
        private void Download(string url, Action<string, byte[]> complete)
        {
            StartCoroutine(DownloadAction(url, complete));
        }

        private IEnumerator DownloadAction(string url, Action<string, byte[]> complete)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);
            request.timeout = 10;
            yield return request.SendWebRequest();

            byte[] bytes = null;
            if (request.result == UnityWebRequest.Result.Success)
            {
                bytes = request.downloadHandler.data;
            }
            else
            {
                Debug.LogError($"下载图片失败：{url}，错误: {request.error}");
            }

            complete?.Invoke(url, bytes);
            yield break;
        }

        // 请求缓存
        private readonly object requestCacheLock = new object();
        private readonly Dictionary<string, List<Action<string, Texture2D, Sprite>>> requestCache =
            new Dictionary<string, List<Action<string, Texture2D, Sprite>>>();
    }
    #endregion

    #region 工具方法，内存缓存
    public partial class AsyncImage
    {
        private readonly object memoryCacheLock = new object();
        private readonly Dictionary<string, Texture2D> memoryCache = new Dictionary<string, Texture2D>();

        public void ClearMemoryCache()
        {
            lock (memoryCacheLock)
            {
                foreach (var texture in memoryCache.Values)
                {
                    if (texture != null)
                    {
                        Destroy(texture);
                    }
                }
                memoryCache.Clear();
            }
        }
    }
    #endregion

    #region 工具方法，获取url的扩展名
    public partial class AsyncImage
    {
        private string GetFileExtension(string url)
        {
            string ext = Path.GetExtension(url)?.ToLower();
            return string.IsNullOrWhiteSpace(ext) ? ".png" : ext;
        }
    }
    #endregion

    public partial class AsyncImage
    {
        public void LoadSprite(string url, Action<string, Texture2D, Sprite> complete)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                complete?.Invoke(null, null, null);
                return;
            }
            if (complete == null)
            {
                return;
            }

            string urlMD5 = url.MD5();

            // 1. 先从内存缓存查找
            lock (memoryCacheLock)
            {
                if (memoryCache.TryGetValue(urlMD5, out Texture2D texture2D) &&
                    texture2D != null)
                {
                    complete.Invoke(url, texture2D, SpriteUtil.Generate(texture2D));
                    return;
                }
            }

            // 2. 再从硬盘缓存查找
            if (!isWebGL)
            {
                string filePath = Path.Combine(webImgDirectory, $"{urlMD5}{GetFileExtension(url)}");
                byte[] diskBytes = FileUtil.ReadBytes(filePath);
                Texture2D texture2D = Texture2DUtil.Generate(diskBytes);
                if (texture2D != null)
                {
                    // 同步到内存缓存
                    lock (memoryCacheLock)
                    {
                        if (!memoryCache.ContainsKey(urlMD5))
                        {
                            memoryCache[urlMD5] = texture2D;
                        }
                    }
                    complete.Invoke(url, texture2D, SpriteUtil.Generate(texture2D));
                    return;
                }
            }

            // 3. 最后从网络下载
            lock (requestCacheLock)
            {
                if (requestCache.TryGetValue(url, out var existingCallbacks))
                {
                    existingCallbacks.Add(complete);
                }
                else
                {
                    var newCallbacks = new List<Action<string, Texture2D, Sprite>> { complete };
                    requestCache[url] = newCallbacks;
                }
            }

            // 发起下载请求
            Download(url, (downloadedUrl, bytes) =>
            {
                Texture2D texture2D = Texture2DUtil.Generate(bytes);
                Sprite sprite = SpriteUtil.Generate(texture2D);

                if (texture2D != null)
                {
                    // 保存到内存缓存
                    lock (memoryCacheLock)
                    {
                        if (!memoryCache.ContainsKey(urlMD5))
                        {
                            memoryCache[urlMD5] = texture2D;
                        }
                    }

                    // 保存到硬盘缓存
                    if (!isWebGL)
                    {
                        try
                        {
                            string savePath = Path.Combine(webImgDirectory, $"{urlMD5}{GetFileExtension(url)}");
                            FileUtil.SaveBytes(savePath, bytes);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"保存图片到本地失败: {ex.Message}");
                        }
                    }
                }

                // 分发所有回调
                List<Action<string, Texture2D, Sprite>> callbacksToInvoke;
                lock (requestCacheLock)
                {
                    if (requestCache.TryGetValue(downloadedUrl, out var tempCallbacks))
                    {
                        callbacksToInvoke = tempCallbacks;
                        requestCache.Remove(downloadedUrl);
                    }
                    else
                    {
                        callbacksToInvoke = new List<Action<string, Texture2D, Sprite>>();
                    }
                }

                foreach (var callback in callbacksToInvoke)
                {
                    callback?.Invoke(downloadedUrl, texture2D, sprite);
                }
            });
        }
    }
}