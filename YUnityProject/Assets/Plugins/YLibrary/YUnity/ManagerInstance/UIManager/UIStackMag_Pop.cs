using System;
using System.Linq;
using DG.Tweening;

namespace YUnity
{
    public partial class UIStackMag
    {
        private void DoAfterPopAniOver(UIStackBaseWnd wnd, PopReason popReason, Action<bool> complete = null)
        {
            // 1.本页面退出
            wnd.OnExit(popReason);
            // 2.底下的页面恢复
            if (_stack.Count > 0)
            {
                _stack.LastOrDefault().OnResume();
            }
            // 3.修改栈的完成状态
            IsPushingOrPoping = false;
            // 4.回调结果
            complete?.Invoke(true);
        }

        /// <summary>
        /// Pop栈顶页面
        /// </summary>
        public void Pop(PopReason popReason, PopAni popAni, Action<bool> complete = null)
        {
            if (_stack.Count == 0 || IsPushingOrPoping)
            {
                complete?.Invoke(false);
                return;
            }
            IsPushingOrPoping = true;

            // 1、计算需要pop掉的页面，并从栈中移除
            UIStackBaseWnd willPopWnd = _stack.LastOrDefault();
            willPopWnd.WillExit(popReason);
            _stack.Remove(willPopWnd);

            // 2、整理页面可见性
            if (willPopWnd.PageType == PageType.NewPage)
            {
                VisibilityChange_AfterStackChanged();
            }

            // 3、执行pop动画
            willPopWnd.RunPopAni(popAni, () =>
            {
                DoAfterPopAniOver(willPopWnd, popReason, complete);
            });
        }

        /// <summary>
        /// Pop栈顶页面
        /// </summary>
        public void Pop(PopReason popReason, Sequence customPopAni, Action<bool> complete = null)
        {
            if (_stack.Count == 0 || IsPushingOrPoping)
            {
                complete?.Invoke(false);
                return;
            }
            IsPushingOrPoping = true;

            // 1、计算需要pop掉的页面，并从栈中移除
            UIStackBaseWnd willPopWnd = _stack.LastOrDefault();
            willPopWnd.WillExit(popReason);
            _stack.Remove(willPopWnd);

            // 2、整理页面可见性
            if (willPopWnd.PageType == PageType.NewPage)
            {
                VisibilityChange_AfterStackChanged();
            }

            // 3、执行pop动画
            if (customPopAni == null)
            {
                DoAfterPopAniOver(willPopWnd, popReason, complete);
            }
            else
            {
                customPopAni.onComplete += () =>
                {
                    DoAfterPopAniOver(willPopWnd, popReason, complete);
                };
            }
        }
    }
}