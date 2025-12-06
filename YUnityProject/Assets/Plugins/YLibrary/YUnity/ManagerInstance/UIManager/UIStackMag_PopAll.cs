using System;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// Pop所有页面
        /// </summary>
        public void PopAll(PopReason popReason, Action<bool> complete = null)
        {
            if (_stack.Count == 0 || IsPushingOrPoping)
            {
                complete?.Invoke(false);
                return;
            }
            IsPushingOrPoping = true;
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                _stack[i].OnExit(popReason);
            }
            _stack.Clear();
            IsPushingOrPoping = false;
            complete?.Invoke(true);
        }
    }
}