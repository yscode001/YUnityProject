using System;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// Pop所有页面
        /// </summary>
        public void PopAll(PopReason popReason, Action<int> complete = null)
        {
            if (_stack.Count == 0 || IsPushingOrPoping)
            {
                complete?.Invoke(0);
                return;
            }
            IsPushingOrPoping = true;
            int willPopTotalCount = _stack.Count;
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                _stack[i].WillExit(popReason);
                _stack[i].OnExit(popReason);
            }
            _stack.Clear();
            IsPushingOrPoping = false;
            complete?.Invoke(willPopTotalCount);
        }
    }
}