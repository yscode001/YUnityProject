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

        private readonly ReactiveProperty<PageState> _pageState =
            new ReactiveProperty<PageState>(YUnity.PageState.UnKnown);

        /// <summary>
        /// 页面状态
        /// </summary>
        public IReadOnlyReactiveProperty<PageState> PageState => _pageState.ToReadOnlyReactiveProperty();
    }

    #endregion

    #region 自定义生命周期函数

    public partial class UIStackBaseWnd
    {
        public virtual void BeforePush()
        {
            CanvasGroupY.blocksRaycasts = false;
            if (_pageState.Value != YUnity.PageState.BeforePush)
            {
                _pageState.Value = YUnity.PageState.BeforePush;
            }
        }

        public virtual void OnPush()
        {
            CanvasGroupY.alpha = 1;
            CanvasGroupY.blocksRaycasts = true;
            if (_pageState.Value != YUnity.PageState.OnPush)
            {
                _pageState.Value = YUnity.PageState.OnPush;
            }
        }

        public virtual void OnPause()
        {
            CanvasGroupY.blocksRaycasts = false;
            if (_pageState.Value != YUnity.PageState.OnPause)
            {
                _pageState.Value = YUnity.PageState.OnPause;
            }
        }

        public virtual void OnResume()
        {
            CanvasGroupY.blocksRaycasts = true;
            if (_pageState.Value != YUnity.PageState.OnResume)
            {
                _pageState.Value = YUnity.PageState.OnResume;
            }
        }

        public virtual void WillExit()
        {
            CanvasGroupY.blocksRaycasts = false;
            if (_pageState.Value != YUnity.PageState.WillExit)
            {
                _pageState.Value = YUnity.PageState.WillExit;
            }
        }

        public virtual void OnExit()
        {
            CanvasGroupY.blocksRaycasts = false;
            if (_pageState.Value != YUnity.PageState.OnExit)
            {
                _pageState.Value = YUnity.PageState.OnExit;
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

        private void ReCreateAniSequence()
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
            if (pushAni == PushAni.Custom)
            {
                AniSequence = CreateCustomPushAniSequence();
                if (AniSequence == null)
                {
                    complete?.Invoke();
                }
                else
                {
                    AniSequence.OnComplete(complete);
                }
            }
            else
            {
                ReCreateAniSequence();
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
        }

        internal void RunPopAni(PopAni popAni, TweenCallback complete)
        {
            if (popAni == PopAni.Custom)
            {
                AniSequence = CreateCustomPopAniSequence();
                if (AniSequence == null)
                {
                    complete?.Invoke();
                }
                else
                {
                    AniSequence.OnComplete(complete);
                }
            }
            else
            {
                ReCreateAniSequence();
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
    }

    #endregion

    #region 自定义 Push or Pop 动画

    public partial class UIStackBaseWnd
    {
        protected virtual Sequence CreateCustomPushAniSequence()
        {
            return null;
        }

        protected virtual Sequence CreateCustomPopAniSequence()
        {
            return null;
        }
    }

    #endregion
}