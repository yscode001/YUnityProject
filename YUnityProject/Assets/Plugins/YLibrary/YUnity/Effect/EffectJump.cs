// Author：yaoshuai
// Email：yscode@126.com
// Date：2022-2-1
// ------------------------------
using System;
using UnityEngine;

namespace YUnity
{
    /// <summary>
    /// 跳跃特效
    /// </summary>
    public partial class EffectJump : MonoBehaviour
    {
        private float DestY = 0;
        private float CurrentY = 0;
        private float UpSpeed = 0;
        private float DownSpeed = 0;
        private Action<float> JumpY;
        private Action UpComplete;
        private Action DownComplete;

        public bool IsJumping => DestY > 0 && JumpY != null;
        public bool IsJumpingUp => UpSpeed > 0 && JumpY != null;
        public bool IsJumpingDown => UpSpeed <= 0 && DownSpeed > 0 && JumpY != null;
    }
    public partial class EffectJump
    {
        private void Update()
        {
            if (!IsJumping)
            {
                return;
            }
            if (UpSpeed > 0)
            {
                // 向上跳跃
                CurrentY += UpSpeed * Time.deltaTime;
                if (CurrentY >= DestY)
                {
                    // 跳至最高点
                    CurrentY = DestY;
                    JumpY?.Invoke(CurrentY);
                    UpComplete?.Invoke();
                    UpSpeed = 0;
                }
                else
                {
                    // 向上跳跃
                    JumpY?.Invoke(CurrentY);
                }
            }
            else if (DownSpeed > 0)
            {
                // 向上跳跃后自由落体
                CurrentY -= DownSpeed * Time.deltaTime;
                if (CurrentY <= 0)
                {
                    // 已落地
                    CurrentY = 0;
                    JumpY?.Invoke(CurrentY);
                    End(true);
                }
                else
                {
                    // 正在落地
                    JumpY?.Invoke(CurrentY);
                }
            }
        }
    }
    public partial class EffectJump
    {
        /// <summary>
        /// 开始跳跃
        /// </summary>
        /// <param name="jumpY">跳跃高度回调</param>
        /// <param name="destY">目标高度(单位m)</param>
        /// <param name="upSpeed">向上速度(单位m/s)</param>
        /// <param name="downSpeed">落地速度(单位m/s)</param>
        /// <param name="upComplete">跳跃至最高点回调</param>
        /// <param name="downComplete">落地后的回调</param>
        public void Jump(Action<float> jumpY, float destY = 2, float upSpeed = 20, float downSpeed = 9.8f, Action upComplete = null, Action downComplete = null)
        {
            if (IsJumping || destY <= 0 || upSpeed <= 0 || downSpeed <= 0 || jumpY == null)
            {
                return;
            }
            CurrentY = 0;
            JumpY = jumpY;
            DestY = destY;
            UpSpeed = upSpeed;
            DownSpeed = downSpeed;
            UpComplete = upComplete;
            DownComplete = downComplete;
        }

        /// <summary>
        /// 结束跳跃
        /// </summary>
        /// <param name="executeDownComplete"></param>
        public void End(bool executeDownComplete)
        {
            if (!IsJumping)
            {
                return;
            }
            DestY = 0;
            CurrentY = 0;
            UpSpeed = 0;
            DownSpeed = 0;
            JumpY = null;
            UpComplete = null;
            if (executeDownComplete)
            {
                DownComplete?.Invoke();
            }
            DownComplete = null;
        }
    }
}