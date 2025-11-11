using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace YUnity
{
    public partial class HttpMag : MonoBehaviourBaseY
    {
        private HttpMag() { }
        public static HttpMag Instance { get; private set; } = null;

        internal void Init()
        {
            Instance = this;
        }
    }
    #region 获取服务器上的文本文件的内容
    public partial class HttpMag
    {
        /// <summary>
        /// 获取服务器上的文本文件的内容
        /// </summary>
        /// <param name="serverTextFileURL"></param>
        /// <param name="complete"></param>
        public void GetServerTextFileContent(string serverTextFileURL, Action<UnityWebRequest.Result, string> complete)
        {
            StartCoroutine(GetServerTextFileContent_action(serverTextFileURL, complete));
        }
        private IEnumerator GetServerTextFileContent_action(string serverTextFileURL, Action<UnityWebRequest.Result, string> complete)
        {
            using UnityWebRequest request = UnityWebRequest.Get(serverTextFileURL);
            yield return request.SendWebRequest();
            string content = null;
            if (request.result == UnityWebRequest.Result.Success)
            {
                content = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
            }
            complete?.Invoke(request.result, content);
        }
    }
    #endregion
    #region 获取服务器上的Texture
    public partial class HttpMag
    {
        public void GetServerTexture(string serverTextureFileURL, Action<UnityWebRequest.Result, Texture2D> complete)
        {
            StartCoroutine(GetServerTexture_Action(serverTextureFileURL, complete));
        }
        private IEnumerator GetServerTexture_Action(string serverTextureFileURL, Action<UnityWebRequest.Result, Texture2D> complete)
        {
            using UnityWebRequest request = UnityWebRequestTexture.GetTexture(serverTextureFileURL);
            yield return request.SendWebRequest();
            Texture2D texture = null;
            if (request.result == UnityWebRequest.Result.Success)
            {
                texture = DownloadHandlerTexture.GetContent(request);
            }
            complete?.Invoke(request.result, texture);
        }
    }
    #endregion
    #region 下载文件
    public partial class HttpMag
    {
        /// <summary>
        /// 下载服务器上的文件(不带进度)
        /// </summary>
        /// <param name="serverFileURL"></param>
        /// <param name="saveDirectory">保存的路径</param>
        /// <param name="saveFileName">保存的文件名(带后缀)</param>
        /// <param name="complete"></param>
        public void DownloadServerFile(string serverFileURL, string saveDirectory, string saveFileName, Action<UnityWebRequest.Result> complete)
        {
            StartCoroutine(DownloadServerFile_action(serverFileURL, saveDirectory, saveFileName, complete));
        }
        private IEnumerator DownloadServerFile_action(string serverFileURL, string saveDirectory, string saveFileName, Action<UnityWebRequest.Result> complete)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(serverFileURL))
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    if (!Directory.Exists(saveDirectory))
                    {
                        Directory.CreateDirectory(saveDirectory);
                    }
                    string path = Path.Combine(saveDirectory, saveFileName);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    File.WriteAllBytes(path, request.downloadHandler.data);
                }
                complete?.Invoke(request.result);
            }
        }

        /// <summary>
        /// 下载服务器上的文件(带进度，0到1)
        /// </summary>
        /// <param name="serverFileURL"></param>
        /// <param name="saveDirectory">保存的路径</param>
        /// <param name="saveFileName">保存的文件名(带后缀)</param>
        /// <param name="complete"></param>
        public void DownloadServerFileProgress(string serverFileURL, string saveDirectory, string saveFileName, Action<UnityWebRequest.Result, float> complete)
        {
            StartCoroutine(DownloadServerFileProgress_action(serverFileURL, saveDirectory, saveFileName, complete));
        }
        private IEnumerator DownloadServerFileProgress_action(string serverFileURL, string saveDirectory, string saveFileName, Action<UnityWebRequest.Result, float> complete)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(serverFileURL))
            {
                request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.DataProcessingError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    complete?.Invoke(request.result, 0);
                    yield break;
                }
                while (!request.isDone)
                {
                    complete?.Invoke(request.result, request.downloadProgress);
                    yield return null;
                }
                // 下载完成
                if (!Directory.Exists(saveDirectory))
                {
                    Directory.CreateDirectory(saveDirectory);
                }
                string path = Path.Combine(saveDirectory, saveFileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                File.WriteAllBytes(path, request.downloadHandler.data);
                complete?.Invoke(request.result, 1);
            }
        }
    }
    #endregion
}