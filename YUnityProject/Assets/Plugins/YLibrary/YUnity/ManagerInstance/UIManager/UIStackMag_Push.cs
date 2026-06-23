using System;
using System.Linq;
using UnityEngine;

namespace YUnity
{
    public partial class UIStackMag
    {
        private bool DoBeforePushAniStart(UIStackBaseWnd wnd, Transform parent, PageType pageType, Action<int> complete = null)
        {
            if (wnd == null || ContainsWnd(wnd) || GetTopWndName() == wnd.name || IsPushingOrPoping)
            {
                complete?.Invoke(0);
                return true;
            }
            IsPushingOrPoping = true;
            // 先暂停底部页面
            UIStackBaseWnd bottomWnd = _stack.LastOrDefault();
            if (bottomWnd != null)
            {
                bottomWnd.OnPause();
            }
            // 再添加新页面
            wnd.RectTransformY.SetParent(parent, false);
            wnd.SetupPageType(pageType);
            wnd.SetAct(true);
            wnd.BeforePush();
            return false;
        }
        private void DoAfterPushAniOver(UIStackBaseWnd wnd, PageType pageType, Action<int> complete = null)
        {
            // 1.入栈
            _stack.Add(wnd);
            if (pageType == PageType.NewPage)
            {
                // 2.如果添加的是新页面，整理页面可见性
                VisibilityChange_AfterStackChanged();
            }
            // 3.修改栈的完成状态
            IsPushingOrPoping = false;
            // 4.执行OnPush
            wnd.OnPush();
            // 5.回调结果
            complete?.Invoke(1);
        }

        public void Push(UIStackBaseWnd wnd, Transform parent, PageType pageType, PushAni pushAni, Action<int> complete = null)
        {
            if (DoBeforePushAniStart(wnd, parent, pageType, complete))
            {
                return;
            }
            wnd.SetupPageTypeAndRunPushAni(pageType, pushAni, () =>
            {
                DoAfterPushAniOver(wnd, pageType, complete);
            });
        }
    }
}