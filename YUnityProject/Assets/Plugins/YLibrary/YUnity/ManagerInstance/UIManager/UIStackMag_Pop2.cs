using System;

namespace YUnity
{
    public partial class UIStackMag
    {
        private void PopIfHaveOnlyOnePage(PopReason popReason, PopAni popAni, Action complete = null)
        {
            UIStackBaseWnd topWnd = GetTopWnd();
            topWnd.RunPopAni(popAni, () =>
            {
                // 出栈
                topWnd.OnExit(popReason);
                Stack.Clear();
                complete?.Invoke();
            });
        }

        /// <summary>
        /// Pop掉栈顶页面
        /// </summary>
        /// <param name="popReason"></param>
        /// <param name="popAni"></param>
        /// <param name="complete"></param>
        public void Pop(PopReason popReason, PopAni popAni, Action complete = null)
        {
            if (Stack.Count == 0) { return; }
            if (Stack.Count == 1) { PopIfHaveOnlyOnePage(popReason, popAni, complete); return; }
        }

        /// <summary>
        /// Pop指定数量的页面，即连续Pop几次
        /// </summary>
        /// <param name="complete"></param>
        public void PopCount(Action complete = null)
        {
            if (Stack.Count == 0) { return; }
            if (Stack.Count == 1) { PopIfHaveOnlyOnePage(PopReason.Done, PopAni.None, complete); return; }
        }

        /// <summary>
        /// Pop至栈底页面
        /// </summary>
        /// <param name="complete"></param>
        public void PopToRoot(Action complete = null)
        {
            if (Stack.Count == 0) { return; }
            if (Stack.Count == 1) { PopIfHaveOnlyOnePage(PopReason.Done, PopAni.None, complete); return; }
        }

        /// <summary>
        /// Pop所有页面
        /// </summary>
        /// <param name="complete"></param>
        public void PopAll(Action complete = null)
        {
            if (Stack.Count == 0) { return; }
        }

        /// <summary>
        /// Pop所有中间的页面，除了栈顶和栈底页面
        /// </summary>
        /// <param name="complete"></param>
        public void PopMiddleAll(Action complete = null)
        {
            if (Stack.Count == 0) { return; }
        }

        /// <summary>
        /// Pop所有底下的页面，除了栈顶页面
        /// </summary>
        /// <param name="complete"></param>
        public void PopBottomAll(Action complete = null)
        {
            if (Stack.Count == 0) { return; }
        }
    }
}