using System.Collections.Generic;
using System.Linq;

namespace YUnity
{
    /// <summary>
    /// UI栈结构管理器
    /// </summary>
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
        /// <summary>
        /// 是否正在push页面或pop页面
        /// </summary>
        public bool IsPushingOrPoping { get; private set; } = false;

        /// <summary>
        /// 栈里面页面的数量
        /// </summary>
        /// <returns></returns>
        public int GetWndCount() => Stack.Count;

        /// <summary>
        /// 获取栈顶页面
        /// </summary>
        /// <returns></returns>
        public UIStackBaseWnd GetTopWnd() => Stack.LastOrDefault();

        /// <summary>
        /// 栈中是否包含某个页面
        /// </summary>
        /// <param name="wnd"></param>
        /// <returns></returns>
        public bool Contains(UIStackBaseWnd wnd) => Stack.Contains(wnd);

        /// <summary>
        /// 栈中是否包含某个页面，这个页面的名字和参数名字一样
        /// </summary>
        /// <param name="wndName"></param>
        /// <returns></returns>
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
}