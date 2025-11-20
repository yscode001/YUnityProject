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
        /// <summary>
        /// 遮罩背景，主要应用于弹框，新页面可以不设置
        /// </summary>
        [Header("遮罩背景，主要应用于弹框，新页面可以不设置，通常是自己的RectTransform上的Image")]
        [SerializeField] protected Image MaskBGImg;

        /// <summary>
        /// 页面内容的容器，做 Push 或 Pop 动画使用
        /// </summary>
        [Header("页面内容的容器，做 Push 或 Pop 动画使用")]
        [SerializeField] protected RectTransform ContentContainerRT;

        public PageType PageType { get; private set; } = PageType.NewPage;

        private BehaviorSubject<PageState> _pageState = null;
        public BehaviorSubject<PageState> PageState
        {
            get
            {
                _pageState ??= new BehaviorSubject<PageState>(YUnity.PageState.OnPush);
                return _pageState;
            }
        }
    }
    #endregion
    #region 自定义生命周期函数
    public partial class UIStackBaseWnd
    {
        public virtual void OnPush()
        {
            CanvasGroupY.alpha = 1;
            CanvasGroupY.blocksRaycasts = true;
            if (PageState.Value != YUnity.PageState.OnPush)
            {
                PageState.OnNext(YUnity.PageState.OnPush);
            }
        }

        public virtual void OnPause()
        {
            CanvasGroupY.blocksRaycasts = false;
            if (PageState.Value != YUnity.PageState.OnPause)
            {
                PageState.OnNext(YUnity.PageState.OnPause);
            }
        }

        public virtual void OnResume()
        {
            CanvasGroupY.blocksRaycasts = true;
            if (PageState.Value != YUnity.PageState.OnResume)
            {
                PageState.OnNext(YUnity.PageState.OnResume);
            }
        }

        public virtual void OnExit(PopReason popReason)
        {
            CanvasGroupY.blocksRaycasts = false;
            if (PageState.Value != YUnity.PageState.OnExit)
            {
                PageState.OnNext(YUnity.PageState.OnExit);
            }
            PageState.OnCompleted();
            PageState.Dispose();
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
        internal void SetupPageTypeAndRunPushAni(PageType pageType, PushAni pushAni, TweenCallback complete)
        {
            PageType = pageType;
            CreateAniSequence();
            switch (pushAni)
            {
                case PushAni.LeftToRight:
                    {
                        Vector2 originalPos = ContentContainerRT.anchoredPosition;
                        Vector2 fromPos = new Vector2(originalPos.x - ContentContainerRT.rect.width, originalPos.y);
                        ContentContainerRT.anchoredPosition = fromPos;
                        AniSequence.Append(ContentContainerRT.DOAnchorPos(originalPos, AniSeconds));
                        break;
                    }
                case PushAni.RightToLeft:
                    {
                        Vector2 originalPos = ContentContainerRT.anchoredPosition;
                        Vector2 fromPos = new Vector2(originalPos.x + ContentContainerRT.rect.width, originalPos.y);
                        ContentContainerRT.anchoredPosition = fromPos;
                        AniSequence.Append(ContentContainerRT.DOAnchorPos(originalPos, AniSeconds));
                        break;
                    }
                case PushAni.BottomToTop:
                    {
                        Vector2 originalPos = ContentContainerRT.anchoredPosition;
                        Vector2 fromPos = new Vector2(originalPos.x, originalPos.y - ContentContainerRT.rect.height);
                        ContentContainerRT.anchoredPosition = fromPos;
                        AniSequence.Append(ContentContainerRT.DOAnchorPos(originalPos, AniSeconds));
                        break;
                    }
                case PushAni.TopToBottom:
                    {
                        Vector2 originalPos = ContentContainerRT.anchoredPosition;
                        Vector2 fromPos = new Vector2(originalPos.x, originalPos.y + ContentContainerRT.rect.height);
                        ContentContainerRT.anchoredPosition = fromPos;
                        AniSequence.Append(ContentContainerRT.DOAnchorPos(originalPos, AniSeconds));
                        break;
                    }
                case PushAni.ScaleSmallToBig:
                    ContentContainerRT.localScale = ScaleSmallValue;
                    AniSequence.Append(ContentContainerRT.DOScale(Vector3.one, AniSeconds));
                    break;
                case PushAni.ScaleBigToSmall:
                    ContentContainerRT.localScale = ScaleBigValue;
                    AniSequence.Append(ContentContainerRT.DOScale(Vector3.one, AniSeconds));
                    break;
                case PushAni.FadeIn:
                    CanvasGroupY.alpha = 0;
                    AniSequence.Append(CanvasGroupY.DOFade(1, AniSeconds));
                    break;
                default:
                    break;
            }
            if (pushAni != PushAni.None && PageType == PageType.Dialog && MaskBGImg != null)
            {
                MaskBGImg.color = MaskColorTouMing;
                AniSequence.Join(MaskBGImg.DOColor(MaskColorShown, AniSeconds));
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
                        Vector2 originalPos = ContentContainerRT.anchoredPosition;
                        Vector2 toPos = new Vector2(originalPos.x + ContentContainerRT.rect.width, originalPos.y);
                        AniSequence.Append(ContentContainerRT.DOAnchorPos(toPos, AniSeconds));
                        break;
                    }
                case PopAni.RightToLeft:
                    {
                        Vector2 originalPos = ContentContainerRT.anchoredPosition;
                        Vector2 toPos = new Vector2(originalPos.x - ContentContainerRT.rect.width, originalPos.y);
                        AniSequence.Append(ContentContainerRT.DOAnchorPos(toPos, AniSeconds));
                        break;
                    }
                case PopAni.BottomToTop:
                    {
                        Vector2 originalPos = ContentContainerRT.anchoredPosition;
                        Vector2 toPos = new Vector2(originalPos.x, originalPos.y + ContentContainerRT.rect.height);
                        AniSequence.Append(ContentContainerRT.DOAnchorPos(toPos, AniSeconds));
                        break;
                    }
                case PopAni.TopToBottom:
                    {
                        Vector2 originalPos = ContentContainerRT.anchoredPosition;
                        Vector2 toPos = new Vector2(originalPos.x, originalPos.y - ContentContainerRT.rect.height);
                        AniSequence.Append(ContentContainerRT.DOAnchorPos(toPos, AniSeconds));
                        break;
                    }
                case PopAni.ScaleSmallToBig:
                    ContentContainerRT.localScale = Vector3.one;
                    AniSequence.Append(ContentContainerRT.DOScale(ScaleBigValue, AniSeconds));
                    break;
                case PopAni.ScaleBigToSmall:
                    ContentContainerRT.localScale = Vector3.one;
                    AniSequence.Append(ContentContainerRT.DOScale(ScaleSmallValue, AniSeconds));
                    break;
                case PopAni.FadeOut:
                    CanvasGroupY.alpha = 1;
                    AniSequence.Append(CanvasGroupY.DOFade(0, AniSeconds));
                    break;
                default:
                    break;
            }
            if (popAni != PopAni.None && PageType == PageType.Dialog && MaskBGImg != null)
            {
                MaskBGImg.color = MaskColorShown;
                AniSequence.Join(MaskBGImg.DOColor(MaskColorTouMing, AniSeconds));
            }
            AniSequence.OnComplete(complete);
        }
    }
    #endregion
}