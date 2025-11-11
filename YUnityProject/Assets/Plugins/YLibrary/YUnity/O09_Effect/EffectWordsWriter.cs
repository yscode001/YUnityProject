// Author：yaoshuai
// Email：yscode@126.com
// Date：2022-1-12
// ------------------------------
using System;
using UnityEngine;
using UnityEngine.UI;

namespace YUnity
{
    [RequireComponent(typeof(Text))]

    /// <summary>
    /// 打字机特效，请加在Text组件上
    /// </summary>
    public partial class EffectWordsWriter : MonoBehaviour
    {
        private Text TextCtrl; // 文字组件

        private string content = ""; // 内容
        private float wordSecond = 0; // 每一个字占用秒数
        private float overDelaySecondComplete = 0; // 打字结束延迟多少秒后执行回调
        private Action complete; // 打字结束完成后执行的回调

        private int currentPos = 0; // 当前打字位置
        private float timer = 0; // 经过的时间
        private bool isWriting = false; // 是否正在进行打字机(打字机的完整声明周期)
        private bool IsShowAllText = true; // 文字是否全部显示(全部显示不代表完成，因为有可能会有延迟回调)

        private void Awake()
        {
            TextCtrl = GetComponent<Text>();
        }
    }
    #region 打字
    public partial class EffectWordsWriter
    {
        private void Update()
        {
            if (isWriting)
            {
                timer += Time.deltaTime;
                if (IsShowAllText)
                {
                    // 文字已全部显示，能走到这里，说明有延迟回调
                    if (timer >= overDelaySecondComplete)
                    {
                        End(true);
                    }
                }
                else if (timer >= wordSecond)
                {
                    // 文字还在显示中
                    timer = 0;
                    currentPos++;
                    TextCtrl.text = content.Substring(0, currentPos);
                    if (currentPos >= content.Length)
                    {
                        if (overDelaySecondComplete > 0 && complete != null)
                        {
                            // 有延迟回调
                            IsShowAllText = true;
                        }
                        else
                        {
                            // 没有延迟回调，立即结束
                            End(true);
                        }
                    }
                }
            }
        }
    }
    #endregion
    #region 开始、修改、结束
    public partial class EffectWordsWriter
    {
        /// <summary>
        /// 开始打字
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="wordSecond">每一个字占用秒数</param>
        /// <param name="overDelayComplete">打字结束延迟多少秒后执行回调</param>
        /// <param name="complete">打字结束完成后执行的回调</param>
        public void Begin(string content, float wordSecond, float overDelaySecondComplete = 0, Action complete = null)
        {
            if (string.IsNullOrWhiteSpace(content) || wordSecond <= 0) { return; }

            this.content = content;
            this.wordSecond = wordSecond;
            this.overDelaySecondComplete = Math.Max(0, overDelaySecondComplete);
            this.complete = complete;

            currentPos = 0;
            timer = 0;
            TextCtrl.text = "";
            IsShowAllText = false;
            isWriting = true;
        }

        /// <summary>
        /// 修改时间间隔(仅在正在打字时有效)
        /// </summary>
        /// <param name="wordSecond">每一个字占用秒数</param>
        public void Edit(float wordSecond)
        {
            if (isWriting)
            {
                this.wordSecond = wordSecond;
            }
        }

        /// <summary>
        /// 强制结束(打字内容会全部显示出来)
        /// </summary>
        /// <param name="executeComplete">是否执行结束时的回调</param>
        public void End(bool executeComplete)
        {
            if (isWriting)
            {
                isWriting = false;
                TextCtrl.text = content;

                IsShowAllText = true;
                content = "";
                wordSecond = 0;
                overDelaySecondComplete = 0;
                currentPos = 0;
                timer = 0;
                if (executeComplete)
                {
                    complete?.Invoke();
                }

                // 这里不要设置为null，防止重复打字，下一次设置了complete，然后又置为了null
                // complete = null;
            }
        }
    }
    #endregion
}