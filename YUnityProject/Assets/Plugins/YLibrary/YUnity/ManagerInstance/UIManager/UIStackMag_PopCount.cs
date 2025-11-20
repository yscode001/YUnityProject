using System;
using System.Collections.Generic;
using UnityEngine;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// Pop指定数量的页面，即连续Pop几次
        /// </summary>
        /// <param name="popCount"></param>
        /// <param name="popReason"></param>
        /// <param name="complete"></param>
        public void PopCount(int popCount, PopReason popReason, Action complete = null)
        {
            if (Stack.Count == 0 || popCount <= 0 || IsPushingOrPoping) { return; }
            IsPushingOrPoping = true;

            // 计算需要pop掉的页面
            int willPopTotalCount = Mathf.Min(popCount, Stack.Count);
            List<UIStackBaseWnd> willPopWnds = new List<UIStackBaseWnd>();
            for (int i = Stack.Count - 1; i >= 0; i--)
            {
                if (willPopTotalCount <= willPopWnds.Count)
                {
                    break;
                }
                willPopWnds.Add(Stack[i]);
                Stack.RemoveAt(i);
            }

            // 然后先整理页面可见性
            VisibilityChange();

            // 然后执行pop
            foreach (var item in willPopWnds)
            {
                item.OnExit(popReason);
            }

            // 再恢复底下的页面
            AfterPopResumeBottomWnd(complete);
        }
    }
}