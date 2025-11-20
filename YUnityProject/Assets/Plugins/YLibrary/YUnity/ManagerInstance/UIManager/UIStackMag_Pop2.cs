using System;
using System.Linq;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// Pop完成后，恢复底下的页面
        /// </summary>
        /// <param name="complete"></param>
        private void AfterPopResumeBottomWnd(Action complete)
        {
            if (Stack.Count > 0)
            {
                Stack.LastOrDefault().OnResume();
            }
            complete?.Invoke();
            IsPushingOrPoping = false;
        }
    }
}