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
        /// <param name="popReason"></param>
        /// <param name="complete"></param>
        public void PopToRoot(PopReason popReason, Action complete = null)
        {
            if (Stack.Count <= 1 || IsPushingOrPoping) { return; }
            IsPushingOrPoping = true;

            // 1、计算需要pop掉的页面，并从栈中移除
            List<UIStackBaseWnd> willPopWnds = new List<UIStackBaseWnd>();
            for (int i = Stack.Count - 1; i > 0; i--)
            {
                willPopWnds.Add(Stack[i]);
                Stack.RemoveAt(i);
            }

            // 2、整理页面可见性
            VisibilityChange();

            // 3、退出页面
            foreach (var item in willPopWnds)
            {
                item.OnExit(popReason);
            }

            // 4、底下的页面恢复
            if (Stack.Count > 0)
            {
                Stack.LastOrDefault().OnResume();
            }

            // 5、完成
            IsPushingOrPoping = false;
            complete?.Invoke();
        }
    }
}