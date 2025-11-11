using UnityEngine;

namespace YUnity
{
    #region 属性
    /// <summary>
    /// 翻牌特效
    /// </summary>
    public partial class EffectCardTurnOver : MonoBehaviour
    {
        /// <summary>
        /// 正面的卡牌
        /// </summary>
        public GameObject CardFrontGO = null;

        /// <summary>
        /// 背面的卡牌
        /// </summary>
        public GameObject CardBackGO = null;

        /// <summary>
        /// 卡牌当前的状态，是正面还是背面
        /// </summary>
        public EffectCardState CurrentCardState = EffectCardState.front;

        /// <summary>
        /// 翻转方式，水平翻转还是垂直翻转
        /// </summary>
        public EffectCardTurnWay TurnWay = EffectCardTurnWay.horizontal;

        /// <summary>
        /// 翻牌的速度，一秒钟翻转多少度
        /// </summary>
        public float TurnSpeed = 360;

        /// <summary>
        /// 是否正在翻转
        /// </summary>
        private bool IsTurning = false;

        /// <summary>
        /// 隐藏时的角度
        /// </summary>
        private Vector3 HideEuler = Vector3.zero;

        /// <summary>
        /// 显示时的角度
        /// </summary>
        private Vector3 ShowEuler = Vector3.zero;
    }
    #endregion
    #region 初始化
    public partial class EffectCardTurnOver
    {
        private bool isInited = false;

        private void Start()
        {
            if (!isInited)
            {
                Init();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="CardFrontGO">正面的卡牌</param>
        /// <param name="CardBackGO">背面的卡牌</param>
        /// <param name="CurrentCardState">卡牌当前的状态，是正面还是背面</param>
        /// <param name="TurnWay">翻转方式，水平翻转还是垂直翻转</param>
        /// <param name="TurnSpeed">翻牌的速度，一秒钟翻转多少度</param>
        public void Init(GameObject CardFrontGO, GameObject CardBackGO, EffectCardState CurrentCardState, EffectCardTurnWay TurnWay, float TurnSpeed)
        {
            this.CardFrontGO = CardFrontGO;
            this.CardBackGO = CardBackGO;
            this.CurrentCardState = CurrentCardState;
            this.TurnWay = TurnWay;
            this.TurnSpeed = TurnSpeed;

            isInited = false;
            Init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (isInited) { return; }
            isInited = true;
            IsTurning = false;
            if (TurnWay == EffectCardTurnWay.horizontal)
            {
                HideEuler = new Vector3(0, 90, 0);
                ShowEuler = Vector3.zero;
            }
            else
            {
                HideEuler = new Vector3(90, 0, 0);
                ShowEuler = Vector3.zero;
            }
            if (CurrentCardState == EffectCardState.front)
            {
                // 如果是从正面开始，则将背面旋转90度，这样就看不见背面了
                CardFrontGO.transform.eulerAngles = ShowEuler;
                CardBackGO.transform.eulerAngles = HideEuler;
                CardFrontGO.SetActive(true);
                CardBackGO.SetActive(false);
            }
            else
            {
                // 从背面开始，同理
                CardFrontGO.transform.eulerAngles = HideEuler;
                CardBackGO.transform.eulerAngles = ShowEuler;
                CardFrontGO.SetActive(false);
                CardBackGO.SetActive(true);
            }
        }
    }
    #endregion
    public partial class EffectCardTurnOver
    {
        /// <summary>
        /// 开始翻转至正面
        /// </summary>
        public void StartTurnToFront(bool animate = true)
        {
            if (!isInited)
            {
                Init();
            }
            if (animate)
            {
                if (CurrentCardState == EffectCardState.front || IsTurning)
                {
                    return;
                }
                RunTurnAction(EffectCardState.front);
            }
            else
            {
                SetEnd(EffectCardState.front);
            }
        }
        /// <summary>
        /// 开始翻转至背面
        /// </summary>
        public void StartTurnToBack(bool animate = true)
        {
            if (!isInited)
            {
                Init();
            }
            if (animate)
            {
                if (CurrentCardState == EffectCardState.back || IsTurning)
                {
                    return;
                }
                RunTurnAction(EffectCardState.back);
            }
            else
            {
                SetEnd(EffectCardState.back);
            }
        }
    }
    #region 具体的翻牌动作
    public partial class EffectCardTurnOver
    {
        // 直接翻转结束
        private void SetEnd(EffectCardState toState)
        {
            CurrentCardState = toState;
            if (CurrentCardState == EffectCardState.front)
            {
                // 如果是从正面开始，则将背面旋转90度，这样就看不见背面了
                CardFrontGO.transform.eulerAngles = ShowEuler;
                CardBackGO.transform.eulerAngles = HideEuler;
                CardFrontGO.SetActive(true);
                CardBackGO.SetActive(false);
            }
            else
            {
                // 从背面开始，同理
                CardFrontGO.transform.eulerAngles = HideEuler;
                CardBackGO.transform.eulerAngles = ShowEuler;
                CardFrontGO.SetActive(false);
                CardBackGO.SetActive(true);
            }
        }

        private void RunTurnAction(EffectCardState toState)
        {
            CardFrontGO.SetActive(true);
            CardBackGO.SetActive(true);
            CurrentCardState = toState;
            IsTurning = true;
        }
        private void Update()
        {
            if (!IsTurning) { return; }
            if (CurrentCardState == EffectCardState.front)
            {
                // 当前是背面，需翻转至正面
                if (CardFrontGO.transform.eulerAngles == ShowEuler && CardBackGO.transform.eulerAngles == HideEuler)
                {
                    // 翻转完成
                    IsTurning = false;
                    CardFrontGO.SetActive(true);
                    CardBackGO.SetActive(false);
                }
                else if (CardBackGO.transform.eulerAngles != HideEuler)
                {
                    // 正在翻转背面(先翻转)
                    if (TurnWay == EffectCardTurnWay.horizontal)
                    {
                        Vector3 euler = CardBackGO.transform.eulerAngles;
                        float y = Mathf.Min(90, euler.y + Time.deltaTime * TurnSpeed);
                        euler.y = y;
                        CardBackGO.transform.eulerAngles = euler;
                    }
                    else
                    {
                        Vector3 euler = CardBackGO.transform.eulerAngles;
                        float x = Mathf.Min(90, euler.x + Time.deltaTime * TurnSpeed);
                        euler.x = x;
                        CardBackGO.transform.eulerAngles = euler;
                    }
                }
                else
                {
                    // 正在翻转正面(后翻转)
                    if (TurnWay == EffectCardTurnWay.horizontal)
                    {
                        Vector3 euler = CardFrontGO.transform.eulerAngles;
                        float y = Mathf.Max(0, euler.y - Time.deltaTime * TurnSpeed);
                        euler.y = y;
                        CardFrontGO.transform.eulerAngles = euler;
                    }
                    else
                    {
                        Vector3 euler = CardFrontGO.transform.eulerAngles;
                        float x = Mathf.Max(0, euler.x - Time.deltaTime * TurnSpeed);
                        euler.x = x;
                        CardFrontGO.transform.eulerAngles = euler;
                    }
                }
            }
            else
            {
                // 当前是正面，需翻转至背面
                if (CardFrontGO.transform.eulerAngles == HideEuler && CardBackGO.transform.eulerAngles == ShowEuler)
                {
                    // 翻转完成
                    IsTurning = false;
                    CardFrontGO.SetActive(false);
                    CardBackGO.SetActive(true);
                }
                else if (CardFrontGO.transform.eulerAngles != HideEuler)
                {
                    // 正在翻转正面(先翻转)
                    if (TurnWay == EffectCardTurnWay.horizontal)
                    {
                        Vector3 euler = CardFrontGO.transform.eulerAngles;
                        float y = Mathf.Min(90, euler.y + Time.deltaTime * TurnSpeed);
                        euler.y = y;
                        CardFrontGO.transform.eulerAngles = euler;
                    }
                    else
                    {
                        Vector3 euler = CardFrontGO.transform.eulerAngles;
                        float x = Mathf.Min(90, euler.x + Time.deltaTime * TurnSpeed);
                        euler.x = x;
                        CardFrontGO.transform.eulerAngles = euler;
                    }
                }
                else
                {
                    // 正在翻转背面(后翻转)
                    if (TurnWay == EffectCardTurnWay.horizontal)
                    {
                        Vector3 euler = CardBackGO.transform.eulerAngles;
                        float y = Mathf.Max(0, euler.y - Time.deltaTime * TurnSpeed);
                        euler.y = y;
                        CardBackGO.transform.eulerAngles = euler;
                    }
                    else
                    {
                        Vector3 euler = CardBackGO.transform.eulerAngles;
                        float x = Mathf.Max(0, euler.x - Time.deltaTime * TurnSpeed);
                        euler.x = x;
                        CardBackGO.transform.eulerAngles = euler;
                    }
                }
            }
        }
    }
    #endregion
}