using System;
using System.Collections.Generic;
using UnityEngine;

namespace YUnity
{
    /// <summary>
    /// UI栈结构管理器，挂在Scene的某个GO上
    /// </summary>
    public partial class UIStackMag : MonoBehaviourBaseY
    {
        private UIStackMag() { }
        public static UIStackMag Instance { get; private set; } = null;

        private Stack<RectTransform> _rtStack;
        private Stack<RectTransform> RTStack
        {
            get
            {
                if (_rtStack == null) { _rtStack = new Stack<RectTransform>(); }
                return _rtStack;
            }
        }

        /// <summary>
        /// 最大Push转场时间，用于Push一个NewPage后，把底下的置为Active=false
        /// </summary>
        public float MaxPushTransitionSeconds { get; private set; } = 0.3f;

        internal void Init()
        {
            Instance = this;
        }

        /// <summary>
        /// 设置最大转场时间，用于Push一个NewPage后，把底下的置为Active=false
        /// </summary>
        /// <param name="maxPushTransitionSeconds"></param>
        public void SetupMaxTransitionSeconds(float maxPushTransitionSeconds)
        {
            MaxPushTransitionSeconds = Mathf.Max(0, maxPushTransitionSeconds);
        }
    }
    #region push
    public partial class UIStackMag
    {
        /// <summary>
        /// 压栈，把RT放入栈中
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="parent"></param>
        /// <param name="pageType"></param>
        public void Push(RectTransform rt, Transform parent, PageType pageType)
        {
            if (rt == null || RTStack.Contains(rt))
            {
                return;
            }
            rt.SetParent(parent, false);
            rt.SetAct(true);
            RectTransform bottomRT = null;
            if (RTStack.Count > 0)
            {
                bottomRT = RTStack.Peek();
                bottomRT.GetOrAddComponent<UIStackBaseWnd>()?.OnPause(rt, pageType);
            }
            rt.GetOrAddComponent<UIStackBaseWnd>()?.OnPush(pageType, bottomRT);
            rt.GetOrAddComponent<UIStackBaseWnd>()?.ExecuteAfterOnPushOrOnResume(true);
            RTStack.Push(rt);
        }
        /// <summary>
        /// 压栈，把RT放入栈中
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="parent"></param>
        /// <param name="pageType"></param>
        /// <param name="before">压栈流程开始之前执行</param>
        public void Push(RectTransform rt, Transform parent, PageType pageType, Action<RectTransform> before)
        {
            if (rt == null || RTStack.Contains(rt))
            {
                return;
            }
            before?.Invoke(rt);
            Push(rt, parent, pageType);
        }
    }
    #endregion
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
    #region other
    public partial class UIStackMag
    {
        /// <summary>
        /// 栈里面元素的个数
        /// </summary>
        public int StackElementsCount => RTStack.Count;

        /// <summary>
        /// 获取栈顶元素
        /// </summary>
        public RectTransform TopElement => RTStack.Peek();

        /// <summary>
        /// 获取栈里所有元素
        /// </summary>
        /// <returns></returns>
        public List<RectTransform> GetStackAllElements()
        {
            List<RectTransform> list = new List<RectTransform>();
            if (_rtStack != null)
            {
                foreach (var item in _rtStack)
                {
                    list.Add(item);
                }
            }
            return list;
        }
    }
    #endregion
}