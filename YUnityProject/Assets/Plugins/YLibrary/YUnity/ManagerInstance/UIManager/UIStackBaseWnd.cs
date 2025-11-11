using UnityEngine;

namespace YUnity
{
    #region 属性
    [RequireComponent(typeof(CanvasGroup))]
    public partial class UIStackBaseWnd : MonoBehaviourBaseY
    {
        public PageState PageState { get; private set; } = PageState.OnPush;
    }
    #endregion
    #region 自定义生命周期函数
    public partial class UIStackBaseWnd
    {
        public virtual void OnPush(PageType pageType, RectTransform bottomRT)
        {
            this.SetAct(true);
            CanvasGroupY.alpha = 1;
            CanvasGroupY.blocksRaycasts = true;
            PageState = PageState.OnPush;
        }
        public virtual void OnPause(RectTransform topRT, PageType topPageType)
        {
            CanvasGroupY.blocksRaycasts = false;
            PageState = PageState.OnPause;
            if (topPageType == PageType.NewPage)
            {
                if (UIStackMag.Instance.MaxPushTransitionSeconds <= 0)
                {
                    //this.SetAct(false);
                }
                else
                {
                    DoAfterDelay(UIStackMag.Instance.MaxPushTransitionSeconds, () =>
                    {
                        if (PageState == PageState.OnPause)
                        {
                            // 确保状态为OnPause的状态下执行
                            //this.SetAct(false);
                        }
                    });
                }
            }
        }

        public virtual void OnResume(RectTransform popedRT)
        {
            this.SetAct(true);
            CanvasGroupY.alpha = 1;
            CanvasGroupY.blocksRaycasts = true;
            PageState = PageState.OnResume;
        }

        public virtual void OnExit(PopType popType, PopReason popReason, float delaySecondsThenDestroy)
        {
            PageState = PageState.OnExit;
            if (delaySecondsThenDestroy <= 0 || gameObject.activeInHierarchy == false)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                DoAfterDelay(delaySecondsThenDestroy, () => { DestroyImmediate(gameObject); });
            }
        }

        public virtual void ExecuteAfterOnPushOrOnResume(bool isAfterPush)
        {

        }
    }
    #endregion
}