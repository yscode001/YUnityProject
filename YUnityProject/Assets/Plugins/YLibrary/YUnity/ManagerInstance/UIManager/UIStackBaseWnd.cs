using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace YUnity
{
    #region 属性
    [RequireComponent(typeof(CanvasGroup))]
    public partial class UIStackBaseWnd : MonoBehaviourBaseY
    {
        private Image _dialogWndMaskBGImg = null;
        private bool _isInitedDialogWndMaskBGImg = false;

        /// <summary>
        /// 遮罩背景，主要应用于弹框，新页面可以不设置，通常是自己的RectTransform上的Image
        /// </summary>
        protected Image DialogWndMaskBGImg
        {
            get
            {
                if (_dialogWndMaskBGImg == null && _isInitedDialogWndMaskBGImg == false)
                {
                    _isInitedDialogWndMaskBGImg = true;
                    _dialogWndMaskBGImg = GetComponent<Image>();
                }
                return _dialogWndMaskBGImg;
            }
        }

        /// <summary>
        /// 页面内容的容器，做 Push 或 Pop 动画使用
        /// </summary>
        [Header("页面内容的容器，做 Push 或 Pop 动画使用")]
        [SerializeField] protected RectTransform ContentBoxRT;

        /// <summary>
        /// 页面类型(新页面 or 新弹框)
        /// </summary>
        public PageType PageType { get; private set; } = PageType.NewPage;

        private readonly ReactiveProperty<PageStateType> _pageState = new ReactiveProperty<PageStateType>(YUnity.PageStateType.UnKnown);

        /// <summary>
        /// 页面状态
        /// </summary>
        public IReadOnlyReactiveProperty<PageStateType> PageState => _pageState.ToReadOnlyReactiveProperty();
    }
    #endregion
    #region 自定义生命周期函数
    public partial class UIStackBaseWnd
    {
        public virtual void BeforePush()
        {
            CanvasGroupY.blocksRaycasts = false;
            if (_pageState.Value != YUnity.PageStateType.BeforePush)
            {
                _pageState.Value = YUnity.PageStateType.BeforePush;
            }
        }
        public virtual void OnPush()
        {
            CanvasGroupY.alpha = 1;
            CanvasGroupY.blocksRaycasts = true;
            if (_pageState.Value != YUnity.PageStateType.OnPush)
            {
                _pageState.Value = YUnity.PageStateType.OnPush;
            }
        }

        public virtual void OnPause()
        {
            CanvasGroupY.blocksRaycasts = false;
            if (_pageState.Value != YUnity.PageStateType.OnPause)
            {
                _pageState.Value = YUnity.PageStateType.OnPause;
            }
        }

        public virtual void OnResume()
        {
            CanvasGroupY.blocksRaycasts = true;
            if (_pageState.Value != YUnity.PageStateType.OnResume)
            {
                _pageState.Value = YUnity.PageStateType.OnResume;
            }
        }

        public virtual void OnExit(PopReason popReason)
        {
            CanvasGroupY.blocksRaycasts = false;
            if (_pageState.Value != YUnity.PageStateType.OnExit)
            {
                _pageState.Value = YUnity.PageStateType.OnExit;
            }
            _pageState.Dispose();
            DestroyImmediate(gameObject);
        }
    }
    #endregion
    #region 执行 Push or Pop 动画
    public partial class UIStackBaseWnd
    {
        private static readonly Color MaskColorTouMing = ColorUtil.Color(0, 0, 0, 0);
        private static readonly Color MaskColorShown = ColorUtil.Color(0, 0, 0, 0.8f);
        private static readonly Vector3 ScaleBigValue = Vector3.one * 1.25f;
        private static readonly Vector3 ScaleSmallValue = Vector3.one * 0.75f;
        private const float AniSeconds = 0.2f;

        private Sequence AniSequence;
        private void CreateAniSequence()
        {
            if (AniSequence != null)
            {
                AniSequence.Kill();
                AniSequence = null;
            }
            AniSequence = DOTween.Sequence();
        }
        internal void SetupPageType(PageType pageType)
        {
            PageType = pageType;
        }
        internal void SetupPageTypeAndRunPushAni(PageType pageType, PushAni pushAni, TweenCallback complete)
        {
            PageType = pageType;
            CreateAniSequence();
            switch (pushAni)
            {
                case PushAni.LeftToRight:
                    {
                        if (ContentBoxRT != null)
                        {
                            Vector2 originalPos = ContentBoxRT.anchoredPosition;
                            Vector2 fromPos = new Vector2(originalPos.x - ContentBoxRT.rect.width, originalPos.y);
                            ContentBoxRT.anchoredPosition = fromPos;
                            AniSequence.Append(ContentBoxRT.DOAnchorPos(originalPos, AniSeconds));
                        }
                        break;
                    }
                case PushAni.RightToLeft:
                    {
                        if (ContentBoxRT != null)
                        {
                            Vector2 originalPos = ContentBoxRT.anchoredPosition;
                            Vector2 fromPos = new Vector2(originalPos.x + ContentBoxRT.rect.width, originalPos.y);
                            ContentBoxRT.anchoredPosition = fromPos;
                            AniSequence.Append(ContentBoxRT.DOAnchorPos(originalPos, AniSeconds));
                        }
                        break;
                    }
                case PushAni.BottomToTop:
                    {
                        if (ContentBoxRT != null)
                        {
                            Vector2 originalPos = ContentBoxRT.anchoredPosition;
                            Vector2 fromPos = new Vector2(originalPos.x, originalPos.y - ContentBoxRT.rect.height);
                            ContentBoxRT.anchoredPosition = fromPos;
                            AniSequence.Append(ContentBoxRT.DOAnchorPos(originalPos, AniSeconds));
                        }
                        break;
                    }
                case PushAni.TopToBottom:
                    {
                        if (ContentBoxRT != null)
                        {
                            Vector2 originalPos = ContentBoxRT.anchoredPosition;
                            Vector2 fromPos = new Vector2(originalPos.x, originalPos.y + ContentBoxRT.rect.height);
                            ContentBoxRT.anchoredPosition = fromPos;
                            AniSequence.Append(ContentBoxRT.DOAnchorPos(originalPos, AniSeconds));
                        }
                        break;
                    }
                case PushAni.ScaleSmallToBig:
                    if (ContentBoxRT != null)
                    {
                        ContentBoxRT.localScale = ScaleSmallValue;
                        AniSequence.Append(ContentBoxRT.DOScale(Vector3.one, AniSeconds));
                    }
                    break;
                case PushAni.ScaleBigToSmall:
                    if (ContentBoxRT != null)
                    {
                        ContentBoxRT.localScale = ScaleBigValue;
                        AniSequence.Append(ContentBoxRT.DOScale(Vector3.one, AniSeconds));
                    }
                    break;
                case PushAni.FadeIn:
                    CanvasGroupY.alpha = 0;
                    AniSequence.Append(CanvasGroupY.DOFade(1, AniSeconds));
                    break;
                default:
                    break;
            }
            if (pushAni != PushAni.None && PageType == PageType.Dialog && DialogWndMaskBGImg != null)
            {
                DialogWndMaskBGImg.color = MaskColorTouMing;
                AniSequence.Join(DialogWndMaskBGImg.DOColor(MaskColorShown, AniSeconds));
            }
            AniSequence.OnComplete(complete);
        }
        internal void RunPopAni(PopAni popAni, TweenCallback complete)
        {
            CreateAniSequence();
            switch (popAni)
            {
                case PopAni.LeftToRight:
                    {
                        if (ContentBoxRT != null)
                        {
                            Vector2 originalPos = ContentBoxRT.anchoredPosition;
                            Vector2 toPos = new Vector2(originalPos.x + ContentBoxRT.rect.width, originalPos.y);
                            AniSequence.Append(ContentBoxRT.DOAnchorPos(toPos, AniSeconds));
                        }
                        break;
                    }
                case PopAni.RightToLeft:
                    {
                        if (ContentBoxRT != null)
                        {
                            Vector2 originalPos = ContentBoxRT.anchoredPosition;
                            Vector2 toPos = new Vector2(originalPos.x - ContentBoxRT.rect.width, originalPos.y);
                            AniSequence.Append(ContentBoxRT.DOAnchorPos(toPos, AniSeconds));
                        }
                        break;
                    }
                case PopAni.BottomToTop:
                    {
                        if (ContentBoxRT != null)
                        {
                            Vector2 originalPos = ContentBoxRT.anchoredPosition;
                            Vector2 toPos = new Vector2(originalPos.x, originalPos.y + ContentBoxRT.rect.height);
                            AniSequence.Append(ContentBoxRT.DOAnchorPos(toPos, AniSeconds));
                        }
                        break;
                    }
                case PopAni.TopToBottom:
                    {
                        if (ContentBoxRT != null)
                        {
                            Vector2 originalPos = ContentBoxRT.anchoredPosition;
                            Vector2 toPos = new Vector2(originalPos.x, originalPos.y - ContentBoxRT.rect.height);
                            AniSequence.Append(ContentBoxRT.DOAnchorPos(toPos, AniSeconds));
                        }
                        break;
                    }
                case PopAni.ScaleSmallToBig:
                    if (ContentBoxRT != null)
                    {
                        ContentBoxRT.localScale = Vector3.one;
                        AniSequence.Append(ContentBoxRT.DOScale(ScaleBigValue, AniSeconds));
                    }
                    break;
                case PopAni.ScaleBigToSmall:
                    if (ContentBoxRT != null)
                    {
                        ContentBoxRT.localScale = Vector3.one;
                        AniSequence.Append(ContentBoxRT.DOScale(ScaleSmallValue, AniSeconds));
                    }
                    break;
                case PopAni.FadeOut:
                    CanvasGroupY.alpha = 1;
                    AniSequence.Append(CanvasGroupY.DOFade(0, AniSeconds));
                    break;
                default:
                    break;
            }
            if (popAni != PopAni.None && PageType == PageType.Dialog && DialogWndMaskBGImg != null)
            {
                DialogWndMaskBGImg.color = MaskColorShown;
                AniSequence.Join(DialogWndMaskBGImg.DOColor(MaskColorTouMing, AniSeconds));
            }
            AniSequence.OnComplete(complete);
        }
    }
    #endregion
    #region 添加push快捷方式，实质还是调用的UIStackMag的push方法
    public partial class UIStackBaseWnd
    {
        /// <summary>
        /// Push新页面或新弹框，实质还是调用的UIStackMag的push方法
        /// </summary>
        public void Push(UIStackBaseWnd wnd, Transform parent, PageType pageType, PushAni pushAni, Action<bool> complete = null)
        {
            UIStackMag.Instance.Push(wnd, parent, pageType, pushAni, complete);
        }

        /// <summary>
        /// Push新页面或新弹框，实质还是调用的UIStackMag的push方法
        /// </summary>
        public void Push(UIStackBaseWnd wnd, Transform parent, PageType pageType, Sequence pushAni, Action<bool> complete = null)
        {
            UIStackMag.Instance.Push(wnd, parent, pageType, pushAni, complete);
        }

        /// <summary>
        /// Push自己，实质还是调用的UIStackMag的push方法
        /// </summary>
        public void PushThis(Transform parent, PageType pageType, PushAni pushAni, Action<bool> complete = null)
        {
            UIStackMag.Instance.Push(this, parent, pageType, pushAni, complete);
        }

        /// <summary>
        /// Push自己，实质还是调用的UIStackMag的push方法
        /// </summary>
        public void PushThis(Transform parent, PageType pageType, Sequence pushAni, Action<bool> complete = null)
        {
            UIStackMag.Instance.Push(this, parent, pageType, pushAni, complete);
        }

        /// <summary>
        /// Push自己，实质还是调用的UIStackMag的push方法
        /// </summary>
        public void PushSelf(Transform parent, PageType pageType, PushAni pushAni, Action<bool> complete = null)
        {
            UIStackMag.Instance.Push(this, parent, pageType, pushAni, complete);
        }

        /// <summary>
        /// Push自己，实质还是调用的UIStackMag的push方法
        /// </summary>
        public void PushSelf(Transform parent, PageType pageType, Sequence pushAni, Action<bool> complete = null)
        {
            UIStackMag.Instance.Push(this, parent, pageType, pushAni, complete);
        }
    }
    #endregion
    #region 添加pop的快捷方式，实质还是调用的UIStackMag的pop方法
    public partial class UIStackBaseWnd
    {
        /// <summary>
        /// Pop掉栈顶页面，实质还是调用的UIStackMag的pop方法
        /// </summary>
        public void Pop(PopReason popReason, PopAni popAni, Action<bool> complete = null)
        {
            UIStackMag.Instance.Pop(popReason, popAni, complete);
        }

        /// <summary>
        /// Pop掉栈顶页面，实质还是调用的UIStackMag的pop方法
        /// </summary>
        public void Pop(PopReason popReason, Sequence customPopAni, Action<bool> complete = null)
        {
            UIStackMag.Instance.Pop(popReason, customPopAni, complete);
        }

        /// <summary>
        /// Pop所有页面，实质还是调用的UIStackMag的pop方法
        /// </summary>
        public void PopAll(PopReason popReason, Action<bool> complete = null)
        {
            UIStackMag.Instance.PopAll(popReason, complete);
        }

        /// <summary>
        /// Pop所有底下的页面，除了栈顶页面，实质还是调用的UIStackMag的pop方法
        /// </summary>
        public void PopBottoms(PopReason popReason, Action<bool> complete = null)
        {
            UIStackMag.Instance.PopBottoms(popReason, complete);
        }

        /// <summary>
        /// Pop指定数量的页面，即连续Pop几次，实质还是调用的UIStackMag的pop方法
        /// </summary>
        public void PopCount(int popCount, PopReason popReason, Action<bool> complete = null)
        {
            UIStackMag.Instance.PopCount(popCount, popReason, complete);
        }

        /// <summary>
        /// Pop所有中间的页面，除了栈顶和栈底页面，实质还是调用的UIStackMag的pop方法
        /// </summary>
        public void PopMiddles(PopReason popReason, Action<bool> complete = null)
        {
            UIStackMag.Instance.PopMiddles(popReason, complete);
        }

        /// <summary>
        /// Pop至栈底页面，实质还是调用的UIStackMag的pop方法
        /// </summary>
        public void PopToRoot(PopReason popReason, Action<bool> complete = null)
        {
            UIStackMag.Instance.PopToRoot(popReason, complete);
        }

        /// <summary>
        /// Pop掉指定的页面，实质还是调用的UIStackMag的pop方法
        /// </summary>
        public void PopWnd(UIStackBaseWnd wnd, PopReason popReason, Action<bool> complete = null)
        {
            UIStackMag.Instance.PopWnd(wnd, popReason, complete);
        }

        /// <summary>
        /// Pop掉自己，实质还是调用的UIStackMag的pop方法
        /// </summary>
        public void PopThis(PopReason popReason, Action<bool> complete = null)
        {
            UIStackMag.Instance.PopWnd(this, popReason, complete);
        }

        /// <summary>
        /// Pop掉自己，实质还是调用的UIStackMag的pop方法
        /// </summary>
        public void PopSelf(PopReason popReason, Action<bool> complete = null)
        {
            UIStackMag.Instance.PopWnd(this, popReason, complete);
        }
    }
    #endregion
}