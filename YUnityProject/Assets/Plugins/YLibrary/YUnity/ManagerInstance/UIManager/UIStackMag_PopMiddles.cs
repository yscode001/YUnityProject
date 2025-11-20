using System;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// Pop所有中间的页面，除了栈顶和栈底页面
        /// </summary>
        /// <param name="popReason"></param>
        /// <param name="complete"></param>
        public void PopMiddles(PopReason popReason, Action complete = null)
        {
            if (Stack.Count <= 2 || IsPushingOrPoping) { return; }
            IsPushingOrPoping = true;
        }
    }
}