using System.Linq;
using UniRx;

namespace YUnity
{
    #region 单例

    public partial class UIStackMag : MonoBehaviourBaseY
    {
        private UIStackMag()
        {
        }

        public static UIStackMag Instance { get; private set; } = null;

        internal void Init()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }

    #endregion

    #region 内部工具方法

    public partial class UIStackMag
    {
        /// <summary>
        /// 整理页面可见性
        /// </summary>
        internal void VisibilityChange_AfterStackChanged()
        {
            bool topWndIsNewPage = false;
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                UIStackBaseWnd wnd = _stack[i];
                if (topWndIsNewPage)
                {
                    // 上面被新页面覆盖，本页面可隐藏
                    wnd.SetAct(false);
                    continue;
                }

                // 上面未被新页面覆盖，本页面显示
                wnd.SetAct(true);
                topWndIsNewPage = wnd.PageType == PageType.NewPage;
            }
        }

        /// <summary>
        /// 新栈顶页面的恢复
        /// </summary>
        internal void ResumeNewTopWnd_AfterPop()
        {
            if (_stack.Count > 0)
            {
                _stack.LastOrDefault().OnResume();
            }
        }
    }

    #endregion

    #region 定义外界可使用的属性和方法

    public partial class UIStackMag
    {
        private readonly ReactiveCollection<UIStackBaseWnd> _stack = new ReactiveCollection<UIStackBaseWnd>();
        public IReadOnlyReactiveCollection<UIStackBaseWnd> Stack => _stack;

        public bool IsPushingOrPoping { get; private set; } = false;

        public UIStackBaseWnd GetTopWnd() => _stack.LastOrDefault();
        public string GetTopWndName() => _stack.LastOrDefault()?.name;
        public UIStackBaseWnd GetBottomWnd() => _stack.FirstOrDefault();
        public string GetBottomWndName() => _stack.FirstOrDefault()?.name;
        public bool ContainsWnd(UIStackBaseWnd wnd) => _stack.Contains(wnd);
        public bool ContainsWnd(string wndName) => _stack.FirstOrDefault(m => m.name == wndName) != null;
        public bool ContainsAnyWnd(UIStackBaseWnd[] wnds) => _stack.FirstOrDefault(wnds.Contains) != null;
        public bool ContainsAnyWnd(string[] wndNames) => _stack.FirstOrDefault(m => wndNames.Contains(m.name)) != null;
    }

    #endregion
}