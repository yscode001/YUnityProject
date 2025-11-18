using System.Collections.Generic;
using UnityEngine;

namespace YUnity
{
    #region 私有API:pop工具方法
    public partial class UIStackMag
    {
        private void PrivatePop(PopType popType, PopReason popReason, int popCount, float delaySecondsThenDestroy)
        {
            if (popCount <= 0 || RTStack.Count <= 0) { return; };
            int pc = popCount;
            switch (popType)
            {
                case PopType.Pop: pc = 1; break;
                case PopType.PopCount: pc = Mathf.Clamp(popCount, 0, RTStack.Count); break;
                case PopType.PopToRoot: pc = RTStack.Count - 1; break;
                case PopType.PopAll: pc = RTStack.Count; break;
                default: break;
            }
            if (pc <= 0) { return; }
            PopType type = pc == 1 ? PopType.Pop : popType;

            List<RectTransform> willPopRTList = new List<RectTransform>();
            for (int times = 1; times <= pc && RTStack.Count > 0; times++)
            {
                RectTransform willPopRT = RTStack.Pop();
                if (willPopRT != null)
                {
                    willPopRTList.Add(willPopRT);
                }
            }
            if (RTStack.Count > 0)
            {
                RectTransform topRT = RTStack.Peek();
                if (topRT != null)
                {
                    RectTransform popFirstRT = null;
                    if (willPopRTList.Count > 0) { popFirstRT = willPopRTList[0]; }
                    topRT.GetOrAddComponent<UIStackBaseWnd>()?.OnResume(popFirstRT);
                    topRT.GetOrAddComponent<UIStackBaseWnd>()?.ExecuteAfterOnPushOrOnResume(false);
                }
            }
            foreach (RectTransform rt in willPopRTList)
            {
                rt.GetOrAddComponent<UIStackBaseWnd>()?.OnExit(popType, popReason, delaySecondsThenDestroy);
            }
        }
    }
    #endregion
    #region pop
    public partial class UIStackMag
    {
        /// <summary>
        /// 出栈，把栈顶元素pop掉(延迟销毁时间，期间用于执行pop动画)
        /// </summary>
        /// <param name="popReason"></param>
        /// <param name="delaySecondsThenDestroy"></param>
        public void Pop(PopReason popReason, float delaySecondsThenDestroy)
        {
            PrivatePop(PopType.Pop, popReason, 1, delaySecondsThenDestroy);
        }

        /// <summary>
        /// 指定次数的pop，即pop掉指定个数的栈顶元素
        /// </summary>
        /// <param name="popReason"></param>
        /// <param name="popCount"></param>
        public void PopCount(PopReason popReason, int popCount)
        {
            PrivatePop(PopType.PopCount, popReason, popCount, 0);
        }

        /// <summary>
        /// 出栈至栈中只剩一个元素
        /// </summary>
        /// <param name="popReason"></param>
        public void PopToRoot(PopReason popReason)
        {
            PrivatePop(PopType.PopToRoot, popReason, RTStack.Count - 1, 0);
        }

        /// <summary>
        /// pop掉全部元素
        /// </summary>
        /// <param name="popReason"></param>
        public void PopAll(PopReason popReason)
        {
            PrivatePop(PopType.PopAll, popReason, RTStack.Count, 0);
        }
    }
    #endregion
}