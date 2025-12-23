using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace YUnity
{
    public static class ButtonExt
    {
        /// <summary>
        /// button.interactable = true;
        /// </summary>
        public static void Enable(this Button button)
        {
            if (button != null)
            {
                button.interactable = true;
            }
        }

        /// <summary>
        /// button.interactable = false;
        /// </summary>
        public static void Disable(this Button button)
        {
            if (button != null)
            {
                button.interactable = false;
            }
        }

        /// <summary>
        /// 按钮点击扩展：过滤单点触摸 + 0.5秒节流 + 主线程执行
        /// </summary>
        public static IDisposable OnClickLimit(this Button button, Action<Button> callback)
        {
            if (button == null || callback == null)
            {
                return Disposable.Empty;
            }
            return button.onClick.AsObservable()
                  .Where(_ => Input.touchCount == 1)
                  .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
                  .ObserveOnMainThread()
                  .Select(_ => button)
                  .Subscribe(callback)
                  .AddTo(button);
        }

        /// <summary>
        /// 按钮点击扩展：过滤单点触摸 + 0.5秒节流 + 主线程执行
        /// </summary>
        public static IDisposable OnClickLimit(this Button button, Action callback)
        {
            if (button == null || callback == null)
            {
                return Disposable.Empty;
            }
            return button.onClick.AsObservable()
                  .Where(_ => Input.touchCount == 1)
                  .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
                  .ObserveOnMainThread()
                  .Subscribe(_ => { callback.Invoke(); })
                  .AddTo(button);
        }
    }
}