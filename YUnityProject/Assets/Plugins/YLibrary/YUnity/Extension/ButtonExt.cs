using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace YUnity
{
    public static partial class ButtonExt
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
    }
    public static partial class ButtonExt
    {
        private static readonly TimeSpan TimeInterval = TimeSpan.FromSeconds(0.5f);

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
                  .Where(_ => Input.touchCount < 2)
                  .ThrottleFirst(TimeInterval)
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
                  .Where(_ => Input.touchCount < 2)
                  .ThrottleFirst(TimeInterval)
                  .ObserveOnMainThread()
                  .Subscribe(_ => callback.Invoke())
                  .AddTo(button);
        }
    }
    public static partial class ButtonExt
    {
        /// <summary>
        /// RectTransform点击扩展(会自动添加Button)：过滤单点触摸 + 0.5秒节流 + 主线程执行
        /// </summary>
        public static IDisposable OnClickLimit(this RectTransform rectTransform, Action callback)
        {
            if (rectTransform == null || callback == null)
            {
                return Disposable.Empty;
            }
            return rectTransform.GetOrAddComponent<Button>().OnClickLimit(callback);
        }

        /// <summary>
        /// Text点击扩展(会自动添加Button)：过滤单点触摸 + 0.5秒节流 + 主线程执行
        /// </summary>
        public static IDisposable OnClickLimit(this Text text, Action callback)
        {
            if (text == null || callback == null)
            {
                return Disposable.Empty;
            }
            return text.GetOrAddComponent<Button>().OnClickLimit(callback);
        }

        /// <summary>
        /// Image点击扩展(会自动添加Button)：过滤单点触摸 + 0.5秒节流 + 主线程执行
        /// </summary>
        public static IDisposable OnClickLimit(this Image image, Action callback)
        {
            if (image == null || callback == null)
            {
                return Disposable.Empty;
            }
            return image.GetOrAddComponent<Button>().OnClickLimit(callback);
        }

        /// <summary>
        /// RawImage点击扩展(会自动添加Button)：过滤单点触摸 + 0.5秒节流 + 主线程执行
        /// </summary>
        public static IDisposable OnClickLimit(this RawImage rawImage, Action callback)
        {
            if (rawImage == null || callback == null)
            {
                return Disposable.Empty;
            }
            return rawImage.GetOrAddComponent<Button>().OnClickLimit(callback);
        }

        /// <summary>
        /// EmptyRaycast点击扩展(会自动添加Button)：过滤单点触摸 + 0.5秒节流 + 主线程执行
        /// </summary>
        public static IDisposable OnClickLimit(this EmptyRaycast emptyRaycast, Action callback)
        {
            if (emptyRaycast == null || callback == null)
            {
                return Disposable.Empty;
            }
            return emptyRaycast.GetOrAddComponent<Button>().OnClickLimit(callback);
        }
    }
}