using System;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// Pop所有页面
        /// </summary>
        /// <param name="popReason"></param>
        /// <param name="complete"></param>
        public void PopAll(PopReason popReason, Action complete = null)
        {
            if (Stack.Count == 0 || IsPushingOrPoping) { return; }
            IsPushingOrPoping = true;

            for (int i = Stack.Count - 1; i >= 0; i--)
            {
                Stack[i].OnExit(popReason);
            }

            IsPushingOrPoping = false;
            complete?.Invoke();
        }
    }
}