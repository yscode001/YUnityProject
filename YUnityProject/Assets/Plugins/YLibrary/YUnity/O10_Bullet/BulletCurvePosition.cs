using System;
using UnityEngine;

namespace YUnity
{
    /// <summary>
    /// 子弹曲线追踪固定位置
    /// </summary>
    public partial class BulletCurvePosition : MonoBehaviourBaseY
    {
        /// <summary>
        /// 目标位置
        /// </summary>
        private Vector3 TargetPos = Vector3.zero;

        /// <summary>
        /// 子弹速度
        /// </summary>
        private float MoveSpeed = 0;

        /// <summary>
        /// 误差距离
        /// </summary>
        private float ErrorDistance = 0;

        /// <summary>
        /// 达到目标位置后的回调
        /// </summary>
        private Action ReachedComplete = null;

        // 下面是辅助属性
        private bool IsFlying = false; // 是否正在飞行
        private Vector3 CurveDir = Vector3.zero; // 曲线方向

        private void Clear()
        {
            IsFlying = false;
            CurveDir = Vector3.zero;

            TargetPos = Vector3.zero;
            MoveSpeed = ErrorDistance = 0;
            ReachedComplete = null;
        }
    }
    public partial class BulletCurvePosition
    {
        /// <summary>
        /// 开始飞行
        /// </summary>
        /// <param name="curveDir">曲线方向，zero表示随机曲线方向</param>
        /// <param name="curveRandomSeed">随机方向种子，仅在curveDir为zero时有意义</param>
        /// <param name="curveWeight">曲线权重(0-1)</param>
        /// <param name="targetPos">目标位置</param>
        /// <param name="moveSpeed">飞行速度</param>
        /// <param name="errorDistance">误差距离</param>
        /// <param name="reachedComplete">达到目标位置后的回调</param>
        public void BeginFly(Vector3 curveDir, int curveRandomSeed, float curveWeight, Vector3 targetPos, float moveSpeed, float errorDistance, Action reachedComplete)
        {
            Clear();
            if (moveSpeed <= 0 || errorDistance < 0 || (curveDir == Vector3.zero && curveRandomSeed <= 0))
            {
                return;
            }
            TargetPos = targetPos;
            MoveSpeed = moveSpeed;
            ErrorDistance = errorDistance;
            ReachedComplete = reachedComplete;
            if (curveDir == Vector3.zero)
            {
                // 随机曲线弹道
                System.Random ran = new System.Random(curveRandomSeed);
                Vector3 v1 = Vector3.Cross(targetPos - TransformY.position, Vector3.up).normalized;
                v1 *= ran.Next(-100, 100);
                Vector3 v2 = Vector3.up * ran.Next(0, 100);
                CurveDir = (v1 + v2).normalized * Mathf.Clamp(curveWeight, 0, 1);
            }
            else
            {
                // 指定曲线弹道
                CurveDir = curveDir * Mathf.Clamp(curveWeight, 0, 1);
            }
            IsFlying = true; // 开始飞行
        }
    }
    public partial class BulletCurvePosition
    {
        private void Update()
        {
            if (IsFlying == false)
            {
                return;
            }
            if (Vector3.Distance(TargetPos, TransformY.position) <= ErrorDistance)
            {
                // 抵达终点
                ReachedComplete?.Invoke();
                Clear();
            }
            else
            {
                // 飞向目标
                Vector3 dir = (TargetPos - TransformY.position).normalized;
                if (CurveDir != Vector3.zero)
                {
                    dir = (dir + CurveDir).normalized;
                }
                TransformY.Translate(MoveSpeed * Time.deltaTime * dir, Space.World);
            }
        }
    }
}