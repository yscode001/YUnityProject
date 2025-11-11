// Author：yaoshuai
// Email：yscode@126.com
// Date：2022-5-19
// ------------------------------

namespace YUnity
{
    public enum PageType
    {
        NewPage,
        Dialog,
    }

    public enum PageState
    {
        OnPush,
        OnPause,
        OnResume,
        OnExit,
    }

    public enum PopType
    {
        Pop,
        PopCount,
        PopToRoot,
        PopAll,
    }

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
}