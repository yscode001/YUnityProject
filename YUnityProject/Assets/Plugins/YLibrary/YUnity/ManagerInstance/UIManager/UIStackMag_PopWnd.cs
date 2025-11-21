using System;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// Pop掉指定的页面
        /// </summary>
        /// <param name="wnd"></param>
        /// <param name="popReason"></param>
        /// <param name="complete"></param>
        public void PopWnd(UIStackBaseWnd wnd, PopReason popReason, Action<bool> complete = null)
        {
            if (wnd == null || !Stack.Contains(wnd) || IsPushingOrPoping)
            {
                complete?.Invoke(false);
                return;
            }
            if (wnd == GetTopWnd())
            {
                // 栈顶页面
                Pop(popReason, PopAni.None, complete);
            }
            else
            {
                // 非栈顶元素
                IsPushingOrPoping = true;
                // 1、从栈中移除
                Stack.Remove(wnd);

                // 2、整理页面可见性
                VisibilityChange();

                // 3、页面退出
                wnd.OnExit(popReason);

                // 4、完成
                IsPushingOrPoping = false;
                complete?.Invoke(true);
            }
        }
    }
}