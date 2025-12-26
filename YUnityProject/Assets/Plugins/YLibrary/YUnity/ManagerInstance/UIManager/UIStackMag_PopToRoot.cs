using System;
using System.Collections.Generic;
using System.Linq;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// Pop至栈底页面
        /// </summary>
        public void PopToRoot(PopReason popReason, Action<bool> complete = null)
        {
            if (_stack.Count <= 1 || IsPushingOrPoping)
            {
                complete?.Invoke(false);
                return;
            }
            IsPushingOrPoping = true;

            // 1、计算需要pop掉的页面，并从栈中移除
            List<UIStackBaseWnd> willPopWnds = new List<UIStackBaseWnd>();
            for (int i = _stack.Count - 1; i > 0; i--)
            {
                willPopWnds.Add(_stack[i]);
                _stack.RemoveAt(i);
            }

            // 2、整理页面可见性
            VisibilityChange_AfterStackChanged();

            // 3、退出页面
            foreach (var item in willPopWnds)
            {
                item.WillExit(popReason);
                item.OnExit(popReason);
            }

            // 4、底下的页面恢复
            if (_stack.Count > 0)
            {
                _stack.LastOrDefault().OnResume();
            }

            // 5、完成
            IsPushingOrPoping = false;
            complete?.Invoke(true);
        }
    }
}