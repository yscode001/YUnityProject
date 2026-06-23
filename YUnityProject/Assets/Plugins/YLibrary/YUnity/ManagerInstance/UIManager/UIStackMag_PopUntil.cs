using System;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// 一直pop下去，直到遇到wndName为止，wndName不会pop
        /// </summary>
        public void PopUntil(string wndName, PopReason popReason, Action<int> complete = null)
        {
            if (_stack.Count == 0 || IsPushingOrPoping)
            {
                complete?.Invoke(0);
                return;
            }
            int willPopCount = 0;
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                if (_stack[i].name == wndName)
                {
                    break;
                }
                else
                {
                    willPopCount += 1;
                }
            }
            if (willPopCount == 0)
            {
                complete?.Invoke(0);
            }
            else
            {
                PopCount(willPopCount, popReason, complete);
            }
        }

        /// <summary>
        /// 一直pop下去，直到遇到wnd为止，wnd不会pop
        /// </summary>
        public void PopUntil(UIStackBaseWnd wnd, PopReason popReason, Action<int> complete = null)
        {
            if (_stack.Count == 0 || IsPushingOrPoping)
            {
                complete?.Invoke(0);
                return;
            }
            int willPopCount = 0;
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                if (_stack[i] == wnd)
                {
                    break;
                }
                else
                {
                    willPopCount += 1;
                }
            }
            if (willPopCount == 0)
            {
                complete?.Invoke(0);
            }
            else
            {
                PopCount(willPopCount, popReason, complete);
            }
        }
    }
}