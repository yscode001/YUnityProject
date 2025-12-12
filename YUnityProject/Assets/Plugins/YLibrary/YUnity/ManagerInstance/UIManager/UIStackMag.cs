using System.Linq;
using UniRx;

namespace YUnity
{
    #region 单例
    public partial class UIStackMag : MonoBehaviourBaseY
    {
        private UIStackMag() { }
        public static UIStackMag Instance { get; private set; } = null;

        internal void Init()
        {
            Instance = this;
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
        public bool ContainsWnd(UIStackBaseWnd wnd) => _stack.Contains(wnd);
        public bool ContainsWnd(string wndName) => _stack.FirstOrDefault(m => m.name == wndName) != null;
    }
    #endregion
    #region 工具方法，整理页面可见性
    public partial class UIStackMag
    {
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
    }
    #endregion
}