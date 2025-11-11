// Author：yaoshuai
// Email：yscode@126.com
// Date：2023-5-29
// ------------------------------
using System;
using UnityEngine;

namespace YUnity
{
    public class EffectShake : MonoBehaviour
    {
        /// <summary>
        /// 需要震动的游戏物体
        /// </summary>
        private Transform ShakeTransform;

        /// <summary>
        /// 震动前的位置
        /// </summary>
        private Vector3 PositionBeforeShake;

        /// <summary>
        /// 震动频率
        /// </summary>
        private float ShakeCD;

        /// <summary>
        /// 震动次数
        /// </summary>
        private int ShakeCount;

        /// <summary>
        /// 震动最大半径
        /// </summary>
        private float MaxShakeRadius;

        private float ShakeTime;

        private Action ShakeComplete;

        /// <summary>
        /// 是否正在震动
        /// </summary>
        public bool IsShaking { get; private set; } = false;

        private void ClearProperty()
        {
            ShakeTransform = null;
            PositionBeforeShake = Vector3.zero;
            ShakeCD = 0;
            ShakeCount = 0;
            MaxShakeRadius = 0;
            ShakeTime = 0;
            ShakeComplete = null;
            IsShaking = false;
        }

        /// <summary>
        /// 开始震动
        /// </summary>
        /// <param name="shakeTransform">需要震动的游戏物体</param>
        /// <param name="shakeCD">震动频率</param>
        /// <param name="shakeCount">震动次数</param>
        /// <param name="maxShakeRadius">最大震动半径</param>
        /// <param name="shakeComplete">完成回调</param>
        public void BeginShake(Transform shakeTransform, float shakeCD = 0.002f, int shakeCount = 10, float maxShakeRadius = 0.05f, Action shakeComplete = null)
        {
            ClearProperty();
            if (shakeTransform != null && shakeCD > 0 && shakeCount > 0 && maxShakeRadius > 0)
            {
                ShakeTransform = shakeTransform;
                PositionBeforeShake = shakeTransform.localPosition;
                ShakeCD = shakeCD;
                ShakeCount = shakeCount;
                MaxShakeRadius = maxShakeRadius;
                ShakeComplete = shakeComplete;
                IsShaking = true;
            }
        }
        /// <summary>
        /// 强制结束震动
        /// </summary>
        public void StopShake()
        {
            ClearProperty();
        }

        private void Update()
        {
            if (ShakeTransform == null || ShakeCD <= 0 || ShakeCount <= 0 || MaxShakeRadius <= 0 || ShakeTime + ShakeCD > Time.time) { return; }
            ShakeCount -= 1;
            float radio = UnityEngine.Random.Range(-MaxShakeRadius, MaxShakeRadius);
            if (ShakeCount == 0)
            {
                // 震动最后一次时设置为震动前记录的位置
                radio = 0;
            }
            ShakeTime = Time.time;
            ShakeTransform.localPosition = PositionBeforeShake + Vector3.one * radio;
            if (ShakeCount == 0)
            {
                ShakeComplete?.Invoke();
                StopShake();
            }
        }
    }
}