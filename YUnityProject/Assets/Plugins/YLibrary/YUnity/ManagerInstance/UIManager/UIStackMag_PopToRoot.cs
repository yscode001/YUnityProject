using System;
using System.Collections.Generic;

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

            // 计算需要pop掉的页面
            List<UIStackBaseWnd> willPopWnds = new List<UIStackBaseWnd>();
            for (int i = Stack.Count - 1; i > 0; i--)
            {
                willPopWnds.Add(Stack[i]);
                Stack.RemoveAt(i);
            }

            // 然后先整理页面可见性
            VisibilityChange();

            // 然后执行pop
            foreach (var item in willPopWnds)
            {
                item.OnExit(popReason);
            }

            // 再恢复底下的页面
            AfterPopResumeBottomWnd(complete);
        }
    }
}