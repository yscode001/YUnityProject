using System;
using UnityEngine;

namespace YUnity
{
    /// <summary>
    /// 子弹直线飞行到固定位置
    /// </summary>
    public partial class BulletLinePosition : MonoBehaviourBaseY
    {
        /// <summary>
        /// 目标位置
        /// </summary>
        private Vector3 TargetPosition;

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

        private void Clear()
        {
            IsFlying = false;

            TargetPosition = Vector3.zero;
            MoveSpeed = ErrorDistance = 0;
            ReachedComplete = null;
        }
    }
    public partial class BulletLinePosition
    {
        /// <summary>
        /// 子弹开始飞行
        /// </summary>
        /// <param name="targetPosition">目标位置</param>
        /// <param name="moveSpeed">飞行速度</param>
        /// <param name="errorDistance">误差距离</param>
        /// <param name="reachedComplete">达到目标位置后的回调</param>
        public void BeginFly(Vector3 targetPosition, float moveSpeed, float errorDistance, Action reachedComplete)
        {
            Clear();
            if (moveSpeed <= 0 || errorDistance < 0)
            {
                return;
            }
            TargetPosition = targetPosition;
            MoveSpeed = moveSpeed;
            ErrorDistance = errorDistance;
            ReachedComplete = reachedComplete;

            TransformY.LookAt(targetPosition);

            IsFlying = true;
        }
    }
    public partial class BulletLinePosition
    {
        private void Update()
        {
            if (IsFlying == false)
            {
                return;
            }
            if (Vector3.Distance(TargetPosition, TransformY.position) <= ErrorDistance)
            {
                // 抵达终点
                ReachedComplete?.Invoke();
                Clear();
            }
            else
            {
                // 飞向目标
                Vector3 willMove = MoveSpeed * Time.deltaTime * TransformY.forward.normalized;
                TransformY.Translate(willMove, Space.World);
            }
        }
    }
}