using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace YUIControl
{
    public class SequenceFrameImage : UnityEngine.UI.Image
    {
        [Header("帧动画核心配置")]
        [SerializeField] private Sprite[] frameSprites; // 帧序列（按播放顺序）
        [SerializeField] private float animDuration = 1.5f; // 单轮总时长

        [Header("循环配置")]
        [SerializeField] private bool isInfiniteLoop = false; // 是否无限循环
        [SerializeField][Min(1)] private int loopCount = 1; // 有限循环次数（最小1次）
        [SerializeField] private LoopType loopType = LoopType.Restart; // 循环类型

        [Header("回调配置（编辑器直接绑定）")]
        [Tooltip("每轮循环结束回调（参数：当前已播放次数）")]
        public UnityEvent<int> onLoopStepComplete; // 每轮结束回调（带当前次数）
        [Tooltip("所有循环完成后回调（仅有限循环触发）")]
        public UnityEvent onAllLoopComplete; // 全部完成回调

        [Header("其他配置")]
        [SerializeField] private bool resetOnDisable = true; // 禁用时重置到第一帧

        private Tweener _frameTweener; // 动画句柄
        private int _currentLoopCount; // 当前已完成的循环次数
        private int _totalLoopCount; // 总循环次数（-1=无限）

        protected override void OnEnable()
        {
            // 基础判空防护
            if (frameSprites == null || frameSprites.Length == 0)
            {
                Debug.LogWarning($"帧动画配置不完整： 帧数={frameSprites?.Length ?? 0}", this);
                return;
            }

            // 销毁旧动画，避免叠加
            _frameTweener?.Kill();

            // 初始化循环计数
            _currentLoopCount = 0;
            _totalLoopCount = isInfiniteLoop ? -1 : loopCount;

            // 初始化显示第一帧
            this.sprite = frameSprites[0];

            // 创建帧动画
            _frameTweener = DOTween.To(
                    () => 0f,
                    progress => UpdateFrame(progress),
                    1f,
                    animDuration
                )
                .SetEase(Ease.Linear)
                .SetLoops(_totalLoopCount, loopType)
                .OnStepComplete(OnSingleLoopComplete) // 每轮循环结束回调
                .OnComplete(OnAllLoopComplete); // 全部循环完成回调
        }

        protected override void OnDisable()
        {
            _frameTweener?.Kill();
            _frameTweener = null;
            _currentLoopCount = 0; // 禁用时重置计数

            if (resetOnDisable && frameSprites != null && frameSprites.Length > 0)
            {
                this.sprite = frameSprites[0];
            }
        }

        /// <summary>
        /// 更新帧（核心逻辑）
        /// </summary>
        private void UpdateFrame(float progress)
        {
            progress = Mathf.Clamp01(progress);
            int frameIndex = Mathf.FloorToInt(progress * (frameSprites.Length - 1));
            if (this.sprite != frameSprites[frameIndex])
            {
                this.sprite = frameSprites[frameIndex];
            }
        }

        /// <summary>
        /// 单轮循环结束回调（核心：统计当前次数并通知）
        /// </summary>
        private void OnSingleLoopComplete()
        {
            _currentLoopCount++; // 每完成一轮，计数+1

            // 触发「单轮完成」回调，传入当前已播放次数
            onLoopStepComplete?.Invoke(_currentLoopCount);

            // 调试日志（可选）
            Debug.Log($"第 {_currentLoopCount} 轮循环完成 | 总需播放：{(_totalLoopCount == -1 ? "无限" : _totalLoopCount + "轮")}", this);
        }

        /// <summary>
        /// 所有循环完成回调（仅有限循环触发）
        /// </summary>
        private void OnAllLoopComplete()
        {
            // 触发「全部完成」回调
            onAllLoopComplete?.Invoke();

            // 调试日志（可选）
            Debug.Log($"所有循环完成！共播放 {_currentLoopCount} 轮", this);

            // 可选：播放完关闭物体（根据需求调整）
            // gameObject.SetActive(false);
        }

        // ---------------------- 外部调用接口（灵活控制） ----------------------
        /// <summary>
        /// 手动播放动画（覆盖循环次数）
        /// </summary>
        /// <param name="customLoopCount">自定义循环次数（-1=无限，≥1=有限次数）</param>
        public void PlayAnim(int customLoopCount = -1)
        {
            gameObject.SetActive(true);
            _totalLoopCount = customLoopCount;
            _currentLoopCount = 0; // 重置计数

            if (_frameTweener != null)
            {
                _frameTweener.SetLoops(_totalLoopCount, loopType);
                _frameTweener.Play();
            }
        }

        /// <summary>
        /// 暂停动画
        /// </summary>
        public void PauseAnim() => _frameTweener?.Pause();

        /// <summary>
        /// 继续播放动画
        /// </summary>
        public void ResumeAnim() => _frameTweener?.Play();

        /// <summary>
        /// 停止动画并重置计数和帧
        /// </summary>
        public void StopAnim()
        {
            _frameTweener?.Kill();
            _frameTweener = null;
            _currentLoopCount = 0;

            if (frameSprites != null && frameSprites.Length > 0)
            {
                this.sprite = frameSprites[0];
            }
        }

        // 编辑器校验
        protected override void OnValidate()
        {
            animDuration = Mathf.Max(0.1f, animDuration);
            loopCount = Mathf.Max(1, loopCount);
        }

        // ---------------------- 拓展：代码绑定回调的示例（可选） ----------------------
        protected override void Start()
        {
            // 方式1：代码绑定「单轮完成」回调
            onLoopStepComplete.AddListener((currentCount) =>
            {
                Debug.Log($"代码绑定 - 第 {currentCount} 轮播放完成");
                // 此处可写业务逻辑：比如更新UI显示当前播放次数、播放音效等
            });

            // 方式2：代码绑定「全部完成」回调
            onAllLoopComplete.AddListener(() =>
            {
                Debug.Log($"代码绑定 - 所有循环播放完成！");
                // 此处可写业务逻辑：比如关闭弹窗、触发下一步剧情等
            });
        }
    }
}