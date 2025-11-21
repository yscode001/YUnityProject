using System;
using System.Collections.Generic;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// Pop所有底下的页面，除了栈顶页面
        /// </summary>
        /// <param name="popReason"></param>
        /// <param name="complete"></param>
        public void PopBottoms(PopReason popReason, Action<bool> complete = null)
        {
            if (Stack.Count <= 1 || IsPushingOrPoping)
            {
                complete?.Invoke(false);
                return;
            }
            IsPushingOrPoping = true;

            // 1、计算需要pop掉的页面，并从栈中移除
            List<UIStackBaseWnd> willPopWnds = new List<UIStackBaseWnd>();
            for (int i = Stack.Count - 2; i >= 0; i--)
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

            // 5、完成
            IsPushingOrPoping = false;
            complete?.Invoke(true);
        }
    }
}