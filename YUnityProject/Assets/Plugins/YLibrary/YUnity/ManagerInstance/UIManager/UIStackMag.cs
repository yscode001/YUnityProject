using System.Collections.Generic;
using System.Linq;

namespace YUnity
{
    public partial class UIStackMag : MonoBehaviourBaseY
    {
        private UIStackMag() { }
        public static UIStackMag Instance { get; private set; } = null;

        internal void Init()
        {
            Instance = this;
        }

        private List<UIStackBaseWnd> _stack;
        private List<UIStackBaseWnd> Stack
        {
            get
            {
                _stack ??= new List<UIStackBaseWnd>();
                return _stack;
            }
        }
    }
    #region 对外提供属性和方法
    public partial class UIStackMag
    {
        public bool IsPushingOrPoping { get; private set; } = false;

        public int GetWndCount() => Stack.Count;

        public UIStackBaseWnd GetTopWnd() => Stack.LastOrDefault();

        public bool Contains(UIStackBaseWnd wnd) => Stack.Contains(wnd);

        public bool ContainsName(string wndName)
        {
            foreach (var item in Stack)
            {
                if (item.name == wndName)
                {
                    return true;
                }
            }
            return false;
        }
    }
    #endregion
    #region 工具方法，整理页面可见性
    public partial class UIStackMag
    {
        internal void VisibilityChange()
        {
            bool topIsNewPage = false;
            for (int i = Stack.Count - 1; i >= 0; i--)
            {
                UIStackBaseWnd wnd = Stack[i];
                if (topIsNewPage)
                {
                    // 上面被新页面覆盖，本页面可隐藏
                    wnd.SetAct(false);
                }
                else
                {
                    // 上面未被新页面覆盖，本页面显示
                    wnd.SetAct(true);
                    topIsNewPage = wnd.PageType == PageType.NewPage;
                }
            }
        }
    }
    #endregion
}