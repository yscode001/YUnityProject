using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YUnity
{
    public partial class UIStackMag
    {
        /// <summary>
        /// Pop指定数量的页面，即连续Pop几次
        /// </summary>
        public void PopCount(int popCount, PopReason popReason, Action<bool> complete = null)
        {
            if (_stack.Count == 0 || popCount <= 0 || IsPushingOrPoping)
            {
                complete?.Invoke(false);
                return;
            }
            IsPushingOrPoping = true;

            // 1、计算需要pop掉的页面，并从栈中移除
            int willPopTotalCount = Mathf.Min(popCount, _stack.Count);
            List<UIStackBaseWnd> willPopWnds = new List<UIStackBaseWnd>();
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                if (willPopTotalCount <= willPopWnds.Count)
                {
                    break;
                }
                willPopWnds.Add(_stack[i]);
                _stack.RemoveAt(i);
            }

            // 2、整理页面可见性
            VisibilityChange_AfterStackChanged();

            // 3、退出页面
            foreach (var item in willPopWnds)
            {
                item.OnExit(popReason);
            }

            // 4、底下的页面恢复
            if (_stack.Count > 0)
            {
                _stack.LastOrDefault().OnResume();
            }

            // 5、完成
            IsPushingOrPoping = false;
            complete?.Invoke(true);
        }
    }
}