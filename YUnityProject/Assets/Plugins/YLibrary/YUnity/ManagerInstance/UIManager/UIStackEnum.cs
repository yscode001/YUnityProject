// Author：yaoshuai
// Email：yscode@126.com
// Date：2022-5-19
// ------------------------------

namespace YUnity
{
    /// <summary>
    /// 页面类型(新页面 or 新弹框)
    /// </summary>
    public enum PageType
    {
        NewPage,
        Dialog,
    }

    /// <summary>
    /// 页面状态
    /// </summary>
    public enum PageStateType
    {
        BeforePush,
        OnPush,
        OnPause,
        OnResume,
        WillExit,
        OnExit,
        UnKnown,
    }

    /// <summary>
    /// Pop原因
    /// </summary>
    public enum PopReason
    {
        Close,
        Cancel,
        Back,
        Exit,
        Destroy,
        Submit,
        Delete,
        Done,
        Send,
        Confirm,
    }

    /// <summary>
    /// Push动画
    /// </summary>
    public enum PushAni
    {
        None,
        LeftToRight,
        RightToLeft,
        BottomToTop,
        TopToBottom,
        ScaleSmallToBig,
        ScaleBigToSmall,
        FadeIn,
    }

    /// <summary>
    /// Pop动画
    /// </summary>
    public enum PopAni
    {
        None,
        LeftToRight,
        RightToLeft,
        BottomToTop,
        TopToBottom,
        ScaleSmallToBig,
        ScaleBigToSmall,
        FadeOut,
    }
}