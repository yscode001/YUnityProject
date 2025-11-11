using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YUnity
{
    /// <summary>
    /// 场景管理器
    /// </summary>
    public partial class SceneMag : MonoBehaviourBaseY
    {
        private SceneMag() { }
        public static SceneMag Instance { get; private set; } = null;

        internal void Init()
        {
            Instance = this;
        }
    }
    public partial class SceneMag
    {
        private Action progressCB = null;

        // 对外的回调
        private float progressValue = 0f;
        private Action<float> progressCallback = null;
        private Action complete = null;

        /// <summary>
        /// 最后一次使用LoadSceneAsync切换的场景的名称
        /// </summary>
        public string LastSwitchedSceneName { get; private set; } = null;

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="begin"></param>
        /// <param name="progress"></param>
        /// <param name="complete"></param>
        public void LoadSceneAsync(string sceneName, Action<AsyncOperation> begin, Action<float> progress, Action complete)
        {
            if (string.IsNullOrWhiteSpace(sceneName) || progressCB != null) { return; } // 上一场景还在加载
            progressValue = 0f;
            progressCallback = progress;
            this.complete = complete;

            AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
            begin?.Invoke(sceneAsync);
            progressCB = () =>
            {
                if (sceneAsync.progress < 1f)
                {
                    if (progressValue != sceneAsync.progress)
                    {
                        progressValue = sceneAsync.progress;
                        this.progressCallback?.Invoke(progressValue);
                    }
                }
                else
                {
                    progressValue = 0f;
                    progressCallback = null;
                    this.complete?.Invoke();
                    this.complete = null;
                    sceneAsync = null;
                    progressCB = null;
                    LastSwitchedSceneName = sceneName;
                }
            };
        }

        private void Update()
        {
            progressCB?.Invoke();
        }
    }
}