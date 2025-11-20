using System;
using UnityEngine;

namespace YUnity
{
    public partial class UIStackMag
    {
        public void Push(UIStackBaseWnd wnd, Transform parent, PageType pageType, PushAni pushAni, Action complete = null)
        {
            if (wnd == null || Contains(wnd) || IsPushingOrPoping)
            {
                return;
            }
            IsPushingOrPoping = true;
            // 先暂停底部页面
            UIStackBaseWnd bottomWnd = GetTopWnd();
            if (bottomWnd != null)
            {
                bottomWnd.OnPause();
            }
            // 再添加新页面
            wnd.RectTransformY.SetParent(parent, false);
            wnd.SetAct(true);
            wnd.SetupPageTypeAndRunPushAni(pageType, pushAni, () =>
            {
                // 新页面push动画完成
                wnd.OnPush();
                // 入栈
                Stack.Add(wnd);
                if (pageType == PageType.NewPage)
                {
                    // 如果添加的是新页面，整理页面可见性
                    VisibilityChange();
                }
                // 完成
                complete?.Invoke();
                IsPushingOrPoping = false;
            });
        }
    }
}