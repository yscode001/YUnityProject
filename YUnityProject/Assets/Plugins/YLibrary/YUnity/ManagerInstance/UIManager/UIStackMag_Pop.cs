using System;
using System.Linq;

namespace YUnity
{
    public partial class UIStackMag
    {
        private void DoAfterPopAniOver(UIStackBaseWnd wnd, Action<int> complete = null)
        {
            // 1.本页面退出
            wnd.OnExit();
            // 2.修改栈的完成状态
            IsPushingOrPoping = false;
            // 3.新栈顶页面的恢复
            ResumeNewTopWnd_AfterPop();
            // 4.回调结果
            complete?.Invoke(1);
        }

        public void Pop(PopAni popAni, Action<int> complete = null)
        {
            if (_stack.Count == 0 || IsPushingOrPoping)
            {
                complete?.Invoke(0);
                return;
            }

            IsPushingOrPoping = true;

            // 1、计算需要pop掉的页面，并从栈中移除
            UIStackBaseWnd willPopWnd = _stack.LastOrDefault();
            willPopWnd.WillExit();
            _stack.Remove(willPopWnd);

            // 2、整理页面可见性
            if (willPopWnd.PageType == PageType.NewPage)
            {
                VisibilityChange_AfterStackChanged();
            }

            // 3、执行pop动画
            willPopWnd.RunPopAni(popAni, () => { DoAfterPopAniOver(willPopWnd, complete); });
        }
    }
}