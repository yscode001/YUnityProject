using System;
using UnityEngine;

namespace YUnity
{
    #region push
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
                bottomWnd.OnPause(wnd);
            }
            // 再添加新页面
            wnd.RectTransformY.SetParent(parent, false);
            wnd.SetAct(true);
            wnd.SetupPageTypeAndRunPushAni(pageType, pushAni, () =>
            {
                // 新页面添加完成，入栈
                wnd.OnPush(bottomWnd);
                Stack.Add(wnd);
                if (pageType == PageType.NewPage)
                {
                    // 如果添加的是新页面，底下的页面可以隐藏
                    if (bottomWnd != null)
                    {
                        bottomWnd.SetAct(false);
                    }
                }
                IsPushingOrPoping = false;
                complete?.Invoke();
            });
        }
    }
    #endregion
}