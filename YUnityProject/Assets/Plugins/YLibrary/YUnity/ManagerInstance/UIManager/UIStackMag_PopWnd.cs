using System;
using System.Linq;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// Pop掉指定的页面
        /// </summary>
        public void PopWnd(UIStackBaseWnd wnd, PopReason popReason, Action<bool> complete = null)
        {
            if (wnd == null || !_stack.Contains(wnd) || IsPushingOrPoping)
            {
                complete?.Invoke(false);
                return;
            }
            if (wnd == _stack.LastOrDefault())
            {
                // 栈顶页面
                Pop(popReason, PopAni.None, complete);
            }
            else
            {
                // 非栈顶元素
                IsPushingOrPoping = true;
                // 1、从栈中移除
                _stack.Remove(wnd);

                // 2、整理页面可见性
                VisibilityChange_AfterStackChanged();

                // 3、页面退出
                wnd.WillExit(popReason);
                wnd.OnExit(popReason);

                // 4、完成
                IsPushingOrPoping = false;
                complete?.Invoke(true);
            }
        }
    }
}