using System;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// Pop所有底下的页面，除了栈顶页面
        /// </summary>
        /// <param name="popReason"></param>
        /// <param name="complete"></param>
        public void PopBottoms(PopReason popReason, Action complete = null)
        {
            if (Stack.Count <= 1 || IsPushingOrPoping) { return; }

            IsPushingOrPoping = true;
        }
    }
}