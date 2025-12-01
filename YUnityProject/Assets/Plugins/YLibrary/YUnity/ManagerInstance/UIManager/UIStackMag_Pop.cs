using System;
using System.Linq;
using DG.Tweening;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// Pop掉栈顶页面
        /// </summary>
        /// <param name="popReason"></param>
        /// <param name="popAni"></param>
        /// <param name="complete"></param>
        public void Pop(PopReason popReason, PopAni popAni, Action<bool> complete = null)
        {
            if (Stack.Count == 0 || IsPushingOrPoping)
            {
                complete?.Invoke(false);
                return;
            }
            IsPushingOrPoping = true;

            // 1、计算需要pop掉的页面，并从栈中移除
            UIStackBaseWnd willPopWnd = GetTopWnd();
            Stack.Remove(willPopWnd);

            // 2、整理页面可见性
            if (willPopWnd.PageType == PageType.NewPage)
            {
                VisibilityChange();
            }

            // 3、执行pop动画
            willPopWnd.RunPopAni(popAni, () =>
            {
                // 4、动画完成，本页面退出
                willPopWnd.OnExit(popReason);

                // 5、底下的页面恢复
                if (Stack.Count > 0)
                {
                    Stack.LastOrDefault().OnResume();
                }

                // 6、完成
                IsPushingOrPoping = false;
                complete?.Invoke(true);
            });
        }

        public void Pop(PopReason popReason, Sequence customPushAni, Action<bool> complete = null)
        {
            if (Stack.Count == 0 || IsPushingOrPoping)
            {
                complete?.Invoke(false);
                return;
            }
            IsPushingOrPoping = true;

            // 1、计算需要pop掉的页面，并从栈中移除
            UIStackBaseWnd willPopWnd = GetTopWnd();
            Stack.Remove(willPopWnd);

            // 2、整理页面可见性
            if (willPopWnd.PageType == PageType.NewPage)
            {
                VisibilityChange();
            }

            // 3、执行pop动画
            if (customPushAni == null)
            {
                // 4、动画完成，本页面退出
                willPopWnd.OnExit(popReason);

                // 5、底下的页面恢复
                if (Stack.Count > 0)
                {
                    Stack.LastOrDefault().OnResume();
                }

                // 6、完成
                IsPushingOrPoping = false;
                complete?.Invoke(true);
            }
            else
            {
                customPushAni.onComplete += () =>
                {
                    // 4、动画完成，本页面退出
                    willPopWnd.OnExit(popReason);

                    // 5、底下的页面恢复
                    if (Stack.Count > 0)
                    {
                        Stack.LastOrDefault().OnResume();
                    }

                    // 6、完成
                    IsPushingOrPoping = false;
                    complete?.Invoke(true);
                };
            }
        }
    }
}