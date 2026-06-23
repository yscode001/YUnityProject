using System;
using System.Collections.Generic;
using System.Linq;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// Pop掉指定的页面
        /// </summary>
        public void PopWnds(List<UIStackBaseWnd> wnds, PopReason popReason, Action<int> complete = null)
        {
            if (wnds == null || wnds.Count == 0 || _stack.Count == 0)
            {
                complete?.Invoke(0);
                return;
            }
            List<UIStackBaseWnd> willPopWnds = new List<UIStackBaseWnd>();
            foreach (var wnd in _stack)
            {
                if (wnds.Contains(wnd))
                {
                    willPopWnds.Add(wnd);
                }
            }
            if (willPopWnds.Count == 0)
            {
                complete?.Invoke(0);
                return;
            }
            bool willPopWndsContainsTopWnd = willPopWnds.Contains(_stack.LastOrDefault());
            int willPopTotalCount = willPopWnds.Count;
            // 1、从栈中移除
            foreach (var wnd in willPopWnds)
            {
                _stack.Remove(wnd);
            }
            // 2、整理页面可见性
            VisibilityChange_AfterStackChanged();
            // 3、退出页面
            foreach (var item in willPopWnds)
            {
                item.WillExit(popReason);
                item.OnExit(popReason);
            }
            // 4、修改栈的完成状态
            IsPushingOrPoping = false;
            // 5、新栈顶页面的恢复
            if (willPopWndsContainsTopWnd)
            {
                ResumeNewTopWnd_AfterPop();
            }
            // 6、回调结果
            complete?.Invoke(willPopTotalCount);
        }
    }
}